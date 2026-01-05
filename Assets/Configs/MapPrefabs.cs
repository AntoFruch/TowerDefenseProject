using UnityEngine;

[CreateAssetMenu(
    fileName = "MapPrefabs",
    menuName = "PrefabsConfig/Map"
)]
public class MapPrefabs : ScriptableObject
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
    public GameObject endHitParticles;

    [Header("Decorative Tiles")]
    public GameObject singleTree;
    public GameObject duoTrees;
    public GameObject quadTrees;
    public GameObject hill;
    public GameObject crystals;
    
    
}
