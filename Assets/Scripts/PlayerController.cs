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
    bool openHUD;
    
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
        toggleBuildsHUD();
        if (openHUD)
        {
            cameraController.MoveCam(moveCamAction, dragAction);
            cameraController.Zoom(zoomAction);
            cameraController.MoveSelector();
        }
    }

    void toggleBuildsHUD()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            openHUD = !openHUD;
            if (!openHUD){

                Game.Instance.HUD.ShowWheelAtPosition(Mouse.current.position.ReadValue());
            } else
            {
                Game.Instance.HUD.HideWheel();
            }
        }
    }
}
