using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{

    public Button[] upgradeBtns;


    static private UpgradeScreen _instance;
    static public UpgradeScreen Instance { get { return _instance; } }
    void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        upgradeBtns = GetComponentsInChildren<Button>();
    }

    void ShowUpgrades()
    {
        gameObject.SetActive(true);
    }

    void PressUpgrade(Button pressed)
    {
        
    }
    // Update is called once per frame


}
