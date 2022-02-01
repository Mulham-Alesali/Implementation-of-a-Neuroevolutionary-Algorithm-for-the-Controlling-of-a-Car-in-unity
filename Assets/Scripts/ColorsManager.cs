using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsManager
{

    public static Color ConvertToColor(int color)
    {
        Color c;
        float R, G, B;
        R = color & 255;
        G = (color >> 8) & 255;
        B = (color >> 16) & 255;
        // Debug.Log(color + " " + R + " " + G + " " + B);
        R = R / 256;
        G = G / 256;
        B = B / 256;
        c = new Color(R, G, B);
        return c;
    }

    //public int ConvertToInteger(Color color)
    //{

    //}
}
