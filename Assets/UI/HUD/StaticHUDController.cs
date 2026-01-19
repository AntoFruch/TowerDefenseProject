using UnityEngine;
using UnityEngine.UIElements;

public class StaticHUDController : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private VisualElement root; 
    private Button towerButton;
    private Button energyButton;
    private Button boostButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        UIDoc.sortingOrder = 0;
        root = UIDoc.rootVisualElement;

        towerButton = UIDoc.rootVisualElement.Q<Button>("tower-range-btn");
        towerButton.RegisterCallback<ClickEvent>(OnTowerButtonClicked);
        energyButton = UIDoc.rootVisualElement.Q<Button>("energy-range-btn");
        energyButton.RegisterCallback<ClickEvent>(OnEnergyButtonClicked);
        boostButton = UIDoc.rootVisualElement.Q<Button>("boost-range-btn");
        boostButton.RegisterCallback<ClickEvent>(OnBoostButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //CALLBACKS : 
    void OnTowerButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Towers);
    }
    void OnEnergyButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Energy);
    }
    void OnBoostButtonClicked(ClickEvent evt)
    {
        RangesManager.Instance.SetMode(RangeMode.Boost);
    }

    
    
}
