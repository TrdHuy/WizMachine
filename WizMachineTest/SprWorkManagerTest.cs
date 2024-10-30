using System.Reflection;
using System.Text;
using WizMachine;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Impl;
using WizMachine.Services.Utils;
using WizMachineTest.Utils;
using static FileLockManager.TestAsset;

namespace SPRNetToolTest.Domain
{
    [TestFixture]
    internal class SprWorkManagerTest
    {
        private class SprWorkManagerTestObject : SprWorkManagerAdvance
        {
            public long GetFrameDataBegPosCache()
            {
                return FrameDataBegPos;
            }

            public FrameRGBA[]? GetFrameDataCache()
            {
                return FrameData;
            }
        }



        private ISprWorkManagerAdvance sprWorkManager;
        private SprWorkManagerTestObject sprWorkManagerTestObject;

        [SetUp]
        public void Setup()
        {
            sprWorkManager = new SprWorkManagerTestObject();
            sprWorkManagerTestObject = (SprWorkManagerTestObject)sprWorkManager;
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void test_SaveCurrentWorkToSpr()
        {
            FileLockManager.AcquireFiles(_12345SprFile, BinFile319x319);
            using (FileStream fs = new FileStream(_12345SprFile.AssetPath, FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                var frameRGBAs = sprWorkManagerTestObject.GetFrameDataCache();
                Assert.NotNull(frameRGBAs);
                Assert.That(frameRGBAs[0].modifiedFrameRGBACache.frameWidth == 300);
                Assert.That(frameRGBAs[0].modifiedFrameRGBACache.frameHeight == 300);
                // Change frameSize 
                frameRGBAs[0].modifiedFrameRGBACache.frameWidth = 319;
                frameRGBAs[0].modifiedFrameRGBACache.frameHeight = 319;

                sprWorkManager.SaveCurrentWorkToSpr("Resources\\test_SaveCurrentWorkToSpr.spr", true);
            }

            using (FileStream fs = new FileStream("Resources\\test_SaveCurrentWorkToSpr.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);

                var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
                var frameData1FromFile = TestUtil.ReadBytesFromFile(BinFile319x319.AssetPath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }
            FileLockManager.ReleaseFiles(_12345SprFile, BinFile319x319);
        }

        [Test]
        public void test_SaveCurrentWorkToSpr_AlphaFile()
        {
            FileLockManager.AcquireFiles(AlphaFile);

            var cacheFrameData = new byte[0];
            using (FileStream fs = new FileStream(AlphaFile.AssetPath, FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                var frameRGBAs = sprWorkManagerTestObject.GetFrameDataCache();
                Assert.NotNull(frameRGBAs);
                Assert.NotNull(frameRGBAs[0].originDecodedBGRAData);
                cacheFrameData = new byte[frameRGBAs[0].originDecodedBGRAData.Length];
                Array.Copy(frameRGBAs[0].originDecodedBGRAData,
                    cacheFrameData,
                    frameRGBAs[0].originDecodedBGRAData.Length);

                sprWorkManager.SaveCurrentWorkToSpr("Resources\\test_SaveCurrentWorkToSpr_AlphaFile.spr", true);
            }

            using (FileStream fs = new FileStream("Resources\\test_SaveCurrentWorkToSpr_AlphaFile.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);

                Assert.That(TestUtil.AreByteArraysEqual(cacheFrameData, sprWorkManagerTestObject
                    .GetFrameDataCache()![0].originDecodedBGRAData));
            }
            FileLockManager.ReleaseFiles(AlphaFile);
        }

        [Test]
        public void test_SaveBitmapSourceToSprFile()
        {
            FileLockManager.AcquireFiles(PngFile, BinFile);

            string imagePath = PngFile.AssetPath.FullPath();
            var bmpSource = TestUtil.LoadBitmapFromFile(imagePath);
            Assert.NotNull(bmpSource);
            sprWorkManager.SaveBitmapSourceToSprFile(bmpSource, "Resources\\test_SaveBitmapSourceToSprOutput.spr");

            using (FileStream fs = new FileStream("Resources\\test_SaveBitmapSourceToSprOutput.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);

                var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
                var frameData1FromFile = TestUtil.ReadBytesFromFile(BinFile.AssetPath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }

            FileLockManager.ReleaseFiles(PngFile, BinFile);
        }


        [Test]
        public void test_InitWorkManagerFromSprFile_file12345()
        {
            FileLockManager.AcquireFiles(_12345SprFile, BinFile1, BinFile2, BinFile3, BinFile4, BinFile5);

            var initResult = sprWorkManager.InitWorkManagerFromSprFile(_12345SprFile.AssetPath);
            Assert.That(initResult);
            Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);


            var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
            var frameData1FromFile = TestUtil.ReadBytesFromFile(BinFile1.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));


            var frameData2Byte = sprWorkManagerTestObject.GetFrameDataCache()![1].originDecodedBGRAData;
            var frameData2FromFile = TestUtil.ReadBytesFromFile(BinFile2.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData2Byte, frameData2FromFile!));


            var frameData3Byte = sprWorkManagerTestObject.GetFrameDataCache()![2].originDecodedBGRAData;
            var frameData3FromFile = TestUtil.ReadBytesFromFile(BinFile3.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData3Byte, frameData3FromFile!));


            var frameData4Byte = sprWorkManagerTestObject.GetFrameDataCache()![3].originDecodedBGRAData;
            var frameData4FromFile = TestUtil.ReadBytesFromFile(BinFile4.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData4Byte, frameData4FromFile!));


            var frameData5Byte = sprWorkManagerTestObject.GetFrameDataCache()![4].originDecodedBGRAData;
            var frameData5FromFile = TestUtil.ReadBytesFromFile(BinFile5.AssetPath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData5Byte, frameData5FromFile!));
            FileLockManager.ReleaseFiles(_12345SprFile, BinFile1, BinFile2, BinFile3, BinFile4, BinFile5);
        }

        [Test]
        public void test_InitWorkManagerFromSprFile()
        {
            FileLockManager.AcquireFiles(SprFile);

            using (FileStream fs = new FileStream(SprFile.AssetPath, FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);
            }
            FileLockManager.ReleaseFiles(SprFile);

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
            using (FileStream fs = new FileStream($"{outputRootPath}\\data\\12345.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);

                var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
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

        //TODO: Add unittest for MemoryManager native code
    }
}
