#include "pch.h"
#include "cert.h"
#include "sha256.h"
#include <windows.h>
#include <wincrypt.h>
#include <wintrust.h>

// Hàm chuyển đổi FILETIME thành Unix timestamp (long long)
const long long TICKS_PER_SECOND = 10000000;
const long long TICKS_PER_MILLISECOND = 10000;
const long long EPOCH_DIFFERENCE = 504911232000000000LL;

long long FileTimeToTicks(const FILETIME& ft) {
	ULARGE_INTEGER ull;
	ull.LowPart = ft.dwLowDateTime;
	ull.HighPart = ft.dwHighDateTime;
	return static_cast<long long>(ull.QuadPart) + EPOCH_DIFFERENCE;
}

size_t GetCertSize(const CertInfo& certinfo) {
	return strlen(certinfo.Subject) + 1 +
		strlen(certinfo.Issuer) + 1 +
		sizeof(long long) * 2 +
		strlen(certinfo.Thumbprint) + 1 +
		strlen(certinfo.SerialNumber) + 1;
}

void SerializeCertInfo(const CertInfo& certinfo, std::vector<uint8_t>& buffer) {
	size_t offset = 0;
	auto AppendString = [&buffer, &offset](const char* str) {
		size_t lenght = strlen(str) + 1;
		memcpy(buffer.data() + offset, str, lenght);
		offset += lenght;
		};

	auto AppendInt64 = [&buffer, &offset](long long value) {
		memcpy(buffer.data() + offset, &value, sizeof(long long));
		offset += sizeof(long long);
		};

	AppendString(certinfo.Subject);
	AppendString(certinfo.Issuer);
	AppendInt64(certinfo.ValidFrom);
	AppendInt64(certinfo.ValidTo);
	AppendString(certinfo.Thumbprint);
	AppendString(certinfo.SerialNumber);
}

bool CompareByteArrays(const std::vector<uint8_t>& a, std::vector<uint8_t>& b) {

	if (a.size() != b.size()) {
		return false;
	}
	return std::memcmp(a.data(), b.data(), a.size()) == 0;
}

void WriteByteToFile(const std::vector<uint8_t>& buffer, const std::string& filePath) {
	std::ofstream outFile(filePath, std::ios::binary);
	if (outFile.is_open()) {
		outFile.write(reinterpret_cast<const char*>(buffer.data()), buffer.size());
		outFile.close();
	}
	else {

	}
}

void ForceCheckCertPermissionInternal(CertInfo certinfo) {
	size_t  size = GetCertSize(certinfo);
	std::vector<uint8_t> buffer(size);
	SerializeCertInfo(certinfo, buffer);
	SHA256 sha256;
	sha256.update(buffer.data(), size);
	auto result = sha256.final();
	if (result != DEZONE99_SIGNING_CERT_INFO_V0_HASH) {
		throw std::exception("Security exception: cert is invalid!");
	}
}

int GetCertificateInfoInternal(const char* filePath, CertInfo* certInfo) {
	if (!filePath || !certInfo) {
		return -1; // Invalid argument
	}

	std::wstring wFilePath = std::wstring(filePath, filePath + strlen(filePath));
	HCERTSTORE hStore = NULL;
	HCRYPTMSG hMsg = NULL;
	DWORD dwEncoding, dwContentType, dwFormatType;

	if (!CryptQueryObject(CERT_QUERY_OBJECT_FILE, wFilePath.c_str(),
		CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED,
		CERT_QUERY_FORMAT_FLAG_BINARY,
		0, &dwEncoding, &dwContentType, &dwFormatType, &hStore, &hMsg, NULL)) {
		return -2; // Failed to query object
	}

	PCCERT_CONTEXT pCertContext = NULL;
	pCertContext = CertFindCertificateInStore(hStore, dwEncoding, 0, CERT_FIND_ANY, NULL, NULL);
	if (!pCertContext) {
		CertCloseStore(hStore, 0);
		return -3; // No certificate found
	}

	char subjectName[256];
	char issuerName[256];
	CertGetNameStringA(pCertContext, CERT_NAME_SIMPLE_DISPLAY_TYPE, 0, NULL, subjectName, 256);
	CertGetNameStringA(pCertContext, CERT_NAME_SIMPLE_DISPLAY_TYPE, CERT_NAME_ISSUER_FLAG, NULL, issuerName, 256);

	DWORD thumbprintSize = 0;
	CertGetCertificateContextProperty(pCertContext, CERT_KEY_IDENTIFIER_PROP_ID, NULL, &thumbprintSize);
	std::vector<BYTE> thumbprint(thumbprintSize);
	CertGetCertificateContextProperty(pCertContext, CERT_KEY_IDENTIFIER_PROP_ID, thumbprint.data(), &thumbprintSize);

	std::string thumbprintHex;
	for (DWORD i = 0; i < thumbprintSize; i++) {
		char hex[3];
		sprintf_s(hex, "%02x", thumbprint[i]);
		thumbprintHex += hex;
	}

	std::string serialNumberHex;
	for (DWORD i = 0; i < pCertContext->pCertInfo->SerialNumber.cbData; i++) {
		char hex[3];
		sprintf_s(hex, "%02x", pCertContext->pCertInfo->SerialNumber.pbData[i]);
		serialNumberHex += hex;
	}

	certInfo->Subject = _strdup(subjectName);
	certInfo->Issuer = _strdup(issuerName);
	certInfo->ValidFrom = FileTimeToTicks(pCertContext->pCertInfo->NotBefore);
	certInfo->ValidTo = FileTimeToTicks(pCertContext->pCertInfo->NotAfter);
	certInfo->Thumbprint = _strdup(thumbprintHex.c_str());
	certInfo->SerialNumber = _strdup(serialNumberHex.c_str());

	CertFreeCertificateContext(pCertContext);
	CertCloseStore(hStore, 0);
	CryptMsgClose(hMsg);

	return 0; // Success
}

