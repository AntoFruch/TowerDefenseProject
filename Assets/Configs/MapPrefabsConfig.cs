using UnityEngine;

[CreateAssetMenu(
    fileName = "MapPrefabsConfig",
    menuName = "Config/Map Prefabs"
)]
public class MapPrefabsConfig : ScriptableObject
{
    public GameObject cornerPath;
    public GameObject straightPath;
    public GameObject splitPath;
    public GameObject crossPath;

    public GameObject constructibleTile;
    public GameObject edgeTile;

    public GameObject startTileEnd;
    public GameObject startTileStraight;

    public GameObject endTileEnd;
    public GameObject endTileStraight;
    
}
