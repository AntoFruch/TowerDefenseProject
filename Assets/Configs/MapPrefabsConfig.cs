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

    [Header("Non-Buildable Tiles")]
    public GameObject edgeTile;

    [Header("Start Tiles")]
    public GameObject startTileEnd;
    public GameObject startTileStraight;

    [Header("End Tiles")]
    public GameObject endTileEnd;
    public GameObject endTileStraight;

    [Header("Decorative Tiles")]
    public GameObject singleTree;
    public GameObject duoTrees;
    public GameObject quadTrees;
    public GameObject hill;
    public GameObject crystals;

    [Header("Buildings")]
    public GameObject redTower;
    public GameObject yellowTower;
    public GameObject blueTower;
    public GameObject greenTower;
    public GameObject powerPlant;
    public GameObject radar;
    public GameObject factory;
    public GameObject storage;
    
}
