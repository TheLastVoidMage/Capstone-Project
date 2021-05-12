using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirlockController : MonoBehaviour
{
    private LevelTransition myTransition;
    private CircleCollider2D myColider;
    private Canvas myMenu;
    private GameObject myMenuObject;
    // Start is called before the first frame update
    void Start()
    {
        myTransition = GameObject.FindObjectOfType<LevelTransition>();
        myColider = this.gameObject.AddComponent<CircleCollider2D>();
        myColider.radius = 2;
        myColider.isTrigger = true;
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in allCanvases)
        {
            if (c.name == "LeaveMenu")
            {
                myMenu = c;
            }
        }
        myMenuObject = myMenu.GetComponentInChildren<Image>().gameObject;
        foreach (Button b in myMenuObject.GetComponentsInChildren<Button>())
        {
            if (b.name == "YesButton")
            {
                b.onClick.AddListener(leaveLevel);
            }
            else if (b.name == "NoButton")
            {
                b.onClick.AddListener(closeLevel);
            }
        }
        myMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() != null)
        {
            Debug.Log("Would you like to leave");
            myMenuObject.SetActive(true);
        }
    }

    private void leaveLevel()
    {
        myTransition.LoadLevel(2);
    }

    private void closeLevel()
    {
        
    }
}
