using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    static public UpgradeManager Instance;
    static private UpgradeManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    public void IncreaseMaxHealth(int ammout)
    {
        player.maxHealth += ammout;
    }
    
}
