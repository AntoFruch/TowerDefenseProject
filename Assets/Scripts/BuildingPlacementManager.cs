using System;
using System.Linq;
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
                if (CanPlace(UEExtension.Vector3toVector2Int(selectedBuild.transform.position)))
                {
                    // mettre un indicateur vert
                } else {
                    // mettre un indicateur rouge
                }
            }
        }
    }
    public void StartMoving(Building build)
    {
        selectedBuild = build;
        moving = true;
        Game.Instance.buildings.Remove(selectedBuild);
    }
    public void Place()
    {
        if (CanPlace(UEExtension.Vector3toVector2Int(selectedBuild.transform.position))){
            moving=false;
            selectedBuild.transform.position = Game.Instance.selector.position;
            Game.Instance.buildings.Add(selectedBuild);
        }      
    }

    public static bool CanPlace(Vector2Int pos)
    {
        return !IsPlaceTaken(pos) && Game.Instance.map[pos.y][pos.x] == TileType.CONSTRUCTIBLE;
    }

    public static bool IsPlaceTaken(Vector2Int pos)
    {  
        try
        {
            Debug.Log(pos);
            Debug.Log(Game.Instance.selector.position);
            Game.Instance.buildings.First(
                b => UEExtension.Vector3toVector2Int(b.transform.position) == pos);
            
            return true;

        } catch(InvalidOperationException e)
        {
            return false;
        } 
    }
}