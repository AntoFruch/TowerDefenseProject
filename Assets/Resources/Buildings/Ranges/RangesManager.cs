using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class RangesManager : MonoBehaviour
{   
    public static RangesManager Instance { get; private set; }
    public RangesMode mode{get;private set;}

    public static int towerEnergyRange = 3;
    public static int powerPlantEnergyRange = 4;

    public Dictionary<Building, List<GameObject>> ranges;
    
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
        ranges = new Dictionary<Building, List<GameObject>>();
        mode = RangesMode.None;
    }
    void SetMode(RangesMode mode)
    {
        this.mode = mode;
    }
    public void DrawRanges()
    {
        switch (mode)
        {
            case RangesMode.Energy:
                DrawEnergyRanges();
                break;
            case RangesMode.Towers:
                DrawTowerRanges();
                break;
            case RangesMode.Boost:
                DrawBoostRanges();
                break;
        }
    }

    void DrawTowerRanges()
    {
        ClearRanges();
        foreach(Tower tower in Game.Instance.buildings.Where(b=>b is Tower))
        {
            List<GameObject> list = new List<GameObject>();
            for(int x = -tower.Range; x<=tower.Range; x++)
            {
                for(int y = -tower.Range; y<=tower.Range; y++)
                {
                    
                    if (Math.Abs(x)+Math.Abs(y) <= tower.Range){
                        list.Add(
                            Instantiate(Game.Instance.buildingsPrefabs.towerRange,
                                        tower.transform.position + new Vector3(x,0, y), 
                                        Quaternion.identity)
                                        );
                    }
                }   
            }
            ranges.Add(tower, list);
        }
    }

    void DrawEnergyRanges()
    {
        ClearRanges();
        foreach(Building build in Game.Instance.buildings.Where(b=>b is Tower || b is PowerPlant))
        {
            int range = build is Tower ? towerEnergyRange : powerPlantEnergyRange;
            List<GameObject> list = new List<GameObject>();
            for(int x = -range; x<=range; x++)
            {
                for(int y = -range; y<=range; y++)
                {
                    
                    if (Math.Abs(x)+Math.Abs(y) <= range){
                        list.Add(
                            Instantiate(Game.Instance.buildingsPrefabs.energyRange,
                                        build.transform.position + new Vector3(x,0, y), 
                                        Quaternion.identity)
                                        );
                    }
                }   
            }
            ranges.Add(build, list);
        }
    }

    void DrawBoostRanges()
    {
        
    }

    void ClearRanges()
    {
        foreach(var key in ranges.Keys)
        {
            foreach(var go in ranges[key])
            {
                Destroy(go);
            }
        }
        ranges.Clear();
    }
}

public enum RangesMode
{
    Towers, Energy, Boost, None
}