using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour

{
    public static AudioManager Instance;
    
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer mixer;


    [Header("Sources Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Bibliothèque de Sons")]
    //UI 
    public AudioClip clickSound;
    public AudioClip deathSound;

    //Music

    public AudioClip menuAmbientSound;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);   
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Mixer 
    public void SetMixerVolume(string name, float value)
    {
        mixer.SetFloat(name, value);
    }

    //SFX
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayClick() => PlaySFX(clickSound);
    public void PlayDeath() => PlaySFX(deathSound);
    
    //Musique
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip == musicClip) return;

        musicSource.Stop();
        musicSource.clip=musicClip;
        musicSource.Play();
    }

    public void PlaymenuAmbientSound() => PlayMusic(menuAmbientSound);


}
