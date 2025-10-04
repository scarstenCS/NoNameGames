using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NewDialogue", menuName="Dialogue/Sequence")]
public class DialogueSequence : ScriptableObject
{
    [System.Serializable]
    public struct Line {
        public string speaker;
        [TextArea(2, 5)] public string text;
        public Sprite portrait; 
    }
    public Line[] lines;
    public bool pausesCombat = true; // Possibly pause combat during dialogue

}
