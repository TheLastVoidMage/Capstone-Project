using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWall : MonoBehaviour
{
    LevelTransition myTransition;
    private Text myText;
    private string[] Lines = null;
    private int currentLine = 0;
    // Start is called before the first frame update
    void Start()
    {
        myTransition = GameObject.FindObjectOfType<LevelTransition>();
        Text[] textItems = this.gameObject.GetComponentsInChildren<Text>();
        foreach (Text t in textItems)
        {
            if (t.name == "TextWall")
            {
                myText = t;
            }
        }
        Lines = PlayerPrefs.GetString("wallText").Split('|');
        myText.text = Lines[currentLine];
        myText.fontSize = PlayerPrefs.GetInt("wallTextSize");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Continue()
    {
        currentLine++;
        if (currentLine < Lines.Length)
        {
            myText.text = Lines[currentLine];
        }
        else
        {
            myTransition.LoadLevel(PlayerPrefs.GetInt("transitionTo"));
        }
    }
    public void Skip()
    {
        myTransition.LoadLevel(PlayerPrefs.GetInt("transitionTo"));
    }
}
