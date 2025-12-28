using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class TargetStrategies
{
    public static GameObject Mono(Transform rotatingPart, GameObject target, float realRange){
        List<GameObject> inRangeEnemies = GameObject.FindGameObjectsWithTag("Enemy")
                                                    .Where(e => (e.transform.position - rotatingPart.position).magnitude < realRange 
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
}