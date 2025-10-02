using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


// https://www.youtube.com/watch?v=DU7cgVsU2rM
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playerAttack, playerHit, enemyHit;
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

    private void PlaySound(AudioClip[] clip)
    {
        int idx = Random.Range(0, clip.Length);
        audioSource.clip = clip[idx];
        audioSource.Play();
    }

    static public void SfxPlayerAttack()
    {
        Instance.PlaySound(Instance.playerAttack);
    }

    static public void SfxEnemyHit()
    {
        Instance.PlaySound(Instance.enemyHit);
    }

    static public void SfxPlayerHit()
    {
        Instance.PlaySound(Instance.playerHit);
    }


     public void setMainVol(float value)
    {
        Instance.audioMixer.SetFloat("mainVolume", Mathf.Log10(value) * 20f);
    }

     public void setSFXVol(float value)
    {
        Instance.audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20f);
    }

     public void setMusicVol(float value)
    {
        Instance.audioMixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
    }

}
