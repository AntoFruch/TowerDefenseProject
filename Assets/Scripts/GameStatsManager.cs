using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int finalScore {  get; private set; }
    public int monstersDefeated {get;private set;}
    public int wavesSurvived {get; private set;}
    public int moneySpent {get; private set;}
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalScore = 0;
        monstersDefeated = 0;
        wavesSurvived = 0;
        moneySpent = 0;
    }
    void Update()
    {
        wavesSurvived = WaveManager.Instance.waveIndex-1;
    }

    public void AddScoreStats(int amount)
    {
        finalScore += amount;
    }
    public void AddMonsterDefeated()
    {
        monstersDefeated++;
    }

    public void AddMoneySpent(int amount)
    {
        moneySpent+= amount;
    }
}
