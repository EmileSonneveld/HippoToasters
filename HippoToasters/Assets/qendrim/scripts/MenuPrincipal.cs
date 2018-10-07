using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour {

    // Use this for initialization
    public Transform canvas;
    // Update is called once per frame

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void SceneLoading() {

        SceneManager.LoadScene("EmileScene");
	}

    public void LoadMainMenu()
    {

        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        Pause();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

        }
    }
    
        
    
}
