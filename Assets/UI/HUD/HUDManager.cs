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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWheelMenu(Vector2 pos)
    {
        Vector2Int selectorPos = UEExtension.Vector3toVector2Int(Game.Instance.selector.position);
        try
        {
            Building build = Game.Instance.buildings.First(b => UEExtension.Vector3toVector2Int(b.transform.position)==selectorPos);
            delMov.Show(pos, build);
        } catch (InvalidOperationException e)
        {
            wheel.ShowWheelAtPosition(pos);
        }
    }
}
