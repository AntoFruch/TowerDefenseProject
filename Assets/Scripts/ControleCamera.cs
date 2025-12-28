using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;


public class ControleCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    //Move
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputAction moveAction;
    [SerializeField]
    private float moveSpeed = 5f;

    //Zoom
    [SerializeField]
    private InputAction zoomAction;
    [SerializeField]
    private Transform cameraStatic;
    [SerializeField]
    private float zoomSpeed = 4f;
    
    //Selection
    [SerializeField]
    private GameObject selectiontile;

    //MouseMove 
    [SerializeField]
    private InputAction mousemoveAction;
    private Vector3 dragOrigin;
    [SerializeField]
    private InputActionAsset inputActions;

    //J'essaie de réparer l'InputAction 
   
    void Awake()
{
    var gameplay = inputActions.FindActionMap("Player", true);

    moveAction = gameplay.FindAction("Move", true);
    zoomAction = gameplay.FindAction("Zoom", true);
    mousemoveAction = gameplay.FindAction("MouseMove", true);

}


    void OnEnable()
    {
        moveAction.Enable();
        zoomAction.Enable();
        mousemoveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        zoomAction.Disable();
        mousemoveAction.Disable();
    }
    void Update()

    {   
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
    {
        Debug.Log("Le clavier est bien détecté par Unity !");
    }


        //MouseMove
        if (mousemoveAction.WasPressedThisFrame())
        {
            dragOrigin = GetMouseWorldPosition();
        }
        if (mousemoveAction.IsPressed())
        {
            Vector3 difference = dragOrigin - GetMouseWorldPosition();
            cameraTransform.position += difference;
        }

        //Move
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        
        float Movey = inputVector.y * moveSpeed * Time.deltaTime;
        float Movex = inputVector.x * moveSpeed * Time.deltaTime;

        cameraTransform.position += new Vector3(Movex, 0, Movex) ;
        cameraTransform.position += new Vector3(-Movey, 0, Movey);
        

        // Zoom
        float zoomInput = zoomAction.ReadValue<float>();
        float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
        cameraStatic.position += cameraTransform.forward * zoomAmount;



        //RayCast
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cameraTransform.GetComponentInChildren<Camera>().ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            GameObject hitObject = hit.collider.gameObject;
            float z = hitObject.transform.position.z;
            float x = hitObject.transform.position.x;
            int X = Mathf.RoundToInt(x);
            int Y = Mathf.RoundToInt(z);
            



            
            
            
            //if (hitObject.name == "tile_with_collider(Clone)")
            if (MapGenerator.map[Y][X] == TileType.CONSTRUCTIBLE)
            {
                Vector3 CurrentlySelectedPosition=hitObject.transform.position;

                selectiontile.transform.position = CurrentlySelectedPosition + new Vector3(0,0.25f,0);    
            }

            else 
            {
                selectiontile.transform.position= new Vector3(0,100,0); 
            }

            

        }

        
    
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cameraTransform.GetComponentInChildren<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }

        return Vector3.zero;
    }


}
