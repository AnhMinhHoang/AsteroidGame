using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}

    [Header("--------- Audio Source ----------")]
    public AudioSource musicSorce;
    public AudioSource sfxSource;

    [Header("--------- Audio Clip ----------")]
    public AudioClip Background;
    public AudioClip MenuBGM;
    public AudioClip Death;
    public AudioClip CoinCollected;
    public AudioClip Shoot;
    public AudioClip AsteroidDestroyed;
    public AudioClip ClickSFX;
    public AudioClip DificultyUp;
    [Range(0, 100)]
    public int BGMVolumePercentage = 20;
    [Range(0, 100)]
    public int SFXVolumePercentage = 100;

    private float BGMVolume => BGMVolumePercentage / 100f;
    private float SFXVolume => SFXVolumePercentage / 100f;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(Instance == this) {
            Instance = null;
        }
    }

    public void Start()
    {
        musicSorce.volume = BGMVolume;
        sfxSource.volume = SFXVolume;
        if (SceneManager.GetActiveScene().name.Equals("Asteroids")){
            musicSorce.clip = Background;
        }
        else {
            musicSorce.clip = MenuBGM;
        }
        musicSorce.loop = true;
        musicSorce.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
