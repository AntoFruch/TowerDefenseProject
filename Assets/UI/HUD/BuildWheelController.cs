using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BuildWheelController : MonoBehaviour
{
    [SerializeField] UIDocument uIDocument;

    private VisualElement wheel;


    void OnEnable()
    {
        this.wheel = uIDocument.rootVisualElement.Q("wheel-container");
        this.HideWheel();    
    }

    public void ShowWheelAtPosition(Vector2 pos)
    {
        Debug.Log("showing Wheel");
        this.uIDocument.rootVisualElement.RemoveFromClassList("hide");
        this.wheel.style.position = Position.Absolute;

        float wheelWidth = this.wheel.resolvedStyle.width;
        float wheelHeight = this.wheel.resolvedStyle.height;

        Debug.Log(wheelHeight + " " + wheelWidth);

        this.wheel.style.left = pos.x - 200;
        this.wheel.style.bottom = pos.y - 200; // A MODIFER ABSOLUMENT ( TRICHE )
    }
    public void HideWheel()
    {
        this.uIDocument.rootVisualElement.AddToClassList("hide");
    }
}
