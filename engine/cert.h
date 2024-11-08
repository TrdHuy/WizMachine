#pragma once


#ifndef __ENGINE_CERT_H__
#define __ENGINE_CERT_H__
#define DEZONE99_SIGNING_CERT_INFO_V0_HASH "aa5f856f92696ea5b708d74ca795829e97fd8742231df90890d821a9108bdb9e"

struct CertInfo {
	const char* Subject;
	const char* Issuer;
	long long ValidFrom;
	long long ValidTo;
	const char* Thumbprint;
	const char* SerialNumber;
};

bool ForceCheckCertPermissionInternal(CertInfo& certinfo);

int GetCertificateInfoInternal(const char* filePath, CertInfo* certInfo);
APIResult GetCertificateInfoInternal2(const char* filePath, CertInfo* certInfo);
#endif