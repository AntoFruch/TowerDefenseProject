using UnityEngine;

public class PowerPlant : Building
{
    [SerializeField] private int powerOutput;
    public int PowerOutput => powerOutput;
}
