using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;

    static public GameObject player;
    static public GameManager Instance;
    static public GameObject pauseMenu;
    static public bool isPaused = false;
    void Awake()
    {
        _instance = this;
    }

    static public void PlayerTakeDamage(int ammout)
    {
        Player p = player.GetComponent<Player>();
        p.TakeDamage(ammout);
    }

    /// <summary>
    /// Toggles weather tha game is paused or not
    /// </summary>
    public void TogglePause()
    {
        isPaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f:1f;
        //if (isPaused)
        //{
        //    EventSystem.current.SetSelectedGameObject(null);
        //    EventSystem.current.SetSelectedGameObject(resumeBtn);
        //}

    }

}
