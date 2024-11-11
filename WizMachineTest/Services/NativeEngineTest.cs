using System.Reflection;
using System.Text;
using WizMachine.Data;
using WizMachine.Services.Utils;
using WizMachine;
using WizMachineTest.Utils;
using static FileLockManager.TestAsset;


namespace WizMachineTest.Services
{
    [TestFixture]

    internal class NativeEngineTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void test_CertUtil()
        {
            EngineKeeper.ForceCheckCallingSignature();
        }

        [Test]
        public void test_ExtractPakFile()
        {
            FileLockManager.AcquireFiles(InputDaPak, InputDaPakTxt, BinFile1);

            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";

            string outputRootPath = exeDirectory + "\\test_ExtractPakFile_output";
            Assert.IsTrue(NativeAPIAdapter.ExtractPakFile(InputDaPak.AssetPath,
                InputDaPakTxt.AssetPath,
                outputRootPath: outputRootPath));
            // Kiểm tra nội dung của file spr sau khi extract từ pak
            {
                byte[] fileData = File.ReadAllBytes($"{outputRootPath}\\data\\12345.spr");
                var initResult = NativeAPIAdapter.LoadSPRFromMemory(fileData, out SprFileHead sprFileHead,
                    out Palette palette,
                    out int frameDataBeginPos,
                    out FrameRGBA[] frameData);
                Assert.That(initResult);
                Assert.That(sprFileHead.GlobalHeight * sprFileHead.GlobalWidth == 90000);
                var frameData1Byte = frameData[0].originDecodedBGRAData;
                var frameData1FromFile = TestUtil.ReadBytesFromFile(BinFile1.AssetPath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }


            // Kiểm tra nội dung file test.txt
            string testTxtPath = Path.Combine(outputRootPath, "data", "test.txt");
            string testTxtContent = File.ReadAllText(testTxtPath);
            Assert.That(testTxtContent.Trim(), Is.EqualTo("12345"), "Nội dung file test.txt không đúng.");

            // Kiểm tra nội dung file test.xml
            string testXmlPath = Path.Combine(outputRootPath, "data", "test.xml");
            string testXmlContent = File.ReadAllText(testXmlPath);
            Assert.That(testXmlContent.Trim(), Is.EqualTo("<config></config>"), "Nội dung file test.xml không đúng.");
            FileLockManager.ReleaseFiles(InputDaPak, InputDaPakTxt, BinFile1);
        }

        [Test]
        public void test_ExtractPakFile_WithoutPakInfo()
        {
            FileLockManager.AcquireFiles(InputDaPak);

            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";
            string outputRootPath = Path.Combine(exeDirectory, "test_ExtractPakFile_WithoutPakInfo_output");

            // Thực hiện extract
            Assert.IsTrue(NativeAPIAdapter.ExtractPakFile(InputDaPak.AssetPath, outputRootPath: outputRootPath));

            // Kiểm tra số lượng file trong thư mục
            string[] extractedFiles = Directory.GetFiles(outputRootPath);
            Assert.AreEqual(3, extractedFiles.Length, "Tổng số file không đúng!");

            // Kiểm tra tên các file
            Assert.IsTrue(extractedFiles.Any(f => Path.GetFileName(f) == "extracted_block_0.bin"), "File extracted_block_0.bin không tồn tại!");
            Assert.IsTrue(extractedFiles.Any(f => Path.GetFileName(f) == "extracted_block_1.bin"), "File extracted_block_1.bin không tồn tại!");
            Assert.IsTrue(extractedFiles.Any(f => Path.GetFileName(f) == "extracted_block_2.spr"), "File extracted_block_2.spr không tồn tại!");

            // Kiểm tra nội dung của extracted_block_0.bin
            string contentBlock0 = File.ReadAllText(Path.Combine(outputRootPath, "extracted_block_0.bin"));
            Assert.AreEqual("<config></config>", contentBlock0, "Nội dung của extracted_block_0.bin không đúng!");

            // Kiểm tra nội dung của extracted_block_1.bin
            string contentBlock1 = File.ReadAllText(Path.Combine(outputRootPath, "extracted_block_1.bin"));
            Assert.AreEqual("12345", contentBlock1, "Nội dung của extracted_block_1.bin không đúng!");
            FileLockManager.ReleaseFiles(InputDaPak);
        }

        [Test]
        public void test_CompressPakFile()
        {
            FileLockManager.AcquireFiles(InputDataFolderForCompress);

            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";
            string outputRootPath = Path.Combine(exeDirectory, "test_CompressPakFile_output");

            // Thực hiện nén folder vào file .pak
            var initResult = NativeAPIAdapter.CompressFolderToPakFile(InputDataFolderForCompress.AssetPath, outputRootPath: outputRootPath);

            // Assert để kiểm tra kết quả nén
            Assert.IsTrue(initResult, "Compress operation failed!");

            // Kiểm tra số lượng file trong thư mục
            string[] compressedFiles = Directory.GetFiles(outputRootPath);
            Assert.AreEqual(2, compressedFiles.Length, "Tổng số file không đúng!");

            // Kiểm tra tên các file
            Assert.IsTrue(compressedFiles.Any(f => Path.GetFileName(f) == "data.pak"), "File data.pak không tồn tại!");
            Assert.IsTrue(compressedFiles.Any(f => Path.GetFileName(f) == "data.pak.txt"), "File data.pak.txt không tồn tại!");

            string pakInfoOutput = Path.Combine(outputRootPath, "data.pak.txt");
            var pakInfo = NativeAPIAdapter.ParsePakInfoFile(pakInfoOutput);
            Assert.That(pakInfo.fileMap.Count, Is.EqualTo(3), "Tổng số file trong pak phải là 3!");
            Assert.That(pakInfo.fileMap.Values.ElementAt(0).id, Is.EqualTo("95a1ffa3"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(1).id, Is.EqualTo("95ad8b72"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(2).id, Is.EqualTo("a9c272ec"));

            Assert.That(pakInfo.crc, Is.EqualTo("49bc4c3a"));
            FileLockManager.ReleaseFiles(InputDataFolderForCompress);
        }

        [Test]
        public void test_ParsePakInfo()
        {
            FileLockManager.AcquireFiles(InputDaPakTxt);

            var pakInfo = NativeAPIAdapter.ParsePakInfoFile(InputDaPakTxt.AssetPath);

            Assert.That(pakInfo.fileMap.Count, Is.EqualTo(3), "Tổng số file trong pak phải là 3!");
            Assert.That(pakInfo.fileMap.Values.ElementAt(0).id, Is.EqualTo("95a1ffa3"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(1).id, Is.EqualTo("95ad8b72"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(2).id, Is.EqualTo("a9c272ec"));

            Assert.That(pakInfo.pakTime, Is.EqualTo("2024-7-6 14:16:4"));
            Assert.That(pakInfo.pakTimeSave, Is.EqualTo("6688ef34"));
            Assert.That(pakInfo.crc, Is.EqualTo("49bc4c3a"));

            FileLockManager.ReleaseFiles(InputDaPakTxt);
        }

        [Test]
        public void test_LoadPakToWorkManager()
        {
            FileLockManager.AcquireFiles(InputDaPak);

            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";
            string outputRootPath = Path.Combine(exeDirectory, "test_LoadPakToWorkManager");
            string extractedFilePath = outputRootPath + "\\testExtractFileFromPak1";

            Assert.That(!NativeAPIAdapter.CloseSession("test_token"));
            var sessionToken = NativeAPIAdapter.LoadPakFileToWorkManager(InputDaPak.AssetPath, out PakInfo pakFileInfo);
            NativeAPIAdapter.ExtractFileFromPak(sessionToken, 0, extractedFilePath);

            string fileContent = File.ReadAllText(extractedFilePath);

            // Kiểm tra nội dung file có bằng "<config></config>"
            Assert.AreEqual("<config></config>", fileContent);

            var blockData = NativeAPIAdapter.ReadBlockFromPak(sessionToken, 0);
            string blockContent = Encoding.UTF8.GetString(blockData);
            Assert.AreEqual("<config></config>", blockContent);

            Assert.That(NativeAPIAdapter.CloseSession(sessionToken));
            FileLockManager.ReleaseFiles(InputDaPak);

        }

        [Test]
        public void test_LoadSprFromMemory()
        {
            FileLockManager.AcquireFiles(_12345SprFile, BinFile1, BinFile2, BinFile3, BinFile4, BinFile5);
            // Đọc toàn bộ nội dung file vào mảng byte
            byte[] fileData = File.ReadAllBytes(_12345SprFile.AssetPath.FullPath());
            var initResult = NativeAPIAdapter.LoadSPRFromMemory(fileData, out SprFileHead sprFileHead,
                out Palette palette,
                out int frameDataBeginPos,
                out FrameRGBA[] frameData);

            Assert.That(initResult);
            Assert.That(sprFileHead.GlobalHeight * sprFileHead.GlobalWidth == 90000);

            var frameData1Byte = frameData[0].originDecodedBGRAData;
            var frameData1FromFile = TestUtil.ReadBytesFromFile(BinFile1.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));


            var frameData2Byte = frameData[1].originDecodedBGRAData;
            var frameData2FromFile = TestUtil.ReadBytesFromFile(BinFile2.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData2Byte, frameData2FromFile!));


            var frameData3Byte = frameData[2].originDecodedBGRAData;
            var frameData3FromFile = TestUtil.ReadBytesFromFile(BinFile3.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData3Byte, frameData3FromFile!));


            var frameData4Byte = frameData[3].originDecodedBGRAData;
            var frameData4FromFile = TestUtil.ReadBytesFromFile(BinFile4.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData4Byte, frameData4FromFile!));


            var frameData5Byte = frameData[4].originDecodedBGRAData;
            var frameData5FromFile = TestUtil.ReadBytesFromFile(BinFile5.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData5Byte, frameData5FromFile!));
            FileLockManager.ReleaseFiles(_12345SprFile, BinFile1, BinFile2, BinFile3, BinFile4, BinFile5);
        }


