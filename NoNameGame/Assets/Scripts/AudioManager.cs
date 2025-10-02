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

}
