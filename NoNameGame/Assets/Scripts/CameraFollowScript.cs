using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera smoothly follows the player within an arena using dead zone to reduce camera jitter.
/// It only moves when the player leaves the dead zone, and its clamped to arena bounds.
/// </summary>
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
    }

    void LateUpdate()
    {
        if (!followPlayer) return;
        var shake = CameraShake.Instance;

        // work from a base position that ignores shake
        Vector3 camBase = transform.position;
        if (shake != null) camBase -= shake.Offset;

        Vector3 desired = camBase;
        Vector3 delta = followPlayer.position - camBase;

        if (Mathf.Abs(delta.x) > deadZone.x)
            desired.x = followPlayer.position.x - Mathf.Sign(delta.x) * deadZone.x;

        if (Mathf.Abs(delta.y) > deadZone.y)
            desired.y = followPlayer.position.y - Mathf.Sign(delta.y) * deadZone.y;

        desired.z = camBase.z;

        Vector3 followPos = Vector3.SmoothDamp(camBase, desired, ref vel, smoothTime);

        // clamp follow position (unshaken)
        followPos.x = Mathf.Clamp(followPos.x, GameManager.minX, GameManager.maxX);
        followPos.y = Mathf.Clamp(followPos.y, GameManager.minY, GameManager.maxY);

        // shake camera
        Vector3 shakeOffset = shake != null ? shake.Offset : Vector3.zero;
        float shakeRotZ = shake != null ? shake.RotationZ : 0f;

        transform.position = followPos + shakeOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotZ);
    }

}
