using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startNewGame()
    {
        Debug.Log("New Game Pressed");
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
