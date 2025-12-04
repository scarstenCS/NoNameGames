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
    public RectTransform rectTransform;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string description)
    {
        content.text = description;
        int contentLength = content.text.Length;

        layoutElement.enabled = (contentLength > characterWrapLimit)? true:false;
    }
    private void Update()
    {
        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width;
        float pivotY = (float)((position.y / Screen.height));

        rectTransform.pivot = new Vector2(pivotX,-0.05f);
        transform.position = position;


    }

}
