﻿using System;

namespace Assets.Scripts.ArxNative
{
    [Flags]
    public enum PolyType : int
    {
        None = 0,
        NO_SHADOW = 1 << 0,
        DOUBLESIDED = 1 << 1,
        TRANS = 1 << 2,
        WATER = 1 << 3,
        GLOW = 1 << 4,
        IGNORE = 1 << 5,
        QUAD = 1 << 6,
        TILED = 1 << 7,
        METAL = 1 << 8,
        HIDE = 1 << 9,
        STONE = 1 << 10,
        WOOD = 1 << 11,
        GRAVEL = 1 << 12,
        EARTH = 1 << 13,
        NOCOL = 1 << 14,
        LAVA = 1 << 15,
        CLIMB = 1 << 16,
        FALL = 1 << 17,
        NOPATH = 1 << 18,
        NODRAW = 1 << 19,
        PRECISE_PATH = 1 << 20,
        NO_CLIMB = 1 << 21,
        ANGULAR = 1 << 22,
        ANGULAR_IDX0 = 1 << 23,
        ANGULAR_IDX1 = 1 << 24,
        ANGULAR_IDX2 = 1 << 25,
        ANGULAR_IDX3 = 1 << 26,
        LATE_MIP = 1 << 27,
    }
}
