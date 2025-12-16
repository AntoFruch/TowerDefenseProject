using UnityEngine;
using UnityEngine.InputSystem;


public class ControleCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    //Move
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputAction moveAction;

    //Zoom
    [SerializeField]
    private InputAction zoomAction;
    [SerializeField]
    private Transform cameraStatic;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");    
        zoomAction = InputSystem.actions.FindAction("Zoom");
    }
    // Update is called once per frame
    void Update()
    {

        //Move
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        float Movey = inputVector.y * 0.1f;
        float Movex = inputVector.x * 0.1f;

        cameraTransform.position += new Vector3(Movex, 0, 0) ;
        cameraTransform.position += new Vector3(0, 0, Movey);

        // Zoom
        float zoomInput = zoomAction.ReadValue<float>();
        float zoomAmount = zoomInput * 1f; // Adjust zoom speed as needed
        cameraStatic.position += cameraStatic.forward * zoomAmount;
        
    }
}
