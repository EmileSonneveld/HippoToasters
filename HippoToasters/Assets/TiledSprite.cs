using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var sr = GetComponent<SpriteRenderer>();
        sr.material.SetTextureScale("_MainTex", new Vector2(99, 99));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
