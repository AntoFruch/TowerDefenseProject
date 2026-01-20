using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private VisualElement brightness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        var uiDoc = this.GetComponent<UIDocument>();
        brightness = uiDoc.rootVisualElement.Q<VisualElement>("Brightness");
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    void Update()
    {
        Debug.Log(brightness.style.opacity);
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
