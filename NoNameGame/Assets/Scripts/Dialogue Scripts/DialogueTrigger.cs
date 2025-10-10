using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSequence sequence;
    public List<DialogueSequence> sequencesByWave = new List<DialogueSequence>();
    public DialogueManager manager;
    public bool oneShot = false;
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

    public void OnWaveEnd(int waveNumber)
    {
        //Debug.Log($"Wave {waveNumber} ended");
        var seq = GetSequenceForWave(waveNumber);
        if (manager && seq) manager.StartSequence(seq);
        if (oneShot) gameObject.SetActive(false);

    }
    
    private DialogueSequence GetSequenceForWave(int waveNumber)
    {
        // Prefer a wave-specific sequence if valid and assigned
        if (waveNumber >= 0 &&
            sequencesByWave != null &&
            waveNumber < sequencesByWave.Count &&
            sequencesByWave[waveNumber] != null)
        {
            return sequencesByWave[waveNumber];
        }
        return sequence; // Fallback to default sequence
    }
}
