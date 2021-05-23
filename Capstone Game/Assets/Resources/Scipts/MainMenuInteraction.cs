using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

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
            if (b.gameObject.name == "New Game")
            {
                //b.GetComponent<RectTransform>().offsetMin = new Vector2(Screen.width / 1.90f, Screen.height);
                //b.GetComponent<RectTransform>().offsetMax = new Vector2(-Screen.width / 1.90f, -Screen.width / 4.5f);
            }
            else if (b.gameObject.name == "Load Game")
            {
                if (File.Exists(Application.persistentDataPath + "/save.save") == false)
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
        PlayerPrefs.SetInt("newGame", 1);
        myTransision.LoadLevel(2, "If you can read this\nyou can read");
        // Start new game
    }

    public void loadGame()
    {
        Debug.Log("Load Game Pressed");
        PlayerPrefs.SetInt("newGame", 0);
        myTransision.LoadLevel(2);
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
