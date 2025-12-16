using UnityEngine;
using System.IO;

public class MapManager : MonoBehaviour
{
    private Texture2D image; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        string path = Path.Combine(Application.dataPath, "../Maps/map_01.png");

        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);

            image = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            image.LoadImage(bytes);
        }
        else
        {
            Debug.LogError("Map image not found: " + path);
        }

        MapGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapGenerator()
    {
        Color[] pixels = image.GetPixels();
        Debug.Log("Total Pixels: " + pixels.Length);

        int width  = image.width;
        int height = image.height;
        Debug.Log("Width: " + width + ", Height: " + height);

        Color GRAY   = new Color(229f/255f, 229f/255f, 229f/255f);
        Color YELLOW = new Color(255f/255f, 233f/255f, 127f/255f);
        Color RED = new Color(255f/255f, 0f, 0f);
        Color ORANGE = new Color(255f/255f, 178f/255f, 127f/255f);
        Color GREEN = new Color(0f, 255f/255f, 33f/255f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;

                Color pixelColor = pixels[index];
                switch (pixelColor)
                {
                    case var c when c == GRAY:
                        Instantiate(Resources.Load("FBX format/tile"), new Vector3(x, 0, y), Quaternion.identity);
                        Instantiate(Resources.Load("FBX format/wood-structure-part"), new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    case var c when c == YELLOW:
                        Instantiate(Resources.Load("FBX format/tile-dirt"), new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    case var c when c == RED:
                        Instantiate(Resources.Load("FBX format/tile-spawn-end-round"), new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    case var c when c == ORANGE:
                        Instantiate(Resources.Load("FBX format/tile-corner-square"), new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    case var c when c == GREEN:
                        Instantiate(Resources.Load("FBX format/tile-spawn-end"), new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    default :
                        Instantiate(Resources.Load("FBX format/tile"), new Vector3(x, 0, y), Quaternion.identity);
                        break;
                }

            }
        }
    }
}
