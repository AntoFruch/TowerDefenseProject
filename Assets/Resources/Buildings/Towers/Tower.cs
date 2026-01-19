using UnityEngine;
using System.Collections.Generic;

public abstract class Tower : Building
{
    
    [SerializeField] protected float fireRate ;
    [SerializeField] protected float damage ;
    public float Damage => damage;

    [Header("Fonctional Assignements")]
    [SerializeField] protected float lerpStep = 10f;
    [SerializeField] protected Transform rotatingPart;

    [SerializeField] protected Transform projectileSpawn; // Empty GameObject that is the spawn point for the projectiles
    
    protected Vector3 lookDirection;
    protected GameObject target = null;
    protected List<GameObject> targets = null;

    [SerializeField] float idleRotationChangeTime = 2f;
    [SerializeField] float idleTurnSpeed = 1f;

    private float idleTimer;
    private Quaternion idleTargetRotation;

    [Header("Energy")]
    [SerializeField] private int powerConsumption;
    public int PowerConsumption => powerConsumption;
    private int power;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    float clock;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (IsPowered())
        {
            UpdateTarget();
            RotateTowardTarget();
            
            // Shooting routine
            clock += Time.deltaTime;
            if (clock > 1 / fireRate && (target != null || targets !=null))
            {
                Shoot();
                clock=0;
            }
        }
    }
    
    void UpdateIdleRotation()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            idleTimer = idleRotationChangeTime;

            float randomY = Random.Range(0f, 360f);
            idleTargetRotation = Quaternion.Euler(0f, randomY, 0f);
        }

        rotatingPart.rotation = Quaternion.Lerp(
            rotatingPart.rotation,
            idleTargetRotation,
            idleTurnSpeed * Time.deltaTime
        );
    }
    protected void RotateTowardTarget()
    {
        if (target != null)
        {
            lookDirection = (target.transform.position - rotatingPart.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, lerpStep*Time.deltaTime).eulerAngles;
            rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            
        } else
        {
            UpdateIdleRotation();
        }
        
    }
    
    protected abstract void UpdateTarget();
    protected abstract void Shoot();

    public void SetPower(int power)
    {
        this.power = power;
    }

    protected bool IsPowered()
    {
        return power >= powerConsumption;
    }
    public int GetPower()
    {
        return power;
    }
}
