using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    private Graph<Building> energyGraph;


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
        foreach (Vertex<Building> pp in powerplants)
        {
            List<Vertex<Building>> unvisited = energyGraph.GetVertices().Where(v => v != pp).ToList();
            ConnectBuildings(unvisited, pp);
        }

        Debug.Log(energyGraph.ToString());
    }

    // recursive function to connect buildings in the graph based on their energy range
    void ConnectBuildings(List<Vertex<Building>> unvisited, Vertex<Building> current)
    {
        unvisited.Remove(current);
        foreach (Vertex<Building> vertex in unvisited)
        {
            if (IsInRange(current, vertex))
            {
                current.AddNeighbor(vertex);
            }
        }
        foreach (Vertex<Building> neighbor in current.GetNeighbors().Keys)
        {
            if (unvisited.Contains(neighbor))
            {
                ConnectBuildings(unvisited, neighbor);
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
            return distance <= RangesManager.PowerPlantEnergyRange;
        }
        else if (v1.label is Tower)
        {
            return distance <= RangesManager.TowerEnergyRange;
        } else
        {
            throw new System.Exception("Building type must be PowerPlant or Tower");
        }
    }

}