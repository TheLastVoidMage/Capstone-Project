using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public Animator myAnimator;
    public float transitionTime = 3f;
    private Sprite[,] doorTextures;
    private Sprite selectedLeft;
    private Sprite selectedRight;
    private GameObject leftDoor;
    private GameObject rightDoor;

    public void Start()
    {
        doorTextures = new Sprite[,] { { null, null }, { null, null}, { Resources.Load<Sprite>("Images/LoadingScreens/LevelLeftShip"), Resources.Load<Sprite>("Images/LoadingScreens/LevelRightShip")}, { Resources.Load<Sprite>("Images/LoadingScreens/LevelLeft"), Resources.Load<Sprite>("Images/LoadingScreens/LevelRight") } };
        Image[] doors = this.gameObject.GetComponentsInChildren<Image>();
        for (int x = 0; x < doors.Length; x++)
        {
            if (doors[x].name == "Left Image")
            {
                leftDoor = doors[x].gameObject;
            }
            else
            {
                rightDoor = doors[x].gameObject;
            }
        }
        if (PlayerPrefs.GetInt("transitionTo") != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            int levelFrom = PlayerPrefs.GetInt("transitionTo");
            Debug.Log("From: " + levelFrom);
            if (doorTextures[levelFrom, 0] != null)
            {
                selectedLeft = doorTextures[levelFrom, 0];
            }
            else
            {
                selectedLeft = Resources.Load<Sprite>("Images/LoadingScreens/LoadingScreenLeft");
            }
            if (doorTextures[levelFrom, 1] != null)
            {
                selectedRight = doorTextures[levelFrom, 1];
            }
            else
            {
                selectedRight = Resources.Load<Sprite>("Images/LoadingScreens/LoadingScreenRight");
            }
            leftDoor.GetComponent<Image>().sprite = selectedLeft;
            rightDoor.GetComponent<Image>().sprite = selectedRight;
        }
    }

    public void setDoors(Sprite leftImage, Sprite rightImage)
    {
        leftDoor.GetComponent<Image>().sprite = leftImage;
        rightDoor.GetComponent<Image>().sprite = rightImage;
    }

    public void LoadLevel(int levelID, string wallText = "None", int textSize = 50)
    {
        if (doorTextures[levelID,0] != null)
        {
            selectedLeft = doorTextures[levelID, 0];
        }
        else
        {
            selectedLeft = Resources.Load<Sprite>("Images/LoadingScreens/LoadingScreenLeft");
        }
        if (doorTextures[levelID, 1] != null)
        {
            selectedRight = doorTextures[levelID, 1];
        }
        else
        {
            selectedRight = Resources.Load<Sprite>("Images/LoadingScreens/LoadingScreenRight");
        }
        leftDoor.GetComponent<Image>().sprite = selectedLeft;
        rightDoor.GetComponent<Image>().sprite = selectedRight;
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            PlayerPrefs.SetInt("transitionFrom", SceneManager.GetActiveScene().buildIndex);
        }
        PlayerPrefs.SetInt("transitionTo", levelID);
        if (wallText == "None")
        {
            StartCoroutine(TransitionLevel(levelID));
        }
        else
        {
            PlayerPrefs.SetString("wallText", wallText);
            PlayerPrefs.SetInt("wallTextSize", textSize);
            StartCoroutine(TransitionLevel(4));
        }
    }

    IEnumerator TransitionLevel(int levelID)
    {
        myAnimator.SetTrigger("onExitLevel");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelID);
    }
}
