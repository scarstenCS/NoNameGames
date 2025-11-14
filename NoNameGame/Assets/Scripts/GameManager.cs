using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;
    public GameObject player, pauseMenu, menuButton;
    static private GameObject _player, _pauseMenu, _menuButton;
    static public GameManager Instance { get { return _instance; } }
    private UpgradeManager upgradeManager;
    static public bool isPaused = false;
    private PlayerControls controls;
    static public GameObject _gameOverPanel, _mainMenuSelected;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject mainMenuSelected;
    private Player playerStats;
    [SerializeField] TMP_Text[] statsLabels;
    static public float
    minX = -10f,
    maxX = 4.45f,
    minY = -6f,
    maxY = 5.31f;

    static private float timeScale = 1f;

    void Awake()
    {
        _instance = this;
        isPaused = false; 
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
        UnityEngine.Debug.Log("ispaused: " + isPaused);
        Time.timeScale = isPaused ? 0f : timeScale;
        Player p = _player.GetComponent<Player>();
        int numberOfUpgrades = UpgradeManager.Instance.totalUpgrades;
        //updates stats labels
        if (Instance != null && Instance.statsLabels != null)
        {
            Instance.statsLabels[0].text = "SPD: " + p.Speed;
            Instance.statsLabels[1].text = "HP: " + p.MaxHealth;
            Instance.statsLabels[2].text = "Range: " + p.basicWeaponDistance;
            Instance.statsLabels[3].text = "ATK: " + p.basicWeaponDmg;
            Instance.statsLabels[4].text = "ATK SPD: " + p.basicWeaponSpeed;
            Instance.statsLabels[5].text = "ATK Spread: " + p.totalBasicAttacksCount;
            Instance.statsLabels[6].text = "Pierce: " + p.basicWeaponPierce;
            Instance.statsLabels[7].text = "ATK Size: " + (p.basicWeaponSize.x * 100).ToString("F0") + "%";

        }

    }

    static public void PlayerDied()
    {
        Time.timeScale = 0f;
        if (_gameOverPanel) _gameOverPanel.SetActive(true);

        if (_mainMenuSelected && EventSystem.current)
            EventSystem.current.SetSelectedGameObject(_mainMenuSelected);
    }

    /// <summary>
    /// sets time scale while saving value in timescale privale variable
    /// </summary>
    /// <param name="time"></param>
    static public void ChangeTimeScale(float time)
    {
        timeScale = time;
        Time.timeScale = timeScale;
        
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void OnEnable()
    {
        
        if (!player)
        {
            Player found = FindObjectOfType<Player>();
            if (found) player = found.gameObject;
        }

        
        if (player)
        {
            Player comp = player.GetComponent<Player>();
            if (comp) comp.OnDied += HandlePlayerDied;
        }

        if (_gameOverPanel) _gameOverPanel.SetActive(false); 
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
    public void TogglePauseButton()
    {
        
        TogglePause();

    }

}
