using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WizMachine.Data
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CertInfo
    {
        public string Subject;
        public string Issuer;
        public long ValidFrom;
        public long ValidTo;
        public string Thumbprint;
        public string SerialNumber;
    }
}
