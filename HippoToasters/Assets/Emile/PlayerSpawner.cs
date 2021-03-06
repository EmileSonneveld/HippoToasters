﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner singleton;
    public PlayerPlatformerController lastSpawnedPlayer;
    public GameObject playerPrefab;

    void Awake()
    {
        if (singleton)
            Debug.LogWarning("Singleton already assigned!");
        singleton = this;

        lastSpawnedPlayer = Object.FindObjectOfType<PlayerPlatformerController>();
    }

    void Update()
    {

    }

    public void SpawnPlayer()
    {

        var obj = Instantiate(playerPrefab);
        obj.transform.position = transform.position;
        lastSpawnedPlayer = obj.GetComponent<PlayerPlatformerController>();
    }
}
