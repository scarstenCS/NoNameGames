using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


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

    // the number of characters that fit on a line in a 16:9 ratio
    const int charsPerLine = 42;
    public void Show(bool on) => rootPanel.SetActive(on);

    public void Render(string speaker, string body, Sprite portrait, AudioClip dialogueSFX)
    {
        StopAllCoroutines();
        if (speakerText) speakerText.text = speaker ?? "";
        StartCoroutine(BodyTextByCharacter(body, defaultCharPrintSpeed,dialogueSFX));
        //if (bodyText) bodyText.text = body ?? "";
        if (portraitImage) portraitImage.sprite = portrait;
    }

    private IEnumerator BodyTextByCharacter(string text, float charPrintSpeed, AudioClip printSFX)
    {
        List<string> words = text.Split(' ').ToList();
        isAnimating = true;
        bodyText.text = "";

        int lineLen =0;


        for (int i = 0; i < words.Count(); i++)
        {
            lineLen += words[i].Length +1;

            // add newline if word won't fit on line
            if (lineLen > charsPerLine)
            {
                lineLen=0;
                bodyText.text += "\n";
            }


            // add word
            for(int c =0; c < words[i].Length;c++){

                bodyText.text += words[i][c];

                if (isAnimating){
                    AudioManager.Instance.PlayClip(printSFX);
                     yield return new WaitForSecondsRealtime(charPrintSpeed);
                }
            }

            bodyText.text += " ";
        }
        //bodyText.text = text;

        isAnimating =false;
        yield return null;
    }
}
