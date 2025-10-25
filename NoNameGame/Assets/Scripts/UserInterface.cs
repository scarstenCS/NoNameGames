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
    public Animator animator;
    private RectTransform damagedTakenRectTransform; // cache RectTransform
    private float _lastSpeed = 1f;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        UnityEngine.Debug.Log($"[DEBUG] Enabling UI");
        if (player != null)
            player.HealthChanged += OnHealthChanged;

        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            UnityEngine.Debug.Log($"[DEBUG] last speed: {_lastSpeed}");
            animator.SetFloat("SpeedMultiplier", _lastSpeed);
        }    
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
        //now I would like to add functionality to the animator to increase as damge is taken
        //if (animator == null) animator = damagedTakenImage.GetComponent<Animator>();
        if (damagedTakenRectTransform)
        {
            float damagePercent = 1f - ((float)current / (float)max);
            float maxWidth = 64f; // adjust based on your image size
            damagedTakenRectTransform.sizeDelta = new Vector2(damagePercent * maxWidth, 8f);
            UnityEngine.Debug.Log($"[DEBUG] Damage Percent: {damagePercent}");
            if (damagePercent <= 0.001f || damagePercent >= 0.999f)
            {
                //UnityEngine.Debug.Log($"[DEBUG] Setting animator speed to 1f");
                animator.SetFloat("SpeedMultiplier", 0.5f);
            }
            else
            {
                //UnityEngine.Debug.Log($"[DEBUG] Setting animator speed to {damagePercent * 10f}");
                animator.SetFloat("SpeedMultiplier", damagePercent * 10f);
            }
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
