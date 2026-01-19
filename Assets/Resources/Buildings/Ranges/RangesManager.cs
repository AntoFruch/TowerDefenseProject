using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangesManager : MonoBehaviour
{   
    public static RangesManager Instance { get; private set; }

    public static int TowerEnergyRange = 3;
    public static int PowerPlantEnergyRange = 4;

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
    }
    public void DrawRanges()
    {
        // eventuellement mettre un toggle pour passer de vue energie a vue tours
        DrawTowerRanges();
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