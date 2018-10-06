using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private Animator anim;
    public GameObject colliderObj;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }

    bool playerIsIn = false;
    void Update()
    {
        if (this.playerIsIn)
        {
            var pl = PlayerSpawner.singleton.lastSpawnedPlayer;
            if (pl.velocity.sqrMagnitude > 5.4f * 5.4f)
            {
                //Debug.Log("Vel: " + pl.velocity.sqrMagnitude);
                if (!WiggleSequence_runing)
                    StartCoroutine(WiggleSequence());
            }
        }
    }

    public bool WiggleSequence_runing = false;
    IEnumerator WiggleSequence()
    {
        WiggleSequence_runing = true;

        Debug.Log("Bridge Break!");
        this.anim.SetTrigger("wiggle");
        yield return new WaitForSeconds(1.3f);
        colliderObj.SetActive(false);

        yield return new WaitForSeconds(3);
        colliderObj.SetActive(true);
        WiggleSequence_runing = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Bridge OnTriggerEnter");
        playerIsIn = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Bridge OnTriggerExit");
        playerIsIn = false;
    }
}
