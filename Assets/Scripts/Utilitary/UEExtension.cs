using UnityEngine;

public static class UEExtension
{
    public static Vector2Int Vector3toVector2Int(Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z));
    }
}