using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuInteraction : MonoBehaviour
{
    private Button[] buttons = null;
    private bool foundSave = false;
    private LevelTransition myTransision;
    // Start is called before the first frame update
    void Start()
    {
        myTransision = GameObject.FindObjectOfType<LevelTransition>();
        buttons = this.gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            if (b.gameObject.name == "Load Game")
            {
                if (foundSave == false)
                {
                    b.interactable = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startNewGame()
    {
        Debug.Log("New Game Pressed");
        myTransision.LoadLevel(2);
        // Start new game
    }

    public void loadGame()
    {
        Debug.Log("Load Game Pressed");
        // Load game
    }

    public void openSettings()
    {
        Debug.Log("Settings Pressed");
        // Open Settings
    }

    public void openCredits()
    {
        Debug.Log("Credits Pressed");
        // Open Credits
    }

    public void leaveGame()
    {
        Debug.Log("Exit Pressed");
        Application.Quit();
    }
}
