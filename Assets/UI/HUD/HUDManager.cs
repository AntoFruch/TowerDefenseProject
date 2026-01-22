using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{   
    [SerializeField] private BuildWheelController wheel;
    public BuildWheelController Wheel => wheel;
    [SerializeField] private DelMovController delMov;
    public DelMovController DelMov => delMov;
    [SerializeField] private StaticHUDController staticHUD;
    public StaticHUDController StaticHUD => staticHUD;
    
    private IPanel[] uiPanels;
    void Start()
    {
        this.uiPanels = this.GetComponentsInChildren<UIDocument>()
                            .OrderByDescending(document=>document.sortingOrder)
                            .Select(document => document.rootVisualElement.panel)
                            .ToArray();
    }

    VisualElement hoveredElement;
    void UpdateHoveredElement(Vector2 mousePos)
    {
        // Compute hovered VisualElement.
        hoveredElement = null;
        Vector2 mousePosition = mousePos;

        // InputSystem uses coordinates with bottom-left as the origin. UI
        // Toolkit wants top-left origin screen coordinates. You need to
        // pass in a flipped y coordinate.
        mousePosition.y = Screen.height - mousePosition.y;

        for (int index = 0; index < this.uiPanels.Length; index++)
        {
            Vector2 localPosition = RuntimePanelUtils.ScreenToPanel(
                this.uiPanels[index],
                mousePosition
            );

            VisualElement element = this.uiPanels[index].Pick(localPosition);
            if (element != null)
            {
                hoveredElement = element;
                break;
            }
        }
    }
    public void ShowWheelMenu(Vector2 pos)
    {
        UpdateHoveredElement(pos);
        if (hoveredElement==null) 
        {
            if (BuildingPlacementManager.IsPlaceTaken(UEExtension.Vector3toVector2Int(Game.Instance.selector.position)))
            {
                delMov.Show(pos);
            } else
            {
                wheel.ShowWheelAtPosition(pos);
            }
        }
    }
}
