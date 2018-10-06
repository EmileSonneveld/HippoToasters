using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float lives = 100;


    private Transform mainCamera;

    void Awake()
    {
        this.mainCamera = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var tmp = mainCamera.position;
        tmp.x = transform.position.x;
        tmp.y = transform.position.y;
        mainCamera.position = tmp;

        if (lives < 0)
        {
            lives = 0;
            DieSequence();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.relativeVelocity.sqrMagnitude > 5)
        {
            Debug.Log("OnCollisionEnter2D" + col.relativeVelocity.magnitude);
            this.lives -= col.relativeVelocity.magnitude * 10; // random multiplier
        }
    }


    void DieSequence()
    {
        Debug.Log("We died!");

        Destroy(GetComponent<PlayerPlatformerController>());
        PlayerSpawner.singleton.SpawnPlayer();
    }

}
