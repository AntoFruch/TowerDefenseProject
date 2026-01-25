using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildWheelController : MonoBehaviour
{
    // BuildsWheel
    [SerializeField] UIDocument buildWheelUIDoc;
    private VisualElement buildWheel;

    private Button towersButton;
    private Button powerPlantButton;
    private Button radarButton;
    private Button storageButton;
    private Button factoryButton;

    private Button buildsQuitButton;

    // TowersWheel 
    [SerializeField] UIDocument towersWheelUIDoc;
    private VisualElement towersWheel;

    private Button redButton;
    private Button blueButton;
    private Button greenButton;
    private Button yellowButton;

    private Button towersQuitButton;

    // Costs Labels
    private Label redTowerCost;
    private Label blueTowerCost;
    private Label greenTowerCost;
    private Label yellowTowerCost;
    private Label radarCost;
    private Label powerPlantCost;
    private Label storageCost;
    private Label factoryCost;

    private Vector2 wheelPos;
    public bool active {get;private set;}

    private bool ignoreNextClick;


    void OnEnable()
    {
        // queries and callbacks for buildsWheel
        this.buildWheel = buildWheelUIDoc.rootVisualElement.Q("wheel-container");

        this.towersButton = buildWheelUIDoc.rootVisualElement.Q<Button>("towers");
        towersButton.RegisterCallback<ClickEvent>(onTowersButtonClick);

        this.powerPlantButton = buildWheelUIDoc.rootVisualElement.Q<Button>("power-plant");
        powerPlantButton.RegisterCallback<ClickEvent>(onPowerPlantButtonClick);

        this.radarButton = buildWheelUIDoc.rootVisualElement.Q<Button>("radar");
        radarButton.RegisterCallback<ClickEvent>(onRadarButtonClick);

        this.storageButton = buildWheelUIDoc.rootVisualElement.Q<Button>("storage");
        storageButton.RegisterCallback<ClickEvent>(onStorageButtonClick);

        this.factoryButton = buildWheelUIDoc.rootVisualElement.Q<Button>("factory");
        factoryButton.RegisterCallback<ClickEvent>(onFactoryButtonClick);

        this.buildsQuitButton = buildWheelUIDoc.rootVisualElement.Q<Button>("quit-btn");
        buildsQuitButton.RegisterCallback<PointerUpEvent>(onQuitClick);
        
        // queries and callbacks for towersWheel
        this.towersWheel = towersWheelUIDoc.rootVisualElement.Q("wheel-container");

        this.redButton = towersWheelUIDoc.rootVisualElement.Q<Button>("red-btn");
        redButton.RegisterCallback<ClickEvent>(onRedButtonClick);

        this.greenButton = towersWheelUIDoc.rootVisualElement.Q<Button>("green-btn");
        greenButton.RegisterCallback<ClickEvent>(onGreenButtonClick);

        this.yellowButton = towersWheelUIDoc.rootVisualElement.Q<Button>("yellow-btn");
        yellowButton.RegisterCallback<ClickEvent>(onYellowButtonClick);

        this.blueButton = towersWheelUIDoc.rootVisualElement.Q<Button>("blue-btn");
        blueButton.RegisterCallback<ClickEvent>(onBlueButtonClick);

        this.towersQuitButton = towersWheelUIDoc.rootVisualElement.Q<Button>("quit-btn");
        towersQuitButton.RegisterCallback<PointerUpEvent>(onQuitClick);

        // Costs initialisation
        this.redTowerCost = towersWheelUIDoc.rootVisualElement.Q<Label>("red-cost-label");
        this.blueTowerCost = towersWheelUIDoc.rootVisualElement.Q<Label>("blue-cost-label");
        this.yellowTowerCost = towersWheelUIDoc.rootVisualElement.Q<Label>("yellow-cost-label");
        this.greenTowerCost = towersWheelUIDoc.rootVisualElement.Q<Label>("green-cost-label");
        this.radarCost = buildWheelUIDoc.rootVisualElement.Q<Label>("radar-cost-label");
        this.powerPlantCost = buildWheelUIDoc.rootVisualElement.Q<Label>("powerplant-cost-label");
        this.factoryCost = buildWheelUIDoc.rootVisualElement.Q<Label>("factory-cost-label");
        this.storageCost = buildWheelUIDoc.rootVisualElement.Q<Label>("storage-cost-label");

        if(MoneyManager.Instance != null)
        {
            this.redTowerCost.text = MoneyManager.Instance.GetCost(BuildingType.RedTower).ToString();
            this.blueTowerCost.text = MoneyManager.Instance.GetCost(BuildingType.BlueTower).ToString();
            this.yellowTowerCost.text = MoneyManager.Instance.GetCost(BuildingType.YellowTower).ToString();
            this.greenTowerCost.text = MoneyManager.Instance.GetCost(BuildingType.GreenTower).ToString();
            this.radarCost.text = MoneyManager.Instance.GetCost(BuildingType.Radar).ToString();
            this.powerPlantCost.text = MoneyManager.Instance.GetCost(BuildingType.PowerPlant).ToString();
            this.factoryCost.text = MoneyManager.Instance.GetCost(BuildingType.Factory).ToString();
            this.storageCost.text = MoneyManager.Instance.GetCost(BuildingType.Storage).ToString();
        }
        

        // Hide everything
        this.HideWheel();
    }

    // Show methods
    public void ShowWheelAtPosition(Vector2 pos)
    {
        ignoreNextClick = true;
        active = true;
        wheelPos = pos;

        this.buildWheelUIDoc.rootVisualElement.RemoveFromClassList("hide");
        this.buildWheel.style.position = Position.Absolute;

        this.buildWheel.style.left = pos.x ;
        this.buildWheel.style.bottom = pos.y;
        this.buildWheel.style.translate = new Translate(
            new Length(-50, LengthUnit.Percent),
            new Length(50, LengthUnit.Percent)
        );
    }

    public void ShowTowersWheel()
    {
        this.towersWheelUIDoc.rootVisualElement.RemoveFromClassList("hide");
        this.towersWheel.style.position = Position.Absolute;

        this.towersWheel.style.left = wheelPos.x;
        this.towersWheel.style.bottom = wheelPos.y;
        this.towersWheel.style.translate = new Translate(
            new Length(-50, LengthUnit.Percent),
            new Length(50, LengthUnit.Percent)
        );
    }
    
    // Hide Methods
    public void HideWheel()
    {
        active = false;
        HideTowersWheel();
        HideBuildsWheel();
    }
    
    public void HideBuildsWheel()
    {
        this.buildWheelUIDoc.rootVisualElement.AddToClassList("hide");
    }
    public void HideTowersWheel()
    {
        this.towersWheelUIDoc.rootVisualElement.AddToClassList("hide");
    }

    //============== Button Callbacks ================ //
    // Builds Menu
    void onTowersButtonClick(ClickEvent evt)
    {
        HideBuildsWheel();
        ShowTowersWheel();
    }
    void onRadarButtonClick(ClickEvent evt)
    {
        GameObject radarPrefab = Game.Instance.buildingsPrefabs.radar;
 
        int radarCost = MoneyManager.Instance.GetCost(BuildingType.Radar);
        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(radarCost))
        {
            Instantiate(radarPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }

    } 
    void onPowerPlantButtonClick(ClickEvent evt)
    {
        GameObject powerPlantPrefab = Game.Instance.buildingsPrefabs.powerPlant;

        int powerPlantCost = MoneyManager.Instance.GetCost(BuildingType.PowerPlant);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(powerPlantCost))
        {
            Instantiate(powerPlantPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 
    void onStorageButtonClick(ClickEvent evt)
    {
        GameObject storagePrefab = Game.Instance.buildingsPrefabs.storage;

        int storageCost = MoneyManager.Instance.GetCost(BuildingType.Storage);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(storageCost))
        {
            Instantiate(storagePrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 
    void onFactoryButtonClick(ClickEvent evt)
    {
        GameObject factoryPrefab = Game.Instance.buildingsPrefabs.factory;

        int factoryCost = MoneyManager.Instance.GetCost(BuildingType.Factory);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(factoryCost))
        {
            Instantiate(factoryPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 

    void onQuitClick(PointerUpEvent evt)
    {
        if (ignoreNextClick)
        {
            ignoreNextClick = false;
            return;
        }   
        HideWheel();
    }

    // Towers Menu
    void onRedButtonClick(ClickEvent evt)
    {
        // Instantiate red tower
        GameObject redPrefab = Game.Instance.buildingsPrefabs.redTower;

        int redCost = MoneyManager.Instance.GetCost(BuildingType.RedTower);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(redCost))
        {
            Instantiate(redPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    }
    void onBlueButtonClick(ClickEvent evt)
    {
        GameObject bluePrefab = Game.Instance.buildingsPrefabs.blueTower;

        int blueCost = MoneyManager.Instance.GetCost(BuildingType.BlueTower);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(blueCost))
        {
            Instantiate(bluePrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 
    void onYellowButtonClick(ClickEvent evt)
    {
        GameObject yellowPrefab = Game.Instance.buildingsPrefabs.yellowTower;

        int yellowCost = MoneyManager.Instance.GetCost(BuildingType.YellowTower);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(yellowCost))
        {
            Instantiate(yellowPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 
    void onGreenButtonClick(ClickEvent evt)
    {
        GameObject greenPrefab = Game.Instance.buildingsPrefabs.greenTower;

        int greenCost = MoneyManager.Instance.GetCost(BuildingType.GreenTower);

        //Check if the player have enough money to build the building
        if (MoneyManager.Instance.SpendMoney(greenCost))
        {
            Instantiate(greenPrefab, Game.Instance.selector.position, Quaternion.identity);
            HideWheel();
        }
        else
        {
            Debug.Log("Can't build : not enough money");
        }
    } 
}
