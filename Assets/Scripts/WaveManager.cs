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

    public void Init()
    {
        spawns = new();
        GetSpawns();
    }

    void LoadNextWave()
    {
        // algorithme qui doit rendre le jeu de plus en plus difficile au fur et a mesure que la partie avance
        currentWave = new List<MonsterType> {MonsterType.GroBleu, MonsterType.GroJaune, MonsterType.Shell, MonsterType.GroJaune};
    }

    public void StartNextWave()
    {
        LoadNextWave();
        StartCoroutine(SpawnWave(currentWave));
    }
    private IEnumerator SpawnWave(List<MonsterType> wave)
    {   
        int i = 0;
        while (i < wave.Count)
        {
            foreach (Vector2Int spawnPos in spawns)
            {
                switch (wave[i])
                {
                    case MonsterType.GroBleu:
                        Instantiate(Game.Instance.monstersPrefabs.groBleu,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.GroJaune:
                        Instantiate(Game.Instance.monstersPrefabs.groJaune,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.Shell:
                        Instantiate(Game.Instance.monstersPrefabs.shell,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    case MonsterType.Blob:
                        Instantiate(Game.Instance.monstersPrefabs.blob,new Vector3(spawnPos.x, 0.2f, spawnPos.y), Quaternion.identity);
                        break;
                    
                }
            }
            i++;
            yield return new WaitForSeconds(1/spawnRate);
        }
        
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
}

public enum MonsterType
{
    Blob, GroBleu, GroJaune, Shell
}
