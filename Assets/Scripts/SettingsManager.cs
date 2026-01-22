using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private VisualElement brightness;

    [SerializeField] private GameObject audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        var uiDoc = this.GetComponent<UIDocument>();
        
        //Brightness 
        brightness = uiDoc.rootVisualElement.Q<VisualElement>("Brightness");
        
        
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    void Update()
    {
        
    }

    public void SetSound(float value)
    {
        //pas fini putain
    }

    public void SetMusic(float value)
    {
        
    }

    public void SetMaster(float value)
    {
        
    }

    public void SetBrightness(float value)
    {
        brightness.style.opacity = value;
        PlayerPrefs.SetFloat("Brightness",value);
    }

    private void LoadSettings()
    {
        // Brightness
        float b = PlayerPrefs.GetFloat("Brightness",0f); 
        SetBrightness(b);  
    }
}
