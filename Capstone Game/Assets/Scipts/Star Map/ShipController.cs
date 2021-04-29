using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    private StarMap myMap;
    private GameObject playerShip;
    public Sprite playerShipSprite;
    private Canvas bottomBar;
    private Button travelButton;
    private Text levelText;
    public Sprite selectorSprite;
    private RectTransform[] UIObjects;
    private GameObject selector;
    private float shipChance = 30;
    private float[] timeLastMoved = { 0, 0 };
    private int[] playerCoordinates = { 0, 0 };
    private int[] selectorCoordinates = { 0, 0 };
    private int fuel = 20;
    // Start is called before the first frame update
    void Start()
    {
        myMap = this.gameObject.GetComponent<StarMap>();
        playerCoordinates[1] = Mathf.RoundToInt(myMap.levelHeight / 2);
        selectorCoordinates[1] = playerCoordinates[1];
        playerShip = new GameObject("Player Ship");
        playerShip.transform.parent = this.transform;
        playerShip.AddComponent<SpriteRenderer>().sprite = playerShipSprite;
        playerShip.transform.localPosition = new Vector3((playerCoordinates[0] * myMap.levelSpacing) - 1, (playerCoordinates[1] * myMap.levelSpacing) + 1);
        playerShip.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        for (int x = 0; x < canvases.Length;x++)
        {
            if (canvases[x].gameObject.name == "BottomBar")
            {
                bottomBar = canvases[x];
            }
        }
        UIObjects = bottomBar.gameObject.transform.GetComponentsInChildren<RectTransform>();
        for (int x = 0; x < UIObjects.Length; x++)
        {
            if (UIObjects[x].name == "TravelButton")
            {
                travelButton = UIObjects[x].GetComponent<Button>();
            }
            else if (UIObjects[x].name == "LevelInfo")
            {
                levelText = UIObjects[x].GetComponent<Text>();
            }
        }
        selector = new GameObject();
        selector.name = "Selector";
        selector.transform.parent = this.transform;
        selector.transform.localPosition = playerShip.transform.localPosition;
        selector.AddComponent<SpriteRenderer>().sprite = selectorSprite;
        selector.GetComponent<SpriteRenderer>().sortingOrder = -1;
        Camera.main.transform.parent = selector.transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        timeLastMoved[0] = Time.time;
        timeLastMoved[1] = Time.time;
    }

    private void handleTravelButton()
    {
        bool validButton = false;
        string displayString = travelButton.GetComponentInChildren<Text>().text = "Travel\nCost: " + Mathf.Max(Mathf.Abs(selectorCoordinates[0] - playerCoordinates[0]), Mathf.Abs(selectorCoordinates[1] - playerCoordinates[1])) + " / " + fuel;
        selector.transform.position = Vector3.Lerp(selector.transform.position, new Vector3(selectorCoordinates[0] * myMap.levelSpacing, selectorCoordinates[1] * myMap.levelSpacing), Time.deltaTime * 5);
        if (playerShip.transform.localPosition != new Vector3((playerCoordinates[0] * myMap.levelSpacing) - 1, (playerCoordinates[1] * myMap.levelSpacing) + 1))
        {
            playerShip.transform.localPosition = Vector3.Lerp(playerShip.transform.localPosition, new Vector3((playerCoordinates[0] * myMap.levelSpacing) - 1, (playerCoordinates[1] * myMap.levelSpacing) + 1), Time.deltaTime * 5);
        }
        if (playerCoordinates[0] == selectorCoordinates[0] && playerCoordinates[1] == selectorCoordinates[1])
        {
            if (playerCoordinates[0] >= 0 && playerCoordinates[1] >= 0 && playerCoordinates[0] < myMap.levelWidth && playerCoordinates[1] < myMap.levelHeight)
            {
                if (myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]] != null)
                {
                    if (myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]].isVisited == false)
                    {
                        displayString = "Board";
                        validButton = true;
                    }
                }
            }
        }
        else
        {
            if (fuel >= Mathf.Max(Mathf.Abs(selectorCoordinates[0] - playerCoordinates[0]), Mathf.Abs(selectorCoordinates[1] - playerCoordinates[1])))
            {
                validButton = true;
            }

        }
        travelButton.GetComponentInChildren<Text>().text = displayString;
        travelButton.interactable = validButton;
    }

    public void Travel()
    {
        if (playerCoordinates[0] == selectorCoordinates[0] && playerCoordinates[1] == selectorCoordinates[1])
        {
            if (playerCoordinates[0] >= 0 && playerCoordinates[1] >= 0 && playerCoordinates[0] < myMap.levelWidth && playerCoordinates[1] < myMap.levelHeight)
            {
                if (myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]] != null)
                {
                    if (myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]].isVisited == false)
                    {
                        myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]].isVisited = true;
                        myMap.ShipMap[playerCoordinates[0], playerCoordinates[1]].loadLevel();
                    }
                }
            }
        }
        else
        {
            if (fuel >= Mathf.Max(Mathf.Abs(selectorCoordinates[0] - playerCoordinates[0]), Mathf.Abs(selectorCoordinates[1] - playerCoordinates[1])))
            {
                fuel = fuel - Mathf.Max(Mathf.Abs(selectorCoordinates[0] - playerCoordinates[0]), Mathf.Abs(selectorCoordinates[1] - playerCoordinates[1]));
                playerCoordinates[0] = selectorCoordinates[0];
                playerCoordinates[1] = selectorCoordinates[1];
            }
        }
        handleTravelButton();
    }

    private void manageControls()
    {
        if (Time.time - timeLastMoved[1] > .25f)
        {
            if (Input.GetKey(KeyCode.W))
            {
                selectorCoordinates[1] = selectorCoordinates[1] + 1;
                timeLastMoved[1] = Time.time;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                selectorCoordinates[1] = selectorCoordinates[1] - 1;
                timeLastMoved[1] = Time.time;
            }
        }
        if (Time.time - timeLastMoved[0] > .25f)
        {
            if (Input.GetKey(KeyCode.D))
            {
                selectorCoordinates[0] = selectorCoordinates[0] + 1;
                timeLastMoved[0] = Time.time;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                selectorCoordinates[0] = selectorCoordinates[0] - 1;
                timeLastMoved[0] = Time.time;
            }
        }
        handleTravelButton();
        if (selectorCoordinates[0] < 0 || selectorCoordinates[1] < 0 || selectorCoordinates[0] >= myMap.levelWidth || selectorCoordinates[1] >= myMap.levelHeight)
        {
            levelText.text = "Dead Space Detected";
        }
        else
        {
            if (myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]] != null)
            {
                levelText.text = "Ship Detected\nShip Visited: " + myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]].isVisited + "\nSize: " + Level.valueNames[0, myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]].levelValues[0]] + "\nLife sign Density: " + Level.valueNames[1, myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]].levelValues[1]] + "\nLife Sign Strength: " + Level.valueNames[2, myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]].levelValues[2]] + "\nLife Sign Type: " + Level.factionNames[myMap.ShipMap[selectorCoordinates[0], selectorCoordinates[1]].levelValues[3]];
            }
            else
            {
                levelText.text = "No Ship Found";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        manageControls();
    }
}
