using Unity.VisualScripting;
using UnityEngine;

public class Canon : Tower
{
    [Header("Canon-Specific fields")]
    [SerializeField] private float shootForce = 4f;
    [SerializeField] private CanonPrefabs prefabs;
    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void UpdateTarget()
    {
        target = TargetStrategies.Mono(rotatingPart, target, CurrentRange);
    }
    protected override void Shoot()
    {
        audioSource.Play();
        GameObject projectile = Instantiate(prefabs.projectile, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
        projectile.GetComponent<Projectile>().Init(this);
        
        projectile.GetComponent<Rigidbody>().AddForce(shootForce * lookDirection, ForceMode.Impulse);
        Instantiate(prefabs.explosion, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
    }
}
