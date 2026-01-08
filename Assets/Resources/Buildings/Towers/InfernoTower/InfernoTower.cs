using UnityEngine;
using Unity.VisualScripting;
public class InfernoTower : Tower
{
    [Header("InfernoTower-Specific fields")]
    [SerializeField] 
    private float shootForce = 1f;
    [SerializeField]
    private InfernoTowerPrefabs prefabs;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateTarget()
    {
        target = TargetStrategies.Mono(rotatingPart, target, realRange);
    }
    protected override void Shoot()
    {
        GameObject projectile = Instantiate(prefabs.projectile, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
        projectile.GetComponent<Projectile>().Init(this);
        
        projectile.GetComponent<Rigidbody>().AddForce(shootForce * lookDirection, ForceMode.Impulse);
        Instantiate(prefabs.gunShotParticles, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
        
        projectileSpawn.localPosition = new Vector3(-projectileSpawn.localPosition.x, projectileSpawn.localPosition.y,projectileSpawn.localPosition.z);
    }

}
