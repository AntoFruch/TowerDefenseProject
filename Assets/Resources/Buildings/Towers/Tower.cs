using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private float range = 3f;
    private float realRange;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float damage = 10f;
    public float Damage => damage;
    [SerializeField] private float lerpStep;
    [SerializeField] private float shootForce = 4f;
    [SerializeField] private Transform rangeArea;
    [SerializeField] private Transform rotatingPart;
    [SerializeField] private TowerType towerType;

    [SerializeField] private Transform projectileSpawn; // Empty GameObject that is the spawn point for the projectiles
    private Vector3 lookDirection;

    private GameObject target=null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        realRange = (2 * range + 1)/2;
        rangeArea.localScale = new Vector3(2*realRange,0.01f,2*realRange); 
    }

    // Update is called once per frame
    void Update()
    {
        // Targetting the nearest alive enemy, the target must not change if one enemy comes closer after targetting. 
        // Target changes only if the previous target is dead or not in range anymore.
        List<GameObject> inRangeEnemies = GameObject.FindGameObjectsWithTag("Enemy")
                                                    .Where(e => (e.transform.position - rotatingPart.position).magnitude < realRange 
                                                                && e.GetComponent<MonsterController>().Life > 0 )
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
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, lerpStep).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    float clock;
    private void Shoot()
    {
        if (clock > 1 / fireRate)
        {
            if (target != null)
            {
                GameObject projectile;

                switch (towerType)
                {
                    case TowerType.RED:
                        projectile = Instantiate(Game.Instance.buildingsPrefabs.redTowerProjectile, projectileSpawn.position, Quaternion.identity);
                        Instantiate(Game.Instance.particlesPrefabs.canonExplosion,projectileSpawn.position, Quaternion.identity);                        
                        break;
                    case TowerType.YELLOW:
                        projectile = Instantiate(Game.Instance.buildingsPrefabs.yellowTowerProjectile, projectileSpawn.position, Quaternion.identity);
                        //TODO
                        break;
                    case TowerType.GREEN:
                        projectile = Instantiate(Game.Instance.buildingsPrefabs.greenTowerProjectile, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
                        projectileSpawn.localPosition = new Vector3(-projectileSpawn.localPosition.x, projectileSpawn.localPosition.y, projectileSpawn.localPosition.z);
                        Instantiate(Game.Instance.particlesPrefabs.gunShot, projectileSpawn.position, Quaternion.identity);
                        break;
                    case TowerType.BLUE:
                        projectile = Instantiate(Game.Instance.buildingsPrefabs.blueTowerProjectile, projectileSpawn.position, Quaternion.identity);
                        // TODO
                        break;
                    default:
                        projectile = null;
                        break;
                }
                projectile.GetComponent<Projectile>().Init(this);
                projectile.GetComponent<Rigidbody>().AddForce(lookDirection * shootForce, ForceMode.Impulse);
                
                
            }
            clock = 0;
        } else
        {
            clock += Time.deltaTime;
        }
    }
    
    
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rotatingPart.position, range);
    }

}
