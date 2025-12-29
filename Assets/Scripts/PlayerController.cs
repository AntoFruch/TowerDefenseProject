using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ControleCamera cameraController;
    
    // Actions
    InputAction selectAction;
    InputAction moveCamAction;
    InputAction dragAction;
    InputAction zoomAction;
    

    // States
    bool hUDWheelActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectAction = InputSystem.actions.FindAction("Interact");
        moveCamAction = InputSystem.actions.FindAction("Move");    
        zoomAction = InputSystem.actions.FindAction("Zoom");
        dragAction = InputSystem.actions.FindAction("MouseMove");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
        
        if (BuildingPlacementManager.Instance.moving && selectAction.WasPerformedThisFrame())
        {
            BuildingPlacementManager.Instance.Place();
        }else if (!hUDWheelActive)
        {
            toggleBuildsHUD();
            cameraController.MoveCam(moveCamAction, dragAction);
            cameraController.Zoom(zoomAction);
            cameraController.MoveSelector();
        } 
    }

    void toggleBuildsHUD()
    {
        if (selectAction.WasPerformedThisFrame() && Game.Instance.selector.position.y > 0)
        {
            Game.Instance.HUD.ShowWheelMenu(Mouse.current.position.ReadValue());
        }
    }
    void UpdateStates()
    {
        hUDWheelActive = Game.Instance.HUD.Wheel.active || Game.Instance.HUD.DelMov.active;
    }
}