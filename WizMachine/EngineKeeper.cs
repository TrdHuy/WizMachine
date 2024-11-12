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
        public StreamWriter? LogWriter { get; private set; }

        public static ISprWorkManagerAdvance GetSprWorkManagerService()
        {
            if (_engineInstance == null) throw new Exception("Engine was not inited yet");

            if (_engineInstance.sprWorkManagerAdvanceInstance == null)
            {
                _engineInstance.sprWorkManagerAdvanceInstance = new SprWorkManagerAdvance();
            }
            return _engineInstance.sprWorkManagerAdvanceInstance;
        }

        public static IPakWorkManager GetPakWorkManagerService()
        {
            if (_engineInstance == null) throw new Exception("Engine was not inited yet");

            if (_engineInstance.pakWorkManagerInstance == null)
            {
                _engineInstance.pakWorkManagerInstance = new PakWorkManager();
            }
            return _engineInstance.pakWorkManagerInstance;
        }

        public static void ForceCheckCallingSignature()
        {
            if (_engineInstance == null) throw new Exception("Engine was not inited yet");

            var calling = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).Location;
            Logger.Raw.I($"EngineKeeper: Force check calling {calling}");

            CertManagerUtil.ForceCheckCert(calling);
        }

        public static void Init(StreamWriter logWriter)
        {
            if (_engineInstance != null) throw new Exception("Engine has already inited!");
            _engineInstance = new EngineKeeper();
            _engineInstance.LogWriter = logWriter;
            Logger.Init(logWriter);

            ForceCheckCallingSignature();
        }

        private ISprWorkManagerAdvance? sprWorkManagerAdvanceInstance = null;
        private IPakWorkManager? pakWorkManagerInstance = null;

        private EngineKeeper()
        {
        }
    }
}
