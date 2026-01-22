using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    private Graph<Building> energyGraph;
    private List<EnergyLink> links;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        energyGraph = new Graph<Building>();
        links = new();
    }

    public void UpdateEnergyGraph()
    {
        energyGraph.Clear();
        
        // We add to the graph all power plants and towers in game as vertices
        foreach (Building building in Game.Instance.buildings)
        {
            if (building is PowerPlant || building is Tower)
            {
                energyGraph.AddVertex(building, UEExtension.Vector3toVector2Int(building.transform.position));
            }
        }
        List<Vertex<Building>> powerplants = energyGraph.GetVertices().Where(v => v.label is PowerPlant).ToList();
        ConnectBuildings(powerplants);
        SetEnergyToBuilding(powerplants);
        CreateVisualLinks();
    }

    // Makes the graph as a spanning tree
    void ConnectBuildings(List<Vertex<Building>> powerplants)
    {
        List<Vertex<Building>> visited = new List<Vertex<Building>>();
        Queue<Vertex<Building>> queue = new Queue<Vertex<Building>>();
        Dictionary<Vertex<Building>, int> distances = new Dictionary<Vertex<Building>, int>();
        
        foreach (Vertex<Building> powerplant in powerplants)
        {
            // adds the initial nodes to the queue ( powerplants with distance 0 )
            // it is the first layer of the graph
            queue.Enqueue(powerplant);
            visited.Add(powerplant);
            distances[powerplant] = 0;
        }

        // while the queue is not empty
        while (queue.Count > 0)
        {
            // We take the next in the queue
            Vertex<Building> current = queue.Dequeue();
            int currentDistance = distances[current];
            
            // We find all unvisited buildings in range
            List<Vertex<Building>> potentialNeighbors = energyGraph.GetVertices()
                .Where(v => !visited.Contains(v) && IsInRange(current, v))
                .ToList();
            
            //And for each of them 
            foreach (Vertex<Building> neighbor in potentialNeighbors)
            {
                // We check if there already is a path to that build
                if (!distances.ContainsKey(neighbor))
                {
                    // First time we find this vertex, add the edge
                    current.AddNeighbor(neighbor, 1);
                    distances[neighbor] = currentDistance + 1;
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
                // if distances[neighbor] < currentDistance + 1, we do nothing
                // bc it means that a shorter path already exists
            }
        }
    }
    // Check if building2 is in range of building1
    bool IsInRange(Vertex<Building> v1, Vertex<Building> v2)
    {
        Vector2Int pos1 = v1.position;
        Vector2Int pos2 = v2.position;
        
        //Manhattan distance
        int distance = Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        if (v1.label is PowerPlant)
        {
            return distance <= RangesManager.powerPlantEnergyRange;
        }
        else if (v1.label is Tower)
        {
            return distance <= RangesManager.towerEnergyRange;
        } else
        {
            throw new System.Exception("Building type must be PowerPlant or Tower");
        }
    }

    void SetEnergyToBuilding(List<Vertex<Building>> powerplants)
    {
        List<Tower> reachedTowers = new();
        foreach (Vertex<Building> powerPlant in powerplants)
        {
            if (powerPlant.label is PowerPlant pp)
            {
                // we get all the towers linked to the powerplant's tree
                List<Vertex<Building>> towers = new List<Vertex<Building>>();
                Queue<Vertex<Building>> queue = new Queue<Vertex<Building>>();
                queue.Enqueue(powerPlant);
                while (queue.Count > 0)
                {
                    Vertex<Building> cur = queue.Dequeue();
                    foreach(Vertex<Building> v in cur.GetNeighbors().Keys)
                    {
                        queue.Enqueue(v);
                        towers.Add(v);
                    }
                }

                // and we update the power
                if (towers.Count>0){
                    int toGive = pp.PowerOutput / towers.Count();
                    int remainder = pp.PowerOutput % towers.Count();
                    foreach(Vertex<Building> v in towers)
                    {
                        Tower tower = v.label as Tower;
                        tower.SetPower(remainder <= 0 ? toGive : toGive + 1);
                        reachedTowers.Add(tower);
                        remainder--;
                    }
                }
            }
        }
        // handling towers that are not connected (default is no power, but if they are disconnected they would keep their previous power without that)
        foreach(Tower tower in Game.Instance.buildings.Where(b=>b is Tower))
        {
            if (!reachedTowers.Contains(tower))
            {
                tower.SetPower(0);
            }
        }
    }

    void CreateVisualLinks()
    {
        ClearLinks();
        List<Vertex<Building>> powerplants = energyGraph.GetVertices().Where(v=>v.label is PowerPlant).ToList();

        foreach(Vertex<Building> pp in powerplants)
        {
            Queue<Vertex<Building>> queue = new Queue<Vertex<Building>>();
            queue.Enqueue(pp);
            while (queue.Count > 0)
            {
                Vertex<Building> cur = queue.Dequeue();
                foreach(Vertex<Building> v in cur.GetNeighbors().Keys)
                {
                    EnergyLink link = Instantiate(Game.Instance.buildingsPrefabs.energylinks).GetComponent<EnergyLink>();
                    link.SetNodeA(cur.label.transform);
                    link.SetNodeB(v.label.transform);
                    links.Add(link);
                    queue.Enqueue(v);
                }
            }
        }
    }

    void ClearLinks()
    {
        foreach (EnergyLink link in links)
        {
            Destroy(link.gameObject);
        }
        links.Clear();
    }
}
