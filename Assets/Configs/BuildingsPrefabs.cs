using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsPrefabs", menuName = "PrefabsConfig/Buildings")]
public class BuildingsPrefabs : ScriptableObject
{
    [Header("Buildings")]
    public GameObject redTower;
    public GameObject redTowerProjectile;
    public GameObject yellowTower;
    public GameObject yellowTowerProjectile;
    public GameObject blueTower;
    public GameObject blueTowerProjectile;
    public GameObject greenTower;
    public GameObject greenTowerProjectile;
    public GameObject powerPlant;
    public GameObject radar;
    public GameObject factory;
    public GameObject storage;
}
