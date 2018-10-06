using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public PlayerScript playerScript;

    public RectTransform livesTrans;
    //private float origLivesSize;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        //origLivesSize = livesTrans.right.x;
    }

    // Update is called once per frame
    void Update()
    {
        var tmp = livesTrans.localScale;
        tmp.x = (this.playerScript.lives / 100);
        livesTrans.localScale = tmp;
    }
}
