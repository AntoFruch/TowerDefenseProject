using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Tower tower;

    public void Init(Tower tower)
    {
        this.tower = tower;
    }
 
    // Update is called once per frame
    void Update()
    {
        if ((transform.position - tower.transform.position).magnitude > tower.Range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterController controller = collision.gameObject.GetComponent<MonsterController>();

            Instantiate(controller.Prefabs.hit, collision.gameObject.transform.position, Quaternion.identity);
            controller.TakeDamage((int)tower.Damage);
            Destroy(gameObject);
        }
    }
}
