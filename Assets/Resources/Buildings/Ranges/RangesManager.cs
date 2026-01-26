using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangesManager : MonoBehaviour
{   
    public static RangesManager Instance { get; private set; }
    [SerializeField] private RangeMode mode;


    public static int towerEnergyRange = 3;
    public static int powerPlantEnergyRange = 4;

    public GameObject towerRangeParent{get;private set;}
    GameObject energyRangeParent;
    GameObject boostRangeParent;
    
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

    void Start()
    {
        towerRangeParent = new("TowerRangesParents");
        energyRangeParent = new("EnergyRangesParents");
        boostRangeParent = new("BoostRangesParents");
    }

    int lastBuildingCount = 0;
    void Update()
    {
        if (Game.Instance.buildings.Count != lastBuildingCount)
        {
            ShowRanges();
            lastBuildingCount = Game.Instance.buildings.Count;
        }
    }
    public void SetMode(RangeMode mode)
    {
        this.mode = mode;
        ShowRanges();
    }
    public void ShowRanges()
    {
        LoadEnergyRanges();
        LoadTowerRanges();
        LoadBoostRanges();
        switch (mode)
        {
            case RangeMode.Energy:
                energyRangeParent.SetActive(true);
                break;
            case RangeMode.Towers:
                towerRangeParent.SetActive(true);
                break;
            case RangeMode.Boost:
                boostRangeParent.SetActive(true);
                break;
        }
    }

    void LoadTowerRanges()
    {
        ClearRanges(towerRangeParent);
        foreach(Tower tower in Game.Instance.buildings.Where(b=>b is Tower))
        {
            for(int x = -tower.CurrentRange; x<=tower.CurrentRange; x++)
            {
                for(int y = -tower.CurrentRange; y<=tower.CurrentRange; y++)
                {
                    if (Math.Abs(x)+Math.Abs(y) <= tower.CurrentRange){
                        GameObject child =  Instantiate(Game.Instance.buildingsPrefabs.towerRange,
                                                        tower.transform.position + new Vector3(x,0, y), 
                                                        Quaternion.identity);
                        child.transform.parent = towerRangeParent.transform;
                    }
                }   
            }
        }
        towerRangeParent.SetActive(false);
    }

    void LoadEnergyRanges()
    {
        ClearRanges(energyRangeParent);
        foreach(Building build in Game.Instance.buildings.Where(b=>b is Tower || b is PowerPlant))
        {
            int range = build is Tower ? towerEnergyRange : powerPlantEnergyRange;
            List<GameObject> list = new List<GameObject>();
            for(int x = -range; x<=range; x++)
            {
                for(int y = -range; y<=range; y++)
                {
                    
                    if (Math.Abs(x)+Math.Abs(y) <= range){
                        GameObject child =  Instantiate(Game.Instance.buildingsPrefabs.energyRange,
                                                        build.transform.position + new Vector3(x,0, y), 
                                                        Quaternion.identity);
                        child.transform.parent = energyRangeParent.transform;
                    }
                }   
            }
        }
        energyRangeParent.SetActive(false);
    }

    void LoadBoostRanges()
    {
        ClearRanges(boostRangeParent);
        foreach(Installation install in Game.Instance.buildings.Where(b=>b is Installation))
        {
            for(int x = -install.Range; x<=install.Range; x++)
            {
                for(int y = -install.Range; y<=install.Range; y++)
                {
                    if (Math.Abs(x)+Math.Abs(y) <= install.Range){
                        GameObject child = Instantiate(Game.Instance.buildingsPrefabs.boostRange,
                                                            install.transform.position + new Vector3(x,0, y), 
                                                            Quaternion.identity);
                        child.transform.parent = boostRangeParent.transform;
                    }
                }
            }
        }
        boostRangeParent.SetActive(false);
    }

    void ClearRanges(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

public enum RangeMode
{
    Towers, Energy, Boost, None
}
