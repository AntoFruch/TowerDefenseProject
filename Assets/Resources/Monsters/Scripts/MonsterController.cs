using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem.Controls;

public class MonsterController : MonoBehaviour
{
    private CharacterController controller;
    public Animator animator{ get; private set; }

    // Life
    [SerializeField] private int life = 10;
    public int Life => life;

    //Economy
    [SerializeField] private int moneyValue = 100;
    private bool isDead = false;
    
    // Constants
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] float rotationSpeed = 7f;
    private float gravity = -9.81f;

    // PathFinding
    private Graph<VertexLabel> graph;
    private List<Vertex<VertexLabel>> path;
    private int index = 0;

    [SerializeField] private MonstersFXPrefabs prefabs;
    public MonstersFXPrefabs Prefabs => prefabs;

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
        if (isDead) return;

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
        isDead = true;


        // triggers the death animation.
        // When the animation ends, a animator event calls DestroySelf()
        GetComponent<Animator>().SetTrigger("Death");

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(moneyValue);
        }

    }

    void Attack()
    {
        Instantiate(
            Game.Instance.mapPrefabs.endHitParticles,
            transform.position,
            Quaternion.identity
        );
    }
    // called by an animator event
    void DestroySelf()
    {
        Instantiate(prefabs.death, transform.position, Quaternion.Euler(-90,0,0));
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        //Game.Instance.monstersPrefabs.Remove(this);
    }
}
