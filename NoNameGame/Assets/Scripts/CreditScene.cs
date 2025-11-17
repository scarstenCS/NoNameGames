using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CreditScene : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;  
    void Start()
    {
        scoreText.text = "Score: " + SceneDataTransfer.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
