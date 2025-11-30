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
    public struct ChangeOption
    {
        public string optionName;
        public string description;
        public Sprite icon;
        public System.Action applyChange;
    }
    [System.Serializable]
    public struct ChangeValues
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
        public int healthDecrease;
        public int weaponDamageDecrease;
        public float weaponDistanceDecrease;
        public float weaponSpeedDecrease;
        public float playerSpeedDecrease;
        public float basicAttackSizeDecrease;
        public int playerNumOfProjectilesDecrease;
        public int playerShieldAmountDecrease;
        public float playerShieldRechargeAmountDecrease;
        public float weaponPierceDecrease;
        public float weaponPierceIncrease;
    }
    private ChangeValues defaults = new ChangeValues
    {
        healthIncrease = 5,
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 0.5f,
        weaponSpeedIncrease = 0.25f,
        playerSpeedIncrease = 0.25f,
        basicAttackSizeIncrease = 0.25f,
        playerShieldAmountIncrease = 1,
        playerShieldRechargeAmountIncrease = 0.33f,
        playerNumOfProjectilesIncrease = 1,
        healthDecrease = 5,
        weaponDamageDecrease = 1,
        weaponDistanceDecrease = 0.5f,
        weaponSpeedDecrease = 0.25f,
        playerSpeedDecrease = 0.25f,
        basicAttackSizeDecrease = 0.25f,
        playerShieldAmountDecrease = 1,
        playerShieldRechargeAmountDecrease = 0.33f,
        playerNumOfProjectilesDecrease = 1,
        weaponPierceDecrease = 1,
        weaponPierceIncrease = 1

    };
    // APATHY
    [SerializeField]
    private ChangeValues apathyNumbToPain = new ChangeValues {
        healthIncrease = 5,
        weaponDamageDecrease = 1,
        weaponSpeedDecrease = 0.10f
    };

    [SerializeField]
    private ChangeValues apathyCheckedOut = new ChangeValues {
        playerShieldRechargeAmountIncrease = 0.20f,
        playerSpeedDecrease = 0.10f,
        basicAttackSizeDecrease = 0.10f
    };


    // LONELINESS
    [SerializeField]
    private ChangeValues lonelinessDistantHeart = new ChangeValues {
        weaponDistanceIncrease = 0.75f,
        basicAttackSizeDecrease = 0.15f,
        playerShieldAmountDecrease = 1
    };

    [SerializeField]
    private ChangeValues lonelinessSolitaryShot = new ChangeValues {
        weaponDamageIncrease = 2,
        basicAttackSizeIncrease = 0.15f,
        playerNumOfProjectilesDecrease = 1
    };


    // SLOTH
    [SerializeField]
    private ChangeValues slothDeadWeight = new ChangeValues {
        healthIncrease = 7,
        playerShieldAmountIncrease = 1,
        playerSpeedDecrease = 0.25f
    };

    [SerializeField]
    private ChangeValues slothLaziness = new ChangeValues {
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 0.25f,
        weaponSpeedDecrease = 0.15f,
        playerSpeedDecrease = 0.15f
    };


    // JEALOUSY
    [SerializeField]
    private ChangeValues jealousyEnviousStrike = new ChangeValues {
        weaponDamageIncrease = 2,
        healthDecrease = 5
    };

    [SerializeField]
    private ChangeValues jealousyCovetousHarvest = new ChangeValues {
        weaponDamageIncrease = 1,
        playerNumOfProjectilesIncrease = 1,
        healthDecrease = 3,
        playerShieldAmountDecrease = 1
    };


    // GREED
    [SerializeField]
    private ChangeValues greedLongShot = new ChangeValues {
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 0.25f,
        playerNumOfProjectilesIncrease = 1,
        healthDecrease = 5,
        playerShieldAmountDecrease = 1
    };

    [SerializeField]
    private ChangeValues greedAllIn = new ChangeValues {
        weaponDamageIncrease = 2,
        weaponSpeedIncrease = 0.30f,
        healthDecrease = 10,
        playerShieldAmountDecrease = 1
    };


    // DEPRESSION
    [SerializeField]
    private ChangeValues depressionWeightedShots = new ChangeValues {
        weaponDamageIncrease = 1,
        basicAttackSizeIncrease = 0.30f,
        weaponSpeedDecrease = 0.25f,
        playerSpeedDecrease = 0.10f
    };

    [SerializeField]
    private ChangeValues depressionWallsUp = new ChangeValues {
        playerShieldAmountIncrease = 2,
        weaponDamageDecrease = 1,
        playerSpeedDecrease = 0.2f
    };


    [SerializeField] private TMP_Text[] buttonLabels;
    [SerializeField] private TMP_Text[] descLabels;
    [SerializeField] private Image[] buttonImages;
    [SerializeField] private BasicAttack launcher;
    private readonly List<ChangeOption> options = new List<ChangeOption>();
    private List<ChangeOption> _offered = new List<ChangeOption>();
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
        _offered = OfferedChangeOptions();
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
        
        options.Add(new ChangeOption
        {
            optionName = "Apathy: Numb to Pain",
            description =
                $"+{apathyNumbToPain.healthIncrease} HP, " +
                $"{-apathyNumbToPain.weaponDamageDecrease} ATK, " +
                $"{-apathyNumbToPain.weaponSpeedDecrease * 100f}% attack speed",
            icon = null,
            applyChange = () =>
            {
                ChangeMaxHealth(apathyNumbToPain.healthIncrease);
                ChangeWeaponDamage(apathyNumbToPain.weaponDamageDecrease);
                ChangeWeaponSpeed(apathyNumbToPain.weaponSpeedDecrease);
            }
        });

        options.Add(new ChangeOption()
        {
            optionName = "Apathy: Checked Out",
            description =
                $"+{apathyCheckedOut.playerShieldRechargeAmountIncrease*100f }% faster shield recharge, " +
                $"{-apathyCheckedOut.playerSpeedDecrease} Speed, " +
                $"{-apathyCheckedOut.basicAttackSizeDecrease * 100f}% size",
            icon = null,
            applyChange = () => 
            {
                ChangePlayerShieldRecharge(apathyCheckedOut.playerShieldRechargeAmountIncrease);
                ChangePlayerSpeed(-apathyCheckedOut.playerSpeedDecrease);
                ChangeBasicAttackSize(-apathyCheckedOut.basicAttackSizeDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Loneliness: Distant Heart",
                description =
                $"+{lonelinessDistantHeart.weaponDistanceIncrease*100f }% distance, " +
                $"{-lonelinessDistantHeart.basicAttackSizeDecrease* 100f}% Weapon Size, " +
                $"{-lonelinessDistantHeart.playerShieldAmountDecrease } shield",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangePlayerShieldAmount(-lonelinessDistantHeart.playerShieldAmountDecrease);
                ChangeBasicAttackSize(-lonelinessDistantHeart.basicAttackSizeDecrease);
                ChangeWeaponDistance(lonelinessDistantHeart.weaponDistanceIncrease);

            }
        });
        options.Add(new ChangeOption()
        {
            //how to handle when someone doesn't pick projectiles the whole game. 
            optionName = "Loneliness: Solitary Shot",
                description =
                $"+{lonelinessSolitaryShot.weaponDamageIncrease } damage, " +
                $"{lonelinessSolitaryShot.basicAttackSizeIncrease* 100f}% Weapon Size, " +
                $"{-lonelinessSolitaryShot.playerNumOfProjectilesDecrease } projectiles",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(lonelinessSolitaryShot.weaponDamageIncrease);
                ChangeBasicAttackSize(lonelinessSolitaryShot.basicAttackSizeIncrease);
                ChangeShotsPerAttack(-lonelinessSolitaryShot.playerNumOfProjectilesDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Sloth: Dead Weight",
                description =
                $"+{slothDeadWeight.healthIncrease} health, " +
                $"{slothDeadWeight.playerShieldAmountIncrease} shield amount, " +
                $"{-slothDeadWeight.playerSpeedDecrease* 100f}% speed",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeMaxHealth(slothDeadWeight.healthIncrease);
                ChangePlayerShieldAmount(slothDeadWeight.playerShieldAmountIncrease);
                ChangePlayerSpeed(-slothDeadWeight.playerSpeedDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Sloth: Laziness",
                description =
                $"+{slothLaziness.weaponDamageIncrease} DMG, " +
                $"{slothLaziness.weaponDistanceIncrease* 100f}% range, " +
                $"{-slothLaziness.weaponSpeedDecrease* 100f}% weapon speed, " +
                $"{-slothLaziness.playerSpeedDecrease* 100f}% speed",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(slothLaziness.weaponDamageIncrease);
                ChangeWeaponDistance(slothLaziness.weaponDistanceIncrease);
                ChangePlayerSpeed(-slothLaziness.playerSpeedDecrease);
                ChangeWeaponSpeed(-slothLaziness.playerSpeedDecrease);
            }
        });

        options.Add(new ChangeOption()
        {
            optionName = "Jealousy: Envious Strike",
                description =
                $"+{jealousyEnviousStrike.weaponDamageIncrease} DMG, " +
                $"{-jealousyEnviousStrike.healthDecrease} HP",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(jealousyEnviousStrike.weaponDamageIncrease);
                ChangeMaxHealth(-jealousyEnviousStrike.healthDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Jealousy: Envious Strike",
                description =
                $"+{jealousyCovetousHarvest.weaponDamageIncrease} DMG, " +
                $"{jealousyCovetousHarvest.playerNumOfProjectilesIncrease} projectiles, " +
                $"+{-jealousyCovetousHarvest.healthDecrease} HP, " +
                $"{-jealousyCovetousHarvest.playerShieldAmountDecrease} shields",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(jealousyCovetousHarvest.weaponDamageIncrease);
                ChangeMaxHealth(-jealousyCovetousHarvest.healthDecrease);
                ChangeShotsPerAttack(jealousyCovetousHarvest.playerNumOfProjectilesIncrease);
                ChangePlayerShieldAmount(-jealousyCovetousHarvest.playerShieldAmountDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Greed: Long Shot",
                description =
                $"+{greedLongShot.weaponDamageIncrease} DMG, " +
                $"{greedLongShot.weaponDistanceIncrease* 100f}% range, " +
                $"+{-greedLongShot.healthDecrease} HP",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(greedLongShot.weaponDamageIncrease);
                ChangeMaxHealth(-greedLongShot.healthDecrease);
                ChangeWeaponDistance(greedLongShot.weaponDistanceIncrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Greed: All in",
                description =
                $"+{greedAllIn.weaponDamageIncrease} DMG, " +
                $"{greedAllIn.weaponSpeedIncrease* 100f}% weapon speed, "+
                $"+{-greedAllIn.healthDecrease} HP, "+
                $"+{-greedAllIn.playerShieldAmountDecrease} shields",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(greedAllIn.weaponDamageIncrease);
                ChangeMaxHealth(-greedAllIn.healthDecrease);
                ChangeWeaponSpeed(greedAllIn.weaponSpeedIncrease);
                ChangePlayerShieldAmount(-greedAllIn.playerShieldAmountDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Depression: Weighted Shots",
                description =
                $"+{depressionWeightedShots.weaponDamageIncrease} DMG, " +
                $"{depressionWeightedShots.basicAttackSizeIncrease* 100f}% weapon size, "+
                $"+{-depressionWeightedShots.weaponSpeedDecrease* 100f}% weapon speed, "+
                $"+{-depressionWeightedShots.playerSpeedDecrease* 100f}% speed",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangeWeaponDamage(depressionWeightedShots.weaponDamageIncrease);
                ChangeWeaponSpeed(-depressionWeightedShots.weaponSpeedDecrease);
                ChangeBasicAttackSize(depressionWeightedShots.basicAttackSizeIncrease);
                ChangePlayerSpeed(-depressionWeightedShots.playerSpeedDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Depression: Walls up",
                description =
                $"+{depressionWallsUp.playerShieldAmountIncrease} shields, " +
                $"{-depressionWallsUp.weaponDamageDecrease} DMG, " +
                $"+{-depressionWallsUp.playerSpeedDecrease* 100f}% player speed",
            //icon = Resources.Load<Sprite>("Images/charged-arrow"),
            applyChange = () => {
                ChangePlayerShieldAmount(depressionWallsUp.playerShieldAmountIncrease);
                ChangeWeaponDamage(-depressionWallsUp.weaponDamageDecrease);
                ChangePlayerSpeed(-depressionWallsUp.playerSpeedDecrease);
            }
        });
    }
    public List<ChangeOption> OfferedChangeOptions()
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
        int newMax = player.MaxHealth + amount;
        if(newMax <= 0)
        {
            player.MaxHealth = 1;

        }
        else
        {
            player.MaxHealth = newMax;
        }
        
        
    }
    /// <summary>
    /// Increases basic weapon damage 
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangeWeaponDamage(int amount)
    {
        int newDMG = player.basicWeaponDmg += amount;
        if(newDMG <= 0)
        {
            player.basicWeaponDmg = 1;
        }
        else
        {
            player.basicWeaponDmg = newDMG;
        }
        
        //Debug.Log($"Basic weapon damage increased by {amount}");
    }

    /// <summary>
    /// Increases basic weapon distance
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangeWeaponDistance(float amount)
    {
        player.basicWeaponDistance += player.basicWeaponDistance*amount;
        
        //Debug.Log($"Basic weapon distance increased by {amount}");
    }
    /// <summary>
    /// increases weapon speed
    /// </summary>
    /// <param name="amount">Amount to increase by</param>
    public void ChangeWeaponSpeed(float amount)
    {
        player.basicWeaponSpeed += player.basicWeaponSpeed * amount;
        //Debug.Log($"Basic weapon speed increased by {amount}");
    }

    /// <summary>
    /// Increases Player speed
    /// </summary>
    /// <param name="amount">amount to increase by</param>
    public void ChangePlayerSpeed(float amount)
    {
        player.Speed += player.Speed *amount;
        //Debug.Log($"Player speed increased by {amount}");
    }
    public void ChangePlayerShieldRecharge(float newRecharge)
    {

        player.sheildRegenerateTime -= player.sheildRegenerateTime *newRecharge;
    }
    public void ChangePlayerShieldAmount(int shieldAmount)
    {
        int newShieldAmt = player.totalShieldCount += shieldAmount;
        if(newShieldAmt <= 0)
        {
            player.totalShieldCount = 0;
        }
        else
        {
            player.totalShieldCount = newShieldAmt;
        }
        
    }
    public void ChangePierce(int pierceAmount)
    {
        int pierceTotal = player.basicWeaponPierce + pierceAmount;
        if(pierceTotal <= 0)
        {
            player.basicWeaponPierce = 0;
        }
        else
        {
            player.basicWeaponPierce = pierceTotal;
        }
        
    }
    public void ChangeBasicAttackSize(float sizeIncrease)
    {

        player.basicWeaponSize += sizeIncrease * player.basicWeaponSize;

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
    //     options.Add(new ChangeOption()
    //     {
    //         optionName = "Increase Max Health",
    //         description = "+" + dU.healthIncrease + " HP",
    //         icon = Resources.Load<Sprite>("Images/heart-plus"),
    //         applyChange = () => ChangeMaxHealth(dU.healthIncrease)
    //     });

    //     options.Add(new ChangeOption()
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
    //     options.Add(new ChangeOption()
    //     {
    //         optionName = "Increase Shield Amount",
    //         description = "+" + dU.playerShieldAmountIncrease + " shield but lose" + dD.playerSpeedDecrease*100+"% speed.",
    //         //icon = Resources.Load<Sprite>("Images/charged-arrow"),
    //         applyChange = () => {
    //             ChangePlayerShieldAmount(dU.playerShieldAmountIncrease);
    //             ChangePlayerSpeed(dD.playerSpeedDecrease);

    //         }
    //     });
    //     options.Add(new ChangeOption()
    //     {
    //         optionName = "Reduce Shield recharge time",
    //         description = "+" + dU.playerShieldRechargeAmountIncrease*100 + "% reduced time",
    //         //icon = Resources.Load<Sprite>("Images/charged-arrow"),
    //         applyChange = () => ChangePlayerShieldRecharge(dU.playerShieldRechargeAmountIncrease)
    //     });

    //     options.Add(new ChangeOption()
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

    //     options.Add(new ChangeOption()
    //     {   
    //         optionName = "Increase Weapon Speed",
    //         description = "+" + dU.weaponSpeedIncrease*100 + "% ATK SPD but reduce weapon size by "+dD.basicAttackSizeDecrease +"%",
    //         icon = Resources.Load<Sprite>("Images/supersonic-bullet"),
    //         applyChange = () =>{
    //             ChangeWeaponSpeed(dU.weaponSpeedIncrease);
    //             ChangeBasicAttackSize(-dD.basicAttackSizeDecrease);
    //         }
    //     });

    //     options.Add(new ChangeOption()
    //     {
    //         optionName = "Increase Player Speed",
    //         description = "+" + dU.playerSpeedIncrease*100 + "% SPD",
    //         icon = Resources.Load<Sprite>("Images/wingfoot"),
    //         applyChange = () => 
    //         {
    //             ChangePlayerSpeed(dU.playerSpeedIncrease);

    //         }
    //     });
    //     options.Add(new ChangeOption()
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
    //     options.Add(new ChangeOption()
    //     {
    //         optionName = "Increase Basic Attack Size",
    //         description = "+" + dU.basicAttackSizeIncrease*100 + "% ATK Size but lower attack speed by " + (dD.basicAttackSizeDecrease-1)*100 + "%",
    //         icon = Resources.Load<Sprite>("Images/resize"),
    //         applyChange = () => {
    //             ChangeBasicAttackSize(dU.basicAttackSizeIncrease);
    //             ChangeWeaponSpeed(-dD.weaponSpeedDecrease);
    //         }

    //     });
    //     options.Add(new ChangeOption()
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
