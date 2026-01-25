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

        Hide();
    }

    public void Show(Vector2 pos)
    {
        ignoreNextClick = true;
        try {
            selectedBuild = Game.Instance.buildings.First(
                b => UEExtension.Vector3toVector2Int(b.transform.position) == UEExtension.Vector3toVector2Int(Game.Instance.selector.position));
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
