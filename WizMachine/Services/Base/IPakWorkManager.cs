﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;

namespace WizMachine.Services.Base
{
    public interface IPakWorkManager
    {
        public PakInfo ParsePakInfoFile(string pakInfoPath);
    }
}