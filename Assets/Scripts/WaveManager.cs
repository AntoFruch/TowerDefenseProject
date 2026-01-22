using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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

    private int waveIndex;

    public bool finished {get; private set;}

    public void Init()
    {
        spawns = new();
        GetSpawns();
        finished = true;
        currentWave = new();
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
        if (Game.Instance.monsters.Count == 0 && !finished)
        {
            Game.Instance.SetState(GameState.Preparation); // pas necesairement changer comme ca, je dirai qu'il faut plusieurs vagues de suite pour constituer une vague entiere
            finished = true;
        }
    }

    // Generation of the next wave 
    void LoadNextWave()
    {
        currentWave.Clear();
        waveIndex++;
        int monsterCount = 3 + waveIndex * 2;

        for (int i = 0; i < monsterCount; i++)
        {
            if (waveIndex < 3)
                currentWave.Add(MonsterType.Shell);
            else if (waveIndex < 6)
                currentWave.Add(Random.value < 0.7f ? MonsterType.GroJaune : MonsterType.GroBleu);
            else
                currentWave.Add(MonsterType.Blob);
        }
    }
}

public enum MonsterType
{
    Blob, GroBleu, GroJaune, Shell
}
