﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTL_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_ACTIONLIST_FTL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] name;
        public int idx; // index vertex;
        public int action;
        public int sfx;
    }
}