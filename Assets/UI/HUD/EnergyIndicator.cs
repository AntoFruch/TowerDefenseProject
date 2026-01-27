using UnityEngine;
using UnityEngine.UIElements;

public class EnergyIndicator : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private VisualElement root;
    private Label energyLevel;
    private Building build;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        root = UIDoc.rootVisualElement;
        energyLevel = root.Q<Label>("energy-label");
        
        build = this.transform.parent.gameObject.GetComponent<Building>();
        Show();
    }
    void Update()
    {
        TrackPosition();
        UpdateLabel();
    }

    void Show()
    {
        root.RemoveFromClassList("hide");
    }
    void Hide()
    {
        root.AddToClassList("hide");
    }
    [SerializeField] Vector2 offset;
    void TrackPosition()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(build.transform.position) + new Vector3(offset.x, offset.y, 0);
        root.style.left = pos.x;
        root.style.top = Screen.height - pos.y;
    }
    
    void UpdateLabel()
    {
        if (build is Tower tower)
        {
            energyLevel.text = tower.GetPower().ToString()+"/"+tower.PowerConsumption.ToString();
        } else if (build is PowerPlant pp) {
            energyLevel.text = pp.PowerOutput.ToString();
        } else
        {
            Hide();
        }
    }
}
