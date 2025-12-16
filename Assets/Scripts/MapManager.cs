using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
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
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {

                TileType tileType = map[y][x];
                switch (tileType)
                {
                    case TileType.EDGE:
                        Instantiate(Resources.Load("FBX format/tile-tree-quad"), new Vector3(x, 0, y), PathRotations(x, y));   
                        break;
                    case TileType.PATH:
                        Instantiate(Resources.Load("FBX format/tile-straight"), new Vector3(x, 0, y), PathRotations(x, y));   
                        break;
                    case TileType.END:
                        Instantiate(Resources.Load("FBX format/tile-spawn-end-round"), new Vector3(x, 0, y), PathRotations(x, y));   
                        break;
                    case TileType.INTERSECTION:
                        Instantiate(Resources.Load("FBX format/tile-corner-square"), new Vector3(x, 0, y), PathRotations(x, y));   
                        break;
                    case TileType.SPAWN:
                        Instantiate(Resources.Load("FBX format/tile-spawn-end"), new Vector3(x, 0, y), PathRotations(x, y));   
                        break;
                    case TileType.CONSTRUCTIBLE:
                        Instantiate(Resources.Load("FBX format/tile"), new Vector3(x, 0, y), PathRotations(x, y));
                        break;
                }
            }
            }
    }
    
    // Returns the tile types of the four adjacent tiles (UP, DOWN, LEFT, RIGHT)
    Dictionary<UDLR, TileType> adjascentTiles(int x, int y)
    {
        return new Dictionary<UDLR, TileType>
        {
            {UDLR.UP,    (y + 1 < map.Length)          ? map[y + 1][x] : TileType.VOID},
            {UDLR.DOWN,  (y - 1 >= 0)                  ? map[y - 1][x] : TileType.VOID},
            {UDLR.LEFT,  (x - 1 >= 0)                  ? map[y][x - 1] : TileType.VOID},
            {UDLR.RIGHT, (x + 1 < map[0].Length) ? map[y][x + 1] : TileType.VOID}
        };
    }

    //Returns the rotation needed for a path tile at (x, y)
    Quaternion PathRotations(int x, int y)
    {
        Dictionary<UDLR, TileType> adjascent = adjascentTiles(x, y);
        bool up    = (adjascent[UDLR.UP]    == TileType.PATH || adjascent[UDLR.UP]    == TileType.INTERSECTION);
        bool down  = (adjascent[UDLR.DOWN]  == TileType.PATH || adjascent[UDLR.DOWN]  == TileType.INTERSECTION);
        bool left  = (adjascent[UDLR.LEFT]  == TileType.PATH || adjascent[UDLR.LEFT]  == TileType.INTERSECTION);
        bool right = (adjascent[UDLR.RIGHT] == TileType.PATH || adjascent[UDLR.RIGHT] == TileType.INTERSECTION);
        
        switch(map[y][x]){

            case TileType.INTERSECTION:
                if (left && up) return Rotation.NORTH;
                else if (up && right) return Rotation.EAST;
                else if (right && down) return Rotation.SOUTH;
                else if (down && left) return Rotation.WEST;
                else return Rotation.NORTH;

            case TileType.PATH:
                if (left || right) return Rotation.EAST;
                else if (up || down) return Rotation.NORTH;
                else return Rotation.NORTH;

            case TileType.SPAWN or TileType.END:
                if (up) return Rotation.NORTH;
                else if (right) return Rotation.EAST;
                else if (down) return Rotation.SOUTH;
                else if (left) return Rotation.WEST;
                else return Rotation.NORTH;

            default:
                return Rotation.NORTH;

        }
        
    } 
}

class Rotation
    {
        public static Quaternion NORTH = Quaternion.Euler(0, 0, 0);
        public static Quaternion EAST  = Quaternion.Euler(0, 90, 0);
        public static Quaternion SOUTH = Quaternion.Euler(0, 180, 0);
        public static Quaternion WEST  = Quaternion.Euler(0, 270, 0);
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
enum UDLR
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }