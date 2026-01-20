using System.Collections.Generic;
using UnityEngine;

// class Game is the game manager, it initiates the game and provides the logic for everything.
public class Game : MonoBehaviour
{
    public static Game Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    
    //Dev
    [SerializeField] bool spawnWaves;

    // Map
    private MapGenerator mapGenerator;
    public TileType[][] map {get;private set;}
    public Graph<VertexLabel> graph {get;private set;}

    // Prefabs
    [Header("Prefabs Assets")]
    [SerializeField] public MapPrefabs mapPrefabs;
    [SerializeField] public BuildingsPrefabs buildingsPrefabs;
    [SerializeField] public MonstersPrefabs monstersPrefabs;

    // HUD
    [Header("HUD Element")]
    [SerializeField] public HUDManager HUD;

    // Selector
    [Header("Selector")]
    [SerializeField] public Transform selector; 
    
    // Buildings list
    public List<Building> buildings {get; private set;}

    // Alive Monster List
    public List<MonsterController> monsters {get;private set;} 


    // Game State
    [SerializeField] public GameState state;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        map = GameConfig.map;
        graph = GameConfig.graph;

        // mode dev
        if (map == null)
        {
            map = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D("../Maps/map_03.png"));
            graph = PathVerifier.CreatePathGraph(map);
        }

        mapGenerator.GenerateMap();
        buildings = new();
        
        monsters = new();
        WaveManager.Instance.Init();
    }

    // Update is called once per frame
    int lastBuildingCount = 0;
    void Update()
    {
        if (buildings.Count != lastBuildingCount)
        {
            OnNewBuildingUpdate();
            lastBuildingCount = buildings.Count;
        }
        // DEBUG : Instanciation d'ennemis sur les cases de départ
        if (spawnWaves && state == GameState.Defense)
        {
            WaveManager.Instance.StartNextWave();
        }
    }

    void OnNewBuildingUpdate()
    {
        EnergyManager.Instance.UpdateEnergyGraph();    
        RangesManager.Instance.DrawRanges();
        LogBuildings();
    }

    void LogBuildings()
    {
        string str ="buildings : ";
        foreach (Building build in buildings)
        {
            str += build.ToString();
            if (build is Tower t){ str += t.GetPower();}
            str += " " ;
        }
        Debug.Log(str);
    }
}

public enum GameState
{
    Preparation, Defense
}
