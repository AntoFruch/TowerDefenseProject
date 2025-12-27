using System.Collections.Generic;
using System.Linq;
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

    // Style 
    [SerializeField] public MapPrefabsConfig prefabConfig;

    //HUD
    [SerializeField] public HUDManager HUD;

    // Selector position
    [SerializeField] public Transform selector; 
    
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

        map = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D("../Maps/map_03_fix.png"));
        try 
        {
            graph = PathVerifier.CreatePathGraph(map);
            PathVerifier.IsValidGraph(graph);
        } catch (System.Exception e)
        {
            // logique pour envoyer le message d'erreur pour que ca affiche un popup coté menu
            SceneManager.LoadScene("MapSelector");
        }

        mapGenerator.GenerateMap();

        // DEBUG : Instanciation d'ennemis sur les cases de départ
        if (spawnMobOnStart)
        {
            List<Vertex<VertexLabel>> startTiles 
                = graph.GetVertices()
                        .Where(v => v.label == VertexLabel.START )
                        .ToList();
            /*
            foreach (Vertex<VertexLabel> v in startTiles)
            {
                Instantiate(Resources.Load("Monsters/test"), new Vector3(v.position.x, 2f, v.position.y), Quaternion.identity);
            } 
            */
            Instantiate(Resources.Load("Monsters/blob/blob"), new Vector3(startTiles[2].position.x, 2f, startTiles[2].position.y), Quaternion.identity);
            Instantiate(Resources.Load("Monsters/Gros/GroBleu"), new Vector3(startTiles[0].position.x, 2f, startTiles[0].position.y), Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
