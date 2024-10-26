using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Utils;

namespace WizMachine.Utils
{
    internal static class CertManagerUtil
    {
        private static Dictionary<string, CertInfo> _certCache = new Dictionary<string, CertInfo>();
        public static CertInfo AssemblySignedCert { get; private set; }
        static CertManagerUtil()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            Logger.Raw.I($"CertManagerUtil:: assemblyPath={assemblyPath}");
            AssemblySignedCert = NativeAPIAdapter.GetSignedCertInfoFromFile(assemblyPath);
        }

        public static void ForceCheckCurrentAssemblyCert()
        {
            NativeAPIAdapter.ForceCheckCertPermission(AssemblySignedCert);
        }

        public static void ForceCheckCert(string filePath)
        {
            CertInfo ci;
            ci = NativeAPIAdapter.GetSignedCertInfoFromFile(filePath);
            _certCache[filePath] = ci;
            NativeAPIAdapter.ForceCheckCertPermission(ci);
        }
    }
}
