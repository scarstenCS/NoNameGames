using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


// https://www.youtube.com/watch?v=DU7cgVsU2rM
public class AudioManager : MonoBehaviour
{
    public AudioClip playerAttack, playerHit, enemyHit;
    static private AudioManager _instance;
    static public AudioManager Instance { get { return _instance; } }



    [SerializeField] private static AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    void Awake()
    {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    static public void SfxPlayerAttack()
    {
        audioSource.clip = Instance.playerAttack;
        audioSource.Play();
    }

    static public void SfxEnemyHit()
    {
        audioSource.clip = Instance.enemyHit;
        audioSource.Play();
    }

    static public void SfxPlayerHit()
    {
        audioSource.clip = Instance.playerHit;
        audioSource.Play();
    }


    static public void setMasterVol(float value)
    {
        Instance.audioMixer.SetFloat("masterVolume", Mathf.Log10(value) * 20f);
    }

    static public void setSFXVol(float value)
    {
        Instance.audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20f);
    }

    static public void setMusicVol(float value)
    {
        Instance.audioMixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
    }

}
