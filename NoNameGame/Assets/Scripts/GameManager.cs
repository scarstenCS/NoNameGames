using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;

    static public GameObject player, pauseMenu;
    static public GameManager Instance;
    static public bool isPaused = false;
    private PlayerControls controls;
    void Awake()
    {
        _instance = this;
        controls.Player.Pause.performed += ctx => TogglePause();
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

    }

}
