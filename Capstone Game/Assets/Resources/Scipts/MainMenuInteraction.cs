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
    private string startText = "A. V. A.: (Automated Virtual Assistant)\nExiting rift space. Searching for station…\nNo station detected.\n|You:\nDam, Ava reminds me to kill that snake and his faulty rift gate the next time we see him. \n|A. V. A.:\nNote added.\n|You:\nSo where are we?\n|A. V. A.:\nCalculating query…\nThe ship is positioned 27 light years from expected exit.\nNo recognized government claims this area of space.\n|You:\nDead space then?\n|A. V. A.:\nNegative. Multiple man-made objects detected in vicinity.\n|You:\nFriendly?\n|A. V. A.:\nUnknown. None are answering our hails.\nHigh probability of objects being abandoned. \n|You:\nDerelicts then…\n|A. V. A.:\nPlease disregard previous statement.\nWe are receiving a response.\n|???:\nWelcome, I am a Dexincorp Automated Greeter.\nWill you be doing business with us today?\n|You:\nDexincorp? They make rift gates, right Ava?\n|D. A. G.:\nDexincorp make a wide range of products for you pleasure.\nOur rift gates are top of the line and the safest way to travel.\nRemember, when riding with Dexincorp the first trip is free.\n|You:\nAnd that is what you are selling?\n|D. A. G.:\nAffirmative!\n|You:\nI think you got yourself a deal.\n|D. A. G.:\nExcellent! Transmitting coordinates now.\n|You:\nThat is on the other side of this junkyard\n|A. V. A.:\nThe ship contains insufficient fuel to make such a journey.\nCalculating alternatives…\n|A. V. A.:\nNone found. \nSuggested course of action:\nSearch abandoned objects for additional fuel supplies.\n|You:\nBetter than sitting around.";
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
        myTransision.LoadLevel(2, startText, 30);
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
