using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    static public UpgradeManager Instance {get{ return _instance; }}
    static private UpgradeManager _instance;
    public GameObject upgradeWindow;
    public struct UpgradeOption
    {
        public string optionName;
        public string description;
        public Sprite icon;
        public System.Action applyUpgrade;
    }
    [System.Serializable]
    public struct UpgradeValues
    {
        public int healthIncrease;
        public int weaponDamageIncrease;
        public float weaponDistanceIncrease;
        public float weaponSpeedIncrease;
        public float playerSpeedIncrease;
    }
    [SerializeField]
    private UpgradeValues defaults = new UpgradeValues
    {
        healthIncrease = 5,
        weaponDamageIncrease = 2,
        weaponDistanceIncrease = 3f,
        weaponSpeedIncrease = 1f,
        playerSpeedIncrease = 0.5f
    };
    [SerializeField] private TMP_Text[] buttonLabels;
    [SerializeField] private TMP_Text[] descLabels;
    private readonly List<UpgradeOption> options = new List<UpgradeOption>();
    private List<UpgradeOption> _offered = new List<UpgradeOption>();
    private void Awake()
    {
        _instance = this;
        BuildOptions();
    }


    public void ShowUpgradeWindow()
    {
        GameManager.ChangeTimeScale(0f);
        upgradeWindow.SetActive(true);
        _offered = OfferedUpgradeOptions();
        for (int i = 0; i < buttonLabels.Length; i++)
        {
            if (i < _offered.Count)
            {
                buttonLabels[i].text = $"{_offered[i].optionName}";
                descLabels[i].text = $"{_offered[i].description}";
            }
            else
            {
                buttonLabels[i].text = "N/A";
                descLabels[i].text = "N/A";
            }
        }

    }

    public void HideUpgradeWindow()
    {
        GameManager.ChangeTimeScale(1f);
        upgradeWindow.SetActive(false);
    }

    static public bool isWindowClosed()
    {
        return !Instance.upgradeWindow.activeInHierarchy;
    }

    private void BuildOptions()
    {
        var d = defaults; 
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Max Health",
            description = "Increases your maximum health by 5 points.",
            icon = null,
            applyUpgrade = () => IncreaseMaxHealth(d.healthIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Damage",
            description = "Increases your basic weapon damage by 2 points.",
            icon = null,
            applyUpgrade = () => IncreaseWeaponDamage(d.weaponDamageIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Distance",
            description = "Increases your basic weapon distance by 3 units.",
            icon = null,
            applyUpgrade = () => IncreaseWeaponDistance(d.weaponDistanceIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Speed",
            description = "Increases your basic weapon speed by 1 unit.",
            icon = null,
            applyUpgrade = () => IncreaseWeaponSpeed(d.weaponSpeedIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Player Speed",
            description = "Increases your player speed by 0.5 units.",
            icon = null,
            applyUpgrade = () => IncreasePlayerSpeed(d.playerSpeedIncrease)
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Extra Basic Attack Pierce",
            description = "Increases your basic attack pierce by 1.",
            icon = null,
            applyUpgrade = () => ExtraBasicAttack()
        });
    }
    public List<UpgradeOption> OfferedUpgradeOptions()
    {
        _offered.Clear();
        List<int> usedIndices = new List<int>();
        while (_offered.Count < 3 && _offered.Count < options.Count)
        {
            int index = Random.Range(0, options.Count);
            if (!usedIndices.Contains(index))
            {
                usedIndices.Add(index);
                _offered.Add(options[index]);
            }
        }
        //TODO: Do I need to delete used indices?
        return _offered;
    }

    public void button1Pressed()
    {
        _offered[0].applyUpgrade();
        HideUpgradeWindow();
    }
    public void button2Pressed()
    {
        _offered[1].applyUpgrade();
        HideUpgradeWindow();
    }
    public void button3Pressed()
    {
        _offered[2].applyUpgrade();
        HideUpgradeWindow();
    }
    //? Feel free to remove all the debug.logs if the API is working

    /// <summary>
    /// increased player's max health
    /// </summary>
    /// <param name="ammout">ammount to increase by</param>
    public void IncreaseMaxHealth(int ammout)
    {
        player.MaxHealth += ammout;
        //Debug.Log($"Max health increased by {ammout}");
    }
    /// <summary>
    /// Increases basic weapon damage 
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreaseWeaponDamage(int ammount)
    {
        player.basicWeaponDmg += ammount;
        //Debug.Log($"Basic weapon damage increased by {ammount}");
    }

    /// <summary>
    /// Increases basic weapon distance
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreaseWeaponDistance(float ammount)
    {
        player.basicWeaponDistance += ammount;
        //Debug.Log($"Basic weapon distance increased by {ammount}");
    }
    /// <summary>
    /// increases weapon speed
    /// </summary>
    /// <param name="ammount">Ammount to increase by</param>
    public void IncreaseWeaponSpeed(float ammount)
    {
        player.basicWeaponSpeed += ammount;
        //Debug.Log($"Basic weapon speed increased by {ammount}");
    }

    /// <summary>
    /// Increases Player speed
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void IncreasePlayerSpeed(float ammount)
    {
        player.Speed += ammount;
        //Debug.Log($"Player speed increased by {ammount}");
    }
    public void ExtraBasicAttack()
    {
        player.basicWeaponPierce += 1;
    }
    public void IncreasedBasicAttackSize()
    {
        Vector3 scaleChange = new Vector3(0.5f, 0.5f, 0.5f);
        player.basicWeaponSize += scaleChange;
    }
}
