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
    public static class CertManagerUtil
    {
        private static Dictionary<string, CertInfo> _certCache = new Dictionary<string, CertInfo>();
        public static CertInfo AssemblySignedCert { get; private set; }
        static CertManagerUtil()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            X509Certificate cert = X509Certificate.CreateFromSignedFile(assemblyPath);
            X509Certificate2 cert2 = new X509Certificate2(cert);
            AssemblySignedCert = new CertInfo
            {
                Subject = cert2.Subject,
                Issuer = cert2.Issuer,
                ValidFrom = cert2.NotBefore.Ticks,
                ValidTo = cert2.NotAfter.Ticks,
                Thumbprint = cert2.Thumbprint,
                SerialNumber = cert2.SerialNumber
            };
        }

        public static void ForceCheckCurrentAssemblyCert()
        {
            NativeAPIAdapter.ForceCheckCertPermission(AssemblySignedCert);
        }

        public static void ForceCheckCert(string filePath)
        {
            CertInfo ci;
            if (_certCache.ContainsKey(filePath))
            {
                ci = _certCache[filePath];
            }
            else
            {
                ci = NativeAPIAdapter.GetSignedCertInfoFromFile(filePath);
                _certCache[filePath] = ci;
            }

            NativeAPIAdapter.ForceCheckCertPermission(ci);
        }
    }
}
