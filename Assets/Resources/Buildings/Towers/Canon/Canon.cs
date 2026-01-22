using Unity.VisualScripting;
using UnityEngine;

public class Canon : Tower
{
    [Header("Canon-Specific fields")]
    [SerializeField] private float shootForce = 4f;
    [SerializeField] private CanonPrefabs prefabs;
    
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
        Instantiate(prefabs.explosion, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
    }
}
