using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameObject[] pauseObjects;
    private LevelTransition myTransision;
    private playerController myPlayer;

    // Use this for initialization
    void Start()
    {
        myTransision = GameObject.FindObjectOfType<LevelTransition>();
        Time.timeScale = 1;
        myPlayer = GameObject.FindObjectOfType<playerController>();
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        bool isPaused = false;
        if (myPlayer != null)
        {
            isPaused = myPlayer.isPaused;
        }
        //uses the ESC button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0 || isPaused == false)
            {
                if (Time.timeScale == 1)
                {
                    Debug.Log("Paused");
                    Time.timeScale = 0;
                    showPaused();
                }
                else if (Time.timeScale == 0)
                {
                    Debug.Log("Unpaused");
                    Time.timeScale = 1;
                    hidePaused();
                }
            }
        }
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Debug.Log("Resume Pressed");
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
            if (myPlayer != null)
            {
                myPlayer.isPaused = true;
            }
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
            if (myPlayer != null)
            {
                myPlayer.isPaused = false;
            }
        }
    }

    public void quitToMain()
    {
        Time.timeScale = 1;
        Debug.Log("Quit Button Pressed");
        myTransision.LoadLevel(1);

    }
}