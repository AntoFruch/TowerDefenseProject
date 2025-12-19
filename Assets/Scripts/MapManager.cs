using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;


public class MapManager : MonoBehaviour
{
    [SerializeField]
    private MapPrefabsConfig prefabConfig;

    private Texture2D image;
    private TileType[][] map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        string path = Path.Combine(Application.dataPath, "../Maps/map_03.png");

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

        map = ImageToTileTypeArray(image);
        GenerateMap();
    }

    TileType ColorToTileType(Color color)
    {
        Color GRAY   = new Color(229f/255f, 229f/255f, 229f/255f);
        Color YELLOW = new Color(255f/255f, 233f/255f, 127f/255f);
        Color RED = new Color(255f/255f, 0f, 0f);
        Color ORANGE = new Color(255f/255f, 178f/255f, 127f/255f);
        Color GREEN = new Color(0f, 255f/255f, 33f/255f);

        switch (color)
        {
            case var c when c == GRAY:
                return TileType.EDGE;
            case var c when c == YELLOW:
                return TileType.PATH;
            case var c when c == RED:
                return TileType.END;
            case var c when c == ORANGE:
                return TileType.INTERSECTION;
            case var c when c == GREEN:
                return TileType.SPAWN;
            default:
                return TileType.CONSTRUCTIBLE;
        }
    }

    TileType[][] ImageToTileTypeArray(Texture2D img)
    {
        int width  = img.width;
        int height = img.height;

        TileType[][] tileArray = new TileType[height][];

        Color[] pixels = img.GetPixels();

        for (int y = 0; y < height; y++)
        {
            tileArray[y] = new TileType[width];
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                tileArray[y][x] = ColorToTileType(pixels[index]);
            }
        }
        return tileArray;
    } 


    void GenerateMap()
    {
        for (int y=0; y<map.Length; y++)
        {
         for (int x=0; x<map.Length; x++)
            {
                TileType type = map[y][x];
                switch (type)
                {
                    case TileType.PATH or TileType.INTERSECTION or TileType.SPAWN or TileType.END:
                        PathResolver(x,y,type);
                        break;
                    case TileType.EDGE:
                        Instantiate(prefabConfig.edgeTile, new Vector3(x, 0, y), Quaternion.identity);   
                        break;
                    case TileType.CONSTRUCTIBLE:
                        Instantiate(prefabConfig.constructibleTile, new Vector3(x, 0, y), Quaternion.identity);
                        break;
                }
            }   
        }
    }

    // Quand la tuile est un chemin il faut savoir si c'est un chemin droit, en coin, une interection a 3 coté ou un carrefour
    void PathResolver(int x, int y, TileType type)
    {
        bool[] adj = adjascentPath(x,y);  // {up, down, left, right}
        Quaternion rotation = Quaternion.identity;
        switch (adj.Sum(b => b ? 1 : 0))
        {
            case 4:
                Instantiate(prefabConfig.crossPath, new Vector3(x, 0, y), rotation);
                break;

            case 3:
                if (adj[0] && adj[2] && adj[3]) //  up left right -> vers le haut 
                {
                    rotation = Quaternion.Euler(0f,0f,0f);
                } else if (adj[1] && adj[3] && adj[0]) // down right up -> vers la droite
                {
                    rotation = Quaternion.Euler(0f, 90f, 0f);
                } else if (adj[2] && adj[0] && adj[1]) // left up down -> vers la gauche
                {
                    rotation = Quaternion.Euler(0f, 270f, 0f);
                } else if (adj[3] && adj[1] && adj[2]) // right down left -> vers le bas
                {
                    rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                Instantiate(prefabConfig.splitPath, new Vector3(x,0,y),rotation);
                break;

            case 2:
                GameObject corner = prefabConfig.cornerPath;
                GameObject straight = prefabConfig.straightPath;
                GameObject straightOrCorner = straight;
                if (adj[0] && adj[2] ) //  up left
                {
                    rotation = Quaternion.Euler(0f,0f,0f);
                    straightOrCorner = corner;
                } else if (adj[0] && adj[3]) // up right
                {
                    rotation = Quaternion.Euler(0f, 90f, 0f);
                    straightOrCorner = corner;
                } else if (adj[1] && adj[2]) // down left
                {
                    rotation = Quaternion.Euler(0f, -90f, 0f);
                    straightOrCorner = corner;
                } else if (adj[1] && adj[3]) // down right
                {
                    rotation = Quaternion.Euler(0f,-180f, 0f);
                    straightOrCorner = corner;
                } else if (adj[0] && adj[1]) //up down
                {
                    rotation = Quaternion.Euler(0f,0f, 0f);
                    straightOrCorner = straight;
                } else if (adj[2] && adj[3]) //up down
                {
                    rotation = Quaternion.Euler(0f,90f, 0f);
                    straightOrCorner = straight;
                }
                if (type == TileType.SPAWN)
                {
                    Instantiate(prefabConfig.startTileStraight, new Vector3(x,0,y),rotation);
                } else if (type == TileType.END)
                {
                    Instantiate(prefabConfig.endTileStraight, new Vector3(x,0,y),rotation);
                } else
                {
                    Instantiate(straightOrCorner, new Vector3(x,0,y),rotation);
                }
                
                break;

            case 1:
                if (adj[0])
                {
                    rotation = Quaternion.Euler(0f, 0f, 0f);
                } else if (adj[1])
                {
                    rotation = Quaternion.Euler(0f, 180f, 0f);
                } else if (adj[2])
                {
                    rotation = Quaternion.Euler(0f, -90f, 0f);
                } else if (adj[3])
                {
                    rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                
                if (type == TileType.SPAWN)
                {
                    Instantiate(prefabConfig.startTileEnd, new Vector3(x,0,y),rotation);
                } else if (type == TileType.END)
                {
                    Instantiate(prefabConfig.endTileEnd, new Vector3(x,0,y),rotation);
                } else
                {
                    Instantiate(prefabConfig.straightPath, new Vector3(x,0,y),rotation);

                }
                break;
        }
    }
    bool[] adjascentPath(int x, int y)
    {
        bool up = y<image.height-1 ? 
            map[y+1][x] == TileType.PATH || map[y+1][x] == TileType.INTERSECTION || map[y+1][x] == TileType.SPAWN || map[y+1][x] == TileType.END: false;

        bool down = y>0 ? 
            map[y-1][x] == TileType.PATH || map[y-1][x] == TileType.INTERSECTION || map[y-1][x] == TileType.SPAWN || map[y-1][x] == TileType.END: false;

        bool right = x<image.width-1 ? 
            map[y][x+1] == TileType.PATH || map[y][x+1] == TileType.INTERSECTION || map[y][x+1] == TileType.SPAWN || map[y][x+1] == TileType.END: false;

        bool left = x>0 ? 
            map[y][x-1] == TileType.PATH || map[y][x-1] == TileType.INTERSECTION || map[y][x-1] == TileType.SPAWN || map[y][x-1] == TileType.END: false;

        return new bool[]
        {
            up, down, left, right
        };
    }

}

enum TileType
{
    EDGE, // GRAY
    PATH, // YELLOW
    SPAWN, // GREEN
    END, // RED
    INTERSECTION, // ORANGE

    CONSTRUCTIBLE, // WHITE

    VOID
}
