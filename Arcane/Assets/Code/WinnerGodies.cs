using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerGodies
{
    public int starts = 0;
    public int xp = 0;

    public GodieType[] types;
    public int[] values;

    public WinnerGodies()
    {
        types = new GodieType[3] { GodieType.NONE, GodieType.NONE, GodieType.NONE };
        values = new int[3] { 0, 0, 0 };
    }

    public int Gold { get {
            return Count(GodieType.OURO);
        } }

    public int Chest
    {
        get
        {
            return Count(GodieType.LIVRO);
        }
    }

    public int Key
    {
        get
        {
            return Count(GodieType.CHAVE);
        }
    }

    private int Count(GodieType t)
    {
        int g = 0;
        if (types[0] == t) g += values[0];
        if (types[1] == t) g += values[1];
        if (types[2] == t) g += values[2];
        return g;
    }
}

public enum GodieType
{
    NONE = 0,OURO, LIVRO, CHAVE
}
