using UnityEngine;

public enum InstallationType
{
    Radar,      //Gives a 1 tile boost in range
    Factory,    //Gives a 25% boost in damage
    Storage     //Gives a 25% boost in firerate
}
public class Installation : Building
{
    [Header("Installation Settings")]
    public InstallationType type;
    public float fireRateBonus = 0.25f;
    public float damageBonus = 0.25f;
    public int rangeBonus = 1;

    protected override void Start()
    {
        base.Start();
    }

}
