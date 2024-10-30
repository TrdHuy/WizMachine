using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Impl;
using WizMachine.Services.Utils;
using WizMachine;
using WizMachineTest.Utils;

namespace WizMachineTest.Services
{
    internal class PakWorkManagerTest
    {
        private const string LOG_FOLDER = "temp\\logs";
        private FileStream logFS;
        private StreamWriter logSW;

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
            logFS = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            logSW = new StreamWriter(logFS);
            EngineKeeper.Init(logSW);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            logSW.Close();
            logSW.Dispose();
            logFS.Close();
            logFS.Dispose();
        }

        private class PakWorkManagerTestObject : PakWorkManager
        {

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
        private string _dataWithSprPakPath = "data.pak";
        private string _dataForCompressPakPath = "Resources\\dataForCompressTest.pak";
        private string _output_daPakTxtPath = "data.pak.txt";
        private string _input_daPakPath = "Resources\\dataWithSpr.pak";
        private string _input_daPakTxtPath = "Resources\\dataWithSpr.pak.txt";
        private string _input_dataFolderForCompressPath = "Resources\\dataForCompressTest\\data";
        private IPakWorkManager pakWorkManager;
        private PakWorkManagerTestObject pakWorkManagerTestObject;

        [SetUp]
        public void Setup()
        {
            pakWorkManager = new PakWorkManagerTestObject();
            pakWorkManagerTestObject = (PakWorkManagerTestObject)pakWorkManager;

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void PakWorkManagerOverviewTest()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";
            string outputRootPath = Path.Combine(exeDirectory, "PakWorkManagerOverviewTest_output");
            Directory.CreateDirectory(outputRootPath);
            //Load _input_daPakPath first time => should be success
            Assert.That(pakWorkManager.LoadPakFileToWorkManager(_input_daPakPath));

            //Load _input_daPakPath second time => false to load because it's already existed
            Assert.That(!pakWorkManager.LoadPakFileToWorkManager(_input_daPakPath));

            // Khẳng định chắc chắn tồn tại asset theo đường dẫn \data\12345.spr
            var _12345SprAssetPath = "\\data\\12345.spr";
            var extractPath = outputRootPath+_12345SprAssetPath;

            Assert.That(pakWorkManager.IsBlockExistByPath(_12345SprAssetPath));

            Assert.That(pakWorkManager.ExtractBlockByPath(_12345SprAssetPath, extractPath));

            Assert.That(pakWorkManager.RemovePakFileFromWorkManager(_input_daPakPath));
            Assert.That(pakWorkManager.LoadPakFileToWorkManager(_input_daPakPath));
            pakWorkManager.ResetPakWorkManager();
        }

    }
}
