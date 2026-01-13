using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        ConnectBuildingsOptimized(powerplants);

        Debug.Log(energyGraph.ToString());
    }

    // Makes the graph as a spanning tree
    void ConnectBuildingsOptimized(List<Vertex<Building>> powerplants)
    {
        List<Vertex<Building>> visited = new List<Vertex<Building>>();
        Queue<Vertex<Building>> queue = new Queue<Vertex<Building>>();
        Dictionary<Vertex<Building>, int> distances = new Dictionary<Vertex<Building>, int>();
        
        foreach (Vertex<Building> powerplant in powerplants)
        {
            queue.Enqueue(powerplant);
            visited.Add(powerplant);
            distances[powerplant] = 0;
        }
        
        while (queue.Count > 0)
        {
            Vertex<Building> current = queue.Dequeue();
            int currentDistance = distances[current];
            
            // Trouver tous les voisins potentiels dans le rayon
            List<Vertex<Building>> potentialNeighbors = energyGraph.GetVertices()
                .Where(v => !visited.Contains(v) && IsInRange(current, v))
                .ToList();
            
            foreach (Vertex<Building> neighbor in potentialNeighbors)
            {
                // Vérifier si on a déjà un chemin vers ce sommet
                if (!distances.ContainsKey(neighbor))
                {
                    // Première fois qu'on atteint ce sommet -> créer l'arête
                    current.AddNeighbor(neighbor, 1);
                    distances[neighbor] = currentDistance + 1;
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
                else if (distances[neighbor] == currentDistance + 1)
                {
                    // Même distance - on peut ajouter cette arête alternative
                    current.AddNeighbor(neighbor, 1);
                }
                // Si distances[neighbor] < currentDistance + 1, on ne fait rien
                // car un chemin plus court existe déjà
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