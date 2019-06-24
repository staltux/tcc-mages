using System;


[Flags]
public enum CardLine
{
    LEFT = 1 << 0,
    CENTER = 1 << 1,
    RIGHT = 1 << 2,
    MAGE = 1 << 3,
    ENEMY = 1 << 4
}

