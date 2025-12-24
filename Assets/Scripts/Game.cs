using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// class Game is the game manager, it initiates the game and provides the logic for everything.
public class Game : MonoBehaviour
{
    public static Game Instance;
    
    //Dev
    [SerializeField] bool spawnMobOnStart;

    // Map
    private MapGenerator mapGenerator;
    public TileType[][] map;
    public Graph<VertexLabel> graph;

    // Style 
    [SerializeField] public MapPrefabsConfig prefabConfig;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        // DEV : En attendant que la validation se fasse côté Menu
        if (map == null)
        {
            map = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D("../Maps/map_03_fix.png"));
            graph = PathVerifier.CreatePathGraph(map);
            PathVerifier.IsValidGraph(graph);
        }

        mapGenerator.GenerateMap();

        // DEBUG : Instanciation d'ennemis sur les cases de départ
        if (spawnMobOnStart)
        {
            List<Vertex<VertexLabel>> startTiles 
                = graph.GetVertices()
                        .Where(v => v.label == VertexLabel.START )
                        .ToList();
            foreach (Vertex<VertexLabel> v in startTiles)
            {
                Instantiate(Resources.Load("Monsters/Gros/GroBleu"), new Vector3(v.position.x, 2.5f, v.position.y), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
