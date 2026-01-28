using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Tower tower;

    [SerializeField] float lifeTime = 2;
    float clock;
    public void Init(Tower tower)
    {
        this.tower = tower;
        clock = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if ((transform.position - tower.transform.position).magnitude > tower.CurrentRange || clock > lifeTime)
        {
            Destroy(gameObject);
        }
        clock+=Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterController controller = collision.gameObject.GetComponent<MonsterController>();

            Instantiate(controller.Prefabs.hit, collision.gameObject.transform.position, Quaternion.identity);
            controller.TakeDamage((int)tower.CurrentDamage);
            Destroy(gameObject);
        }
    }
}
