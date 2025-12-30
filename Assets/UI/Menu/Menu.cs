using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class Menu : MonoBehaviour
{   
    //Main Menu
    [SerializeField] private UIDocument mainMenuUIDoc;
    private VisualElement mainMenuContainer;
    private Button playButton;
    private Button settingsButton;
    private Button exitButton;

    

    
    // Map Selection Menu
    [SerializeField] private UIDocument mapSelectionUIDoc;
    private VisualElement mapSelectionContainer;
    
    private VisualElement mapButtonsContainer;

    private Label mapNameLabel;
    private VisualElement preview;
    private Button startButton;
    private Label startLabel;
    private Label errorLabel;

    //other
    private Dictionary<Button,string> mapBtnToFileName;
    

    void OnEnable()
    {   

        mapBtnToFileName = new Dictionary<Button, string>();

        // Main Menu
        
        mainMenuContainer = mainMenuUIDoc.rootVisualElement.Q<VisualElement>("menu-container");
        playButton = mainMenuUIDoc.rootVisualElement.Q<Button>("play-btn");
        settingsButton = mainMenuUIDoc.rootVisualElement.Q<Button>("settings-btn");
        exitButton = mainMenuUIDoc.rootVisualElement.Q<Button>("exit-btn");

        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClick);

        //Map Selection Menu
        
        mapSelectionContainer = mapSelectionUIDoc.rootVisualElement.Q<VisualElement>("menu-container");
        
        mapButtonsContainer = mapSelectionUIDoc.rootVisualElement.Q<ScrollView>("buttons-scrollview");
        
        mapNameLabel = mapSelectionUIDoc.rootVisualElement.Q<Label>("map-name");
        mapNameLabel.text = "";

        preview = mapSelectionUIDoc.rootVisualElement.Q<VisualElement>("preview");
        
        startButton = mapSelectionUIDoc.rootVisualElement.Q<Button>("start-btn");
        startButton.RegisterCallback<ClickEvent>(OnStartButtonClick);
        startButton.SetEnabled(false);
        startLabel = mapSelectionUIDoc.rootVisualElement.Q<Label>("start-label");
        
        errorLabel = mapSelectionUIDoc.rootVisualElement.Q<Label>("error-label");
        errorLabel.text = "";
        
        GenerateMapButtons();
        ShowMainMenu();
        HideMapSelection();
    }

    // Methods
    private void ShowMainMenu()
    {
        mainMenuUIDoc.rootVisualElement.RemoveFromClassList("hide");
    }
    private void ShowMapSelection()
    {
        mapSelectionUIDoc.rootVisualElement.RemoveFromClassList("hide");
    }

    private void HideMainMenu()
    {
        mainMenuUIDoc.rootVisualElement.AddToClassList("hide");
    }
    private void HideMapSelection()
    {
        mapSelectionUIDoc.rootVisualElement.AddToClassList("hide");
    }
    private void GenerateMapButtons()
    {
        string path = "../Maps";
        string folderPath = Path.Combine(Application.dataPath, path);


        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath,"*.png");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                
                Button btn = new Button();
                Label btnLabel = new Label();
                btn.Add(btnLabel);
                btnLabel.text = fileName;

                btn.AddToClassList("btn");
                btn.AddToClassList("btn-map");

                btnLabel.AddToClassList("labels");
                btnLabel.AddToClassList("btn-map-label");

                btn.RegisterCallback<ClickEvent>(OnMapButtonClick);

                mapButtonsContainer.Add(btn);

                mapBtnToFileName.Add(btn, fileName);
            }
        }
        else
        {
            Debug.LogError("Folder not found: " + folderPath);
        }
    }   
    
    // BUTTONS CALLBACKS
    // Main Menu
    private void OnPlayButtonClick(ClickEvent evt)
    {
        ShowMapSelection();
        HideMainMenu();
    }
    private void OnExitButtonClick(ClickEvent evt)
    {
        Application.Quit();
    }
    
    // Map Selection Menu
    private void OnStartButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene("SampleScene");
    }
    void OnMapButtonClick(ClickEvent evt)
    {
        Button btn = evt.currentTarget as Button;
        string fileName = mapBtnToFileName[btn];
        string filePath = Path.Combine(Path.Combine(Application.dataPath, "../Maps"), mapBtnToFileName[btn]);
        
        
        // Change style ( blue button when selected )
        btn.AddToClassList("btn-map-selected");
        foreach (Button b in mapBtnToFileName.Keys)
        {
            if (b != btn)
            {
                b.RemoveFromClassList("btn-map-selected");
            }
        }

        mapNameLabel.text = fileName;

        preview.style.backgroundImage = new StyleBackground(FileAPI.ReadImageAsTexture2D(filePath));

        try
        {
            GameConfig.LoadMap(filePath);
            startButton.SetEnabled(true);
            errorLabel.text = "";
        }catch(System.Exception e)
        {
            errorLabel.text = "Error : " + e.Message;
            startButton.SetEnabled(false);
        }
    }
}
