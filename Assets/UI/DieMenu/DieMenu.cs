using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DieMenu : MonoBehaviour
{
    public static DieMenu Instance;

    private VisualElement root;
    
    //buttons
    private Button restartButton;
    private Button quitButton;

    //stats
    private Label monstersDefeated;
    private Label wavesSurvived;
    private Label moneySpent;



    void Start()
    {
        Instance = this; 

        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.AddToClassList("hide"); 
        restartButton = root.Q<Button>("Restart");
        quitButton = root.Q<Button>("Quit");

        monstersDefeated = root.Q<Label>("monsters-defeated");
        wavesSurvived = root.Q<Label>("waves-survived");
        moneySpent = root.Q<Label>("money-spent");

        restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);
    } 

    public void ShowDieMenu()
    {
        AudioManager.Instance.PlayDeath();
        Game.Instance.selector.GetComponent<Renderer>().enabled = false;
        Time.timeScale=0f;
        root.RemoveFromClassList("hide");
        SetStats();
    }

    void SetStats()
    {
        monstersDefeated.text = "Monster defeated : " + GameStatsManager.Instance.monstersDefeated.ToString();
        wavesSurvived.text = "Waves survived : " + GameStatsManager.Instance.wavesSurvived.ToString();
        moneySpent.text = "Money spent : " + GameStatsManager.Instance.moneySpent.ToString();
    }

    // Callbacks
    private void OnRestartButtonClick(ClickEvent evt)
    {
        Time.timeScale=1f;
        AudioManager.Instance.PlayClick();
        Game.Instance.selector.GetComponent<Renderer>().enabled = true;
        SceneManager.LoadScene("SampleScene");
    }

    private void OnQuitButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        Game.Instance.selector.GetComponent<Renderer>().enabled = true;
        Time.timeScale=1f;
        SceneManager.LoadScene("MainMenu");
    }
}
