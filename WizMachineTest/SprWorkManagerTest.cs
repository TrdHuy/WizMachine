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

        //[Test]
        //public void test_InitWorkManagerSpr()
        //{

        //    var initResult = sprWorkManager.InitWorkManagerFromSprFile("D:\\Workspace\\Temp\\ArtWiz\\SPRNetTool\\bin\\x64\\Debug\\net6.0-windows\\Resources\\cuukiem.spr");
           
        //}

    }
}
