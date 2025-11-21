using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
 using UnityEngine.UI;
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
        public float basicAttackSizeIncrease;
    }
    [SerializeField]
    private UpgradeValues defaults = new UpgradeValues
    {
        healthIncrease = 5,
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 3f,
        weaponSpeedIncrease = 1.25f,
        playerSpeedIncrease = 1.25f,
        basicAttackSizeIncrease = 1.25f

    };
    [SerializeField] private TMP_Text[] buttonLabels;
    [SerializeField] private TMP_Text[] descLabels;
    [SerializeField] private Image[] buttonImages;
    [SerializeField] private BasicAttack launcher;
    private readonly List<UpgradeOption> options = new List<UpgradeOption>();
    private List<UpgradeOption> _offered = new List<UpgradeOption>();
    public int totalUpgrades
    {
        get { return options.Count; }
    }
    [SerializeField] private GameObject basicAttackPrefab; // assign in Inspector
    private void Awake()
    {
        _instance = this;
        BuildOptions();
    }


    public void ShowUpgradeWindow()
    {
        GameManager.ChangeTimeScale(0f);
        GameManager.isPaused = true;
        upgradeWindow.SetActive(true);
        _offered = OfferedUpgradeOptions();
        for (int i = 0; i < descLabels.Length; i++)
        {
            if (i < _offered.Count)
            {
                buttonLabels[i].text = $"{_offered[i].optionName}";
                if (_offered[i].icon != null){
                    buttonImages[i].sprite = _offered[i].icon;
                }
                else{
                    buttonImages[i].sprite = Resources.Load<Sprite>("Sprites/heart-plus");
                }
                descLabels[i].text = $"{_offered[i].description}";
            }
            else
            {
                buttonLabels[i].text = "N/A";
                buttonImages[i].sprite = null;
                descLabels[i].text = "N/A";
            }
        }

    }

    public void HideUpgradeWindow()
    {
        GameManager.isPaused = false;
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
            description = "+" + d.healthIncrease + " HP",
            icon = Resources.Load<Sprite>("Images/heart-plus"),
            applyUpgrade = () => IncreaseMaxHealth(d.healthIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Damage",
            description = "+" + d.weaponDamageIncrease + " ATK",
            icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyUpgrade = () => IncreaseWeaponDamage(d.weaponDamageIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Distance",
            description = "+" + d.weaponDistanceIncrease + " Range",
            icon = Resources.Load<Sprite>("Images/arrow-dunk"),
            applyUpgrade = () => IncreaseWeaponDistance(d.weaponDistanceIncrease)
        });

        options.Add(new UpgradeOption()
        {   
            optionName = "Increase Weapon Speed",
            description = "+" + (d.weaponSpeedIncrease-1)*100 + "% ATK SPD",
            icon = Resources.Load<Sprite>("Images/supersonic-bullet"),
            applyUpgrade = () => IncreaseWeaponSpeed(d.weaponSpeedIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Player Speed",
            description = "+" + (d.playerSpeedIncrease-1)*100 + "% SPD",
            icon = Resources.Load<Sprite>("Images/wingfoot"),
            applyUpgrade = () => IncreasePlayerSpeed(d.playerSpeedIncrease)
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Extra Basic Attack Pierce",
            description = "+1 Pierce",
            icon = Resources.Load<Sprite>("Images/pierced-body"),
            applyUpgrade = () => ExtraPierce()
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Basic Attack Size",
            description = "+" + (d.basicAttackSizeIncrease-1)*100 + "% ATK Size",
            icon = Resources.Load<Sprite>("Images/resize"),
            applyUpgrade = () => IncreasedBasicAttackSize(d.basicAttackSizeIncrease)
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Shots Per Attack",
            description = "+1 Shot Spread",
            icon = Resources.Load<Sprite>("Images/striking-arrows"),
            applyUpgrade = () => IncreaseShotsPerAttack()
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
        player.GetComponent<Player>().animator.SetBool("isAttacking", false);
        AudioManager.SfxSelect();
        _offered[0].applyUpgrade();
        HideUpgradeWindow();
    }
    public void button2Pressed()
    {
        player.GetComponent<Player>().animator.SetBool("isAttacking", false);
        AudioManager.SfxSelect();
        _offered[1].applyUpgrade();
        HideUpgradeWindow();
    }
    public void button3Pressed()
    {
        player.GetComponent<Player>().animator.SetBool("isAttacking", false);
        AudioManager.SfxSelect();
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
        player.basicWeaponSpeed = player.basicWeaponSpeed * ammount;
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
    public void ExtraPierce()
    {
        player.basicWeaponPierce += 1;
    }
    public void IncreasedBasicAttackSize(float sizeIncrease)
    {
        player.basicWeaponSize = sizeIncrease * player.basicWeaponSize;
    }
    public void IncreaseShotsPerAttack()
    {
        var launching = GetComponent<BasicAttack>();
        launcher.numberOfProjectiles += 1;
        launcher.spreadDegree = Mathf.Min(launcher.spreadDegree + 1.5f, 25f);
        launcher.randomJitter = Mathf.Min(launcher.randomJitter + 0.5f, 6f);
        UnityEngine.Debug.Log($"Increased shots per attack to {launcher.numberOfProjectiles}");
    }

}
