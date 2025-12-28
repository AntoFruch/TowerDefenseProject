using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class MonsterController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    // Life
    [SerializeField] private int life = 10;
    public int Life => life;    
    
    // Constants
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] float rotationSpeed = 7f;
    private float gravity = -9.81f;

    // PathFinding
    private Graph<VertexLabel> graph;
    private List<Vertex<VertexLabel>> path;
    private int index = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

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
    void Update()
    {
        ApplyGravity();
        FollowPath();

        animator.SetFloat("Speed", controller.velocity.magnitude);
    }

    float verticalVelocity;
    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // colle au sol

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = new Vector3(0f, verticalVelocity, 0f);
        controller.Move(move * Time.deltaTime);
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

    public void TakeDamage(int dmg)
    {
        Debug.Log(gameObject.name + " : " + life + " " + dmg);
        life -= dmg;
        if (life > 0)
        {
            GetComponent<Animator>().SetTrigger("TakeDMG");    
        } else
        {
            Die();
        }
    }

    private void Die()
    {
        // triggers the death animation.
        // When the animation ends, a animator event calls DestroySelf()
        GetComponent<Animator>().SetTrigger("Death");
    }

    // called by an animator event
    void DestroySelf()
    {
        Instantiate(Game.Instance.prefabConfig.death, transform.position, Quaternion.Euler(-90,0,0));
        Destroy(gameObject);
    }
}
