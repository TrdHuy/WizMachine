using System;
using System.IO;
using System.Reflection;
using WizMachine.Services.Base;
using WizMachine.Services.Impl;
using WizMachine.Utils;

namespace WizMachine
{
    public class EngineKeeper
    {
        private static EngineKeeper? _engineInstance;
        public static StreamWriter? LogWriter { get; private set; }

        public static ISprWorkManagerAdvance GetSprWorkManagerService()
        {
            if (_engineInstance == null) throw new Exception("Engine was not inited yet");

            return _engineInstance.sprWorkManagerAdvanceInstance;
        }

        public static bool ForceCheckCallingSignature()
        {
            try
            {
                var calling = Assembly.GetCallingAssembly().Location;
                CertManagerUtil.ForceCheckCert(calling);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public static void Init(StreamWriter logWriter)
        {
            if (_engineInstance != null) throw new Exception("Engine has already inited!");

            LogWriter = logWriter;
            _engineInstance = new EngineKeeper();
        }

        private ISprWorkManagerAdvance sprWorkManagerAdvanceInstance;
        private EngineKeeper()
        {
            sprWorkManagerAdvanceInstance = new SprWorkManagerAdvance();
        }
    }
}
