using UnityEngine;
using UnityEngine.UIElements;

public class DefenseHUDController : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private VisualElement root;

    // progress bar
    private VisualElement mask;

    //title
    private Label title;

    void OnEnable()
    {
        root = UIDoc.rootVisualElement;
        mask = root.Q<VisualElement>("progressbar-mask");   
    }

    void Show()
    {
        root.RemoveFromClassList("hide");
    }

    void Hide()
    {
        root.AddToClassList("hide");
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.state == GameState.Defense)
        {
            Show();
        } else
        {
            Hide();
        }

        UpdateProgress();
    }

    void UpdateProgress()
    {
        if (WaveManager.Instance.GetCurrentWaveLength() != 0)
        {
            mask.style.width = Length.Percent((float)Game.Instance.monsters.Count / WaveManager.Instance.GetCurrentWaveLength() * 100f);
        }
    }

    void UpdateTitle()
    {
        title.text = "Wave n°"+WaveManager.Instance.waveIndex;
    }
}
