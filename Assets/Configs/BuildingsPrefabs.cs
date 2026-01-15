using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsPrefabs", menuName = "PrefabsConfig/Towers & Buildings")]
public class BuildingsPrefabs : ScriptableObject
{
    [Header("Towers")]
    public GameObject redTower;
    public GameObject yellowTower;
    public GameObject blueTower;
    public GameObject greenTower;
    
    [Header("Buildings")]
    public GameObject powerPlant;
    public GameObject radar;
    public GameObject factory;
    public GameObject storage;

    [Header("Ranges")]
    public GameObject towerRange;
    public GameObject energyRange;
    public GameObject boostRange;
}
