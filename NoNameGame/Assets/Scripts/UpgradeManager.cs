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
        public int weaponPierceDecrease;
        public int weaponPierceIncrease;
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
    // lack of feeling
    private ChangeValues apathyNumbToPain = new ChangeValues {
        playerShieldAmountIncrease = 2,
        weaponDamageDecrease = 1,
        weaponSpeedDecrease = 0.10f
    };

    private ChangeValues apathyCheckedOut = new ChangeValues {
        playerShieldRechargeAmountIncrease = 0.40f,
        playerSpeedDecrease = 0.10f,
        basicAttackSizeDecrease = 0.20f
    };

    // Distance and less shots
    private ChangeValues lonelinessDistantHeart = new ChangeValues {
        weaponDistanceIncrease = 1f,
        basicAttackSizeDecrease = 0.10f,
        weaponPierceIncrease = 1,
        playerShieldAmountDecrease = 1
    };

    private ChangeValues lonelinessSolitaryShot = new ChangeValues {
        weaponDamageIncrease = 1,
        weaponSpeedIncrease = 0.15f,
        playerNumOfProjectilesDecrease = 1
    };
    // Slow but sturrdy
    private ChangeValues slothDeadWeight = new ChangeValues {
        healthIncrease = 10,
        playerShieldAmountIncrease = 1,
        playerSpeedDecrease = 0.25f
    };

    private ChangeValues slothSlowToPower = new ChangeValues {
        weaponDamageIncrease = 3,
        healthIncrease = 5,
        weaponSpeedDecrease = 0.20f,
        playerSpeedDecrease = 0.20f
    };
    private ChangeValues jealousyEnviousStrike = new ChangeValues {
        weaponDamageIncrease = 1,
        healthDecrease = 5,
        weaponPierceIncrease = 1
    };

    private ChangeValues jealousySpitefulHeart = new ChangeValues {
        playerNumOfProjectilesIncrease = 1,
        healthDecrease = 5,
    };

    private ChangeValues greedMoreMoreMore = new ChangeValues {
        playerNumOfProjectilesIncrease = 3,
        weaponDistanceDecrease = 1f,
        healthDecrease = 5,
        playerShieldAmountDecrease = 1
    };
    private ChangeValues greedAllIn = new ChangeValues {
        weaponDamageIncrease = 2,
        weaponSpeedIncrease = 0.30f,
        healthDecrease = 5,
        playerShieldAmountDecrease = 1
    };
    // Heavy feeling upgrades
    private ChangeValues depressionWeightedShots = new ChangeValues {
        weaponDamageIncrease = 1,
        basicAttackSizeIncrease = 0.40f,
        weaponPierceIncrease = 1,
        weaponSpeedDecrease = 0.30f,
        playerSpeedDecrease = 0.10f
    };
    private ChangeValues depressionWallsUp = new ChangeValues {
        playerShieldAmountIncrease = 1,
        playerShieldRechargeAmountIncrease = 0.2f,
        weaponDamageDecrease = 1,
        playerSpeedDecrease = 0.2f
    };
    // Major Upgrades!
    private ChangeValues anxiety = new ChangeValues {
        weaponSpeedIncrease = 0.50f,
        playerSpeedIncrease = 0.20f,
        playerShieldRechargeAmountIncrease = 0.30f,
        weaponDistanceDecrease = 1f,
        basicAttackSizeDecrease = 0.20f
    };
    private ChangeValues socialIsolation = new ChangeValues {
        weaponDamageIncrease = 1,
        weaponDistanceIncrease = 1f,
        weaponPierceIncrease = 1,
        weaponSpeedIncrease= 0.20f,
        playerShieldAmountDecrease = 1,
        healthDecrease = 5
    };
    private ChangeValues heavyHearted = new ChangeValues {
        healthIncrease = 15,
        playerShieldAmountIncrease = 2,
        basicAttackSizeIncrease = 1f ,
        playerSpeedDecrease = 0.40f
    };

    [SerializeField] private TMP_Text[] buttonLabels;
    [SerializeField] private TMP_Text[] descLabels;
    [SerializeField] private Image[] buttonImages;
    [SerializeField] private BasicAttack launcher;
    [SerializeField] TMP_Text[] statsLabels;
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


    public void ShowUpgradeWindow(int waveNumber)
    {
        //Saftey if wavenumber is less than 0. 
        if(waveNumber <= 0) waveNumber = 1;
        GameManager.ChangeTimeScale(0f);
        GameManager.isPaused = true;
        upgradeWindow.SetActive(true);
        var gm = GameManager.Instance;
        _offered = OfferedChangeOptions(waveNumber);
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
        //show all current upgrades in upgrade screen
        if (Instance != null && Instance.statsLabels != null)
        {
            Instance.statsLabels[0].text = "SPD: " + player.Speed;
            Instance.statsLabels[1].text = "HP: " + player.MaxHealth;
            Instance.statsLabels[2].text = "Range: " + player.basicWeaponDistance;
            Instance.statsLabels[3].text = "ATK DMG: " + player.basicWeaponDmg;
            Instance.statsLabels[4].text = "ATK SPD: " + player.basicWeaponSpeed;
            Instance.statsLabels[5].text = "Projetile Count: " + player.totalBasicAttacksCount;
            Instance.statsLabels[6].text = "Pierce: " + player.basicWeaponPierce;
            Instance.statsLabels[7].text = "ATK Size: " + (player.basicWeaponSize.x * 100).ToString("F0") + "%";
            Instance.statsLabels[8].text = "Shields: " + player.totalShieldCount;
            Instance.statsLabels[9].text = "S.Recharge time: " + player.sheildRegenerateTime + "s";
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
                $"<nobr>+{apathyNumbToPain.playerShieldAmountIncrease} shields,</nobr> " +
                $"<nobr>{-apathyNumbToPain.weaponDamageDecrease} weapon damage,</nobr> " +
                $"<nobr>{-apathyNumbToPain.weaponSpeedDecrease * 100f}% attack speed</nobr>",
            icon = Resources.Load<Sprite>("Images/apathy-numb-to-pain"),
            applyChange = () =>
            {
                ChangePlayerShieldAmount(apathyNumbToPain.playerShieldAmountIncrease);
                ChangeWeaponDamage(apathyNumbToPain.weaponDamageDecrease);
                ChangeWeaponSpeed(apathyNumbToPain.weaponSpeedDecrease);
            }
        });

        options.Add(new ChangeOption()
        {
            optionName = "Apathy: Checked Out",
            description =
                $"<nobr>+{apathyCheckedOut.playerShieldRechargeAmountIncrease*100f }% faster shield recharge rate,</nobr> " +
                $"<nobr>{-apathyCheckedOut.playerSpeedDecrease} player speed,</nobr> " +
                $"<nobr>{-apathyCheckedOut.basicAttackSizeDecrease * 100f}% weapon size</nobr>",
            icon = Resources.Load<Sprite>("Images/apathy-checked-out"),
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
                $"<nobr>+{lonelinessDistantHeart.weaponDistanceIncrease} distance,</nobr> " +
                $"<nobr>{-lonelinessDistantHeart.basicAttackSizeDecrease* 100f}% weapon size,</nobr> " +
                $"<nobr>{-lonelinessDistantHeart.playerShieldAmountDecrease } shield,</nobr> " +
                $"<nobr>+{lonelinessDistantHeart.weaponPierceIncrease } pierce</nobr>",
            icon = Resources.Load<Sprite>("Images/lonliness-distant-heart"),
            applyChange = () => {
                ChangePlayerShieldAmount(-lonelinessDistantHeart.playerShieldAmountDecrease);
                ChangeBasicAttackSize(-lonelinessDistantHeart.basicAttackSizeDecrease);
                ChangeWeaponDistance(lonelinessDistantHeart.weaponDistanceIncrease);
                ChangePierce(lonelinessDistantHeart.weaponPierceIncrease);
            }
        });
        options.Add(new ChangeOption()
        {
            //how to handle when someone doesn't pick projectiles the whole game. 
            optionName = "Loneliness: Solitary Shot",
                description =
                $"<nobr>+{lonelinessSolitaryShot.weaponDamageIncrease } damage,</nobr> " +
                $"<nobr>{lonelinessSolitaryShot.weaponSpeedIncrease* 100f}% weapon speed,</nobr> " +
                $"<nobr>{-lonelinessSolitaryShot.playerNumOfProjectilesDecrease } projectiles</nobr>",
            icon = Resources.Load<Sprite>("Images/lonliness-solitary-shot"),
            applyChange = () => {
                ChangeWeaponDamage(lonelinessSolitaryShot.weaponDamageIncrease);
                ChangeBasicAttackSize(lonelinessSolitaryShot.weaponSpeedIncrease);
                ChangeShotsPerAttack(-lonelinessSolitaryShot.playerNumOfProjectilesDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Sloth: Dead Weight",
                description =
                $"<nobr>+{slothDeadWeight.healthIncrease} max health,</nobr> " +
                $"<nobr>{slothDeadWeight.playerShieldAmountIncrease} shield amount,</nobr> " +
                $"<nobr>{-slothDeadWeight.playerSpeedDecrease* 100f}% player speed</nobr>",
            icon = Resources.Load<Sprite>("Images/sloth-dead-weight"),
            applyChange = () => {
                ChangeMaxHealth(slothDeadWeight.healthIncrease);
                ChangePlayerShieldAmount(slothDeadWeight.playerShieldAmountIncrease);
                ChangePlayerSpeed(-slothDeadWeight.playerSpeedDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Sloth: Slow to Power",
                description =
                $"<nobr>+{slothSlowToPower.weaponDamageIncrease} weapon damage,</nobr> " +
                $"<nobr>+{slothSlowToPower.healthIncrease} max health,</nobr> " +
                $"<nobr>{-slothSlowToPower.weaponSpeedDecrease* 100f}% weapon speed</nobr>, " +
                $"<nobr>{-slothSlowToPower.playerSpeedDecrease* 100f}% player speed</nobr>",
            icon = Resources.Load<Sprite>("Images/sloth-slow-to-power"),
            applyChange = () => {
                ChangeWeaponDamage(slothSlowToPower.weaponDamageIncrease);
                ChangePlayerSpeed(-slothSlowToPower.playerSpeedDecrease);
                ChangeWeaponSpeed(-slothSlowToPower.playerSpeedDecrease);
            }
        });

        options.Add(new ChangeOption()
        {
            optionName = "Jealousy: Envious Strike",
                description =
                $"<nobr>+{jealousyEnviousStrike.weaponDamageIncrease} weapon damage,</nobr> " +
                $"<nobr>{-jealousyEnviousStrike.weaponPierceIncrease} pierce,</nobr> "  +
                $"<nobr>{-jealousyEnviousStrike.healthDecrease} max health</nobr>",
            icon = Resources.Load<Sprite>("Images/jelousy-envious-strike"),
            applyChange = () => {
                ChangeWeaponDamage(jealousyEnviousStrike.weaponDamageIncrease);
                ChangeMaxHealth(-jealousyEnviousStrike.healthDecrease);
                ChangePierce(jealousyEnviousStrike.weaponPierceIncrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Jealousy: Spiteful Heart",
                description =
                $"<nobr>+{jealousySpitefulHeart.playerNumOfProjectilesIncrease} projectiles,</nobr> " +
                $"<nobr>{-jealousySpitefulHeart.healthDecrease} max health</nobr>",
            icon = Resources.Load<Sprite>("Images/jelousy-spiteful-heart"),
            applyChange = () => {
                ChangeMaxHealth(-jealousySpitefulHeart.healthDecrease);
                ChangeShotsPerAttack(jealousySpitefulHeart.playerNumOfProjectilesIncrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Greed: MORE MORE MORE",
                description =
                $"<nobr>+{greedMoreMoreMore.playerNumOfProjectilesIncrease} projectiles,</nobr> " +
                $"<nobr>{-greedMoreMoreMore.weaponDistanceDecrease} range,</nobr> " +
                $"<nobr>{-greedMoreMoreMore.healthDecrease} max health,</nobr> "+
                $"<nobr>{-greedMoreMoreMore.playerShieldAmountDecrease} shields</nobr>",
            icon = Resources.Load<Sprite>("Images/greed-more-more-more"),
            applyChange = () => {
                ChangeWeaponDamage(greedMoreMoreMore.playerNumOfProjectilesDecrease);
                ChangeMaxHealth(-greedMoreMoreMore.healthDecrease);
                ChangePlayerShieldAmount(-greedMoreMoreMore.playerShieldAmountDecrease);
                ChangeWeaponDistance(-greedMoreMoreMore.weaponDistanceDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Greed: All in",
                description =
                $"<nobr>+{greedAllIn.weaponDamageIncrease} weapon damage,</nobr> " +
                $"<nobr>+{greedAllIn.weaponSpeedIncrease* 100f}% weapon speed,</nobr> "+
                $"<nobr>{-greedAllIn.healthDecrease} max health,</nobr> "+
                $"<nobr>{-greedAllIn.playerShieldAmountDecrease} shields</nobr>",
            icon = Resources.Load<Sprite>("Images/greed-all-in"),
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
                $"<nobr>+{depressionWeightedShots.weaponDamageIncrease} weapon damage,</nobr> " +
                $"<nobr>+{depressionWeightedShots.basicAttackSizeIncrease* 100f}% weapon size,</nobr> "+
                $"<nobr>+{depressionWeightedShots.weaponPierceIncrease} pierce,</nobr> " + 
                $"<nobr>{-depressionWeightedShots.weaponSpeedDecrease* 100f}% weapon speed,</nobr> "+
                $"<nobr>{-depressionWeightedShots.playerSpeedDecrease* 100f}% player speed</nobr>",
            icon = Resources.Load<Sprite>("Images/depression-weighted-shots"),
            applyChange = () => {
                ChangeWeaponDamage(depressionWeightedShots.weaponDamageIncrease);
                ChangeWeaponSpeed(-depressionWeightedShots.weaponSpeedDecrease);
                ChangePierce(depressionWeightedShots.weaponPierceIncrease);
                ChangeBasicAttackSize(depressionWeightedShots.basicAttackSizeIncrease);
                ChangePlayerSpeed(-depressionWeightedShots.playerSpeedDecrease);
            }
        });
        options.Add(new ChangeOption()
        {
            optionName = "Depression: Walls up",
                description =
                $"<nobr>+{depressionWallsUp.playerShieldAmountIncrease} shields,</nobr> " +
                $"<nobr>+{depressionWallsUp.playerShieldRechargeAmountIncrease* 100f}% faster shield recharge rate,</nobr> " +
                $"<nobr>{-depressionWallsUp.weaponDamageDecrease} weapon damage,</nobr> " +
                $"<nobr>{-depressionWallsUp.playerSpeedDecrease* 100f}% player speed</nobr>",
            icon = Resources.Load<Sprite>("Images/depression-walls-up"),
            applyChange = () => {
                ChangePlayerShieldAmount(depressionWallsUp.playerShieldAmountIncrease);
                ChangePlayerShieldRecharge(depressionWallsUp.playerShieldRechargeAmountIncrease);
                ChangeWeaponDamage(-depressionWallsUp.weaponDamageDecrease);
                ChangePlayerSpeed(-depressionWallsUp.playerSpeedDecrease);
            }
        });
    }
    public List<ChangeOption> OfferedChangeOptions(int currentWave)
    {
        _offered.Clear();
        if (currentWave == 1)
        {
            // Example: Three major upgrade path options for the player to choose from
            _offered.Add(new ChangeOption {
                optionName = "Anxiety",
                description =
                    $"<nobr>+{anxiety.weaponSpeedIncrease * 100f}% attack speed,</nobr> " +
                    $"<nobr>+{anxiety.playerSpeedIncrease * 100f}% player speed,</nobr> " +
                    $"<nobr>+{anxiety.playerShieldRechargeAmountIncrease * 100f}% faster shield recharge,</nobr> " +
                    $"<nobr>{-anxiety.weaponDistanceDecrease} range,</nobr> " +
                    $"<nobr>{-anxiety.basicAttackSizeDecrease * 100f}% attack size</nobr>",
                icon = Resources.Load<Sprite>("Images/anxiety"), 
                applyChange = () => {
                    ChangeWeaponSpeed(anxiety.weaponSpeedIncrease);
                    ChangePlayerSpeed(anxiety.playerSpeedIncrease);
                    ChangePlayerShieldRecharge(anxiety.playerShieldRechargeAmountIncrease);
                    ChangeWeaponDistance(-anxiety.weaponDistanceDecrease);
                    ChangeBasicAttackSize(-anxiety.basicAttackSizeDecrease);
                }
            });
            _offered.Add(new ChangeOption {
                optionName = "Social Isolation",
                description =
                    $"<nobr>+{socialIsolation.weaponDamageIncrease} weapon damage,</nobr> " +
                    $"<nobr>+{socialIsolation.weaponDistanceIncrease} range,</nobr> " +
                    $"<nobr>+{(int)socialIsolation.weaponPierceIncrease} pierce,</nobr> " +
                    $"<nobr>{socialIsolation.weaponSpeedIncrease* 100f}% weapon speed,</nobr> " +
                    $"<nobr>{-socialIsolation.playerShieldAmountDecrease} shield,</nobr> " +
                    $"<nobr>{-socialIsolation.healthDecrease} max health</nobr>",
                icon = Resources.Load<Sprite>("Images/social-isolation"),
                applyChange = () => {
                    ChangeWeaponDamage(socialIsolation.weaponDamageIncrease);
                    ChangeWeaponDistance(socialIsolation.weaponDistanceIncrease);
                    ChangePierce((int)socialIsolation.weaponPierceIncrease);
                    ChangeWeaponSpeed(socialIsolation.weaponSpeedIncrease);
                    ChangePlayerShieldAmount(-socialIsolation.playerShieldAmountDecrease);
                    ChangeMaxHealth(-socialIsolation.healthDecrease);
                }
            });
            _offered.Add(new ChangeOption {
                optionName = "Heavy Heart",
                description =
                    $"<nobr>+{heavyHearted.healthIncrease} max health</nobr>, " +
                    $"<nobr>+{heavyHearted.basicAttackSizeIncrease* 100f}% weapon size</nobr>, " +
                    $"<nobr>+{heavyHearted.playerShieldAmountIncrease} shields</nobr>, " +
                    $"<nobr>-{heavyHearted.playerSpeedDecrease * 100f}% player speed</nobr>",
                icon = Resources.Load<Sprite>("Images/heavy-heart"),
                applyChange = () => {
                    ChangeMaxHealth(heavyHearted.healthIncrease);
                    ChangeBasicAttackSize(heavyHearted.basicAttackSizeIncrease);
                    ChangePlayerShieldAmount(heavyHearted.playerShieldAmountIncrease);
                    ChangePlayerSpeed(-heavyHearted.playerSpeedDecrease);
                }
            });
            // (Add other major options as needed, up to 3 shown at once due to UI)
            return _offered;
        }
        else
        {
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
        double boundaries = player.basicWeaponDistance += amount;
        if(boundaries > 2.5 && boundaries < 8)
            player.basicWeaponDistance = (float)boundaries;
        else if(boundaries < 2.5)
        {
            boundaries = 2.5;
        }
        else
        {
            boundaries = 8;
        }
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
