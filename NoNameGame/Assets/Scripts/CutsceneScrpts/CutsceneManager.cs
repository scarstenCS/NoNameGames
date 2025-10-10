using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    static private CutsceneManager _instance;
    static public CutsceneManager Instance;
    static public GameObject _skipButton;
    [SerializeField] GameObject skipButton;
    public string game = "Prototype Scene";
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        _skipButton = skipButton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToGame()
    {
        Debug.Log("Going to game");
        Time.timeScale = 1f;
        SceneManager.LoadScene(game);
    }
}
