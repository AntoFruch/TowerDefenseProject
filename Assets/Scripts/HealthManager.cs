using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Player Health
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;
    public float health {get;private set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 10;
    }

    void Update()
    {
        if (health <=0)
        {
            Die();
        }  
    } 
    public void TakeDamage(int dmg)
    {
        health -= dmg;
    }

    public void Die()
    {
        DieMenu.Instance.ShowDieMenu();
    }
}
