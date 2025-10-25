using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [Header("Wire these in the Inspector")]
    public Player player;
    public TMP_Text healthText;              // drag your "Health: ..." TMP Text
    public TMP_Text enemiesLeftText;         // drag your "Enemies left: ..." TMP Text
    public GameObject damagedTakenImage;            
    private RectTransform damagedTakenRectTransform; // cache RectTransform

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
        if (damagedTakenImage)
        {
            damagedTakenRectTransform = damagedTakenImage.GetComponent<RectTransform>();
        }
        if (player != null) OnHealthChanged(player.Health, player.MaxHealth);
        // Debug.Log($"[DEBUG] Player health: {player.Health} / {player.maxHealth}"); // Debug line
        if (enemiesLeftText != null) enemiesLeftText.text = "Enemies left: 0";

    }

    private void OnHealthChanged(int current, int max)
    {

        if (!healthText) return;
        //currently the damage taken makes the rect grow, but not only to the right, i want to
        // make it grow only to the left side
        //now the height is set to 0 but should always be set to 8 pixels high
        //Still not going to the left side correctly
        if (damagedTakenRectTransform)
        {
            float damagePercent = 1f - ((float)current / (float)max);
            float maxWidth = 64f; // adjust based on your image size
            damagedTakenRectTransform.sizeDelta = new Vector2(damagePercent * maxWidth, 8f);
        }

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
