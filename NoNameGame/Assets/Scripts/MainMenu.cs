using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public const string gameSceneName = "Cutscenes";
    public const string titleSceneName = "MainMenu";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void toMainMenu()
    {
        AudioManager.SfxSelect();
        SceneManager.LoadScene(titleSceneName);
    }

    public void StartGame()
    {
        AudioManager.SfxSelect();
        SceneManager.LoadScene(gameSceneName);
    }
}
