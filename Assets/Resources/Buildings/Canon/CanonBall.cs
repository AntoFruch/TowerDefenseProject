using UnityEditor;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
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
            collision.gameObject.GetComponent<MonsterController>().TakeDamage(4);
            Destroy(gameObject);
        }
    }
}
