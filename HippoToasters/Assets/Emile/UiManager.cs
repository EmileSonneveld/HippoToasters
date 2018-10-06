using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public RectTransform livesTrans;
    //private float origLivesSize;

    private void Awake()
    {

    }
    void Start()
    {
        //origLivesSize = livesTrans.right.x;
    }

    void Update()
    {
        var playerScript = PlayerSpawner.singleton.lastSpawnedPlayer;

        var tmp = livesTrans.localScale;
        tmp.x = (playerScript.lives / 100);
        livesTrans.localScale = tmp;
    }
}
