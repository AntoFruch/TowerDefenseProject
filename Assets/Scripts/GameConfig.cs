using System;
using Unity.VisualScripting;
using UnityEngine;

public static class GameConfig
{
    public static TileType[][] map;
    public static Graph<VertexLabel> graph;
    public static void LoadMap(string selectedMapPath)
    {
        TileType[][] map = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D(selectedMapPath));
        Graph<VertexLabel> graph = PathVerifier.CreatePathGraph(map);
        PathVerifier.IsValidGraph(graph);

        GameConfig.map = map;
        GameConfig.graph = graph; 
    }
}
