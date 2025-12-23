using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MEnu : MonoBehaviour
{   
    private UIDocument uiDocument;
    private Button playButton;
    private Button optionButton;
    private Button exitButton;


    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        playButton = uiDocument.rootVisualElement.Q<Button>("Jouer");
        optionButton = uiDocument.rootVisualElement.Q<Button>("Option");
        exitButton = uiDocument.rootVisualElement.Q<Button>("Exit");
        
        
        
        optionButton.clicked += OptionButtonOnClicked;

        playButton.clicked += PlayButtonOnClicked;
    }

    private void PlayButtonOnClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OptionButtonOnClicked()
    {
        Debug.Log("Option button clicked!");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
