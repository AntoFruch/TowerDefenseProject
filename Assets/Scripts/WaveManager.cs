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

    public int waveIndex {get;private set;}

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
        // on choisit un spawn aleatoire
        Vector2Int spawnPos = spawns[Random.Range(0,spawns.Count)];
        
        GameObject particle = Instantiate(spawningFX,new Vector3(spawnPos.x, 0.4f, spawnPos.y), Quaternion.identity);

        int i = 0;
        while (i < wave.Count)
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
            i++;
            yield return new WaitForSeconds(1/spawnRate);
        }

        Destroy(particle);
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

        for (int i = 0; i < Mathf.Clamp((waveIndex-1) * 2, 1,5); i++)
        {
          currentWave.Add(MonsterType.Shell);
        }
        if (waveIndex < 5){
            for (int i = 0; i < Mathf.Clamp(waveIndex/2, 1,5); i++)
            {
                MonsterType monster = Random.value > 0.5 ? MonsterType.GroBleu : MonsterType.GroJaune;
                currentWave.Add(monster);
            }
        } else
        {
            for (int i=0; i<2; i++)
            {
                MonsterType monster = Random.value > 0.5 ? MonsterType.GroBleu : MonsterType.GroJaune;
                currentWave.Add(monster);
            }
            for (int i=0; i<waveIndex/6; i++)
            {
                currentWave.Add(MonsterType.Blob);
            }
            for (int i=0; i<2; i++)
            {
                MonsterType monster = Random.value > 0.5 ? MonsterType.GroBleu : MonsterType.GroJaune;
                currentWave.Add(monster);
            }
        }

        for (int i = 0; i < Mathf.Clamp((waveIndex-1) * 2, 1,5); i++)
        {
          currentWave.Add(MonsterType.Shell);
        }


    }

    public int GetCurrentWaveLength()
    {
        return currentWave.Count * spawns.Count;
    }
}

public enum MonsterType
{
    Blob, GroBleu, GroJaune, Shell
}
