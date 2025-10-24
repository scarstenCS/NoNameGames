using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
// https://www.youtube.com/watch?v=LNLVOjbrQj4&t=998s
public class BasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 mousePosition;

    public GameObject projectile, player;

    public float projectileSpeed = 5,
    projectileMaxDistance = 5;
    public float maxPierce = 0;
    public float pierce = 0;
    [SerializeField] private Animator animator; 
    public int Damage = 1;
    // private float projectileDistance;
    private Rigidbody2D rb2d, rbProjectile;
    private Transform t, playerT;

    public Transform projectileT;
    private Vector2 origin;
    [SerializeField] Transform spinVisual; 

    public int atkStage = 0;
    public int numberOfProjectiles = 1;
    public int inAir = 0;
    public BasicAttack launcher;
    private float baseAngle;
    public float spreadDegree = 10f;
    public float randomJitter = 3f;
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform firePivot;
    [SerializeField] public bool isProjectile = false;
    [SerializeField] float spinSpeed = 720f; // deg/sec
    private Vector2 travelDestination;
    public bool blockWhileInAir = true;
    private Vector2 _origin;
    void Start()
    {
        t = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        projectileT = GetComponent<Transform>();
        // projectileDistance = projectileMaxDistance;
        playerT = player.GetComponent<Transform>();
        rbProjectile = projectile.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDir;
        if (atkStage == 0)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // from https://www.reddit.com/r/Unity2D/comments/swum6c/how_to_make_object_follow_mouse_in_unity_with_new/
            lookDir = mousePosition - (Vector2)t.position;
            rb2d.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            baseAngle = rb2d.rotation;
        }
        PreformAttack();
    }
    /// <summary>
    /// Attempt to start an attack
    /// </summary>
    public void Attack()
    {
        if (isProjectile || GameManager.isPaused) return;

        // Gate by "in-air" vs capacity
        if (blockWhileInAir)
        {
            if (inAir > 0) return;                      
        }
        else
        {
            if (inAir >= numberOfProjectiles) return;   
        }

        Vector2 firePos;
        //set the position of the projectile being fired. 
        if (firePivot != null)
        {
            firePos = firePivot.position;   
        }
        else
        {
            firePos = transform.position;   
        }

        int freeSlots = Mathf.Max(0, numberOfProjectiles - inAir);

        int allowedProjectiles = Mathf.Max(0, freeSlots);
        int desiredProjectiles = Mathf.Max(1, numberOfProjectiles);
        int shotsThisPress = Mathf.Min(desiredProjectiles, allowedProjectiles);

        float totalFan = spreadDegree * (shotsThisPress - 1);
        float startAngle = baseAngle - totalFan * 0.5f;

        for (int i = 0; i < shotsThisPress; i++)
        {
            float angle = startAngle + spreadDegree * i + Random.Range(-randomJitter, randomJitter);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject proj = Instantiate(projectilePrefab, firePos, rotation);
            var ba = proj.GetComponent<BasicAttack>();

            // Hand off runtime params
            ba.isProjectile = true;
            ba.playerT = playerT;
            ba.projectileSpeed = projectileSpeed;
            ba.projectileMaxDistance = projectileMaxDistance;
            ba.maxPierce = maxPierce;
            ba.pierce = ba.maxPierce;

            //set the projectile in motion
            ba.launcher = this;
            ba._origin = firePos;
            ba.atkStage = 1;
            ba.travelDestination = (Vector2)ba.transform.right;
        }

        inAir += shotsThisPress;        

        AudioManager.SfxPlayerAttack();
        //UnityEngine.Debug.Log($"Fired {shotsThisPress}. In-air now {inAir}/{numberOfProjectiles}.");
    }
    private void OnEnable()
    {
        if (isProjectile)
        {
            _origin = transform.position;
            pierce = maxPierce;

            if (atkStage == 0) atkStage = 1;
            travelDestination = (Vector2)transform.right;
        }
    }

    private void PreformAttack()
    {
        if (!isProjectile) return;
        animator = gameObject.GetComponent<Animator>();

        switch (atkStage)
        {
            case 1: // outbound
                transform.position += (Vector3)(travelDestination * projectileSpeed * Time.deltaTime);
                if (spinVisual != null)
                {
                    spinVisual.Rotate(0, 0, spinSpeed * Time.deltaTime);
                }
                if (Vector2.Distance(_origin, transform.position) >= projectileMaxDistance)
                    atkStage = 2;
                    
                break;

            case 2: // return
                Vector2 toPlayer = (Vector2)playerT.position - (Vector2)transform.position;
                float ang = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
                travelDestination = toPlayer.normalized;
                transform.rotation = Quaternion.Euler(0, 0, ang);
                
                if (spinVisual != null)
                {
                    spinVisual.Rotate(0, 0, spinSpeed * Time.deltaTime);
                }

                // move toward player
                transform.position += (Vector3)(travelDestination * projectileSpeed * Time.deltaTime);

                if (toPlayer.sqrMagnitude <= 0.25f)
                {
                    if (launcher != null)
                    {
                        launcher.inAir = Mathf.Max(0, launcher.inAir - 1);
                        // Debug.Log($"Returned. In-air now {launcher.inAir}/{launcher.numberOfProjectiles}");
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
}
        
    
