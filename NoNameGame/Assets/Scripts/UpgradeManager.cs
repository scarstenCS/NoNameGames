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
        public System.Action applyChange;
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
        public int playerShieldAmountIncrease;
        public int playerNumOfProjectilesIncrease;
        public float playerShieldRechargeAmountIncrease;
    }
        public struct DowngradeValues
    {
        public int healthDecrease;
        public int weaponDamageDecrease;
        public float weaponDistanceDecrease;
        public float weaponSpeedDecrease;
        public float playerSpeedDecrease;
        public float basicAttackSizeDecrease;
        public int playerNumOfProjectilesDecrease;
        public int playerShieldAmountDecrease;
        public float playerShieldRechargeAmountDecrease;
    }
    [SerializeField]
    private UpgradeValues defaultsU = new UpgradeValues
    {
        healthIncrease = 5,
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 3f,
        weaponSpeedIncrease = 1.25f,
        playerSpeedIncrease = 1.25f,
        basicAttackSizeIncrease = 1.25f,
        playerShieldAmountIncrease = 1,
        playerShieldRechargeAmountIncrease = 1,
        playerNumOfProjectilesIncrease = 1

    };
    [SerializeField]
    private DowngradeValues defaultsD = new DowngradeValues
    {
        healthDecrease = 5,
        weaponDamageDecrease = 1,
        weaponDistanceDecrease = 3f,
        weaponSpeedDecrease = 1.25f,
        playerSpeedDecrease = 1.25f,
        basicAttackSizeDecrease = 1.25f,
        playerShieldAmountDecrease = 1,
        playerShieldRechargeAmountDecrease = 1,
        playerNumOfProjectilesDecrease =1

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
        //upgrade defaults = dU
        var dU = defaultsU; 
        //downgrade Defaults = dD
        var dD = defaultsD;
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Max Health",
            description = "+" + dU.healthIncrease + " HP",
            icon = Resources.Load<Sprite>("Images/heart-plus"),
            applyChange = () => ChangeMaxHealth(dU.healthIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Damage",
            description = "+" + dU.weaponDamageIncrease + " ATK",
            icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => ChangeWeaponDamage(dU.weaponDamageIncrease)
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Shield Amount",
            description = "+" + dU.playerShieldAmountIncrease + " shield",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => ChangePlayerShieldAmount(dU.playerShieldAmountIncrease)
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Reduce Shield recharge time",
            description = "+" + (dU.playerShieldRechargeAmountIncrease-1)*100 + "% reduced time",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => ChangePlayerShieldRecharge(dU.playerShieldRechargeAmountIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Weapon Distance",
            description = "+" + dU.weaponDistanceIncrease + " Range",
            icon = Resources.Load<Sprite>("Images/arrow-dunk"),
            applyChange = () => ChangeWeaponDistance(dU.weaponDistanceIncrease)
        });

        options.Add(new UpgradeOption()
        {   
            optionName = "Increase Weapon Speed",
            description = "+" + (dU.weaponSpeedIncrease-1)*100 + "% ATK SPD",
            icon = Resources.Load<Sprite>("Images/supersonic-bullet"),
            applyChange = () => ChangeWeaponSpeed(dU.weaponSpeedIncrease)
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Player Speed",
            description = "+" + (dU.playerSpeedIncrease-1)*100 + "% SPD",
            icon = Resources.Load<Sprite>("Images/wingfoot"),
            applyChange = () => ChangePlayerSpeed(dU.playerSpeedIncrease)
        });
        options.Add(new UpgradeOption()
        {
            //gain peirce but lose armor
            optionName = "Extra Basic Attack Pierce",
            description = "+1 Pierce",
            icon = Resources.Load<Sprite>("Images/pierced-body"),
            applyChange = () => ChangePierce()
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Basic Attack Size",
            description = "+" + (dU.basicAttackSizeIncrease-1)*100 + "% ATK Size but lower attack speed by " + (dD.basicAttackSizeDecrease-1)*100 + "%",
            icon = Resources.Load<Sprite>("Images/resize"),
            applyChange = () => {
                ChangeBasicAttackSize(dU.basicAttackSizeIncrease);
                ChangeWeaponSpeed(-dD.weaponSpeedDecrease);
            }

        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Shots Per Attack",
            description = "+"+ (dU.playerNumOfProjectilesIncrease) + "  Shot projectiles",
            icon = Resources.Load<Sprite>("Images/striking-arrows"),
            applyChange = () => ChangeShotsPerAttack(dU.playerNumOfProjectilesIncrease)
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
        _offered[0].applyChange();
        HideUpgradeWindow();
    }
    public void button2Pressed()
    {
        player.GetComponent<Player>().animator.SetBool("isAttacking", false);
        AudioManager.SfxSelect();
        _offered[1].applyChange();
        HideUpgradeWindow();
    }
    public void button3Pressed()
    {
        player.GetComponent<Player>().animator.SetBool("isAttacking", false);
        AudioManager.SfxSelect();
        _offered[2].applyChange();
        HideUpgradeWindow();
    }
    //? Feel free to remove all the debug.logs if the API is working

    /// <summary>
    /// increased player's max health
    /// </summary>
    /// <param name="ammout">ammount to increase by</param>
    public void ChangeMaxHealth(int ammout)
    {
        player.MaxHealth += ammout;
        //Debug.Log($"Max health increased by {ammout}");
    }
    /// <summary>
    /// Increases basic weapon damage 
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void ChangeWeaponDamage(int ammount)
    {
        player.basicWeaponDmg += ammount;
        //Debug.Log($"Basic weapon damage increased by {ammount}");
    }

    /// <summary>
    /// Increases basic weapon distance
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void ChangeWeaponDistance(float ammount)
    {
        player.basicWeaponDistance += ammount;
        //Debug.Log($"Basic weapon distance increased by {ammount}");
    }
    /// <summary>
    /// increases weapon speed
    /// </summary>
    /// <param name="ammount">Ammount to increase by</param>
    public void ChangeWeaponSpeed(float ammount)
    {
        player.basicWeaponSpeed = player.basicWeaponSpeed * ammount;
        //Debug.Log($"Basic weapon speed increased by {ammount}");
    }

    /// <summary>
    /// Increases Player speed
    /// </summary>
    /// <param name="ammount">ammount to increase by</param>
    public void ChangePlayerSpeed(float ammount)
    {
        player.Speed += ammount;
        //Debug.Log($"Player speed increased by {ammount}");
    }
    public void ChangePlayerShieldRecharge(float newRecharge)
    {
        if (newRecharge >0)
        {
                    player.sheildRegenerateTime = (player.sheildRegenerateTime *newRecharge) - player.sheildRegenerateTime;

        }
    }
    public void ChangePlayerShieldAmount(int shieldAmount)
    {
        player.totalShieldCount = player.totalShieldCount + shieldAmount;
    }
    public void ChangePierce()
    {
        player.basicWeaponPierce += 1;
    }
    public void ChangeBasicAttackSize(float sizeIncrease)
    {
        player.basicWeaponSize = sizeIncrease * player.basicWeaponSize;
    }
    public void ChangeShotsPerAttack(int projetileChange)
    {
        var launching = GetComponent<BasicAttack>();
        if(launcher.numberOfProjectiles == 1 && projetileChange < 0) return;
        launcher.numberOfProjectiles += projetileChange;
        launcher.spreadDegree = Mathf.Min(launcher.spreadDegree + 1.5f, 25f);
        launcher.randomJitter = Mathf.Min(launcher.randomJitter + 0.5f, 6f);
        UnityEngine.Debug.Log($"Increased shots per attack to {launcher.numberOfProjectiles}");
    }

}
