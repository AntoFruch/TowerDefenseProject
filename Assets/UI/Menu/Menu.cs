using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
    private Button backButton;
    private DropdownField resolutionDropdown;
    private Resolution[] filteredResolutions;


    // Settings Menu

    [SerializeField] private UIDocument settingsUIDoc;

    [SerializeField] private UIDocument settingsParametersUIDoc;

    private Button backSettingsButton;

    //other
    private Dictionary<Button,string> mapBtnToFileName;
    

    void OnEnable()
    {   
        //Débute la music d'ambiance du main menu.
        AudioManager.Instance.PlaymenuAmbientSound();
        




        mapBtnToFileName = new Dictionary<Button, string>();

        // Main Menu
        
        mainMenuContainer = mainMenuUIDoc.rootVisualElement.Q<VisualElement>("menu-container");
        playButton = mainMenuUIDoc.rootVisualElement.Q<Button>("play-btn");
        settingsButton = mainMenuUIDoc.rootVisualElement.Q<Button>("settings-btn");
        exitButton = mainMenuUIDoc.rootVisualElement.Q<Button>("exit-btn");

        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);

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

        backButton = mapSelectionUIDoc.rootVisualElement.Q<Button>("back-btn");
        backButton.RegisterCallback<ClickEvent>(OnBackMapSelectionButtonClick);

        //Settings Menu
            //Back Settings
        backSettingsButton = settingsUIDoc.rootVisualElement.Q<Button>("back-Settings");
        backSettingsButton.RegisterCallback<ClickEvent>(OnBackSettingsOnCLick);
            //Brightness 
       
        var brightSlider = settingsUIDoc.rootVisualElement.Q<Slider>("BrightnessSlider");
        brightSlider.lowValue=0f;
        brightSlider.highValue=0.8f;
        brightSlider.value=PlayerPrefs.GetFloat("Brightness",0f);       
        brightSlider.RegisterValueChangedCallback(evt=> {SettingsManager.Instance.SetBrightness(evt.newValue);});

            //Sound

        var soundSlider = settingsUIDoc.rootVisualElement.Q<Slider>("SoundSlider");
        soundSlider.lowValue=0f;
        soundSlider.highValue=100f; 
        soundSlider.value = PlayerPrefs.GetFloat("FXVol", 0.001f); 
        soundSlider.RegisterValueChangedCallback(evt => SettingsManager.Instance.SetSound(evt.newValue));
        

            //Music 

        var musicSlider = settingsUIDoc.rootVisualElement.Q<Slider>("MusicSlider");
        soundSlider.lowValue=0f;
        soundSlider.highValue=100f; 
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.001f);
        musicSlider.RegisterValueChangedCallback(evt => SettingsManager.Instance.SetMusic(evt.newValue));
            //Master

        var masterSlider = settingsUIDoc.rootVisualElement.Q<Slider>("MasterSlider");
        soundSlider.lowValue=0f;
        soundSlider.highValue=100f; 
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.001f);
        masterSlider.RegisterValueChangedCallback(evt => SettingsManager.Instance.SetMaster(evt.newValue));

            //Resolution
        resolutionDropdown = settingsUIDoc.rootVisualElement.Q<DropdownField>("ResolutionDropdown");
        if (resolutionDropdown != null)
        {
            SetupResolutionDropdown();
            resolutionDropdown.RegisterValueChangedCallback(evt => OnResolutionChanged());
        }

        GenerateMapButtons();
        ShowMainMenu();
        HideMapSelection();
        HideSettingsMenu();
    }

    // Methods

    private void ShowSettingsMenu()
    {
        settingsUIDoc.rootVisualElement.RemoveFromClassList("hide");
    }
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
    private void HideSettingsMenu()
    {
        settingsUIDoc.rootVisualElement.AddToClassList("hide");
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
        AudioManager.Instance.PlayClick();
        ShowMapSelection();
        HideMainMenu();
    }
    private void OnExitButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        Application.Quit();
    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        ShowSettingsMenu();
        HideMainMenu();        
    }

    // Settings Menu

    private void OnBackSettingsOnCLick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        ShowMainMenu();
        HideSettingsMenu();
    } 

    // Map Selection Menu
    private void OnStartButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadScene("SampleScene");
    }
    void OnMapButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
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
    void OnBackMapSelectionButtonClick(ClickEvent evt)
    {
        AudioManager.Instance.PlayClick();
        HideMapSelection();
        ShowMainMenu();
    }

    private void SetupResolutionDropdown()
    {
        Resolution[] allResolutions = Screen.resolutions;
        List<Resolution> uniqueResolutions = new List<Resolution>();
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = allResolutions.Length - 1; i >= 0; i--)
        {
            Resolution res = allResolutions[i];

            if (!uniqueResolutions.Any(x => x.width == res.width && x.height == res.height))
            {
                uniqueResolutions.Add(res);
                options.Add(res.width + " x " + res.height);

                if (res.width == Screen.width && res.height == Screen.height)
                {
                    currentResIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        filteredResolutions = uniqueResolutions.ToArray();
        resolutionDropdown.choices = options;

        if (options.Count > 0)
        {
            resolutionDropdown.index = currentResIndex;
            resolutionDropdown.value = options[currentResIndex];
        }
    }

    private void OnResolutionChanged()
    {
        int index = resolutionDropdown.index;
        if (index >= 0 && index < filteredResolutions.Length)
        {
            Resolution res = filteredResolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }
}
