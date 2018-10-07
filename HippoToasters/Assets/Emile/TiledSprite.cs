using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledSprite : MonoBehaviour
{
    public float scale = 0.2f;

    void Start()
    {
    }

    void Update()
    {
        //var sr = GetComponent<SpriteRenderer>();
        //sr.material.SetTextureScale("_MainTex", new Vector2(99, 99));
        //GetComponent<SpriteRenderer>().sprite.texture.
        var mr = GetComponent<MeshRenderer>();
        //mr.material.SetTextureScale("_MainTex", new Vector2(99, 99));
        mr.material.mainTextureScale = new Vector2(transform.localScale.x * scale, transform.localScale.y * scale);
    }
}
