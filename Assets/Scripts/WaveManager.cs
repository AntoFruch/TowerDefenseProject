using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    [SerializeField] GameObject spawningFX;
    List<Vector2Int> spawns; 
    List<MonsterType> currentWave;

    [Header("Monster Spawn Rate")]
    [SerializeField] float spawnRate;

    public bool finished {get; private set;}

    public void Init()
    {
        spawns = new();
        GetSpawns();
        finished = true;
    }
    void GetSpawns()
    {
        for (int y=0; y<Game.Instance.map.Length; y++)
        {
         for (int x=0; x<Game.Instance.map[0].Length; x++)
            {
                if (Game.Instance.map[y][x] == TileType.SPAWN)
                {
                    spawns.Add(new Vector2Int(x,y));
                }
            }
        }
    }

    // Main method : loads and spawns the next wave 
    public void StartNextWave()
    {
        if (finished)
        {
            finished=false;
            LoadNextWave();
            StartCoroutine(SpawnWave(currentWave));
        }
        
    }
    
    // SpawningRoutine for a wave
    private IEnumerator SpawnWave(List<MonsterType> wave)
    {   
        int i = 0;
        while (i < wave.Count)
        {
            foreach (Vector2Int spawnPos in spawns)
            {
                GameObject monster;
                switch (wave[i])
                {
                    case MonsterType.GroBleu:
                        monster = Instantiate(Game.Instance.monstersPrefabs.groBleu,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.GroJaune:
                        monster = Instantiate(Game.Instance.monstersPrefabs.groJaune,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.Shell:
                        monster = Instantiate(Game.Instance.monstersPrefabs.shell,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.Blob:
                        monster = Instantiate(Game.Instance.monstersPrefabs.blob,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    default:
                        monster = null;
                        break;
                    
                }
                Game.Instance.monsters.Add(monster.GetComponent<MonsterController>());
            }
            i++;
            yield return new WaitForSeconds(1/spawnRate);
        }
    }

    void Update()
    {
        if (Game.Instance.monsters.Count==0) finished = true;
    }

    // Generation of the next wave 
    void LoadNextWave()
    {
        // algorithme qui doit rendre le jeu de plus en plus difficile au fur et a mesure que la partie avance
        currentWave = new List<MonsterType> {MonsterType.GroBleu, MonsterType.GroJaune, MonsterType.Shell, MonsterType.GroJaune};
    }
}

public enum MonsterType
{
    Blob, GroBleu, GroJaune, Shell
}
