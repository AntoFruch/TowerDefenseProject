using UnityEngine;

[CreateAssetMenu(
    fileName = "MapPrefabsConfig",
    menuName = "Config/Map Prefabs"
)]
public class MapPrefabsConfig : ScriptableObject
{
    [Header("Path Tiles")]
    public GameObject cornerPath;
    public GameObject straightPath;
    public GameObject splitPath;
    public GameObject crossPath;

    [Header("Buildable Tiles")]
    public GameObject constructibleTile;
    public GameObject edgeTile;

    [Header("Start Tiles")]
    public GameObject startTileEnd;
    public GameObject startTileStraight;

    [Header("End Tiles")]
    public GameObject endTileEnd;
    public GameObject endTileStraight;
}
