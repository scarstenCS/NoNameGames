using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DialogueUI : MonoBehaviour
{
    [Header("Wire in Inspector")]
    public GameObject rootPanel;    // overall dialogue panel
    public TMP_Text speakerText;
    public TMP_Text bodyText;
    public Image portraitImage;     
    public GameObject continueHint; 

    public void Show(bool on) => rootPanel.SetActive(on);

    public void Render(string speaker, string body, Sprite portrait)
    {
        if (speakerText) speakerText.text = speaker ?? "";
        if (bodyText) bodyText.text = body ?? "";
        if (portraitImage) portraitImage.sprite = portrait;
    }
}
