using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace WizMachine.Utils
{
    internal class Logger
    {
        private const string PROJECT_TAG = "WizMachine";
        private static StreamWriter _logWriter;
        private string classTag;

        static Logger()
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void Init(StreamWriter streamWriter)
        {
            _logWriter = streamWriter;
        }

        public Logger(string tag)
        {
            classTag = tag;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            _logWriter.WriteLine($"Unhandled Exception: {exception?.Message ?? ""}");
            _logWriter.WriteLine($"StackTrace: {exception?.StackTrace ?? ""}");
            _logWriter.WriteLine($"Occurred at: {DateTime.Now}");
        }

        public void D(string message, [CallerMemberName] string caller = "")
        {
#if DEBUG
            var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tD\t{PROJECT_TAG}\t{classTag}\t{caller}\t{message}";
            Debug.WriteLine(log);
            _logWriter.WriteLine(log);
#endif
        }

        public void I(string message, [CallerMemberName] string caller = "")
        {
            var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tI\t{PROJECT_TAG}\t{classTag}\t{caller}\t{message}";
            Debug.WriteLine(log);
            _logWriter.WriteLine(log);
        }

        public void E(string message, [CallerMemberName] string caller = "")
        {
            var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tE\t{PROJECT_TAG}\t{classTag}\t{caller}\t{message}";
            Debug.WriteLine(log);
            _logWriter.WriteLine(log);
        }


        public static class Raw
        {
            public static void D(string message, [CallerMemberName] string caller = "")
            {
#if DEBUG
                var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tD\t{PROJECT_TAG}\t{caller}\t{message}";
                Debug.WriteLine(log);
                _logWriter.WriteLine(log);
#endif
            }

            public static void I(string message, [CallerMemberName] string caller = "")
            {
                var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tI\t{PROJECT_TAG}\t{caller}\t{message}";
                Debug.WriteLine(log);
                _logWriter.WriteLine(log);
            }

            public static void E(string message, [CallerMemberName] string caller = "")
            {
                var log = $"{DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss:fff")}\tE\t{PROJECT_TAG}\t{caller}\t{message}";
                Debug.WriteLine(log);
                _logWriter.WriteLine(log);
            }
        }
    }
}
