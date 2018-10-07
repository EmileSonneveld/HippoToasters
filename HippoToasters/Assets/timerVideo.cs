using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timerVideo : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(WaitAndLoad(25f, "cinematique"));
    }

    private IEnumerator WaitAndLoad(float value, string scene)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
