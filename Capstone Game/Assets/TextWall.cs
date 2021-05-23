using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWall : MonoBehaviour
{
    LevelTransition myTransition;
    // Start is called before the first frame update
    void Start()
    {
        myTransition = GameObject.FindObjectOfType<LevelTransition>();
        Text[] textItems = this.gameObject.GetComponentsInChildren<Text>();
        foreach (Text t in textItems)
        {
            if (t.name == "TextWall")
            {
                t.text = PlayerPrefs.GetString("wallText");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Continue()
    {
        myTransition.LoadLevel(PlayerPrefs.GetInt("transitionTo"));
    }
}
