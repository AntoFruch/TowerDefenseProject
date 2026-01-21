using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public static bool isPaused = false;
    InputAction PauseT;
    [SerializeField]    private PlayerInput playerInput;
    
    private VisualElement root; 

    private Button resumeButton;

    private Button quitButton;


    
    void Start()
    {
        PauseT = InputSystem.actions.FindAction("PauseMenu");                //Recherche l'ActionMap "Pause"
        playerInput.actions.FindActionMap("UI").Enable();                    // Active l'UI
        playerInput.actions.FindActionMap("Pause").Enable();                 // Active Pause 
        playerInput.actions.FindActionMap("Player").Enable();                // Active Player 
        playerInput.actions.FindActionMap("Wheel").Enable();                 // Active Wheel
        var uiDocument = GetComponent<UIDocument>();                         // Simplification du code 
        root = uiDocument.rootVisualElement;                                 // Simplification du code
        root.AddToClassList("hide");                                         // Cache le menu pause au début
        resumeButton = root.Q<Button>("Resume");                             // Création d'un bouton Resume pour reprendre le jeu
        quitButton = root.Q<Button>("Quit");                                 // Création d'un bouton Quit pour revenir sur le menu

        resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClick);      // Callback resume
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);          // Callback quit
        
    }


    private void OnQuitButtonClick(ClickEvent evt)                           // Méthode exécutée quand je clique sur Quit
    {
        AudioManager.Instance.PlayClick();
        Resume();
        SceneManager.LoadScene("MainMenu");
    }


    private void OnResumeButtonClick(ClickEvent evt)                         // Méthode exécutée quand je clique sur Resume
    {
        AudioManager.Instance.PlayClick();
        Resume();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (PauseT.WasPerformedThisFrame())
        {
            Debug.Log("Bouton Appuyé");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
                
    }
    private void Resume()
    {

        Time.timeScale=1f;
        isPaused = false;
        Debug.Log("Le jeu reprend");
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("Wheel").Enable();
        root.AddToClassList("hide");
        Game.Instance.selector.GetComponent<Renderer>().enabled = true;        
        
    }
    private void Pause()
    {

        Time.timeScale=0f;
        isPaused = true;
        Debug.Log("Le jeu est en pause");
        root.RemoveFromClassList("hide");
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("Wheel").Disable();
        playerInput.actions.FindActionMap("UI").Enable();
        Game.Instance.selector.GetComponent<Renderer>().enabled = false;

        
    }
}
