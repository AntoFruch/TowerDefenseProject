using UnityEngine;
using UnityEngine.UIElements;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] UIDocument UIDoc;
    private VisualElement root;
    private VisualElement mask;
    private MonsterController monster;
    void OnEnable()
    {
        monster = this.transform.parent.GetComponent<MonsterController>();
        root = UIDoc.rootVisualElement;
        mask = root.Q<VisualElement>("mask");
    }

    void Show()
    {
        root.RemoveFromClassList("hide");
    }

    void Hide()
    {
        root.AddToClassList("hide");
    }

    void Update()
    {
        TrackPosition();
        UpdateHealthBar();
    }
    [SerializeField] private Vector2 offset;
    void TrackPosition()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(monster.transform.position) + new Vector3(offset.x, offset.y, 0);
        root.style.left = pos.x;
        root.style.top = Screen.height - pos.y;
    }
    void UpdateHealthBar()
    {
        mask.style.width = Length.Percent(100 * monster.health/monster.maxHealth);
    }
}
