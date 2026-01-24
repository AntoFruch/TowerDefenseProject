using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour

{
    public static AudioManager Instance;
    
    [Header("Sources Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Bibliothèque de Sons")]
    //UI 
    public AudioClip clickSound;

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

    //SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    public void PlayClick() => PlaySFX(clickSound);
    
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
