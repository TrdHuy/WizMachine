# CẤU TRÚC FILE PAK, SPR TRONG JX

File pack của king soft là file nén chứa resources của game như file txt, ini, dat, jpg, bmp, spr .v.v.v(mình sẽ dùng tên gọi chung là res).
Cấu trúc file pak gồm 3 phần:

![image](https://github.com/TrdHuy/kiemthewiki/assets/32306489/610bdfd3-877d-4f9f-8f3d-75d6fe320cb7)

* **Header:** chứa thông tin chung cả file pak(Số lượng res trong file pak, vị trí truy cập của Block Offsets, .v.v.v).
* **Block Datas:** chứa dữ liệu của từng file(Mỗi res được đưa vào file Pak dưới dạng block và được quản lý thông qua id, và id này được hash theo path của res vì vậy các tool unpack không thể tách được tên file, mà chỉ đưa ra dạng chung chung là các số thứ tự).
* **Block Offsets:** chứa thông tin bộ nhớ của từng block(vị trí truy cập trên file pak, độ dài của block tính bằng byte).

**Chi tiết trực quan:**
Phần **Header** chứa 32 byte

<table class="table table-bordered">
    <thead class="thead-light">
      <tr>
        <th>Code</th>
        <th>Chi tiết</th>
      </tr>
    </thead>
      <tr>
        <td>
          <code class="highlighter-rouge">
    struct PACKHEDER<br />
    {<br />
         public byte[] Signature;<br />
         public uint Count;<br />
         public uint Index;<br />
         public uint Data;<br />
         public uint CRC32;<br />
         public byte[] Reserved;<br />
    }
         </code>
        </td>
        <td>
          <ul>
            <li><strong>Signature: </strong>chứa 4 byte nhận dạng file pak có giá trị "PAK "</li>
            <li><strong>Count: </strong> số lượng res được nén trong file pak</li>
            <li><strong>Index: </strong> Vị trí offset của BlockHeader đầu tiên</li>
          </ul>
        </td>
      </tr>
  </table>

Phần **Block header** chứa 16 byte(mỗi block) * tổng số block (tổng số file)

  <table class="table table-bordered">
    <thead class="thead-light">
      <tr>
        <th>Code</th>
        <th>Chi tiết</th>
      </tr>
    </thead>
    <tr>
      <td>
        <code class="highlighter-rouge">
    struct BLOCKHEADER <br />
    { <br />
        unsigned long ID;<br />
        unsigned long Offset;<br />
        unsigned long RealLength;<br />
        BYTE Length[3];<br />
        unsigned char Method;<br />
        inline void operator = (BLOCKHEADER a) <br />
        { <br />
          ID = a.ID;<br />
          Offset = a.Offset; <br />
          RealLength = a.RealLength; <br />
          Length[0] = a.Length[0]; <br />
          Length[1] = a.Length[1]; <br />
          Length[2] = a.Length[2]; <br />
          Method = a.Method;
        } <br />
    };
         </code>
      </td>
      <td>
        <ul>
          <li><strong>ID: </strong>là định danh của file được hash bằng path <br />
            của res (vd: /spr/ui/ui3/series/0.spr -> 127598303, cái này mình ví <br />
            dụ thôi chứ không chính xác) toàn bộ file pak quản lý thông qua ID<br />
            này, chính là lý do cần phải có path/tên mới unpak được res, và không<br />
            thể hash ngược lại thành path/tên nhé = . =  <a href="ID%20của%201%20file%20trong%20tệp%20PAK.md">Chi tiết về ID ở đây</a></li>
          <li><strong>Length: </strong> Kích thước sau khi nén cũng chính là kích<br />
            thước của block nằm trên phần Block Datas, vậy ở đây ta có thể tính<br />
            được độ lớn tối đa của một res trong file pak là <br />
            2^24 = 16777216 byte = 16 mbyte</li>
          <li><strong>Method: </strong> quy định phương thức nén của từng block<br />
            ở đây mình check được một số phương thức như sau
               <ul>
                  <li><strong>Method = 0: </strong> Không nén dữ liệu chỉ đọc và rãi dữ liệu lên block.</li>
                  <li><strong>Method = 1: </strong> Nén toàn bộ block bằng ucl(ucl là thư viện nén dữ liệu <br />mà kingsoft sử dụng).</li>
                   <li><strong>Method = 16: </strong>Hôm qua mới detect được, chưa xem kỹ dữ liệu giãi mã có <br />đúng không nhưng dùng ucl decompress ra thì được đầu ra khớp số liệu trên BlockHeader</li>
                    <li><strong>Method = 32: </strong>tương tự nhưng dạng 1. thường sử dụng để nén cả file spr<br />(file pak mới của vina hay dùng mã này).</li>
                    <li><strong>Method = 17: </strong>thường dùng nén frame file spr.(chi tiết cách nén frame mình sẽ <br />đề cập ở phần tiếp theo).</li>
              </ul>
          </li>
          <li><strong>Offset: </strong> là vị trí bắt đầu dữ liệu của block nằm trên phần Block Datas.</li>
          <li><strong>RealLength: </strong> là kích thước trước khi nén(mọi đơn vị mình sẽ tính bằng byte hết).</li>
        </ul>
      </td>
    </tr>
  </table>

---
```C++
//Init File Mem Poiter
PBYTE oTemp = m_OffsetBuffer;
//Jump to Block Header Info Offset
oTemp += block * 16;
//Get Block Info from Mem
BLOCKHEADER* Header = (BLOCKHEADER*)oTemp;
if (Header->ID <= 0 || Header->Length <= 0 || Header->Offset <= 0) {
    m_ExtractBuffer = new BYTE[1];
    return 0;
}
//Caculate Compress Size of The Block
int size = GetCompressSize(Header);
//Init Block data Buffer
m_BlockBuffer = new BYTE[size];
//Seek File Mem To The Block Offset
m_File.Seek(Header->Offset, 0);
//Read Block Mem To Buffer
m_File.Read(m_BlockBuffer, size);

//Method = 1: Compress Image, ini, txt, ...etc
if (Header->Method == 0) {
    //Get UnCompress Length
    size = Header->RealLength;
    //Return Mem Handle
    *data = (DWORD*)m_BlockBuffer;
}
else if (Header->Method == 1) {
    //Get UnCompress Length
    size = Header->RealLength;
    unsigned int dest_length = Header->RealLength;
    //Init Buffer
    m_ExtractBuffer = new BYTE[Header->RealLength];
    unsigned int CompressSize = GetCompressSize(Header);
    //Call Decompress Source Buffer
    ucl_nrv2b_decompress_8(m_BlockBuffer, CompressSize, m_ExtractBuffer, &dest_length, 0);
    //Release Buffer
    delete m_BlockBuffer;
    //Return Mem Handle
    *data = (DWORD*)m_ExtractBuffer;
}
else if (Header->Method == 32) { // Phương thức nén mới(pak mới của vina), thực chất chỉ đổi id method, cách nén thì như Method 1
    //Get UnCompress Length
    size = Header->RealLength;
    unsigned int dest_length = 0;
    //Init Buffer
    m_ExtractBuffer = new BYTE[Header->RealLength];
    unsigned int CompressSize = GetCompressSize(Header);
    //Call Decompress Source Buffer
    ucl_nrv2b_decompress_8(m_BlockBuffer, CompressSize, m_ExtractBuffer, &dest_length, 0);
    //Release Buffer
    delete m_BlockBuffer;
    //Return Mem Handle
    *data = (DWORD*)m_ExtractBuffer;
}
//Method = 17: Compress Spr Image
else {
    // Update sau phần 2;
}

return size;
```
Nguồn: [clbgame](https://www.clbgamesvn.com/diendan/showthread.php?t=314825), [dungrino](https://github.com/rinodung/jxdocs/issues/127#issue-1556238730)
[tool view spr](https://github.com/mapic91/MpcAsfTool)

