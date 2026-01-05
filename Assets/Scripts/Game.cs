using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;

// class Game is the game manager, it initiates the game and provides the logic for everything.
public class Game : MonoBehaviour
{
    public static Game Instance;
    
    //Dev
    [SerializeField] bool spawnMobOnStart;

    // Map
    private MapGenerator mapGenerator;
    public TileType[][] map {get;private set;}
    public Graph<VertexLabel> graph {get;private set;}

    [Header("Prefabs Assets")]
    [SerializeField] public MapPrefabs mapPrefabs;
    [SerializeField] public BuildingsPrefabs buildingsPrefabs;
    [SerializeField] public MonstersPrefabs monstersPrefabs;

    [Header("HUD Element")]
    [SerializeField] public HUDManager HUD;

    [Header("Selector")]
    [SerializeField] public Transform selector; 
    
    // Buildings positions
    public List<Building> buildings {get; private set;}


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        map = GameConfig.map;
        graph = GameConfig.graph;

        // mode debug
        if (map == null)
        {
            map = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D("../Maps/map_03.png"));
            graph = PathVerifier.CreatePathGraph(map);
        }

        mapGenerator.GenerateMap();
        buildings = new List<Building>();

        // DEBUG : Instanciation d'ennemis sur les cases de départ
        if (spawnMobOnStart)
        {
            List<Vertex<VertexLabel>> startTiles 
                = graph.GetVertices()
                        .Where(v => v.label == VertexLabel.START )
                        .ToList();
            foreach (Vertex<VertexLabel> v in startTiles)
            {
                Instantiate(Resources.Load("Monsters/Prefabs/GroBleu"), new Vector3(v.position.x, 2f, v.position.y), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //LogBuildings();  
    }

    void LogBuildings()
    {
        string str ="buildings : ";
        foreach (Building build in buildings)
        {
            str += build.ToString() + " "; 
        }
        Debug.Log(str);
    }
}
