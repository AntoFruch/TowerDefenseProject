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
                                                                    && e.GetComponent<MonsterController>().Life > 0 )
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
        return Math.Abs(v1.x-v2.x)+Math.Abs(v1.y - v2.y) <= range;
    }
}