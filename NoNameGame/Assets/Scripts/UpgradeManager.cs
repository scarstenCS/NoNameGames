using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    static public UpgradeManager Instance {get{ return _instance; }}
    static private UpgradeManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    public void IncreaseMaxHealth(int ammout)
    {
        player.maxHealth += ammout;
        Debug.Log($"Max health increased by {ammout}");
    }
    public void IncreaseWeaponDamage(int ammount)
    {
        player.basicWeaponDmg += ammount;
        Debug.Log($"Basic weapon damage increased by {ammount}");
    }
    public void IncreaseWeaponDistance(float ammount)
    {
        player.basicWeaponDistance += ammount;
        Debug.Log($"Basic weapon distance increased by {ammount}");
    }
    public void IncreaseWeaponSpeed(float ammount)
    {
        player.basicWeaponSpeed += ammount;
        Debug.Log($"Basic weapon speed increased by {ammount}");
    }

    public void IncreasePlayerSpeed(float ammount)
    {
        player.Speed += ammount;
        Debug.Log($"Player speed increased by {ammount}");
    }
    
}
