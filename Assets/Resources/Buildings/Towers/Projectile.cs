using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    private Tower tower;

    public void Init(Tower tower)
    {
        this.tower = tower;
    }
    private float clock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        if (clock > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Game.Instance.prefabConfig.hit, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<MonsterController>().TakeDamage((int)tower.Damage);
            Destroy(gameObject);
        }
    }
}
