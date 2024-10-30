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
using static FileLockManager.TestAsset;

namespace WizMachineTest.Services
{
    [TestFixture]
    internal class PakWorkManagerTest
    {
        private class PakWorkManagerTestObject : PakWorkManager
        {

        }

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
            FileLockManager.AcquireFiles(InputDaPak);
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath) ?? "";
            string outputRootPath = Path.Combine(exeDirectory, "PakWorkManagerOverviewTest_output");
            Directory.CreateDirectory(outputRootPath);
            //Load _input_daPakPath first time => should be success
            Assert.That(pakWorkManager.LoadPakFileToWorkManager(InputDaPak.AssetPath));

            //Load _input_daPakPath second time => false to load because it's already existed
            Assert.That(!pakWorkManager.LoadPakFileToWorkManager(InputDaPak.AssetPath));

            // Khẳng định chắc chắn tồn tại asset theo đường dẫn \data\12345.spr
            var _12345SprAssetPath = "\\data\\12345.spr";
            var extractPath = outputRootPath+_12345SprAssetPath;

            Assert.That(pakWorkManager.IsBlockExistByPath(_12345SprAssetPath));

            Assert.That(pakWorkManager.ExtractBlockByPath(_12345SprAssetPath, extractPath));

            Assert.That(pakWorkManager.RemovePakFileFromWorkManager(InputDaPak.AssetPath));
            Assert.That(pakWorkManager.LoadPakFileToWorkManager(InputDaPak.AssetPath));
            pakWorkManager.ResetPakWorkManager();
            FileLockManager.ReleaseFiles(InputDaPak);
        }

    }
}
