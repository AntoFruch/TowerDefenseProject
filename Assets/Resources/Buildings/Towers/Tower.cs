using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Attributes")]
    [SerializeField] protected float range = 3f;
    protected float realRange;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float damage = 10f;
    public float Damage => damage;

    [Header("Fonctional Assignements")]
    [SerializeField] protected float lerpStep = 10f;
    [SerializeField] private Transform rangeArea;
    [SerializeField] protected Transform rotatingPart;

    [SerializeField] protected Transform projectileSpawn; // Empty GameObject that is the spawn point for the projectiles
    protected Vector3 lookDirection;

    protected GameObject target=null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        realRange = (2 * range + 1)/2;
        rangeArea.localScale = new Vector3(2*realRange,0.01f,2*realRange); 
    }

    float clock;
    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
        RotateTowardTarget();
        
        // Shooting routine
        clock += Time.deltaTime;
        if (clock > 1 / fireRate && target != null)
        {
            Shoot();
            clock=0;
        }
    }
    protected void RotateTowardTarget()
    {
        if (target != null)
        {
            lookDirection = (target.transform.position - rotatingPart.position).normalized;
            
        } else
        {
            lookDirection = Vector3.forward;
        }
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, lerpStep*Time.deltaTime).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    protected abstract void UpdateTarget();
    protected abstract void Shoot();
}
