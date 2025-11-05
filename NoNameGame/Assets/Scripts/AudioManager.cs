using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


// https://www.youtube.com/watch?v=DU7cgVsU2rM
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playerAttack, playerHit, enemy1Hit, enemy2Hit, turretShoot, enemy1Spawn, enemy2Spawn
    ,enemy1Death, enemy2Death,playerDeath;
    static private AudioManager _instance;
    static public AudioManager Instance { get { return _instance; } }

    const float drumsVolume = -3f;

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

    private void PlaySound(AudioClip[] clip, float volume = 1.0f)
    {
        int idx = Random.Range(0, clip.Length);
        audioSource.clip = clip[idx];
        audioSource.PlayOneShot(audioSource.clip, volume);
    }

    static public void SfxPlayerAttack()
    {
        Instance.PlaySound(Instance.playerAttack);
    }

    static public void SfxEnemy1Hit()
    {
        Instance.PlaySound(Instance.enemy1Hit);
    }
    static public void SfxEnemy1Spawn()
    {
        Instance.PlaySound(Instance.enemy1Spawn);
    }
    static public void SfxEnemy2Hit()
    {
        Instance.PlaySound(Instance.enemy2Hit);
    }
    static public void SfxEnemy2Spawn()
    {
        Instance.PlaySound(Instance.enemy2Spawn);
    }
    static public void SfxTurretShoot()
    {
        Instance.PlaySound(Instance.turretShoot);
    }

    static public void SfxPlayerHit()
    {
        Instance.PlaySound(Instance.playerHit);
    }
    static public void SfxEnemy2Death()
    {
        Instance.PlaySound(Instance.enemy2Death, 1.25f);
    }

    static public void SfxEnemy1Death()
    {
        Instance.PlaySound(Instance.enemy1Death, 0.5f);
    }

    static public void SfxPlayerDeath()
    {
        Instance.PlaySound(Instance.playerDeath);
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

    public void stopDrums()
    {
        Instance.audioMixer.SetFloat("drumVolume", -80);
    }
    
    public void startDrums()
    {
        Instance.audioMixer.SetFloat("drumVolume", drumsVolume);
    }
}
