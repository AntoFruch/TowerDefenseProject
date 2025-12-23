using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private MapPrefabsConfig prefabConfig;

    private Texture2D image;
    public static TileType[][] map;

    public static Graph<VertexLabel> graph;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        image = FileAPI.ReadImageAsTexture2D("../Maps/map_03_fix.png");

        map = ImageToTileTypeArray(image);
        
        
        //FileAPI.Log2DArray<TileType>(map, "log");
        
        try {
            graph = PathVerifier.CreatePathGraph(map);
            Debug.Log(graph);
            PathVerifier.IsValidGraph(graph);
            RenderMap();
        }
        catch(System.Exception e)
        {  
            Debug.Log(e.Message);
        }
        Instantiate(Resources.Load("Monsters/Gros/GroBleu"), new Vector3(15f,0.217999905f,13f),Quaternion.identity);
        Instantiate(Resources.Load("Monsters/Gros/GroJaune"), new Vector3(15f,0.217999905f,17f),Quaternion.identity);
        Instantiate(Resources.Load("Monsters/Blob/Blob"), new Vector3(13f,0.217999905f,15f),Quaternion.identity);
        Instantiate(Resources.Load("Monsters/Shell/Shell"), new Vector3(17f,0.217999905f,15f),Quaternion.identity);
    }
    

    public static TileType ColorToTileType(Color color)
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

    public static TileType[][] ImageToTileTypeArray(Texture2D img)
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

    // Instancie les tuiles au bon endroit en fonction de la map
    private void RenderMap()
    {
        for (int y=0; y<map.Length; y++)
        {
         for (int x=0; x<map[0].Length; x++)
            {
                TileType type = map[y][x];
                bool[] adj = adjascentPath(x,y);
                Quaternion rotation = Quaternion.identity;
                switch (type)
                {
                    case TileType.PATH:
                        if (adj[0] && adj[1])
                        {
                            rotation = Quaternion.Euler(0f, 0f, 0f);
                        } else if (adj[2] && adj[3])
                        {
                            rotation = Quaternion.Euler(0f, 90f, 0f);
                        }

                        Instantiate(prefabConfig.straightPath, new Vector3(x,0,y),rotation);
                        break;
                    case TileType.INTERSECTION:
                        switch (adj.Sum(b => b ? 1 : 0))
                        {
                            case 4: // pas de question a se poser sur l'orientation, on met le carrefour
                                Instantiate(prefabConfig.crossPath, new Vector3(x, 0, y), rotation);
                                break;
                            case 3: // il faut mettre une intersection 
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
                                if (adj[0] && adj[2] ) //  up left
                                {
                                    rotation = Quaternion.Euler(0f,0f,0f);
                                } else if (adj[0] && adj[3]) // up right
                                {
                                    rotation = Quaternion.Euler(0f, 90f, 0f);
                                } else if (adj[1] && adj[2]) // down left
                                {
                                    rotation = Quaternion.Euler(0f, -90f, 0f);
                                } else if (adj[1] && adj[3]) // down right
                                {
                                    rotation = Quaternion.Euler(0f,-180f, 0f); 
                                }
                                Instantiate(prefabConfig.cornerPath, new Vector3(x,0,y),rotation); 
                                break;
                            }
                            break;
                    case TileType.SPAWN or TileType.END:
                        bool end = true;
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
                        
                        if (adj[0] && adj[1])
                        {
                            rotation = Quaternion.Euler(0f, 0f, 0f);
                            end = false;
                        } else if (adj[2] && adj[3])
                        {
                            rotation = Quaternion.Euler(0f, 90f, 0f);
                            end = false;
                        }
                        
                        // TYPE DE TUILE
                        if (type == TileType.SPAWN)
                        {
                            Instantiate(end ? prefabConfig.startTileEnd : prefabConfig.startTileStraight, new Vector3(x,0,y),rotation);
                        } else if (type == TileType.END)
                        {
                            Instantiate(end ? prefabConfig.endTileEnd : prefabConfig.endTileStraight, new Vector3(x,0,y),rotation);
                        }

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

    // retourne un array de boolen representant si les tuiles adjascentes sont des chemins valables ou pas.
    private bool[] adjascentPath(int x, int y)
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
