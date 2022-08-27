using UnityEngine;
using System.IO;

public class PerlinNoise
{
    public static float[,] GenererPerlinNoise(int width, int height, float scale)
    {
        float[,] matrice2D = new float[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float sampleX = x / scale;
                float sampleY = y / scale;
                float perlinValue = (float)Mathf.PerlinNoise(sampleX, sampleY);
                matrice2D[x, y] = Mathf.Clamp((float)perlinValue, 0f, 1f);
            }
        }

        return matrice2D;
    }

    public static Texture2D ArrayToTexture(int width, int height, float[,] donnees)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Color[] pix = new Color[tex.width * tex.height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float valeur = donnees[x, y];
                pix[y * tex.width + x] = new Color(valeur, valeur, valeur);
            }
        }

        tex.SetPixels(pix);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/Images/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllBytes(path + "PerlinNoise" + ".png", bytes);

        return tex;
    }
}
