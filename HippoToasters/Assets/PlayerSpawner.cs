using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner singleton; // { private set; public get; }
    public GameObject playerPrefab;

    void Awake()
    {
        if (singleton)
            Debug.LogWarning("Singleton already assigned!");
        singleton = this;
    }

    void Update()
    {

    }

    public void SpawnPlayer()
    {

        var obj = Instantiate(playerPrefab);
        obj.transform.position = transform.position;

    }
}
