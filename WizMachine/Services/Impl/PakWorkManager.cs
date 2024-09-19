using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Utils;

namespace WizMachine.Services.Impl
{
    internal class PakWorkManager : IPakWorkManager
    {
        PakInfo IPakWorkManager.ParsePakInfoFile(string pakInfoPath)
        {
            return NativeAPIAdapter.ParsePakInfoFile(pakInfoPath);
        }
    }
}
