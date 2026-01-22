using UnityEngine;
using UnityEngine.UIElements;

public class StaticHUDController : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private VisualElement root;

    //Range mode buttons
    private Button towerButton;
    private Button energyButton;
    private Button boostButton;

    // life
    private VisualElement mask;

    // money
    private Label moneyLabel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        root = UIDoc.rootVisualElement;

        // range mode buttons
        towerButton = root.Q<Button>("tower-range-btn");
        towerButton.RegisterCallback<ClickEvent>(OnTowerButtonClicked);
        energyButton = root.Q<Button>("energy-range-btn");
        energyButton.RegisterCallback<ClickEvent>(OnEnergyButtonClicked);
        boostButton = root.Q<Button>("boost-range-btn");
        boostButton.RegisterCallback<ClickEvent>(OnBoostButtonClicked);

        // life
        mask = root.Q<VisualElement>("healthbar-mask");

        //money
        moneyLabel = root.Q<Label>("money-label");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        UpdateMoneyLabel();
    }

    void UpdateHealthBar()
    {
        mask.style.width = Length.Percent(Game.Instance.health / Game.Instance.maxHealth * 100f);
    }
    
    void UpdateMoneyLabel()
    {
        int money = 0; // plutot MoneyManager.GetMoney() ou un truc comme ca
        moneyLabel.text = money.ToString() ;
    }

    //CALLBACKS : 
    void OnTowerButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Towers);
        AudioManager.Instance?.PlayClick();
    }
    void OnEnergyButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Energy);
        AudioManager.Instance?.PlayClick();
    }
    void OnBoostButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Boost);
        AudioManager.Instance?.PlayClick();
    } 
}
