using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static private GameManager _instance;
    public GameObject player, pauseMenu, menuButton;
    static private GameObject _player, _pauseMenu, _menuButton;
    static public GameManager Instance;
    static public bool isPaused = false;
    private PlayerControls controls;
    static public GameObject _gameOverPanel, _mainMenuSelected;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject mainMenuSelected;
    void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        _player = player;
        _pauseMenu = pauseMenu;
        _menuButton = menuButton;
        _gameOverPanel = gameOverPanel;
        _mainMenuSelected = mainMenuSelected;
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
        isPaused = !_pauseMenu.activeSelf;
        _pauseMenu.SetActive(isPaused);
        _menuButton.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    static public void PlayerDied()
    {
        Time.timeScale = 0f;
        if (_gameOverPanel) _gameOverPanel.SetActive(true);

        if (_mainMenuSelected && EventSystem.current)
            EventSystem.current.SetSelectedGameObject(_mainMenuSelected);
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void OnEnable()
    {
        // If no player assigned in Inspector, find one and assign its GameObject
        if (!player)
        {
            Player found = FindObjectOfType<Player>();
            if (found) player = found.gameObject;
        }

        // Subscribe to OnDied on the Player component
        if (player)
        {
            Player comp = player.GetComponent<Player>();
            if (comp) comp.OnDied += HandlePlayerDied;
        }

        if (_gameOverPanel) _gameOverPanel.SetActive(false); // start hidden
    }

    void OnDisable()
    {
        if (player)
        {
            Player comp = player.GetComponent<Player>();
            if (comp) comp.OnDied -= HandlePlayerDied;
        }
    }
    
    void HandlePlayerDied()
    {
        PlayerDied();
    }

}
