using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void Spawn()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
    // GetComponent<Camera>().ViewportToWorldPoint(new Vector3(Random.Range(0, 2), Random.Range(0f, 1f)))
}
