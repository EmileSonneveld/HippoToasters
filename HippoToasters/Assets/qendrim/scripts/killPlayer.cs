using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlayer : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("OnTriggerEnter2D");

            //other.gameObject.tag = "Dead";
            PlayerSpawner.singleton.lastSpawnedPlayer.DieSequence();
        }
    }
}
