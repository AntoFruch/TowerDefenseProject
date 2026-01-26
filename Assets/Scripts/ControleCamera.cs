using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControleCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    //Move
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float moveSpeed = 5f;

    //Zoom
    [SerializeField]
    private Transform cameraStatic;
    [SerializeField]
    private float zoomSpeed = 4f;
    [SerializeField]
    private float minHeight = 6f;
    [SerializeField]
    private float maxHeight = 10f;
    
    //Selection
    [SerializeField]
    private GameObject selectiontile;

    //MouseMove 
    private Vector3 dragOrigin;

    void Start()
    {
        selectiontile.transform.position= new Vector3(0,2,0); 
    }
    // Update is called once per frame
    void Update()
    {   
    
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

    public void MoveCam(InputAction move, InputAction drag)
    {
        // Move with ZQSD
        Vector2 inputVector = move.ReadValue<Vector2>();
        float Movey = inputVector.y * moveSpeed * Time.deltaTime;
        float Movex = inputVector.x * moveSpeed * Time.deltaTime;

        Vector3 newPos;
        
        newPos = cameraTransform.position + new Vector3(Movex-Movey, 0, Movex+Movey); // for a diagonal movement

        // Move with mouse drag
        if (drag.WasPressedThisFrame())
        {
            dragOrigin = GetMouseWorldPosition();
        }
        if (drag.IsPressed())
        {
            Vector3 difference = dragOrigin - GetMouseWorldPosition();
            newPos = cameraTransform.position + difference;
        }

        //Clamping
        int padding = 2;
        int minX = padding;
        int maxX = Game.Instance.map[0].Length-padding;
        int minZ = padding;
        int maxZ = Game.Instance.map.Length-padding;
        
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);
        
        cameraTransform.position = newPos;
    }

    public void Zoom(InputAction action)
    {
        float zoomInput = action.ReadValue<float>();
        float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
        Vector3 targetPos = cameraStatic.position + (cameraTransform.forward * zoomAmount);

        if (targetPos.y < minHeight)
        {
            return;
        }
        else if (targetPos.y > maxHeight)
        {
            return;
        }

        cameraStatic.position = targetPos;

    }

    public void MoveSelector()
    {
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

            if (PauseMenu.isPaused == false)
            {
                try {
                if (Game.Instance.map[Y][X] == TileType.CONSTRUCTIBLE)
                {
                    selectiontile.GetComponent<Renderer>().enabled = true;
                }
                else 
                {
                    selectiontile.GetComponent<Renderer>().enabled = false;
                }
                Vector3 CurrentlySelectedPosition=hitObject.transform.position;
                selectiontile.transform.position = CurrentlySelectedPosition + new Vector3(0,0.25f,0);
                } catch (IndexOutOfRangeException)
                {
                // error handling for when the mouse is out of the map bounds which is not a problem but throws an exception anyway.
                selectiontile.GetComponent<Renderer>().enabled = false;
                }
            }
            else {
                selectiontile.GetComponent<Renderer>().enabled = false;
            }

            }
            
        }
    }

