using UnityEngine;
using System.Collections.Generic;

public abstract class Tower : Building
{
    [Header("Stats de Base")]
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float damage = 10f;

    public float CurrentDamage { get; private set; }
    public float CurrentFireRate { get; private set; }
    public int CurrentRange { get; private set; }
    public float Damage => CurrentDamage;

    public float BaseRange => range;

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

    [SerializeField] private int powerConsumption;
    public int PowerConsumption => powerConsumption;
    private int power;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        CurrentDamage = damage;
        CurrentFireRate = fireRate;
        CurrentRange = range;   //From the Class Building
        CalculateStats();
    }

    float clock;
    // Update is called once per frame
    protected override void Update()
    {
        CalculateStats();

        if (IsPowered() && active){
            base.Update();
            UpdateTarget();
            RotateTowardTarget();

            // Shooting routine
            clock += Time.deltaTime;
            if (clock > 1 / CurrentFireRate && (target != null || targets != null))
            {
                Shoot();
                clock=0;
            }
        }
    }

    //Calculation of each installation type's boost
    void CalculateStats()
    {
        float damageMult = 1f;
        float fireMult = 1f;
        int rangeBonus = 0;

        foreach(Building building in Game.Instance.buildings)
        {
            if(building is Installation installation)
            {
                float dist = Mathf.Abs(transform.position.x - installation.transform.position.x) + Mathf.Abs(transform.position.z - installation.transform.position.z);

                if(dist <= (installation.Range * 2 + 1) / 2)
                {
                    switch (installation.type)
                    {
                        case InstallationType.Radar:
                            rangeBonus += installation.rangeBonus;
                            break;

                        case InstallationType.Factory:
                            damageMult += installation.damageBonus;
                            break;

                        case InstallationType.Storage:
                            fireMult += installation.fireRateBonus;
                            break;
                    }
                }
            }
        }
        CurrentDamage = damage * damageMult;
        CurrentFireRate = fireRate * fireMult;
        CurrentRange = range + rangeBonus;
        RangesManager.Instance.ShowRanges();
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
