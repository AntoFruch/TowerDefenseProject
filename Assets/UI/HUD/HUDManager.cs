using UnityEngine;

public class HUDManager : MonoBehaviour
{
    
    [SerializeField] private BuildWheelController wheel;
    public BuildWheelController Wheel => wheel;
    [SerializeField] private DelMovController delMov;
    public DelMovController DelMov => delMov;

    [SerializeField] private StaticHUDController staticHUD;
    public StaticHUDController StaticHUD => staticHUD;
    
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
        if (BuildingPlacementManager.IsPlaceTaken(UEExtension.Vector3toVector2Int(Game.Instance.selector.position)))
        {
            delMov.Show(pos);
        } else
        {
            wheel.ShowWheelAtPosition(pos);
        }
    }
}
