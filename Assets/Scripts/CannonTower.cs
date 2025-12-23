using UnityEngine;

public class CannonTower : MonoBehaviour
{

    [SerializeField]
    private float range = 5f;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float damage = 10f;
    private GameObject target=null;
    private float distancetarget=Mathf.Infinity;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Retrouver la cible la plus proche dans la portée
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < distancetarget && distanceToEnemy <= range)
            {
                distancetarget = distanceToEnemy;
                target = enemy;
            }
            else
            {
                target = null;
                distancetarget = Mathf.Infinity;
            }
        }

        //Rotation vers la cible
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }


    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
