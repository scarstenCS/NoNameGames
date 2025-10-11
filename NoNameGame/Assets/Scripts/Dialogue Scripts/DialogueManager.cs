using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    [Header("Wire in Inspector")]
    public DialogueUI ui;
    public Behaviour[] combatInputControllers;
    public KeyCode advanceKey = KeyCode.Mouse0; // Left mouse button

    public PlayerInput playerInput; 

    int index = -1;
    DialogueSequence seq;
    bool active;

    public bool IsActive => active; 

    public void StartSequence(DialogueSequence s)
    {
        if (active || s == null) return;
        seq = s;
        index = -1; // Start before first line
        if (ui) ui.Show(true); // Show dialogue UI
        SetCombatEnabled(!s.pausesCombat); // Disable combat if needed
        active = true; // Mark dialogue as active
        Next();
    }

    void Update()
    {
        if (!active) return;
        if (WasAdvancePressed()) Next();
    }

    void Next()
    {
        index++;  // Move to next line
        if (seq == null || seq.lines == null || index >= seq.lines.Length)  // Check if sequence is valid
        {
            EndSequence();
            return;
        }
        var line = seq.lines[index]; // Get current line
        ui.Render(line.speaker, line.text, line.portrait);  // Render line in UI
    }

    void EndSequence()
    {
        active = false;
        if (ui) ui.Show(false); // Hide dialogue UI
        SetCombatEnabled(true);
        Time.timeScale = 1f;
        seq = null;
    }

    void SetCombatEnabled(bool enabled)
    {
        foreach (var c in combatInputControllers) if (c) c.enabled = enabled;
        if (playerInput)
        {
            if (enabled) playerInput.ActivateInput();
            else playerInput.DeactivateInput();
        }
    }
    bool WasAdvancePressed()
    {
        // left click was pressed this frame
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        return false;
    }

    public bool isDialogueFinished()
    {
        return !active;
    }
}
