using UnityEngine;
using Unity.VisualScripting;
public class InfernoTower : Tower
{
    [Header("InfernoTower-Specific fields")]
    [SerializeField] private float shootForce = 0.1f;
    [SerializeField] private InfernoTowerPrefabs prefabs;

    protected override void Start()
    {
        base.Start();
        targets = new System.Collections.Generic.List<GameObject>();
    }

    protected override void UpdateTarget()
    {
        targets = TargetStrategies.Multi(rotatingPart, targets, realRange);
    }
    protected override void Shoot()
    {
        foreach (GameObject Monster in targets)
        {
            if (Monster.CompareTag("Enemy"))
            {
                Monster.GetComponent<MonsterController>().TakeDamage((int)this.damage);
                Debug.Log("test");
            }
        }
    }

}
