using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    
    [SerializeField] private BuildWheelController wheel;
    public BuildWheelController Wheel => wheel;
    [SerializeField] private DelMovController delMov;
    public DelMovController DelMov => delMov;
    


    public void ShowWheelMenu(Vector2 pos)
    {
        if (BuildingPlacementManager.IsPlaceTaken(UEExtension.Vector3toVector2Int(Game.Instance.selector.position)))
        {
            delMov.Show(pos);
        } else
        {
            wheel.ShowWheelAtPosition(pos);
        }
    }
}
