using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private int currentMoney = 200;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        //UI gestion
    }

   
    public void AddMoney(int amount)
    {
        currentMoney += amount;


    }

    public bool SpendMoney(int amount) {
        if (currentMoney >= amount) {
            currentMoney -= amount;
            return true;
        }

        else
        {
            return false;
        }
    }
}
