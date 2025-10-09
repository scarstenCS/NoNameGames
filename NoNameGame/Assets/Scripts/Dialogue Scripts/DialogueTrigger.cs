using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSequence sequence;
    public DialogueManager manager;
    public bool oneShot = true;
    static private DialogueTrigger _instance;
    static public DialogueTrigger Instance;


    void Awake()
    {
        _instance = this;
        Instance = this;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log($"Hit {other.name} / tag={other.tag} / layer={LayerMask.LayerToName(other.gameObject.layer)}");
        if (!other.CompareTag("Player")) return; // Only trigger for player
        if (manager && sequence) manager.StartSequence(sequence);
        if (oneShot) gameObject.SetActive(false); // Disable after triggering once
    }

    public void OnWaveEnd()
    {
        //Debug.Log($"Hit {other.name} / tag={other.tag} / layer={LayerMask.LayerToName(other.gameObject.layer)}");
        if (manager && sequence) manager.StartSequence(sequence);
        if (oneShot) gameObject.SetActive(false); // Disable after triggering once

    }
}
