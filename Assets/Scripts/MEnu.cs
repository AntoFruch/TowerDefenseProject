using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;

public class MEnu : MonoBehaviour



{   
    
    private UIDocument uiDocument;
    private Button jouer;
    private Button option;
    private Button exit;

    private VisualElement MenuContainer;

    
    //MapSelector 
    private VisualElement MapContainer;

    private void Awake()
    {   
        //Menu
        
        uiDocument = GetComponent<UIDocument>();
        MenuContainer = uiDocument.rootVisualElement.Q<VisualElement>("MenuContainer");
        jouer = uiDocument.rootVisualElement.Q<Button>("Jouer");
        option = uiDocument.rootVisualElement.Q<Button>("Option");
        exit = uiDocument.rootVisualElement.Q<Button>("Exit");


        jouer.clicked += ShowMapSelector;
        exit.clicked += ExitOnButtonClicked;

        //MapSelector
        
        MapContainer = uiDocument.rootVisualElement.Q<VisualElement>("MapContainer");

        GenerateMapButtons();   

    }


    
    private void ExitOnButtonClicked()
    {
        Application.Quit();
    }
    private void ShowMapSelector()
    {
        
        MenuContainer.style.display = DisplayStyle.None;
        MapContainer.style.display = DisplayStyle.Flex;
    }

    private void ShowMainMenu()
    {
        MapContainer.style.display = DisplayStyle.None;
        MenuContainer.style.display = DisplayStyle.Flex;
    }
    void GenerateMapButtons()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath,"Maps");


        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath,"*.png");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                
                Button btn = new Button();
                btn.text = fileName;
                btn.focusable = false;
                btn.style.height = 40f;

                btn.AddToClassList("menu-button");

                btn.clicked += () => {
                    GameConfig.SelectedMapPath=filePath;
                    uiDocument.rootVisualElement.Blur();
                    SceneManager.LoadScene("SampleScene");
                };

                MapContainer.Add(btn);

            }
        }
        else
        {
            Debug.LogError("Folder not found: " + folderPath);
        }
    }   
    
}
