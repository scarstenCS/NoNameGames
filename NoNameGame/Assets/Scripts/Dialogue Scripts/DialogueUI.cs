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

    public bool isAnimating = false;

    const float defaultCharPrintSpeed = 0.025f; 

    public void Show(bool on) => rootPanel.SetActive(on);

    public void Render(string speaker, string body, Sprite portrait)
    {
        StopAllCoroutines();
        if (speakerText) speakerText.text = speaker ?? "";
        StartCoroutine(BodyTextByCharacter(body, defaultCharPrintSpeed));
        //if (bodyText) bodyText.text = body ?? "";
        if (portraitImage) portraitImage.sprite = portrait;
    }

    private IEnumerator BodyTextByCharacter(string text, float charPrintSpeed)
    {
        isAnimating = true;
        bodyText.text = "";


        for (int i = 0; i < text.Length && isAnimating; i++)
        {
            // if (!isAnimating)
            // {
            //     bodyText.text = text;
            //     break;
            // }
            bodyText.text += text[i];
           //Debug.Log(i);
            yield return new WaitForSecondsRealtime(charPrintSpeed);
        }
        bodyText.text = text;

        isAnimating =false;
        yield return null;
    }
}
