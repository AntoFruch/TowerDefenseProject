using UnityEngine;

public class HUDManager : MonoBehaviour
{
    
    [SerializeField] private BuildWheelController wheel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BuildWheelController Wheel => wheel;
}
