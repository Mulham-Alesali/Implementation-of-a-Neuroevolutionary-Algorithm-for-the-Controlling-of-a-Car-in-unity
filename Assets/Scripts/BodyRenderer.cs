using System.Linq;
using UnityEngine;

/// <summary>
/// change the color of the car
/// </summary>
public class BodyRenderer : MonoBehaviour
{
    public GameObject body;
    public Color color;
    const int x = 0;
    const int y = 33;
    const int width = 129;
    const int height = 192 - 33;

 /// <summary>
 /// change the color of the vehicle
 /// </summary>
 /// <param name="color">the new color</param>
    public void SetColor(Color color)
    {
        this.color = color;
        var oldTexture = 
        (Texture2D)body.GetComponent<Renderer>().material.mainTexture;
        var newTexture = 
        new Texture2D(oldTexture.width, oldTexture.height); 
        var colors = Enumerable.Repeat(color, width * height).ToArray();
        newTexture.SetPixels(0, 0
            , oldTexture.width, oldTexture.height
            , oldTexture.GetPixels(0, 0
            , oldTexture.width
            , oldTexture.height));
        newTexture.SetPixels(x, y, width, height, colors);
        newTexture.Apply();
        body.GetComponent<Renderer>().material.mainTexture = newTexture;
    }

    /// <summary>
    /// change the color of the vehicle
    /// </summary>
    /// <param name="color">
    /// the new color value as integer
    /// </param>
    public void SetColor(int color)
    {
        Color c;
        float R, G, B;
        R = color & 255;
        G = (color >> 8) & 255;
        B = (color >> 16) & 255;
        R = R / 256;
        G = G / 256;
        B = B / 256;
        c = new Color(R, G, B);
        SetColor(c);
    }

    public Color GetColor()
    {
        return this.color;
    }

    public void Hide()
    {
        foreach (MeshRenderer mr 
            in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
    }

    public void Show()
        {
            foreach (MeshRenderer mr 
            in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = true;
            }
        }
}
