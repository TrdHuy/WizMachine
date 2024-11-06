using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Utils;

namespace WizMachine.Services.Base
{
    public interface IPakWorkManager
    {
        /// <summary>
        /// Phân tích thông tin từ file metadata của tệp PAK và trả về đối tượng <see cref="PakInfo"/>.
        /// </summary>
        /// <param name="pakInfoPath">
        /// Đường dẫn tới tệp thông tin metadata của tệp PAK.
        /// Tệp này chứa các thông tin tổng quan và chi tiết về các file bên trong tệp PAK.
        /// </param>
        /// <returns>
        /// Trả về một đối tượng <see cref="PakInfo"/> chứa các thông tin:
        /// <list type="bullet">
        /// <item>
        /// <term>TotalFiles</term>
        /// <description>Tổng số file được lưu trong tệp PAK.</description>
        /// </item>
        /// <item>
        /// <term>PakTime</term>
        /// <description>Thời gian tạo tệp PAK.</description>
        /// </item>
        /// <item>
        /// <term>PakTimeSave</term>
        /// <description>Thời gian lưu tệp PAK.</description>
        /// </item>
        /// <item>
        /// <term>CRC</term>
        /// <description>Giá trị kiểm tra CRC để xác minh tính toàn vẹn của tệp PAK.</description>
        /// </item>
        /// <item>
        /// <term>FileList</term>
        /// <description>Danh sách thông tin chi tiết của từng file bên trong tệp PAK, bao gồm:
        /// <list type="bullet">
        /// <item>Index: Số thứ tự của file trong PAK.</item>
        /// <item>ID: Mã định danh file.</item>
        /// <item>Time: Thời gian chỉnh sửa cuối.</item>
        /// <item>FileName: Tên file.</item>
        /// <item>Size: Kích thước file gốc.</item>
        /// <item>InPakSize: Kích thước file nén.</item>
        /// <item>ComprFlag: Cờ nén, xác định file có được nén hay không.</item>
        /// <item>CRC: Giá trị CRC của file.</item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu không tìm thấy tệp metadata ở đường dẫn <paramref name="pakInfoPath"/>.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Ném ra nếu dữ liệu trong tệp metadata không đúng định dạng hoặc không hợp lệ.
        /// </exception>
        /// <remarks>
        /// Hàm này yêu cầu tệp metadata phải tuân thủ định dạng cụ thể:
        /// <list type="number">
        /// <item>Dòng đầu chứa các thông tin tổng quan: TotalFiles, PakTime, PakTimeSave, CRC.</item>
        /// <item>Dòng tiếp theo là tiêu đề cột: Index, ID, Time, FileName, Size, InPakSize, ComprFlag, CRC.</item>
        /// <item>Các dòng tiếp theo chứa thông tin chi tiết từng file, được phân tách bằng tab (\t).</item>
        /// </list>
        /// </remarks>

        public PakInfo ParsePakInfoFile(string pakInfoPath);

        /// <summary>
        /// Trích xuất các block (file nén) bên trong tệp PAK và lưu chúng vào một thư mục cụ thể.
        /// </summary>
        /// <param name="pakFilePath">
        /// Đường dẫn đến tệp PAK cần trích xuất.
        /// </param>
        /// <param name="pakInfoPath">
        /// Đường dẫn đến tệp thông tin metadata của tệp PAK (tùy chọn).
        /// Nếu tệp này được cung cấp, nó sẽ được sử dụng để ánh xạ và đặt tên chính xác cho các file được trích xuất.
        /// </param>
        /// <param name="outputRootPath">
        /// Đường dẫn thư mục gốc nơi các file được trích xuất sẽ được lưu.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu việc trích xuất thành công, <c>false</c> nếu xảy ra lỗi.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Hàm này có hai phiên bản:
        /// <list type="bullet">
        /// <item>
        /// Nếu cung cấp <paramref name="pakInfoPath"/>, hàm sẽ sử dụng tệp metadata để ánh xạ thông tin block.
        /// </item>
        /// <item>
        /// Nếu không cung cấp <paramref name="pakInfoPath"/>, các block sẽ được lưu với tên mặc định.
        /// </item>
        /// </list>
        /// </para>
        /// <para>
        /// Trong quá trình trích xuất:
        /// <list type="number">
        /// <item>
        /// Tiến trình sẽ được cập nhật và có thể hiển thị qua giao diện người dùng hoặc log hệ thống.</item>
        /// <item>
        /// Các file được lưu trong thư mục chỉ định, và nếu block tương ứng là tệp SPR, tên file sẽ được ánh xạ.
        /// </item>
        /// <item>
        /// Nếu không tìm thấy ánh xạ trong metadata, file sẽ được lưu với tên mặc định dạng <c>"extracted_block_[index].bin"</c>.
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu không tìm thấy tệp PAK hoặc tệp metadata.
        /// </exception>
        /// <exception cref="IOException">
        /// Ném ra nếu xảy ra lỗi khi đọc tệp PAK hoặc ghi file trích xuất.
        /// </exception>
        public bool ExtractPakFile(string pakFilePath,
            string pakInfoPath,
            string outputRootPath);


