using UnityEngine;
using System.IO;
using System.Text;
public static class FileAPI
{
    static public Texture2D ReadImageAsTexture2D(string path)
    {
        path = Path.Combine(Application.dataPath, path);

        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);

            Texture2D image = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            image.LoadImage(bytes);

            // Inverser verticalement
            Color[] pixels = image.GetPixels();
            Color[] flippedPixels = new Color[pixels.Length];
            
            int width = image.width;
            int height = image.height;
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    flippedPixels[x + y * width] = pixels[x + (height - 1 - y) * width];
                }
            }
            
            image.SetPixels(flippedPixels);
            image.Apply();

            return image;
        }
        else
        {
            throw new System.Exception(path + " is not a valid path");
        }
    }
    public static void Log2DArray<T>(T[][] array, string fileName)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[y].Length; x++)
            {
                sb.Append(array[y][x]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }

        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, sb.ToString());

        Debug.Log($"2D array logged to: {path}");
    }
}

