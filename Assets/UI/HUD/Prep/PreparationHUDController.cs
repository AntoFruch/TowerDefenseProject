using UnityEngine;
using UnityEngine.UIElements;

public class PreparationHUDController : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private VisualElement root;
    
    //fight button
    private Button fightButton;

    // title label
    private Label title;
    void OnEnable()
    {
        root = UIDoc.rootVisualElement;

        fightButton = root.Q<Button>("start-btn");
        fightButton.RegisterCallback<ClickEvent>(OnFightButtonClick);
        title = root.Q<Label>("title");

    }

    void Update()
    {
        if (Game.Instance.state == GameState.Preparation)
        {
            Show();
        } else
        {
            Hide();
        }

        UpdateTitle();
    }

    void Show()
    {
        root.RemoveFromClassList("hide");
    }

    void Hide()
    {
        root.AddToClassList("hide");
    }

    void UpdateTitle()
    {
        title.text = "Preparation - Wave n°" + (WaveManager.Instance.waveIndex + 1);
    }

    // Callbacks    
    void OnFightButtonClick(ClickEvent evt)
    {
        Game.Instance.SetState(GameState.Defense);
        AudioManager.Instance.PlayClick();
    }

}
