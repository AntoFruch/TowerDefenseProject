using UnityEngine;

[System.Serializable] 
public struct BuildingCosts
{
    [Header("Towers")]
    public int redTower;
    public int blueTower;
    public int greenTower;
    public int yellowTower;

    [Header("Installations")]
    public int radar;
    public int factory;
    public int powerPlant;
    public int storage;
}
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private bool infiniteMoney;
    [SerializeField] protected int currentMoney = 200;

    public BuildingCosts costs;


    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {

        if (infiniteMoney)
        {
            currentMoney = int.MaxValue;
        }
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public int GetCost(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.RedTower: return costs.redTower;
            case BuildingType.BlueTower: return costs.blueTower;
            case BuildingType.GreenTower: return costs.greenTower;
            case BuildingType.YellowTower: return costs.yellowTower;
            case BuildingType.Radar: return costs.radar;
            case BuildingType.Factory: return costs.factory;
            case BuildingType.PowerPlant: return costs.powerPlant;
            case BuildingType.Storage: return costs.storage;
            default: return 0;
        }
    }
    public void AddMoney(int amount)
    {
        GameStatsManager.Instance.AddScoreStats(amount);
        currentMoney += amount;
    }

    public bool SpendMoney(int amount) {
        if (currentMoney >= amount) {
            currentMoney -= amount;
            GameStatsManager.Instance.AddMoneySpent(amount);
            return true;
        }

        else
        {
            return false;
        }
    }
}
