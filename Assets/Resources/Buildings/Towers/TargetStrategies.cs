using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class TargetStrategies
{
    public static GameObject Mono(Transform rotatingPart, GameObject target, int range){
        List<GameObject> inRangeEnemies = GameObject.FindGameObjectsWithTag("Enemy")
                                                    .Where(e => 
                                                        IsInManhattanRange(UEExtension.Vector3toVector2Int(e.transform.position),
                                                                        UEExtension.Vector3toVector2Int(rotatingPart.transform.position),
                                                                        range) 
                                                                    && e.GetComponent<MonsterController>().health > 0 )
                                                    .ToList();

        if (target != null && !inRangeEnemies.Contains(target))
        {
            target = null;
        } else if (inRangeEnemies.Count !=0 && target == null) 
        {
            target = inRangeEnemies.OrderBy(e => (e.transform.position - rotatingPart.position).magnitude).First();
        }
        return target;
    }

    static bool IsInManhattanRange(Vector2Int v1, Vector2Int v2, int range)
    {
        Vector2Int v = v1 - v2;
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) <= range;
    }

    public static List<GameObject> Multi(Transform rotatingPart,List<GameObject> targets, float realRange)
    {
        List<GameObject> inRangeEnemies = GameObject.FindGameObjectsWithTag("Enemy")
                                                    .Where(e => (e.transform.position - rotatingPart.position).magnitude < realRange && e.GetComponent<MonsterController>().health > 0)
                                                    .ToList();
        
        targets = inRangeEnemies;
        return targets;
    }
}