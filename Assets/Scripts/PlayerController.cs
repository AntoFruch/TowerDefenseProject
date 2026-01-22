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
        if (Game.Instance.state == GameState.Preparation){
            if (BuildingPlacementManager.Instance.moving && selectAction.WasPerformedThisFrame())
            {
                BuildingPlacementManager.Instance.Place();
            }else if (!hUDWheelActive)
            {
                ToggleBuildsHUD();
                cameraController.MoveCam(moveCamAction, dragAction);
                cameraController.Zoom(zoomAction);
                cameraController.MoveSelector();
            }
        } else if (Game.Instance.state == GameState.Defense)
        {
            cameraController.MoveCam(moveCamAction, dragAction);
            cameraController.Zoom(zoomAction);
        }
    }

    void ToggleBuildsHUD()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        // selector enabled and click => position is correct and menu can be opened
        if (selectAction.WasPerformedThisFrame() && Game.Instance.selector.GetComponent<Renderer>().enabled)
        {
            Game.Instance.HUD.ShowWheelMenu(mousePos);
        }
    }
    void UpdateStates()
    {
        hUDWheelActive = Game.Instance.HUD.Wheel.active || Game.Instance.HUD.DelMov.active;
    }
}