using System.Reflection;
using System.Reflection.Metadata;
using WizMachine;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Impl;
using WizMachine.Services.Utils;
using WizMachine.Utils;
using WizMachineTest.Utils;

namespace SPRNetToolTest.Domain
{
    internal class SprWorkManagerTest
    {
        private const string LOG_FOLDER = "temp\\logs";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Khởi tạo biến global A ở đây
            var dateTimeNow = DateTime.Now.ToString("ddMMyyHHmmss");
            var logFileName =
                Assembly.GetCallingAssembly().GetName().Name + "_" +
                Assembly.GetCallingAssembly().GetName().Version + "_" +
                dateTimeNow + ".txt";

            if (!Directory.Exists(LOG_FOLDER))
            {
                Directory.CreateDirectory(LOG_FOLDER);
            }

            var filePath = LOG_FOLDER + @"\" + logFileName;
            var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            var wr = new StreamWriter(fs);
            EngineKeeper.Init(wr);
        }

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


        private string _sprFilePath = "Resources\\test.spr";
        private string _binFilePath = "Resources\\test.bin";
        private string _pngFilePath = "Resources\\test.png";
        private string _12345sprFilePath = "Resources\\12345.spr";
        private string _alphaFilePath = "Resources\\alpha.spr";
        private string _1binFilePath = "Resources\\1.bin";
        private string _1_319x319binFilePath = "Resources\\1_319x319.bin";
        private string _2binFilePath = "Resources\\2.bin";
        private string _3binFilePath = "Resources\\3.bin";
        private string _4binFilePath = "Resources\\4.bin";
        private string _5binFilePath = "Resources\\5.bin";
        private string _testTxtPakPath = "Resources\\testTxtFile.pak";
        private string _testTxtPakTxtPath = "Resources\\testTxtFile.pak.txt";
        private string _dataWithSprPakPath = "Resources\\dataWithSpr.pak";
        private string _dataForCompressPakPath = "Resources\\dataForCompressTest.pak";
        private string _daPakTxtPath = "Resources\\dataWithSpr.pak.txt";
        private string _dataFolderForCompressPath = "Resources\\dataForCompressTest\\data";
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
            using (FileStream fs = new FileStream(_12345sprFilePath, FileMode.Open, FileAccess.Read))
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
                var frameData1FromFile = TestUtil.ReadBytesFromFile(_1_319x319binFilePath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }
        }

        [Test]
        public void test_SaveCurrentWorkToSpr_AlphaFile()
        {
            var cacheFrameData = new byte[0];
            using (FileStream fs = new FileStream(_alphaFilePath, FileMode.Open, FileAccess.Read))
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
        }

        [Test]
        public void test_SaveBitmapSourceToSprFile()
        {
            string imagePath = _pngFilePath.FullPath();
            var bmpSource = TestUtil.LoadBitmapFromFile(imagePath);
            Assert.NotNull(bmpSource);
            sprWorkManager.SaveBitmapSourceToSprFile(bmpSource, "Resources\\test_SaveBitmapSourceToSprOutput.spr");

            using (FileStream fs = new FileStream("Resources\\test_SaveBitmapSourceToSprOutput.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);

                var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
                var frameData1FromFile = TestUtil.ReadBytesFromFile(_binFilePath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }
        }

        [Test]
        public void test_InitWorkManagerFromSprFile_file12345()
        {
            var initResult = sprWorkManager.InitWorkManagerFromSprFile(_12345sprFilePath);
            Assert.That(initResult);
            Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);


            var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
            var frameData1FromFile = TestUtil.ReadBytesFromFile(_1binFilePath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));


            var frameData2Byte = sprWorkManagerTestObject.GetFrameDataCache()![1].originDecodedBGRAData;
            var frameData2FromFile = TestUtil.ReadBytesFromFile(_2binFilePath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData2Byte, frameData2FromFile!));


            var frameData3Byte = sprWorkManagerTestObject.GetFrameDataCache()![2].originDecodedBGRAData;
            var frameData3FromFile = TestUtil.ReadBytesFromFile(_3binFilePath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData3Byte, frameData3FromFile!));


            var frameData4Byte = sprWorkManagerTestObject.GetFrameDataCache()![3].originDecodedBGRAData;
            var frameData4FromFile = TestUtil.ReadBytesFromFile(_4binFilePath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData4Byte, frameData4FromFile!));


            var frameData5Byte = sprWorkManagerTestObject.GetFrameDataCache()![4].originDecodedBGRAData;
            var frameData5FromFile = TestUtil.ReadBytesFromFile(_5binFilePath);
            Assert.That(TestUtil.AreByteArraysEqual(frameData5Byte, frameData5FromFile!));
        }

        [Test]
        public void test_InitWorkManagerFromSprFile()
        {
            using (FileStream fs = new FileStream(_sprFilePath, FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);
            }
        }

        [Test]
        public void test_ExtractPakFile()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";


            Assert.IsTrue(NativeAPIAdapter.ExtractPakFile(_dataWithSprPakPath,
                _daPakTxtPath,
                outputRootPath: exeDirectory));
            using (FileStream fs = new FileStream("data\\12345.spr", FileMode.Open, FileAccess.Read))
            {
                var initResult = sprWorkManager.InitWorkManagerFromSprFile(fs);
                Assert.That(initResult);
                Assert.That(sprWorkManager.FileHead.GlobalHeight * sprWorkManager.FileHead.GlobalWidth == 90000);

                var frameData1Byte = sprWorkManagerTestObject.GetFrameDataCache()![0].originDecodedBGRAData;
                var frameData1FromFile = TestUtil.ReadBytesFromFile(_1binFilePath);
                Assert.That(TestUtil.AreByteArraysEqual(frameData1Byte, frameData1FromFile!));
            }

            // Kiểm tra nội dung file test.txt
            string testTxtPath = Path.Combine(exeDirectory, "data", "test.txt");
            string testTxtContent = File.ReadAllText(testTxtPath);
            Assert.That(testTxtContent.Trim(), Is.EqualTo("12345"), "Nội dung file test.txt không đúng.");

            // Kiểm tra nội dung file test.xml
            string testXmlPath = Path.Combine(exeDirectory, "data", "test.xml");
            string testXmlContent = File.ReadAllText(testXmlPath);
            Assert.That(testXmlContent.Trim(), Is.EqualTo("<config></config>"), "Nội dung file test.xml không đúng.");
        }

        [Test]
        public void test_CompressPakFile()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";

            var initResult = NativeAPIAdapter.CompressFolderToPakFile(_dataFolderForCompressPath,
                outputRootPath: exeDirectory);
        }

        [Test]
        public void test_ParsePakInfo()
        {
            var pakInfo = NativeAPIAdapter.ParsePakInfoFile(_daPakTxtPath);

            Assert.That(pakInfo.fileMap.Count, Is.EqualTo(3), "Tổng số file trong pak phải là 3!");
            Assert.That(pakInfo.fileMap.Values.ElementAt(0).id, Is.EqualTo("95a1ffa3"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(1).id, Is.EqualTo("95ad8b72"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(2).id, Is.EqualTo("a9c272ec")); 
            Assert.That(pakInfo.pakTime, Is.EqualTo("2024-7-6 14:16:4"));
            Assert.That(pakInfo.pakTimeSave, Is.EqualTo("6688ef34"));
            Assert.That(pakInfo.crc, Is.EqualTo("49bc4c3a"));
        }

        [Test]
        public void test_ParsePakInfo2()
        {
            var pakInfo = EngineKeeper.GetPakWorkManagerService().ParsePakInfoFile(_daPakTxtPath);

            Assert.That(pakInfo.fileMap.Count, Is.EqualTo(3), "Tổng số file trong pak phải là 3!");
            Assert.That(pakInfo.fileMap.Values.ElementAt(0).id, Is.EqualTo("95a1ffa3"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(1).id, Is.EqualTo("95ad8b72"));
            Assert.That(pakInfo.fileMap.Values.ElementAt(2).id, Is.EqualTo("a9c272ec"));
            Assert.That(pakInfo.pakTime, Is.EqualTo("2024-7-6 14:16:4"));
            Assert.That(pakInfo.pakTimeSave, Is.EqualTo("6688ef34"));
            Assert.That(pakInfo.crc, Is.EqualTo("49bc4c3a"));
        }

        [Test]
        public void test_CertUtil()
        {
            EngineKeeper.ForceCheckCallingSignature();
        }
    }
}
