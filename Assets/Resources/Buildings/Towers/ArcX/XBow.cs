using UnityEngine;

public class XBow : Tower
{
    [Header("XBow-Specific fields")]
    [SerializeField] private float shootForce = 1f;
    [SerializeField] private XBowPrefabs prefabs;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateTarget()
    {
        target = TargetStrategies.Mono(rotatingPart, target, range);
    }

    protected override void Shoot()
    {
        GameObject projectile = Instantiate(prefabs.projectile, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
        projectile.GetComponent<Projectile>().Init(this);
        
        projectile.GetComponent<Rigidbody>().AddForce(shootForce * lookDirection, ForceMode.Impulse);
        Instantiate(prefabs.xbowShotParticles, projectileSpawn.position,Quaternion.LookRotation(lookDirection));
        
        projectileSpawn.localPosition = new Vector3(-projectileSpawn.localPosition.x, projectileSpawn.localPosition.y,projectileSpawn.localPosition.z);
    }
    

} 
