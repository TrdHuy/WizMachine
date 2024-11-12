#pragma once

#include <comdef.h>
#include <iostream>
#include <fstream>
#include <vector>
#include "base.h"
#include "cert.h"
#include "PakWorkManager.h"

#ifdef ENGINE_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif


extern "C" {

	/**
	 * @brief Tải dữ liệu từ tệp SPR và trích xuất thông tin chi tiết.
	 *
	 * Hàm này đọc tệp SPR từ đường dẫn được chỉ định bởi `filePath` và trích xuất các thông tin sau:
	 * - Header của tệp SPR (`SPRFileHead`).
	 * - Bảng màu (`palette`) và độ dài bảng màu (`paletteLength`).
	 * - Vị trí dữ liệu frame bắt đầu (`frameDataBeginPos`).
	 * - Dữ liệu các frame (`frame`) và số lượng frame (`frameCount`).
	 *
	 * @param filePath Đường dẫn đầy đủ tới tệp SPR cần tải.
	 * @param fileHead Con trỏ tới `SPRFileHead`, nơi lưu thông tin header của tệp SPR.
	 * @param palette Con trỏ kép tới mảng `Color`, nơi lưu bảng màu của tệp SPR.
	 * @param paletteLength Con trỏ tới một `int` để lưu độ dài bảng màu.
	 * @param frameDataBeginPos Con trỏ tới một `int` để lưu vị trí dữ liệu frame bắt đầu.
	 * @param frame Con trỏ kép tới mảng `FrameData`, nơi lưu dữ liệu các frame.
	 * @param frameCount Con trỏ tới một `int` để lưu số lượng frame.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Tệp SPR phải tồn tại và có định dạng hợp lệ.
	 * - Hàm sẽ tự động giải phóng bộ nhớ của mảng `fileData` được cấp phát trong quá trình tải.
	 * - Nếu xảy ra lỗi khi đọc tệp, tất cả các con trỏ kết quả (`palette`, `frame`, v.v.) sẽ được gán `nullptr`.
	 *
	 * @example
	 * SPRFileHead fileHead;
	 * Color* palette = nullptr;
	 * int paletteLength = 0;
	 * int frameDataBeginPos = 0;
	 * FrameData* frame = nullptr;
	 * int frameCount = 0;
	 *
	 * APIResult result = LoadSPRFile(
	 *     "path/to/sprfile.spr",
	 *     &fileHead,
	 *     &palette,
	 *     &paletteLength,
	 *     &frameDataBeginPos,
	 *     &frame,
	 *     &frameCount
	 * );
	 *
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("SPR file loaded successfully.\n");
	 *     printf("Number of frames: %d\n", frameCount);
	 * } else {
	 *     printf("Error loading SPR file: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl LoadSPRFile(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frameData,
		int* frameCount);

	DLL_API APIResult __cdecl LoadSPRFileForTestOnly(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frameData,
		int* frameCount);
	/**
	 * @brief Tải dữ liệu SPR từ bộ nhớ và trích xuất thông tin chi tiết.
	 *
	 * Hàm này xử lý dữ liệu SPR được cung cấp dưới dạng mảng byte trong bộ nhớ (`data`), sau đó
	 * trích xuất các thông tin sau:
	 * - Header của tệp SPR (`SPRFileHead`).
	 * - Bảng màu (`palette`) và độ dài bảng màu (`paletteLength`).
	 * - Vị trí dữ liệu frame bắt đầu (`frameDataBeginPos`).
	 * - Dữ liệu các frame (`frame`) và số lượng frame (`frameCount`).
	 *
	 * @param data Con trỏ tới mảng byte chứa dữ liệu SPR.
	 * @param dataLength Độ dài của mảng byte `data`.
	 * @param fileHead Con trỏ tới `SPRFileHead`, nơi lưu thông tin header của dữ liệu SPR.
	 * @param palette Con trỏ kép tới mảng `Color`, nơi lưu bảng màu của dữ liệu SPR.
	 * @param paletteLength Con trỏ tới một `int` để lưu độ dài bảng màu.
	 * @param frameDataBeginPos Con trỏ tới một `int` để lưu vị trí dữ liệu frame bắt đầu.
	 * @param frame Con trỏ kép tới mảng `FrameData`, nơi lưu dữ liệu các frame.
	 * @param frameCount Con trỏ tới một `int` để lưu số lượng frame.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Hàm này yêu cầu dữ liệu đầu vào (`data`) phải hợp lệ và chứa dữ liệu SPR đúng định dạng.
	 * - Các con trỏ kết quả (`palette`, `frame`, v.v.) sẽ được khởi tạo động nếu thao tác thành công.
	 * - Người dùng phải giải phóng bộ nhớ của các con trỏ được khởi tạo sau khi sử dụng.
	 * - Nếu xảy ra lỗi, các con trỏ kết quả sẽ được gán `nullptr`.
	 *
	 * @example
	 * const uint8_t* sprData = ...; // Dữ liệu SPR được nạp từ nguồn khác
	 * size_t sprDataLength = ...;  // Độ dài dữ liệu
	 * SPRFileHead fileHead;
	 * Color* palette = nullptr;
	 * int paletteLength = 0;
	 * int frameDataBeginPos = 0;
	 * FrameData* frame = nullptr;
	 * int frameCount = 0;
	 *
	 * APIResult result = LoadSPRMemory(
	 *     sprData,
	 *     sprDataLength,
	 *     &fileHead,
	 *     &palette,
	 *     &paletteLength,
	 *     &frameDataBeginPos,
	 *     &frame,
	 *     &frameCount
	 * );
	 *
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("SPR data loaded successfully.\n");
	 *     printf("Number of frames: %d\n", frameCount);
	 * } else {
	 *     printf("Error loading SPR data: %s\n", result.errorMessage);
	 * }
	 *
	 * // Giải phóng bộ nhớ sau khi sử dụng
	 * if (palette) MemoryManager::getInstance()->deallocate(palette);
	 * if (frame) MemoryManager::getInstance()->deallocate(frame);
	 */
	DLL_API APIResult __cdecl LoadSPRMemory(
		const uint8_t* data,
		size_t dataLength,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frame,
		int* frameCount
	);
	/**
	 * @brief Giải phóng bộ nhớ được cấp phát cho dữ liệu SPR.
	 *
	 * Hàm này giải phóng bộ nhớ đã được cấp phát cho bảng màu (`palette`) và dữ liệu frame (`frameData`) của tệp SPR.
	 * Sau khi gọi hàm, các con trỏ liên quan sẽ không còn hợp lệ và cần được đặt về `nullptr`.
	 *
	 * @param palette Con trỏ tới mảng `Color` (bảng màu) cần giải phóng.
	 *                Nếu `nullptr`, hàm sẽ bỏ qua mà không thực hiện hành động nào.
	 * @param frameData Con trỏ tới mảng `FrameData` cần giải phóng.
	 *                  Nếu `nullptr`, hàm sẽ bỏ qua mà không thực hiện hành động nào.
	 * @param frameCount Số lượng frame trong mảng `frameData`. Nếu bằng `0`, hàm sẽ không giải phóng các frame.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Hàm này chỉ giải phóng bộ nhớ đã được cấp phát trước đó bằng `LoadSPRFile` hoặc `LoadSPRMemory`.
	 * - Hàm đảm bảo rằng tất cả bộ nhớ liên quan được giải phóng đúng cách, bao gồm các tài nguyên nội bộ trong mỗi frame.
	 * - Sau khi gọi hàm này, các con trỏ (`palette`, `frameData`) nên được đặt về `nullptr` để tránh sử dụng lại.
	 *
	 * @example
	 * // Giải phóng dữ liệu SPR sau khi sử dụng
	 * if (palette || frameData) {
	 *     APIResult result = FreeSPRMemory(palette, frameData, frameCount);
	 *     if (result.errorCode == ErrorCode::Success) {
	 *         printf("SPR memory freed successfully.\n");
	 *     } else {
	 *         printf("Error freeing SPR memory: %s\n", result.errorMessage);
	 *     }
	 *     palette = nullptr;
	 *     frameData = nullptr;
	 * }
	 */
	DLL_API APIResult __cdecl  FreeSPRMemory(
		Color* palette,
		FrameData* frame, int frameCount);
	/**
	 * @brief Xuất dữ liệu thành tệp SPR từ thông tin đã cung cấp.
	 *
	 * Hàm này tạo một tệp SPR mới tại `filePath` từ thông tin header, bảng màu, và dữ liệu frame.
	 * Tất cả dữ liệu được mã hóa và lưu trữ trong định dạng SPR theo cấu trúc.
	 *
	 * @param filePath Đường dẫn đầy đủ tới tệp SPR sẽ được xuất.
	 * @param fileHead Thông tin header của tệp SPR (`SPRFileHead`).
	 * @param palette Mảng `Color` chứa bảng màu của tệp SPR.
	 * @param paletteSize Số lượng màu trong bảng màu.
	 * @param frame Mảng `FrameData` chứa dữ liệu frame của tệp SPR.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Hàm sẽ ghi tệp SPR ra đĩa tại vị trí `filePath`.
	 * - Nếu tệp đã tồn tại, nó sẽ bị ghi đè.
	 * - Tất cả dữ liệu frame sẽ được mã hóa trước khi ghi vào tệp.
	 * - Header, bảng màu, thông tin frame, và dữ liệu frame được sắp xếp tuần tự trong tệp.
	 * - Nếu không mở được tệp hoặc xảy ra lỗi trong quá trình mã hóa, hàm sẽ trả về mã lỗi.
	 *
	 * @example
	 * SPRFileHead fileHead = ...; // Khởi tạo header
	 * Color palette[] = { ... };  // Bảng màu
	 * int paletteSize = ...;      // Số lượng màu
	 * FrameData frame[] = { ... };// Dữ liệu frame
	 *
	 * APIResult result = ExportToSPRFile(
	 *     "output.spr",
	 *     fileHead,
	 *     palette,
	 *     paletteSize,
	 *     frame
	 * );
	 *
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("SPR file exported successfully.\n");
	 * } else {
	 *     printf("Error exporting SPR file: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl  ExportToSPRFile(const char* filePath,
		SPRFileHead fileHead,
		Color palette[],
		int paletteSize,
		FrameData frame[]);

	/**
	 * @brief Kiểm tra và xác nhận quyền của chứng chỉ (certificate).
	 *
	 * Hàm này kiểm tra tính hợp lệ của một chứng chỉ (`CertInfo`) bằng cách thực hiện
	 * kiểm tra chữ ký SHA256. Nếu chứng chỉ hợp lệ, hàm trả về `ErrorCode::Success`.
	 * Nếu không, trả về lỗi bảo mật (`SecurityError`).
	 *
	 * @param certinfo Thông tin chứng chỉ cần kiểm tra (`CertInfo`).
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: `ErrorCode::SecurityError` với thông báo "Cert is invalid!".
	 *
	 * @note
	 * - Hàm này là một trình bao bọc (wrapper) để gọi logic kiểm tra nội bộ `ForceCheckCertPermissionInternal`.
	 * - Kết quả kiểm tra dựa trên chữ ký SHA256 được tính từ dữ liệu của chứng chỉ.
	 * - Nếu chứng chỉ không hợp lệ, lỗi bảo mật sẽ được trả về.
	 *
	 * @example
	 * CertInfo certinfo = ...; // Cung cấp thông tin chứng chỉ
	 * APIResult result = ForceCheckCertPermission(certinfo);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Certificate is valid.\n");
	 * } else {
	 *     printf("Error checking certificate: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl 	ForceCheckCertPermission(CertInfo certinfo);
	/**
	 * @brief Lấy thông tin từ chứng chỉ được lưu trong tệp.
	 *
	 * Hàm này đọc và phân tích thông tin chứng chỉ từ tệp được chỉ định bởi `filePath`, sau đó
	 * lưu trữ các thông tin như tên chủ sở hữu, tên nhà phát hành, thời gian hợp lệ, thumbprint,
	 * và số sê-ri vào cấu trúc `CertInfo`.
	 *
	 * @param filePath Đường dẫn đầy đủ tới tệp chứng chỉ.
	 * @param certInfo Con trỏ tới cấu trúc `CertInfo` để lưu trữ thông tin chứng chỉ.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi:
	 *             - `ErrorCode::InvalidArgument`: Nếu `filePath` hoặc `certInfo` là `nullptr`.
	 *             - `ErrorCode::InternalError`: Nếu không tìm thấy chứng chỉ, hoặc xảy ra lỗi trong quá trình xử lý.
	 *
	 * @note
	 * - Tệp chứng chỉ phải ở định dạng PKCS7 hoặc được nhúng trong tệp nhị phân.
	 * - Bộ nhớ được cấp phát cho các chuỗi trong `CertInfo` phải được giải phóng bởi người gọi để tránh rò rỉ bộ nhớ.
	 * - Nếu xảy ra lỗi, các trường trong `CertInfo` có thể không được khởi tạo hoặc chứa giá trị không xác định.
	 *
	 * @example
	 * CertInfo certInfo;
	 * APIResult result = GetCertificateInfo("path/to/certificate.pem", &certInfo);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Certificate Info:\n");
	 *     printf("Subject: %s\n", certInfo.Subject);
	 *     printf("Issuer: %s\n", certInfo.Issuer);
	 *     printf("Valid From: %lld\n", certInfo.ValidFrom);
	 *     printf("Valid To: %lld\n", certInfo.ValidTo);
	 *     printf("Thumbprint: %s\n", certInfo.Thumbprint);
	 *     printf("Serial Number: %s\n", certInfo.SerialNumber);
	 *
	 *     // Giải phóng bộ nhớ được cấp phát
	 *     free(certInfo.Subject);
	 *     free(certInfo.Issuer);
	 *     free(certInfo.Thumbprint);
	 *     free(certInfo.SerialNumber);
	 * } else {
	 *     printf("Error: %s\n", result.errorMessage);
	 * }
	 */
	 //DLL_API APIResult	GetCertificateInfo2(const char* filePath, CertInfo* certInfo);
	DLL_API int		GetCertificateInfo(const char* filePath, CertInfo* certInfo);

	/**
	 * @brief Giải phóng bộ nhớ được cấp phát cho cấu trúc `CertInfo`.
	 *
	 * Hàm này giải phóng bộ nhớ đã được cấp phát động cho các trường trong cấu trúc `CertInfo`,
	 * bao gồm `Subject`, `Issuer`, `Thumbprint`, và `SerialNumber`. Sau khi gọi hàm, các trường
	 * này sẽ được đặt về `nullptr` để tránh sử dụng lại.
	 *
	 * @param certInfo Con trỏ tới cấu trúc `CertInfo` cần giải phóng.
	 *                 - Nếu `certInfo` là `nullptr`, hàm sẽ bỏ qua mà không thực hiện hành động nào.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Hàm không trả về lỗi vì việc giải phóng luôn an toàn (ngay cả với con trỏ null).
	 *
	 * @note
	 * - Hàm này chỉ giải phóng bộ nhớ cho các trường của `CertInfo`, không giải phóng bản thân `CertInfo`.
	 * - Người gọi cần đảm bảo rằng bộ nhớ cho `CertInfo` đã được khởi tạo trước khi gọi hàm này.
	 * - Sau khi gọi hàm, các trường trong `CertInfo` sẽ được đặt về `nullptr`.
	 *
	 * @example
	 * CertInfo certInfo = ...; // Đã được cấp phát và khởi tạo
	 * APIResult result = FreeCertInfo(&certInfo);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Certificate info memory freed successfully.\n");
	 * } else {
	 *     printf("Error freeing certificate info memory: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl 	FreeCertInfo(CertInfo* certInfo);

	/**
	 * @brief Nén một thư mục thành tệp .pak.
	 *
	 * Hàm này thực hiện việc nén tất cả các tệp trong thư mục được chỉ định bởi `inputFolderPath`
	 * thành một tệp .pak được lưu tại `outputFolderPath`. Tiến trình nén được thông báo qua
	 * callback `progressCallback`.
	 *
	 * @param inputFolderPath Đường dẫn đầy đủ tới thư mục đầu vào cần nén.
	 * @param outputFolderPath Đường dẫn đầy đủ tới thư mục đích để lưu tệp .pak.
	 * @param bExcludeOfCheckId Cờ boolean chỉ định có loại trừ kiểm tra ID hay không.
	 *                          - `true`: Loại trừ kiểm tra ID.
	 *                          - `false`: Bao gồm kiểm tra ID.
	 * @param progressCallback Hàm callback để báo cáo tiến trình nén.
	 *                         Callback nhận hai tham số:
	 *                         - `progress`: Tiến trình hiện tại (0-100).
	 *                         - `message`: Thông điệp trạng thái.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Thư mục đầu vào (`inputFolderPath`) phải tồn tại và có quyền đọc.
	 * - Thư mục đầu ra (`outputFolderPath`) phải tồn tại hoặc có thể tạo mới.
	 * - Tiến trình được báo cáo theo phần trăm hoàn thành, từ 0% đến 100%.
	 *
	 * @example
	 * const char* inputFolderPath = "path/to/folder";
	 * const char* outputFolderPath = "path/to/output";
	 * bool excludeCheckId = true;
	 * APIResult result = CompressFolderToPakFile(
	 *     inputFolderPath,
	 *     outputFolderPath,
	 *     excludeCheckId,
	 *     [](int progress, const char* message) {
	 *         printf("Progress: %d%% - %s\n", progress, message);
	 *     });
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Folder compressed successfully.\n");
	 * } else {
	 *     printf("Error compressing folder: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl 	CompressFolderToPakFile(const char* folderPath,
		const char* outputPath,
		bool bExcludeOfCheckId,
		ProgressCallback progressCallback);
	/**
	 * @brief Tải tệp .pak vào PakWorkManager và trả về thông tin phiên làm việc.
	 *
	 * Hàm này thực hiện việc tải tệp .pak vào PakWorkManager, đồng thời trả về thông tin của tệp .pak
	 * thông qua tham số `pakInfo` và chuỗi token phiên làm việc thông qua `sessionToken`.
	 * Hàm hỗ trợ callback để thông báo tiến trình tải.
	 *
	 * @param filePath Đường dẫn đầy đủ tới tệp .pak cần tải.
	 * @param pakInfo Con trỏ tới một cấu trúc `PakInfo` nơi thông tin về tệp .pak sẽ được lưu trữ.
	 * @param progressCallback Hàm callback được gọi để thông báo tiến trình tải.
	 *                         Hàm callback nhận hai tham số:
	 *                         - `progress`: Phần trăm tiến trình (0-100).
	 *                         - `message`: Thông điệp mô tả trạng thái hiện tại.
	 * @param sessionToken Con trỏ kép để lưu trữ chuỗi token phiên làm việc.
	 *                     Chuỗi này được cấp phát động trong bộ nhớ và cần được giải phóng sau khi sử dụng.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success` và chuỗi token hợp lệ trong `sessionToken`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Chuỗi `sessionToken` được cấp phát động trong bộ nhớ. Người dùng phải đảm bảo giải phóng
	 *   bộ nhớ này sau khi sử dụng để tránh rò rỉ bộ nhớ.
	 * - Nếu tệp không thể tải hoặc gặp lỗi nội bộ, `sessionToken` sẽ là `nullptr`.
	 *
	 * @example
	 * char* sessionToken = nullptr;
	 * PakInfo pakInfo;
	 * APIResult result = LoadPakFileToWorkManager("path/to/file.pak", &pakInfo, ProgressCallback, &sessionToken);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Session Token: %s\n", sessionToken);
	 *     // Giải phóng bộ nhớ của sessionToken khi không còn sử dụng
	 *     MemoryManager::getInstance()->deallocate(sessionToken);
	 * } else {
	 *     printf("Error: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API const APIResult __cdecl  LoadPakFileToWorkManager(const char* filePath,
		PakInfo* pakInfo,
		ProgressCallback progressCallback,
		char** sessionToken);
	/**
	 * @brief Giải phóng bộ nhớ được cấp phát cho một cấu trúc `PakInfo`.
	 *
	 * Hàm này được sử dụng để giải phóng bộ nhớ của cấu trúc `PakInfo`, bao gồm các tài nguyên
	 * hoặc con trỏ bên trong mà cấu trúc này sử dụng. Sau khi gọi hàm này, con trỏ `pakInfo`
	 * không còn hợp lệ và không được sử dụng nữa.
	 *
	 * @param pakInfo Con trỏ tới cấu trúc `PakInfo` cần được giải phóng.
	 *                Nếu `pakInfo` là `nullptr`, hàm không thực hiện bất kỳ hành động nào.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Hàm này chỉ nên được gọi sau khi cấu trúc `PakInfo` đã hoàn thành nhiệm vụ và không còn sử dụng.
	 * - Nếu `pakInfo` là `nullptr`, hàm sẽ không trả về lỗi mà chỉ kết thúc mà không thực hiện hành động nào.
	 *
	 * @example
	 * PakInfo* pakInfo = new PakInfo();
	 * // ... sử dụng pakInfo ...
	 * APIResult result = FreePakInfo(pakInfo);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("PakInfo memory successfully freed.\n");
	 * } else {
	 *     printf("Error freeing PakInfo: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl  FreePakInfo(PakInfo* pakInfo);
	/**
	 * @brief Phân tích và trích xuất thông tin từ tệp tin pak info.
	 *
	 * Hàm này đọc và phân tích một tệp tin pak info được chỉ định bởi `pakInfoPath`, sau đó
	 * lưu trữ thông tin đã phân tích vào cấu trúc `PakInfo`. Nếu xảy ra lỗi trong quá trình
	 * phân tích, hàm sẽ trả về mã lỗi phù hợp và thông báo lỗi chi tiết.
	 *
	 * @param pakInfoPath Đường dẫn đầy đủ tới tệp tin pak info cần phân tích.
	 * @param pakInfo Con trỏ tới cấu trúc `PakInfo` nơi thông tin phân tích sẽ được lưu trữ.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Đường dẫn `pakInfoPath` phải chỉ định đúng tệp tin pak info có định dạng hợp lệ.
	 * - Cấu trúc `PakInfo` phải được khởi tạo trước khi gọi hàm này.
	 * - Nếu tệp tin pak info không thể phân tích hoặc không hợp lệ, hàm sẽ trả về mã lỗi `ErrorCode::InternalError`.
	 *
	 * @example
	 * PakInfo pakInfo;
	 * const char* pakInfoPath = "path/to/pakInfo.json";
	 * APIResult result = ParsePakInfoFile(pakInfoPath, &pakInfo);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Pak info parsed successfully.\n");
	 *     // Sử dụng thông tin trong pakInfo
	 * } else {
	 *     printf("Error parsing pak info: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl ParsePakInfoFile(const char* pakInfoPath, PakInfo* pakInfo);
	/**
	 * @brief Trích xuất nội dung của tệp .pak và lưu trữ tại thư mục đích.
	 *
	 * Hàm này thực hiện trích xuất nội dung của tệp .pak được chỉ định bởi `pakFilePath`. Nếu
	 * `pakInfoFilePath` được cung cấp, nó sẽ sử dụng thông tin từ tệp pak info để hỗ trợ trích xuất.
	 * Tiến trình trích xuất sẽ được báo cáo thông qua callback `progressCallback`.
	 *
	 * @param pakFilePath Đường dẫn đầy đủ tới tệp .pak cần trích xuất.
	 * @param pakInfoFilePath Đường dẫn tới tệp tin pak info (nếu có).
	 *                        Nếu `nullptr`, hàm sẽ thực hiện trích xuất mà không cần pak info.
	 * @param outputRootPath Đường dẫn tới thư mục gốc nơi nội dung trích xuất sẽ được lưu trữ.
	 * @param progressCallback Hàm callback để báo cáo tiến trình trích xuất.
	 *                         Callback nhận hai tham số:
	 *                         - `progress`: Tiến trình hiện tại (0-100).
	 *                         - `message`: Thông điệp trạng thái.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi: Một mã lỗi phù hợp trong `ErrorCode` và thông báo lỗi chi tiết.
	 *
	 * @note
	 * - Nếu `pakInfoFilePath` được cung cấp, hàm sẽ phân tích tệp pak info trước khi trích xuất.
	 * - Tiến trình trích xuất (progress) được cập nhật liên tục từ 0% đến 100% qua `progressCallback`.
	 * - Đảm bảo rằng thư mục `outputRootPath` có quyền ghi để lưu trữ nội dung trích xuất.
	 *
	 * @example
	 * const char* pakFilePath = "path/to/file.pak";
	 * const char* pakInfoFilePath = "path/to/file.pakinfo";
	 * const char* outputRootPath = "output/directory";
	 * APIResult result = ExtractPakFile(
	 *     pakFilePath,
	 *     pakInfoFilePath,
	 *     outputRootPath,
	 *     [](int progress, const char* message) {
	 *         printf("Progress: %d%% - %s\n", progress, message);
	 *     });
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Pak file extracted successfully.\n");
	 * } else {
	 *     printf("Error extracting pak file: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl  ExtractPakFile(const char* pakFilePath,
		const char* pakInfoPath,
		const char* outputRootPath,
		ProgressCallback progressCallback);

	/**
	 * @brief Đóng một phiên làm việc của tệp `.pak` dựa trên mã phiên (`sessionString`).
	 *
	 * Hàm này thực hiện việc đóng một phiên làm việc của tệp `.pak` được quản lý bởi `PakWorkManager`.
	 * Nếu phiên làm việc không tồn tại hoặc mã phiên không hợp lệ, hàm sẽ trả về lỗi.
	 * Các tài nguyên liên quan đến phiên sẽ được giải phóng, bao gồm việc xóa các tệp tạm.
	 *
	 * @param sessionString Chuỗi token đại diện cho phiên làm việc cần đóng.
	 *
	 * @return APIResult Kết quả của thao tác:
	 *         - Nếu thành công: `ErrorCode::Success`.
	 *         - Nếu lỗi:
	 *             - `ErrorCode::InternalError`: Nếu mã phiên không hợp lệ hoặc xảy ra lỗi trong quá trình đóng.
	 *
	 * @note
	 * - Hàm này yêu cầu mã phiên (`sessionString`) phải hợp lệ và tồn tại trong `PakWorkManager`.
	 * - Các tệp tạm liên quan đến phiên sẽ được giải phóng và xóa khỏi hệ thống.
	 * - Nếu tệp tạm không thể xóa, lỗi sẽ được ghi log nhưng vẫn trả về thành công.
	 *
	 * @example
	 * const char* sessionString = "some_session_token";
	 * APIResult result = ClosePakFileSession(sessionString);
	 * if (result.errorCode == ErrorCode::Success) {
	 *     printf("Session closed successfully.\n");
	 * } else {
	 *     printf("Error closing session: %s\n", result.errorMessage);
	 * }
	 */
	DLL_API APIResult __cdecl ClosePakFileSession(const char* sessionString);
	DLL_API APIResult __cdecl ExtractBlockFromPakFile(const char* sessionString, int subFileIndex, const char* outputPath);
	DLL_API APIResult __cdecl FreeBuffer(void* buffer);
	DLL_API APIResult __cdecl ReadBlockFromPakFile(const char* sessionToken,
		int subFileIndex,
		size_t* subFileSize,
		char** blockData);
	DLL_API APIResult  __cdecl GetBlockIdFromPath(const char* blockPath, unsigned int* blockId);

	DLL_API APIResult	__cdecl Initialize();
}