using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DelMovController : MonoBehaviour
{
    [SerializeField] private UIDocument delMovUIDoc;
    private VisualElement container;
    private Button deleteButton;
    private Button moveButton;
    private Button quitButton;

    // Stats
    private VisualElement statsContainer;
    private Label nameLabel;
    private Label rangeLabel; 
    private Label damageLabel; 
    private Label fireRateLabel; 

    private Building selectedBuild;

    public bool active {get;private set;}

    private bool ignoreNextClick;

    void OnEnable()
    {
        container = delMovUIDoc.rootVisualElement.Q("wheel-container");

        deleteButton = delMovUIDoc.rootVisualElement.Q<Button>("delete-btn");
        deleteButton.RegisterCallback<ClickEvent>(onDeleteButtonClick);

        moveButton = delMovUIDoc.rootVisualElement.Q<Button>("move-btn");
        moveButton.RegisterCallback<ClickEvent>(onMoveButtonClick);

        quitButton = delMovUIDoc.rootVisualElement.Q<Button>("quit-btn");
        quitButton.RegisterCallback<PointerUpEvent>(onQuitClick);

        statsContainer = delMovUIDoc.rootVisualElement.Q<VisualElement>("stats-container");
        nameLabel = delMovUIDoc.rootVisualElement.Q<Label>("name");
        rangeLabel = delMovUIDoc.rootVisualElement.Q<Label>("range-label");
        damageLabel = delMovUIDoc.rootVisualElement.Q<Label>("damage-label");
        fireRateLabel = delMovUIDoc.rootVisualElement.Q<Label>("firerate-label");
        Hide();
    }

    public void Show(Vector2 pos)
    {
        ignoreNextClick = true;
        try {
            selectedBuild = Game.Instance.buildings.First(
                b => UEExtension.Vector3toVector2Int(b.transform.position) == UEExtension.Vector3toVector2Int(Game.Instance.selector.position));
            nameLabel.text = selectedBuild.name;
            ShowStats();
        } catch (InvalidOperationException e)
        {
            Debug.LogError("No Build at "+pos);
            Debug.LogError(e);
        }

        active=true;

        delMovUIDoc.rootVisualElement.RemoveFromClassList("hide");
        container.style.position = Position.Absolute;

        container.style.left = pos.x;
        container.style.bottom = pos.y;
        this.container.style.translate = new Translate(
            new Length(-50, LengthUnit.Percent),
            new Length(50, LengthUnit.Percent)
        );

    }
    public void Hide()
    {
        active=false;
        delMovUIDoc.rootVisualElement.AddToClassList("hide");
    }

    void ShowStats()
    {
        if (selectedBuild is Tower tower){
            statsContainer.RemoveFromClassList("hide");
            rangeLabel.text = tower.CurrentRange.ToString();
            damageLabel.text = tower.CurrentDamage.ToString();
            fireRateLabel.text = tower.CurrentFireRate.ToString();
        } else
        {
            statsContainer.AddToClassList("hide");
        }
    }

    // Callbacks
    void onQuitClick(PointerUpEvent evt)
    {
        if (ignoreNextClick)
        {
            ignoreNextClick = false;
            return;
        }   
        Hide();
        AudioManager.Instance.PlayClick();
    }

    void onMoveButtonClick(ClickEvent evt)
    {
        BuildingPlacementManager.Instance.StartMoving(selectedBuild); 
        Hide();
        AudioManager.Instance.PlayClick();
    }
    void onDeleteButtonClick(ClickEvent evt)
    {
        if(selectedBuild != null)
        {
            selectedBuild.SellBuilding();
        }
      
        selectedBuild = null;
        Hide();
        AudioManager.Instance.PlayClick();
    }
}
