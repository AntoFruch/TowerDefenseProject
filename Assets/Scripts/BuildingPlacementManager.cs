using System;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
public class BuildingPlacementManager : MonoBehaviour
{
    public static BuildingPlacementManager Instance;

    private Building selectedBuild;
    public bool moving{get;private set;} = false;
    private Vector3 hover;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hover = new Vector3(0,0.3f,0);
    }

    void Update()
    {
        if (selectedBuild!=null)
        {
            if (moving)
            {
                
                selectedBuild.transform.position = Game.Instance.selector.position + hover;
                if (IsValidPosition())
                {
                    // mettre un indicateur vert
                } else {
                    // mettre un indicateur rouge
                }
            }
            Debug.Log(IsValidPosition()+" "+ UEExtension.Vector3toVector2Int(selectedBuild.transform.position) );}
    }
    public void StartMoving(Building build)
    {
        selectedBuild = build;
        moving = true;
    }
    public void Place()
    {
        if (IsValidPosition()){
            moving=false;
            selectedBuild.transform.position = Game.Instance.selector.position;
            Game.Instance.buildings.Remove(selectedBuild);
            Game.Instance.buildings.Add(selectedBuild);
        }      
    }
    
    public bool IsValidPosition()
    {
        Vector2Int selectedBuildPos = UEExtension.Vector3toVector2Int(selectedBuild.transform.position);
        if (Game.Instance.map[selectedBuildPos.y][selectedBuildPos.x] != TileType.CONSTRUCTIBLE)
        {
            return false;
        }
        
        try
        {
            Building build = Game.Instance.buildings.First(
                b => UEExtension.Vector3toVector2Int(b.transform.position) == selectedBuildPos // meme position qu'un batiment existant
                && b != selectedBuild);   // different de lui même
            
            return false;

        } catch(InvalidOperationException e)
        {
            return true;
        }
    }
}