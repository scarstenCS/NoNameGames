using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;

    public GameObject player, pauseMenu;
    static private GameObject _player, _pauseMenu;
    static public GameManager Instance;
    static public bool isPaused = false;
    private PlayerControls controls;
    void Awake()
    {
        _instance = this;
        controls.Player.Pause.performed += ctx => TogglePause();
        
    }

    private void Start()
    {
        _player = player;
        _pauseMenu = pauseMenu;
    }



    static public void PlayerTakeDamage(int ammout)
    {
        Player p = _player.GetComponent<Player>();
        p.TakeDamage(ammout);
    }

    /// <summary>
    /// Toggles weather tha game is paused or not
    /// </summary>
    static public void TogglePause()
    {
        Debug.Log("pause hit!");
        isPaused = !_pauseMenu.activeSelf;
        _pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f:1f;

    }

}
