using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public RectTransform livesTrans;
    public RectTransform foodTrans;

    private void Awake()
    {

    }
    void Start()
    {
    }

    void Update()
    {
        var playerScript = PlayerSpawner.singleton.lastSpawnedPlayer;

        {
            var tmp = livesTrans.localScale;
            tmp.x = (playerScript.lives / 100);
            livesTrans.localScale = tmp;
        }
        {
            var tmp = livesTrans.localScale;
            tmp.x = (playerScript.foodBar / 100);
            foodTrans.localScale = tmp;
        }
        if (shitAlert)
        {
            shitFLipTimer -= Time.deltaTime;
            if (shitFLipTimer < 0)
            {
                shitFLipTimer = 0.5f;
                shitAlertObj.SetActive(!shitAlertObj.activeSelf);
            }
        }
        else
        {

        }
    }

    private float shitFLipTimer;
    public GameObject shitAlertObj;
    public bool shitAlert = false;
}
