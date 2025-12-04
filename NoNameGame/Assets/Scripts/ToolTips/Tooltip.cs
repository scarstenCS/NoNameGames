using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//reference: https://www.youtube.com/watch?v=HXFoUGw7eKk

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI content;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private void Update()
    {
        int contentLength = content.text.Length;

        layoutElement.enabled = (contentLength > characterWrapLimit)? true:false;
    }
}
