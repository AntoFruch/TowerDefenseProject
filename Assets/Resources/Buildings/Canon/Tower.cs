using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private float range = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lerpStep;
    [SerializeField] private float shootForce = 4f;
    [SerializeField] private Transform rangeArea;

    [SerializeField] private Transform projectileSpawn;

    private Vector3 lookDirection;

    private GameObject target=null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rangeArea.localScale = new Vector3(range*2+1,0.01f,range*2+1); 
    }

    // Update is called once per frame
    void Update()
    {
        // Targetting the nearest alive enemy, the target must not change if one enemy comes closer after targetting. 
        // Target changes only if the previous target is dead or not in range anymore.
        List<GameObject> inRangeEnemies = GameObject.FindGameObjectsWithTag("Enemy")
                                                    .Where(e => (e.transform.position - transform.position).magnitude < range && e.GetComponent<MonsterController>().life > 0 )
                                                    .ToList();
    
        if (target != null && !inRangeEnemies.Contains(target))
        {
            target = null;
        } else if (inRangeEnemies.Count !=0 && target == null) 
        {
            target = inRangeEnemies.OrderBy(e => (e.transform.position - transform.position).magnitude).First();
        }
        
        RotateTowardTarget();
        Shoot();
    }
    private void RotateTowardTarget()
    {
        if (target != null)
        {
            lookDirection = (target.transform.position - transform.position).normalized;
            
        } else
        {
            lookDirection = Vector3.up;
        }
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, lerpStep).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    float clock;
    private void Shoot()
    {
        if (clock > 1 / fireRate)
        {
            if (target != null)
            {
                GameObject projectile = Instantiate(Resources.Load<GameObject>("Buildings/Canon/CanonBall"), projectileSpawn.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().AddForce(lookDirection * shootForce, ForceMode.Impulse);
                Instantiate(Resources.Load<GameObject>("Buildings/Canon/explosion"),projectileSpawn.position, Quaternion.identity);
            }
            clock = 0;
        } else
        {
            clock += Time.deltaTime;
        }
    }
    
    
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
