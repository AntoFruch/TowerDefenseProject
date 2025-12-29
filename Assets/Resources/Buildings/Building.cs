using UnityEngine;

public class Building : MonoBehaviour {

    void OnDestroy()
    {
        Game.Instance?.buildings.Remove(this);
    }
}