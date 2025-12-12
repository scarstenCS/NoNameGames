using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// -- summary --
/// Camera smoothly follows the player within an arena using dead zone to reduce camera jitter.
/// It only moves when the player leaves the dead zone, and its clamped to arena bounds.
public class CameraFollowScript : MonoBehaviour
{
    public Transform followPlayer;

    public float smoothTime = 0.12f;
    public Vector2 deadZone = new Vector2(0.6f, 0.35f);

    private Vector3 myPos;
    private Vector3 vel;

    void Start()
    {
        if (!followPlayer)
            followPlayer = GameObject.FindWithTag("Player")?.transform;

        myPos = transform.position;
    }

    void LateUpdate()
    {
        if (!followPlayer) return;

        Vector3 cam = transform.position;
        Vector3 desired = cam;

        Vector3 delta = followPlayer.position - cam;

        if (Mathf.Abs(delta.x) > deadZone.x)
            desired.x = followPlayer.position.x - Mathf.Sign(delta.x) * deadZone.x;

        if (Mathf.Abs(delta.y) > deadZone.y)
            desired.y = followPlayer.position.y - Mathf.Sign(delta.y) * deadZone.y;

        desired.z = cam.z;

        myPos = Vector3.SmoothDamp(cam, desired, ref vel, smoothTime);

        // area bounds
        myPos.x = Mathf.Clamp(myPos.x, GameManager.minX, GameManager.maxX);
        myPos.y = Mathf.Clamp(myPos.y, GameManager.minY, GameManager.maxY);

        transform.position = myPos;
    }
}
