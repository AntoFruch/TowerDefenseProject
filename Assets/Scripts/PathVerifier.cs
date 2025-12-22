using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathVerifier
{
    // *********************** Creation du graphe ************************************************

    // lève des exceptions en cas de :
    // - Chemin qui ne finit pas sur une intersection
    // - Pas de 
    public static Graph<VertexLabel> CreatePathGraph(TileType[][] map)
    {
        
        Graph<VertexLabel> graph = new Graph<VertexLabel>();

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

        List<Vertex<VertexLabel>> visited = new List<Vertex<VertexLabel>>();
        foreach (Vertex<VertexLabel> vertex in graph.GetVertices())
        {
            GraphSearch(graph, map, visited, vertex);
        }

        return graph;
    }

    // Parcours de graphe pour les aretes à partir d'une arete étiquettée START
    private static void GraphSearch(Graph<VertexLabel> graph, TileType[][] map, List<Vertex<VertexLabel>> visited, Vertex<VertexLabel> vertex)
    {
        if (!visited.Contains(vertex))
        {
            //Debug.Log("GraphSearch "+vertex.position);
            visited.Add(vertex);

            Vector2Int[] directions = new Vector2Int[4] {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            foreach (Vector2Int dir in directions)
            {
                BuildEdgeAlongPath(graph, map, vertex, vertex.position, dir, 0);
            }
            foreach (Vertex<VertexLabel> neighbor in vertex.GetNeighbors().Keys)
            {
                GraphSearch(graph , map, visited, neighbor);
            }

        } else return ;
        
    }

    private static void BuildEdgeAlongPath(Graph<VertexLabel> graph, TileType[][] map,Vertex<VertexLabel> vertex, Vector2Int pos, Vector2Int dir, int edgeLength)
    {
        Vector2Int nextpos = pos + dir;
        TileType next = map[nextpos.y][nextpos.x];
        edgeLength+=1;

        if (next == TileType.PATH)
        {
            BuildEdgeAlongPath(graph, map, vertex, nextpos, dir, edgeLength);
        } else if (next == TileType.INTERSECTION || next == TileType.END ||next == TileType.SPAWN )
        {   
            //Debug.Log(graph);
            Vertex<VertexLabel> nextVertex = graph.GetVertices().Where(v => v.position == nextpos).ToList()[0];
            vertex.AddNeighbor(nextVertex, edgeLength);
            nextVertex.AddNeighbor(vertex, edgeLength);
        } else if (map[pos.y][pos.x] == TileType.PATH)  
        {
            // si la tuile suivante est ni un chemin, ni une fin, ni un debut, ni une intersection, 
            // et que la tuiel d'avant était un chemin ( chemin deja commencé )
            // alors le chemin rectiligne ne finit pas sur une intersection => problème.
            throw new System.Exception("Unexpected end of path at "+ nextpos);
        } else return ;
    }
    
    // *********************** Vérification du Graphe ************************************************
    // * Il y a plusieurs points à prendre en compte :
    // *    - existence d'une zone de sortie et une d'entrée au moins.
    // *    - existence d'un chemin entrée/sortie poru chaque entrée.
    // ***********************************************************************************************

    // leve des exceptions en fontions des violations des 2 regles ci-dessus.
    public static void IsValidGraph(Graph<VertexLabel> graph)
        {
            if (!HasEntryAndExitAtLeast(graph))
            {
            throw new System.Exception("No entry or no destination, there should be at least one entry and one destination");  
            } else if (!HasPathBetweenEntryAndExit(graph))
            {
            throw new System.Exception("No valid path from an entry leading to an end, there should be at least one valid path leading to an end for each start");  
            }
        }
    private static bool HasEntryAndExitAtLeast(Graph<VertexLabel> graph)
    {
        List<Vertex<VertexLabel>> starts = graph.GetVertices()
                                    .Where(v => v.label == VertexLabel.START)
                                    .ToList();
        List<Vertex<VertexLabel>> ends = graph.GetVertices()
                                    .Where(v => v.label == VertexLabel.END)
                                    .ToList();
        return starts.Count != 0 && ends.Count != 0;
    }

    private static bool HasPathBetweenEntryAndExit(Graph<VertexLabel> graph)
    {
        List<Vertex<VertexLabel>> starts = graph.GetVertices()
                                    .Where(v => v.label == VertexLabel.START)
                                    .ToList();

        bool everybodyHasEnd = true;
        
        foreach (Vertex<VertexLabel> start in starts)
        {
            everybodyHasEnd = everybodyHasEnd && RecursiveSearchPathToEnd(new List<Vertex<VertexLabel>>(), start); 
        }
        return everybodyHasEnd;
    }

    private static bool RecursiveSearchPathToEnd(List<Vertex<VertexLabel>> visited, Vertex<VertexLabel> from)
        {
            if (visited.Contains(from))
                return false;

            if (from.label == VertexLabel.END)
                return true;

            visited.Add(from);

            foreach (var neighbor in from.GetNeighbors().Keys)
            {
                if (RecursiveSearchPathToEnd(visited, neighbor))
                    return true;
            }

            return false;
    }
}