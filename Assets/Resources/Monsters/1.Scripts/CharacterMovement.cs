using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;

    // Constants
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] float rotationSpeed = 7f;
    private float gravity = -9.81f;

    // PathFinding
    private Graph<VertexLabel> graph;
    private List<Vertex<VertexLabel>> path;
    private int index = 0;

    // Animator parameter
    public float speed {get;private set;}
    void Start()
    {
        controller = GetComponent<CharacterController>();

        graph = Game.Instance.graph;

        Vector2Int groundPos = new Vector2Int((int)transform.position.x,(int)transform.position.z);

        Vertex<VertexLabel> spawnVertex = graph.GetVertices()
                                    .Where(v => v.position == groundPos)
                                    .ToList()[0];

        path = Strategies.ShortestPath(graph, spawnVertex);
        Debug.Log(this.name);
        Debug.Log(spawnVertex);
        foreach (Vertex<VertexLabel> v in path)
        {
            Debug.Log(v);
        }
    }
    private bool toggle;
    void Update()
    {
        ApplyGravity();
        FollowPath();

        speed = controller.velocity.magnitude;
    }

    private void ApplyGravity()
    {
        Vector3 velocity = new Vector3(0f, controller.velocity.y + gravity * Time.deltaTime, 0f);
        controller.Move(velocity * Time.deltaTime);
    }

    private void FollowPath()
    {
        if (index < path.Count && MoveToTile(path[index].position) )
        {
            index+=1;
        }
    }
    private bool MoveToTile(Vector2Int tilePos)
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = new Vector3(tilePos.x, pos.y, tilePos.y);

        Vector3 direction = (targetPos - pos);
        
        // Vérifie si on est déjà proche de la tuile
        if (direction.sqrMagnitude > 0.01f)
        {
            // 1. Rotation vers la direction du mouvements
            Vector3 lookDir = new Vector3(direction.x, 0, direction.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 2. Déplacement vers le tile
            Vector3 move = lookDir * moveSpeed * Time.deltaTime;
            controller.Move(move);
            return false;
        } return true;
    }

}