        /// <summary>
        /// Trích xuất các block (file nén) bên trong tệp PAK mà không sử dụng tệp metadata.
        /// </summary>
        /// <param name="pakFilePath">
        /// Đường dẫn đến tệp PAK cần trích xuất.
        /// </param>
        /// <param name="outputRootPath">
        /// Đường dẫn thư mục gốc nơi các file được trích xuất sẽ được lưu.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu việc trích xuất thành công, <c>false</c> nếu xảy ra lỗi.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Phiên bản này sẽ lưu các block với tên mặc định nếu không cung cấp thông tin metadata.
        /// Các file sẽ được đặt tên dưới dạng <c>"extracted_block_[index].bin"</c>.
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu không tìm thấy tệp PAK.
        /// </exception>
        /// <exception cref="IOException">
        /// Ném ra nếu xảy ra lỗi khi đọc tệp PAK hoặc ghi file trích xuất.
        /// </exception>
        public bool ExtractPakFile(string pakFilePath,
            string outputRootPath);

        /// <summary>
        /// Nén một thư mục thành tệp PAK, lưu trữ tất cả các tệp trong thư mục và các thư mục con.
        /// </summary>
        /// <param name="pakFilePath">
        /// Đường dẫn đến thư mục cần nén.
        /// </param>
        /// <param name="outputRootPath">
        /// Đường dẫn thư mục đích để lưu tệp PAK.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu việc nén thành công, <c>false</c> nếu xảy ra lỗi.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Hàm này sử dụng cơ chế callback để báo cáo tiến trình nén thư mục. 
        /// </para>
        /// <para>
        /// Trong quá trình nén:
        /// <list type="number">
        /// <item>
        /// Tất cả các tệp trong thư mục đầu vào (bao gồm các tệp trong thư mục con) sẽ được thêm vào tệp PAK.
        /// </item>
        /// <item>
        /// Tên file đầu ra được tạo tự động dựa trên tên thư mục đầu vào với phần mở rộng <c>.pak</c>.
        /// </item>
        /// <item>
        /// Tiến trình sẽ được cập nhật qua callback và có thể hiển thị thông qua giao diện người dùng.
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu thư mục đầu vào không tồn tại.
        /// </exception>
        /// <exception cref="IOException">
        /// Ném ra nếu xảy ra lỗi trong quá trình tạo tệp PAK.
        /// </exception>
        public bool CompressFolderToPakFile(string pakFilePath,
           string outputRootPath);


        #region WorkManager
        /// <summary>
        /// Tải tệp PAK vào hệ thống quản lý công việc (Work Manager).
        /// </summary>
        /// <param name="pakFilePath">
        /// Đường dẫn tới tệp PAK cần tải.
        /// </param>
        /// <param name="progressChangedCallback">
        /// Callback để theo dõi tiến trình tải tệp PAK (tuỳ chọn).
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu tải thành công, <c>false</c> nếu xảy ra lỗi.
        /// </returns>
        /// <remarks>
        /// Hàm này sử dụng hệ thống quản lý PAK để lưu trữ và xử lý các tệp PAK.
        /// Nếu callback được truyền vào, tiến trình sẽ được cập nhật.
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu tệp PAK không tồn tại.
        /// </exception>
        /// <exception cref="IOException">
        /// Ném ra nếu xảy ra lỗi khi tải tệp PAK.
        /// </exception>
        public bool LoadPakFileToWorkManager(string pakFilePath, ProgressChangedCallback? progressChangedCallback = null);

        /// <summary>
        /// Đặt lại hệ thống quản lý công việc (Work Manager), xoá tất cả thông tin liên quan đến các tệp PAK.
        /// </summary>
        /// <remarks>
        /// Sau khi gọi hàm này, tất cả các tệp PAK đã tải trước đó sẽ bị loại bỏ.
        /// </remarks>
        public void ResetPakWorkManager();

        /// <summary>
        /// Loại bỏ một tệp PAK khỏi hệ thống quản lý công việc (Work Manager).
        /// </summary>
        /// <param name="pakFilePath">
        /// Đường dẫn tới tệp PAK cần loại bỏ.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu loại bỏ thành công, <c>false</c> nếu xảy ra lỗi hoặc tệp không tồn tại trong Work Manager.
        /// </returns>
        /// <remarks>
        /// Chức năng này hữu ích khi người dùng muốn loại bỏ một tệp PAK cụ thể khỏi danh sách quản lý.
        /// </remarks>
        public bool RemovePakFileFromWorkManager(string pakFilePath);

        /// <summary>
        /// Kiểm tra xem block có tồn tại trong tệp PAK dựa trên đường dẫn hay không.
        /// </summary>
        /// <param name="blockPath">
        /// Đường dẫn tới block cần kiểm tra.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu block tồn tại, <c>false</c> nếu không.
        /// </returns>
        /// <remarks>
        /// Block là một đơn vị dữ liệu được lưu trữ bên trong tệp PAK.
        /// </remarks>
        public bool IsBlockExistByPath(string blockPath);

        /// <summary>
        /// Trích xuất một block cụ thể trong tệp PAK ra đường dẫn đích.
        /// </summary>
        /// <param name="blockPath">
        /// Đường dẫn tới block cần trích xuất.
        /// </param>
        /// <param name="outputPath">
        /// Đường dẫn nơi lưu trữ block sau khi trích xuất.
        /// </param>
        /// <returns>
        /// Trả về <c>true</c> nếu trích xuất thành công, <c>false</c> nếu xảy ra lỗi.
        /// </returns>
        /// <remarks>
        /// Chức năng này cho phép người dùng lấy dữ liệu từ một block cụ thể mà không cần giải nén toàn bộ tệp PAK.
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Ném ra nếu block không tồn tại trong tệp PAK.
        /// </exception>
        /// <exception cref="IOException">
        /// Ném ra nếu xảy ra lỗi khi trích xuất block.
        /// </exception>
        public bool ExtractBlockByPath(string blockPath, string outputPath);
        #endregion
    }
}
