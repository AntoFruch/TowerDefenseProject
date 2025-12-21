using UnityEngine;
using System.IO;
public class FileAPI
{
    static public Texture2D ReadImageAsTexture2D(string path)
    {
        path = Path.Combine(Application.dataPath, path);

        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);

            Texture2D image = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            image.LoadImage(bytes);

            return image;
        }
        else
        {
            throw new System.Exception(path + " is not a valid path");
        }
    }
}