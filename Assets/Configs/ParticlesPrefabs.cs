using UnityEngine;

[CreateAssetMenu(fileName ="ParticlesPrefabs", menuName ="PrefabsConfig/Particles")]
public class ParticlesPrefabs : ScriptableObject
{
     [Header("particles")]
    public GameObject canonExplosion;
    public GameObject gunShot;
    public GameObject hit;
    public GameObject death;
}