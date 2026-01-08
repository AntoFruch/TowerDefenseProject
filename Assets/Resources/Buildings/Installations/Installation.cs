using UnityEngine;

public enum InstallationType
{
    Radar,      //Gives a 25% boost in range
    Factory,    //Gives a 25% boost in damage
    Storage  //Gives a 25% boost in firerate
}
public class Installation : Building
{
    [Header("Installation Settings")]
    public InstallationType type;
    public float bonusPercentage = 0.25f;

    protected override void Start()
    {
        base.Start();
    }

}
