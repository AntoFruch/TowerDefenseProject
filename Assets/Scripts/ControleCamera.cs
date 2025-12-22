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
    
    //Selection
    [SerializeField]
    private GameObject selectiontile;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");    
        zoomAction = InputSystem.actions.FindAction("Zoom");
        selectiontile.transform.position= new Vector3(0,2,0); 
    }
    // Update is called once per frame
    void Update()
    {

        //Move
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        float Movey = inputVector.y * 0.1f;
        float Movex = inputVector.x * 0.1f;

        cameraTransform.position += new Vector3(Movex, 0, Movex) ;
        cameraTransform.position += new Vector3(-Movey, 0, Movey);
        

        // Zoom
        float zoomInput = zoomAction.ReadValue<float>();
        float zoomAmount = zoomInput * 0.1f; // Adjust zoom speed as needed
        cameraStatic.position += cameraTransform.forward * zoomAmount;



        //RayCast
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cameraTransform.GetComponentInChildren<Camera>().ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            Vector3 CurrentlySelectedPosition=hitObject.transform.position;

            selectiontile.transform.position = CurrentlySelectedPosition + new Vector3(0,0.25f,0);
            

            
        }

        
    
    }
}
