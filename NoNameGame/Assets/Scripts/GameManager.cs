using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;

    static public GameObject player;
    static public GameManager Instance;
    void Awake()
    {
        _instance = this;
    }

    static public void PlayerTakeDamage(int ammout)
    {
        Player p = player.GetComponent<Player>();
        p.TakeDamage(ammout);
    }
    
}
