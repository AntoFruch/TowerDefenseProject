using UnityEngine;

public class Gun : Tower
{
    [Header("Gun-Specific fields")]
    [SerializeField] private float shootForce = 4f;
    [SerializeField] private GunPrefabs prefabs;
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
        Instantiate(prefabs.gunShotParticles, projectileSpawn.position, Quaternion.LookRotation(lookDirection));
        
        projectileSpawn.localPosition = new Vector3(-projectileSpawn.localPosition.x, projectileSpawn.localPosition.y,projectileSpawn.localPosition.z);
    }
}
