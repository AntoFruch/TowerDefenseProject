using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph<T>
{
    List<Vertex<T>> vertices ;

    public Graph()
    {
        vertices = new List<Vertex<T>>();
    }
    public void AddVertex(T label, Vector2Int pos)
    {
        Vertex<T> vertex = new Vertex<T>(label, pos);
        this.vertices.Add(vertex);
    }
    public List<Vertex<T>> GetVertices()
    {
        return vertices;
    }

    public override string ToString()
    {
        if (vertices.Count == 0)
            return "Graph: (empty)";
        
        string str = $"Graph ({vertices.Count} vertices):\n";
        foreach (Vertex<T> vertex in vertices)
        {
            str += "  " + vertex.ToString() + "\n";
        }
        return str;
    }
}

public class Vertex<T>
{
    private Dictionary<Vertex<T>, int> neighbors;
    public T label {get; private set;}
    public Vector2Int position {get; private set;}  

    public Vertex(T label,Vector2Int pos)
    {
        this.neighbors = new Dictionary<Vertex<T>, int>();
        this.label = label;
        this.position = pos;
    }

    public void AddNeighbor(Vertex<T> neighbor, int distance)
    {
        if (!neighbors.ContainsKey(neighbor))
        {
            neighbors.Add(neighbor, distance);
        }
    }

    public Dictionary<Vertex<T>, int> GetNeighbors()
    {
        return neighbors;
    }

    public override string ToString()
    {
        string neighborInfo = neighbors.Count > 0 
            ? $" -> {neighbors.Count} neighbor(s): [{string.Join(", ", neighbors.Select(n => $"{n.Key.label}(dist:{n.Value})"))}]"
            : " (no neighbors)";
        
        return $"[{label} at {position}]{neighborInfo}";
    }
}

