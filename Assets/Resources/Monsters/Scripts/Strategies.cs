using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public static class Strategies
{
    public static List<Vertex<VertexLabel>> ShortestPath(Graph<VertexLabel> graph, Vertex<VertexLabel> start)
    {
        if (!graph.GetVertices().Contains(start))
        {
            throw new System.Exception(start.position+" vertex is not a vertex of the given graph");
        }
        
        // unvisited vertices
        List<Vertex<VertexLabel>> unvisited = new List<Vertex<VertexLabel>>(graph.GetVertices());
        // distances from start to key vertex
        Dictionary<Vertex<VertexLabel>, int> distances = new Dictionary<Vertex<VertexLabel>, int>();
        // previous vertex trace
        Dictionary<Vertex<VertexLabel>, Vertex<VertexLabel>> previous = new Dictionary<Vertex<VertexLabel>, Vertex<VertexLabel>>();

        // all distances at +inf, start is 0 away from itself. 
        foreach (Vertex<VertexLabel> v in graph.GetVertices())
        {
            distances[v] = int.MaxValue;
        }
        distances[start] = 0; 
        
        // Main Loop, i just translated in C# the pseudo-code given at "https://fr.wikipedia.org/wiki/Algorithme_de_Dijkstra#Sch%C3%A9ma_de_l'algorithme" 
        while (unvisited.Count != 0)
        {
            Vertex<VertexLabel> minDistanceVertex = unvisited.OrderBy(v => distances[v]).First();
            unvisited.Remove(minDistanceVertex);
            foreach(Vertex<VertexLabel> vertex in minDistanceVertex.GetNeighbors().Keys)
            {
                if (unvisited.Contains(vertex))
                {
                    int newDist = distances[minDistanceVertex] + minDistanceVertex.GetNeighbors()[vertex];
                    if (distances[vertex] > newDist)
                    {
                        distances[vertex] = newDist;
                        previous[vertex] = minDistanceVertex;
                    }
                }
            }
        }

        // Now we have the distances to every vertex, we can get the nearest end vertex with Linq
        Vertex<VertexLabel> endVertex =
            graph.GetVertices()
                .Where(v => v.label == VertexLabel.END)
                .OrderBy(v => distances[v])
                .First();

        // And recreate the path thanks to the "previous" Dictionary
        List<Vertex<VertexLabel>> path = new List<Vertex<VertexLabel>>();

        Vertex<VertexLabel> cur = endVertex;
        while (cur != start)
        {
            path.Add(cur);
            cur = previous[cur];   
        }
        path.Add(start);
        path.Reverse();
        
        return path;
    }

    // Shortest path weighted by how much of the path is covered by towers' range
    public static List<Vertex<VertexLabel>> MinimalTowerOverlap(Graph<VertexLabel> graph, Vertex<VertexLabel> start)
    {
        if (!graph.GetVertices().Contains(start))
        {
            throw new System.Exception(start.position+" vertex is not a vertex of the given graph");
        }
        
        // unvisited vertices
        List<Vertex<VertexLabel>> unvisited = new List<Vertex<VertexLabel>>(graph.GetVertices());
        // distances from start to key vertex
        Dictionary<Vertex<VertexLabel>, int> distances = new Dictionary<Vertex<VertexLabel>, int>();
        // previous vertex trace
        Dictionary<Vertex<VertexLabel>, Vertex<VertexLabel>> previous = new Dictionary<Vertex<VertexLabel>, Vertex<VertexLabel>>();

        // all distances at +inf, start is 0 away from itself. 
        foreach (Vertex<VertexLabel> v in graph.GetVertices())
        {
            distances[v] = int.MaxValue;
        }
        distances[start] = 0; 
        
        // Main Loop, i just translated in C# the pseudo-code given at "https://fr.wikipedia.org/wiki/Algorithme_de_Dijkstra#Sch%C3%A9ma_de_l'algorithme" 
        while (unvisited.Count != 0)
        {
            Vertex<VertexLabel> minDistanceVertex = unvisited.OrderBy(v => distances[v]).First();
            unvisited.Remove(minDistanceVertex);
            foreach(Vertex<VertexLabel> vertex in minDistanceVertex.GetNeighbors().Keys)
            {
                if (unvisited.Contains(vertex))
                {
                    int newDist = distances[minDistanceVertex] + minDistanceVertex.GetNeighbors()[vertex]*Overlap(minDistanceVertex, vertex);
                    if (distances[vertex] > newDist)
                    {
                        distances[vertex] = newDist;
                        previous[vertex] = minDistanceVertex;
                    }
                }
            }
        }

        // Now we have the distances to every vertex, we can get the nearest end vertex with Linq
        Vertex<VertexLabel> endVertex =
            graph.GetVertices()
                .Where(v => v.label == VertexLabel.END)
                .OrderBy(v => distances[v])
                .First();

        // And recreate the path thanks to the "previous" Dictionary
        List<Vertex<VertexLabel>> path = new List<Vertex<VertexLabel>>();

        Vertex<VertexLabel> cur = endVertex;
        while (cur != start)
        {
            path.Add(cur);
            cur = previous[cur];   
        }
        path.Add(start);
        path.Reverse();
        
        return path;
    }

    private static int Overlap(Vertex<VertexLabel> v1, Vertex<VertexLabel> v2)
    {
        if (!v1.GetNeighbors().Keys.Contains(v2))
        {
            throw new System.Exception("Vertex "+v2+" must be a neighbor of vertex "+v1);
        }

        // counter of the number of tiles that overlap the path ( float in order to return a float then, with an int we would have done a integers division which is not what we want)
        int count = 0;
        Vector2Int path = v1.position - v2.position;
        foreach(Transform child in RangesManager.Instance.towerRangeParent.transform)
        {
            Vector2Int childPos = UEExtension.Vector3toVector2Int(child.position);
            Vector2Int relativeChildPos = v1.position - childPos;

            bool cross = path.x*relativeChildPos.y - path.y*relativeChildPos.x == 0;
            bool withinX = childPos.x >= Mathf.Min(v1.position.x, v2.position.x) && childPos.x <= Mathf.Max(v1.position.x, v2.position.x);
            bool withinY = childPos.y >= Mathf.Min(v1.position.y, v2.position.y) && childPos.y <= Mathf.Max(v1.position.y, v2.position.y);

            if (cross && withinX && withinY)
            {
                count++;
            }
        }

        return count;
    }
}