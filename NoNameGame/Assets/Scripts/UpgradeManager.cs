using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    static public UpgradeManager Instance {get{ return _instance; }}
    static private UpgradeManager _instance;

    public GameObject upgradeWindow;
    private void Awake()
    {
        _instance = this;
    }

    //? Feel free to remove all the debug.logs if the API is working

    /// <summary>
    /// increased player's max health
    /// </summary>
    /// <param name="ammout">ammount to increase by</param>
    public void IncreaseMaxHealth(int ammout)
    {
        player.MaxHealth += ammout;
        Debug.Log($"Max health increased by {ammout}");
    }
    /// <summary>
    /// Increases basic weapon damage 
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreaseWeaponDamage(int ammount)
    {
        player.basicWeaponDmg += ammount;
        Debug.Log($"Basic weapon damage increased by {ammount}");
    }

    /// <summary>
    /// Increases basic weapon distance
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreaseWeaponDistance(float ammount)
    {
        player.basicWeaponDistance += ammount;
        Debug.Log($"Basic weapon distance increased by {ammount}");
    }
    /// <summary>
    /// increases weapon speed
    /// </summary>
    /// <param name="ammount">Ammount to increase by</param>
    public void IncreaseWeaponSpeed(float ammount)
    {
        player.basicWeaponSpeed += ammount;
        Debug.Log($"Basic weapon speed increased by {ammount}");
    }

    /// <summary>
    /// Increases Player speed
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreasePlayerSpeed(float ammount)
    {
        player.Speed += ammount;
        Debug.Log($"Player speed increased by {ammount}");
    }

    public void ShowUpgradeWindow()
    {
        upgradeWindow.SetActive(true);
    }

    public void HideUpgradeWindow()
    {
        upgradeWindow.SetActive(false);
    }

    static public bool isWindowClosed()
    {
        return !Instance.upgradeWindow.activeInHierarchy;
    }

    
}
