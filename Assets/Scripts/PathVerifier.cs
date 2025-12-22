using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathVerifier
{
    public enum VertexLabel
    {
        START, END, INTERSECTION
    }

    private static TileType[][] map;
    private static Graph<VertexLabel> graph;

    public static Graph<VertexLabel> CreatePathGraph(TileType[][] map)
    {
        PathVerifier.map = map;
        
        PathVerifier.graph = new Graph<VertexLabel>();

        // On recupère tout les sommets du graphe
        for (int y=0; y<map.Length; y++){
            for (int x=0;x<map[0].Length; x++)
            {
                TileType tile = map[y][x];
                if (tile == TileType.SPAWN)
                {
                    //Debug.Log("Spawn at ("+x+","+y+")");
                    graph.AddVertex(VertexLabel.START, new Vector2Int(x,y));

                } else if (tile == TileType.INTERSECTION )
                {
                    graph.AddVertex(VertexLabel.INTERSECTION, new Vector2Int(x,y));

                } else if (tile == TileType.END)
                {
                    graph.AddVertex(VertexLabel.END, new Vector2Int(x,y));
                }
            } 
        }
    
        var startVertices = graph.GetVertices()
                         .Where(v => v.GetLabel() == VertexLabel.START)
                         .ToList();
        
        if (startVertices.Count != 0)
        {
            Vertex<VertexLabel> start = startVertices[0];
            GraphSearch(new List<Vertex<VertexLabel>>(), start);    
        } else
        {
            throw new System.Exception("No spawn tile in the map");
        }

        return PathVerifier.graph;
    }

    // Parcours de graphe pour les aretes à partir d'une arete étiquettée START
    private static void GraphSearch(List<Vertex<VertexLabel>> visited, Vertex<VertexLabel> vertex)
    {
        if (!visited.Contains(vertex))
        {
            //Debug.Log("GraphSearch "+vertex.position);
            visited.Add(vertex);

            Vector2Int[] directions = new Vector2Int[4] {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            foreach (Vector2Int dir in directions)
            {
                BuildEdgeAlongPath(vertex, vertex.position, dir, 0);
            }
            foreach (Vertex<VertexLabel> neighbor in vertex.GetNeighbors().Keys)
            {
                GraphSearch(visited, neighbor);
            }

        } else return ;
        ;
        
    }

    private static void BuildEdgeAlongPath(Vertex<VertexLabel> vertex, Vector2Int pos, Vector2Int dir, int edgeLength)
    {
        Vector2Int nextpos = pos + dir;
        TileType next = map[nextpos.y][nextpos.x];
        edgeLength+=1;

        if (next == TileType.PATH)
        {
            BuildEdgeAlongPath(vertex, nextpos, dir, edgeLength);
        } else if (next == TileType.INTERSECTION || next == TileType.END ||next == TileType.SPAWN )
        {   
            //Debug.Log(graph);
            Vertex<VertexLabel> nextVertex = graph.GetVertices().Where(v => v.position == nextpos).ToList()[0];
            vertex.AddNeighbor(nextVertex, edgeLength);
            nextVertex.AddNeighbor(vertex, edgeLength);
        } else if (map[pos.y][pos.x] == TileType.PATH)
        {
            throw new System.Exception("Unexpected end of path at "+ nextpos);
        } else return ;
    }
    
}