using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 myPos;
    public Transform myPlay;

    void Start()
    {
        myPlay = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        myPos = transform.position;
        myPos.x = myPlay.position.x;
        myPos.y = myPlay.position.y;
        transform.position = myPos;
    }

}