        [Test]
        public void Test_I127GithubIssue()
        {
            FileLockManager.ReleaseFiles(i127_TestFile);

            // Load từ bộ nhớ
            var loadSprFromMemoryResult = NativeAPIAdapter.LoadSPRFile(i127_TestFile.AssetPath,
                out SprFileHead dataFromMemory_sprFileHead,
                out Palette dataFromMemory_palette,
                out int dataFromMemory_rameDataBeginPos,
                out FrameRGBA[] dataFromMemory_frameData);

            // Load từ file
            var loadSprFromMemoryFile = NativeAPIAdapter.LoadSPRFile_ForTestOnly(i127_TestFile.AssetPath,
                out SprFileHead dataFromFile_sprFileHead,
                out Palette dataFromFile_palette,
                out int dataFromFile_rameDataBeginPos,
                out FrameRGBA[] dataFromFile_frameData);

            // So sánh SprFileHead
            Assert.That(dataFromMemory_sprFileHead.Equals(dataFromFile_sprFileHead), Is.True, "SprFileHead không khớp!");

            // So sánh Palette
            Assert.That(dataFromMemory_palette.Equals(dataFromFile_palette), Is.True, "Palette không khớp!");

            // So sánh FrameData
            Assert.That(dataFromMemory_frameData.Length, Is.EqualTo(dataFromFile_frameData.Length), "Số lượng frame không khớp!");
            for (int i = 0; i < dataFromMemory_frameData.Length; i++)
            {
                Assert.That(dataFromMemory_frameData[i].Equals(dataFromFile_frameData[i]), Is.True, $"FrameData tại index {i} không khớp!");
            }

            FileLockManager.AcquireFiles(i127_TestFile);
        }

        //TODO: Add unittest for MemoryManager native code
    }
}
