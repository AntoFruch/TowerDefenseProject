using UnityEngine;

public class RangesManager : MonoBehaviour
{
    public static RangesManager Instance { get; private set; }

    public static int TowerEnergyRange = 3;
    public static int PowerPlantEnergyRange = 4;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}