using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DieMenu : MonoBehaviour
{
    public static DieMenu Instance;

    private VisualElement root;
    private Button restartButton;
    private Button quitButton;

    void Start()
    {
        Instance = this; 

        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.AddToClassList("hide"); 
        restartButton = root.Q<Button>("Restart");
        quitButton = root.Q<Button>("Quit");

        restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);
    } 

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

    public void ShowDieMenu()
    {
        AudioManager.Instance.PlayDeath();
        Game.Instance.selector.GetComponent<Renderer>().enabled = false;
        Time.timeScale=0f;
        root.RemoveFromClassList("hide");
    }

}
