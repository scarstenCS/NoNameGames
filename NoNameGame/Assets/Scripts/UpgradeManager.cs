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
    [System.Serializable]
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
    private UpgradeValues defaultsU = new UpgradeValues
    {
        healthIncrease = 5,
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 0.5f,
        weaponSpeedIncrease = 0.25f,
        playerSpeedIncrease = 0.25f,
        basicAttackSizeIncrease = 0.25f,
        playerShieldAmountIncrease = 1,
        playerShieldRechargeAmountIncrease = 0.33f,
        playerNumOfProjectilesIncrease = 1

    };
    private DowngradeValues defaultsD = new DowngradeValues
    {
        healthDecrease = 5,
        weaponDamageDecrease = 1,
        weaponDistanceDecrease = 0.5f,
        weaponSpeedDecrease = 0.25f,
        playerSpeedDecrease = 0.25f,
        basicAttackSizeDecrease = 0.25f,
        playerShieldAmountDecrease = 1,
        playerShieldRechargeAmountDecrease = 0.33f,
        playerNumOfProjectilesDecrease = 1

    };
    [System.Serializable]
    public struct ThemedUpgrade
    {
        public UpgradeValues upgrade;     // positive changes
        public DowngradeValues downgrade; // negative changes
    }
    // APATHY
    [SerializeField]
    private ThemedUpgrade apathyNumbToPain = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            healthIncrease = 5
        },
        downgrade = new DowngradeValues {
            weaponDamageDecrease = 1,
            weaponSpeedDecrease = 0.10f
        }
    };

    [SerializeField]
    private ThemedUpgrade apathyCheckedOut = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            playerShieldRechargeAmountIncrease = 0.20f
        },
        downgrade = new DowngradeValues {
            playerSpeedDecrease = 0.10f,
            basicAttackSizeDecrease = 0.10f
        }
    };


    // LONELINESS
    [SerializeField]
    private ThemedUpgrade lonelinessDistantHeart = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDistanceIncrease = 0.75f
        },
        downgrade = new DowngradeValues {
            basicAttackSizeDecrease = 0.15f,
            playerShieldAmountDecrease = 1
        }
    };

    [SerializeField]
    private ThemedUpgrade lonelinessSolitaryShot = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 2,
            basicAttackSizeIncrease = 0.15f
        },
        downgrade = new DowngradeValues {
            playerNumOfProjectilesDecrease = 1
        }
    };


    // SLOTH
    [SerializeField]
    private ThemedUpgrade slothDeadWeight = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            healthIncrease = 7,
            playerShieldAmountIncrease = 1
        },
        downgrade = new DowngradeValues {
            playerSpeedDecrease = 0.25f
        }
    };

    [SerializeField]
    private ThemedUpgrade slothRootedFocus = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 1,
            weaponDistanceIncrease = 0.25f
        },
        downgrade = new DowngradeValues {
            weaponSpeedDecrease = 0.15f,
            playerSpeedDecrease = 0.15f
        }
    };


    // JEALOUSY
    [SerializeField]
    private ThemedUpgrade jealousyEnviousStrike = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 2
        },
        downgrade = new DowngradeValues {
            healthDecrease = 5
        }
    };

    [SerializeField]
    private ThemedUpgrade jealousyCovetousHarvest = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 1,
            playerNumOfProjectilesIncrease = 1
        },
        downgrade = new DowngradeValues {
            healthDecrease = 3,
            playerShieldAmountDecrease = 1
        }
    };


    // GREED
    [SerializeField]
    private ThemedUpgrade greedHoardersBargain = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 1,
            weaponDistanceIncrease = 0.25f,
            playerNumOfProjectilesIncrease = 1
        },
        downgrade = new DowngradeValues {
            healthDecrease = 5,
            playerShieldAmountDecrease = 1
        }
    };

    [SerializeField]
    private ThemedUpgrade greedAllIn = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 2,
            weaponSpeedIncrease = 0.30f
        },
        downgrade = new DowngradeValues {
            healthDecrease = 10,
            playerShieldAmountDecrease = 1
        }
    };


    // DEPRESSION
    [SerializeField]
    private ThemedUpgrade depressionWeightedShots = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 1,
            basicAttackSizeIncrease = 0.30f
        },
        downgrade = new DowngradeValues {
            weaponSpeedDecrease = 0.25f,
            playerSpeedDecrease = 0.10f
        }
    };

    [SerializeField]
    private ThemedUpgrade depressionLastLight = new ThemedUpgrade {
        upgrade = new UpgradeValues {
            weaponDamageIncrease = 1,
            weaponSpeedIncrease = 0.20f
        },
        downgrade = new DowngradeValues {
            playerShieldAmountDecrease = 1,
            playerShieldRechargeAmountDecrease = 0.50f
        }
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
            description = "+" + dU.weaponDamageIncrease + " ATK but lose "+ dD.healthDecrease+" health",
            icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => 
            {
                ChangeWeaponDamage(dU.weaponDamageIncrease);
                ChangeMaxHealth(-dD.healthDecrease);
            }
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Shield Amount",
            description = "+" + dU.playerShieldAmountIncrease + " shield but lose" + dD.playerSpeedDecrease*100+"% speed.",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangePlayerShieldAmount(dU.playerShieldAmountIncrease);
                ChangePlayerSpeed(dD.playerSpeedDecrease);

            }
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Reduce Shield recharge time",
            description = "+" + dU.playerShieldRechargeAmountIncrease*100 + "% reduced time",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => ChangePlayerShieldRecharge(dU.playerShieldRechargeAmountIncrease)
        });

        options.Add(new UpgradeOption()
        {
            //want to add a way for the projectile to increase its size on the way back. 
            optionName = "Increase Weapon Distance and weapon speed",
            description = "+" + dU.weaponDistanceIncrease + " Range and + "+ dU.weaponSpeedIncrease*100+"% WeaponSpeed",
            icon = Resources.Load<Sprite>("Images/arrow-dunk"),
            applyChange = () => {
                ChangeWeaponDistance(dU.weaponDistanceIncrease);
                ChangeWeaponSpeed(dU.weaponSpeedIncrease);
            }
        });

        options.Add(new UpgradeOption()
        {   
            optionName = "Increase Weapon Speed",
            description = "+" + dU.weaponSpeedIncrease*100 + "% ATK SPD but reduce weapon size by "+dD.basicAttackSizeDecrease +"%",
            icon = Resources.Load<Sprite>("Images/supersonic-bullet"),
            applyChange = () =>{
                ChangeWeaponSpeed(dU.weaponSpeedIncrease);
                ChangeBasicAttackSize(-dD.basicAttackSizeDecrease);
            }
        });

        options.Add(new UpgradeOption()
        {
            optionName = "Increase Player Speed",
            description = "+" + dU.playerSpeedIncrease*100 + "% SPD",
            icon = Resources.Load<Sprite>("Images/wingfoot"),
            applyChange = () => 
            {
                ChangePlayerSpeed(dU.playerSpeedIncrease);

            }
        });
        options.Add(new UpgradeOption()
        {
            //gain peirce but lose armor
            optionName = "Extra Basic Attack Pierce",
            description = "+1 Pierce but lose 1 shield",
            icon = Resources.Load<Sprite>("Images/pierced-body"),
            applyChange = () => 
            {
                ChangePierce(true);
                ChangePlayerShieldAmount(dD.playerShieldAmountDecrease);
            }
        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Basic Attack Size",
            description = "+" + dU.basicAttackSizeIncrease*100 + "% ATK Size but lower attack speed by " + (dD.basicAttackSizeDecrease-1)*100 + "%",
            icon = Resources.Load<Sprite>("Images/resize"),
            applyChange = () => {
                ChangeBasicAttackSize(dU.basicAttackSizeIncrease);
                ChangeWeaponSpeed(-dD.weaponSpeedDecrease);
            }

        });
        options.Add(new UpgradeOption()
        {
            optionName = "Increase Shots Per Attack",
            description = "+"+ dU.playerNumOfProjectilesIncrease + "  Shot projectiles but lose " + dD.weaponDamageDecrease+ " Damage and "+ dD.weaponDistanceDecrease +" Range",
            icon = Resources.Load<Sprite>("Images/striking-arrows"),
            applyChange = () => {
                ChangeShotsPerAttack(dU.playerNumOfProjectilesIncrease);
                ChangeWeaponDamage(-dD.weaponDamageDecrease);
                ChangeWeaponDistance(-dD.weaponDistanceDecrease);
            }
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
    /// <param name="amout">amount to increase by</param>
    public void ChangeMaxHealth(int amount)
    {
        if((player.MaxHealth += amount) <= 0)
        {
            player.MaxHealth = 1;
        }
        else
        {
            player.MaxHealth += amount;
        }
        
        //Debug.Log($"Max health increased by {ammout}");
    }
    /// <summary>
    /// Increases basic weapon damage 
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangeWeaponDamage(int amount)
    {
        if ((player.basicWeaponDmg += amount) <= 0)
        {
            player.basicWeaponDmg = 1;
        }
        else
        {
        player.basicWeaponDmg += amount;
        }
        
        //Debug.Log($"Basic weapon damage increased by {amount}");
    }

    /// <summary>
    /// Increases basic weapon distance
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangeWeaponDistance(float amount)
    {
        if((player.basicWeaponDistance += amount) <= 0)
        {
            player.basicWeaponDistance = 0.2f;
        }
        else
        {
            player.basicWeaponDistance += amount;
        }
        
        //Debug.Log($"Basic weapon distance increased by {amount}");
    }
    /// <summary>
    /// increases weapon speed
    /// </summary>
    /// <param name="amount">Amount to increase by</param>
    public void ChangeWeaponSpeed(float amount)
    {
        player.basicWeaponSpeed = player.basicWeaponSpeed + (player.basicWeaponSpeed * amount);
        //Debug.Log($"Basic weapon speed increased by {amount}");
    }

    /// <summary>
    /// Increases Player speed
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangePlayerSpeed(float amount)
    {
        player.Speed += player.Speed + (player.Speed *amount);
        //Debug.Log($"Player speed increased by {amount}");
    }
    public void ChangePlayerShieldRecharge(float newRecharge)
    {

        player.sheildRegenerateTime = player.sheildRegenerateTime - (player.sheildRegenerateTime *newRecharge);
    }
    public void ChangePlayerShieldAmount(int shieldAmount)
    {
        if((player.totalShieldCount = player.totalShieldCount + shieldAmount) < 0)
        {
            player.totalShieldCount = 0;
        }
        else
        {
            player.totalShieldCount = player.totalShieldCount + shieldAmount;
        }
        
    }
    public void ChangePierce(bool positive)
    {
        if(positive != true)
        {
            if ((player.basicWeaponPierce -= 1) < 0)
            {
                player.basicWeaponPierce = 0;
            }
        }
        else
        {
            player.basicWeaponPierce += 1;
        }
        
    }
    public void ChangeBasicAttackSize(float sizeIncrease)
    {

        player.basicWeaponSize = player.basicWeaponSize + (sizeIncrease * player.basicWeaponSize);

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
    // private void BuildOptions()
    // {
    //     //upgrade defaults = dU
    //     var dU = defaultsU; 
    //     //downgrade Defaults = dD
    //     var dD = defaultsD;
    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Max Health",
    //         description = "+" + dU.healthIncrease + " HP",
    //         icon = Resources.Load<Sprite>("Images/heart-plus"),
    //         applyChange = () => ChangeMaxHealth(dU.healthIncrease)
    //     });

    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Weapon Damage",
    //         description = "+" + dU.weaponDamageIncrease + " ATK but lose "+ dD.healthDecrease+" health",
    //         icon = Resources.Load<Sprite>("Images/charged-arrow"),
    //         applyChange = () => 
    //         {
    //             ChangeWeaponDamage(dU.weaponDamageIncrease);
    //             ChangeMaxHealth(-dD.healthDecrease);
    //         }
    //     });
    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Shield Amount",
    //         description = "+" + dU.playerShieldAmountIncrease + " shield but lose" + dD.playerSpeedDecrease*100+"% speed.",
    //         //icon = Resources.Load<Sprite>("Images/charged-arrow"),
    //         applyChange = () => {
    //             ChangePlayerShieldAmount(dU.playerShieldAmountIncrease);
    //             ChangePlayerSpeed(dD.playerSpeedDecrease);

    //         }
    //     });
    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Reduce Shield recharge time",
    //         description = "+" + dU.playerShieldRechargeAmountIncrease*100 + "% reduced time",
    //         //icon = Resources.Load<Sprite>("Images/charged-arrow"),
    //         applyChange = () => ChangePlayerShieldRecharge(dU.playerShieldRechargeAmountIncrease)
    //     });

    //     options.Add(new UpgradeOption()
    //     {
    //         //want to add a way for the projectile to increase its size on the way back. 
    //         optionName = "Increase Weapon Distance and weapon speed",
    //         description = "+" + dU.weaponDistanceIncrease + " Range and + "+ dU.weaponSpeedIncrease*100+"% WeaponSpeed",
    //         icon = Resources.Load<Sprite>("Images/arrow-dunk"),
    //         applyChange = () => {
    //             ChangeWeaponDistance(dU.weaponDistanceIncrease);
    //             ChangeWeaponSpeed(dU.weaponSpeedIncrease);
    //         }
    //     });

    //     options.Add(new UpgradeOption()
    //     {   
    //         optionName = "Increase Weapon Speed",
    //         description = "+" + dU.weaponSpeedIncrease*100 + "% ATK SPD but reduce weapon size by "+dD.basicAttackSizeDecrease +"%",
    //         icon = Resources.Load<Sprite>("Images/supersonic-bullet"),
    //         applyChange = () =>{
    //             ChangeWeaponSpeed(dU.weaponSpeedIncrease);
    //             ChangeBasicAttackSize(-dD.basicAttackSizeDecrease);
    //         }
    //     });

    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Player Speed",
    //         description = "+" + dU.playerSpeedIncrease*100 + "% SPD",
    //         icon = Resources.Load<Sprite>("Images/wingfoot"),
    //         applyChange = () => 
    //         {
    //             ChangePlayerSpeed(dU.playerSpeedIncrease);

    //         }
    //     });
    //     options.Add(new UpgradeOption()
    //     {
    //         //gain peirce but lose armor
    //         optionName = "Extra Basic Attack Pierce",
    //         description = "+1 Pierce but lose 1 shield",
    //         icon = Resources.Load<Sprite>("Images/pierced-body"),
    //         applyChange = () => 
    //         {
    //             ChangePierce(true);
    //             ChangePlayerShieldAmount(dD.playerShieldAmountDecrease);
    //         }
    //     });
    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Basic Attack Size",
    //         description = "+" + dU.basicAttackSizeIncrease*100 + "% ATK Size but lower attack speed by " + (dD.basicAttackSizeDecrease-1)*100 + "%",
    //         icon = Resources.Load<Sprite>("Images/resize"),
    //         applyChange = () => {
    //             ChangeBasicAttackSize(dU.basicAttackSizeIncrease);
    //             ChangeWeaponSpeed(-dD.weaponSpeedDecrease);
    //         }

    //     });
    //     options.Add(new UpgradeOption()
    //     {
    //         optionName = "Increase Shots Per Attack",
    //         description = "+"+ dU.playerNumOfProjectilesIncrease + "  Shot projectiles but lose " + dD.weaponDamageDecrease+ " Damage and "+ dD.weaponDistanceDecrease +" Range",
    //         icon = Resources.Load<Sprite>("Images/striking-arrows"),
    //         applyChange = () => {
    //             ChangeShotsPerAttack(dU.playerNumOfProjectilesIncrease);
    //             ChangeWeaponDamage(-dD.weaponDamageDecrease);
    //             ChangeWeaponDistance(-dD.weaponDistanceDecrease);
    //         }
    //     });
    // }
