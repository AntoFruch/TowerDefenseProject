using UnityEngine;
using System.Linq;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

public class MapGenerator : MonoBehaviour
{
    public static TileType[][] map;
    private MapPrefabsConfig prefabConfig;

    // Instancie les tuiles au bon endroit en fonction de la map
    public void GenerateMap()
    {
        prefabConfig = Game.Instance.prefabConfig;
        map = Game.Instance.map;

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
        GenerateBorders();
    }

    public int borderSize = 30; // combien de tuiles autour de la map
    public GameObject[] borderPrefabs; // arbres, rochers, etc.

    public void GenerateBorders()
    {
        int mapWidth = map[0].Length;
        int mapHeight = map.Length;

        // On boucle sur toute la zone étendue
        for (int y = -borderSize; y < mapHeight + borderSize; y++)
        {
            for (int x = -borderSize; x < mapWidth + borderSize; x++)
            {
                // On skip les tuiles déjà utilisées dans la map
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                    continue;

                // Position pour instancier
                Vector3 pos = new Vector3(x, 0, y);

                if (Random.value < 0.3f)
                {
                    float random = Random.value;
                    if (random < 1/3f)
                    {
                        Instantiate(prefabConfig.singleTree, pos, Quaternion.identity);
                    } else if (random < 2/3f)
                    {
                        Instantiate(prefabConfig.duoTrees, pos, Quaternion.identity);
                    } else 
                    {
                        Instantiate(prefabConfig.quadTrees, pos, Quaternion.identity);
                    }
                } else
                {
                    float random = Random.value;
                    if (random < 0.7f)
                    {
                        Instantiate(prefabConfig.constructibleTile, pos, Quaternion.identity);
                    } else if (random < 0.85f)
                    {
                        Instantiate(prefabConfig.hill, pos, Quaternion.identity);
                    } else
                    {
                        Instantiate(prefabConfig.crystals, pos, Quaternion.identity);

                    }
                }
            }
        }
    }

    // retourne un array de boolen representant si les tuiles adjascentes sont des chemins valables ou pas.
    private bool[] adjascentPath(int x, int y)
    {
        bool up = y<map.Length-1 ? 
            map[y+1][x] == TileType.PATH || map[y+1][x] == TileType.INTERSECTION || map[y+1][x] == TileType.SPAWN || map[y+1][x] == TileType.END: false;

        bool down = y>0 ? 
            map[y-1][x] == TileType.PATH || map[y-1][x] == TileType.INTERSECTION || map[y-1][x] == TileType.SPAWN || map[y-1][x] == TileType.END: false;

        bool right = x<map[0].Length-1 ? 
            map[y][x+1] == TileType.PATH || map[y][x+1] == TileType.INTERSECTION || map[y][x+1] == TileType.SPAWN || map[y][x+1] == TileType.END: false;

        bool left = x>0 ? 
            map[y][x-1] == TileType.PATH || map[y][x-1] == TileType.INTERSECTION || map[y][x-1] == TileType.SPAWN || map[y][x-1] == TileType.END: false;

        return new bool[]
        {
            up, down, left, right
        };
    }
}
