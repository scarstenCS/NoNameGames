using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UserInterface : MonoBehaviour
{
    [Header("Wire these in the Inspector")]
    public Player player;
    public TMP_Text healthText;              // drag your "Health: ..." TMP Text
    public TMP_Text enemiesLeftText;         // drag your "Enemies left: ..." TMP Text
    // Start is called before the first frame update
    void OnEnable()
    {
        if (player != null)
            player.HealthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        if (player != null)
            player.HealthChanged -= OnHealthChanged;
    }

    void Start()
    {
        Debug.Log($"[DEBUG] Initializing UI");
        if (player != null) OnHealthChanged(player.Health, player.MaxHealth);
        // Debug.Log($"[DEBUG] Player health: {player.Health} / {player.maxHealth}"); // Debug line
        if (enemiesLeftText != null) enemiesLeftText.text = "Enemies left: 0";

    }

    private void OnHealthChanged(int current, int max)
    {
        
        if (!healthText) return;
        Debug.Log($"[DEBUG] Health changed: {current} / {max}"); // Debug line

        healthText.text = $"Health: {current} / {max}";
    }

    public void SetEnemiesLeft(int count)
    {
        if (!enemiesLeftText) return;
        enemiesLeftText.text = $"Enemies Left: {count}";
    }

    // Update is called once per frame
    void Update()
    {
        SetEnemiesLeft(WaveManager.enemiesLeft);
    }
}
