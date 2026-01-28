using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Audio;
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
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
        
        
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        var uiDoc = this.GetComponent<UIDocument>();
        //Brightness 
        brightness = uiDoc.rootVisualElement.Q<VisualElement>("Brightness");
        LoadSettings();
    }

    //Volume Audio
    public void SetVolume(string name, float value)
    {
        float normalizedValue = value / 100f;
        float volumeInDb = Mathf.Log10(Mathf.Max(0.0001f, normalizedValue)) * 20f;
        AudioManager.Instance.SetMixerVolume(name, volumeInDb);
        PlayerPrefs.SetFloat(name, value);
        
    }

    public void SetSound(float value) => SetVolume("FXVol",value);
    public void SetMusic(float value) => SetVolume("MusicVol",value);
    public void SetMaster(float value) => SetVolume("MasterVol",value);


    //Brightness
    public void SetBrightness(float value)
    {
        brightness.style.opacity = (1-value)*0.8f;
        PlayerPrefs.SetFloat("Brightness",value);
    }




    //Chargement des paramètres des joueurs
    private void LoadSettings()
    {
        // Brightness
        
        SetBrightness(PlayerPrefs.GetFloat("Brightness",0f));

        //Volume 
        SetSound(PlayerPrefs.GetFloat("FXVol",0f));
        SetMusic(PlayerPrefs.GetFloat("MusicVol",0f));
        SetMaster(PlayerPrefs.GetFloat("MasterVol",0f));
    
    }
}
