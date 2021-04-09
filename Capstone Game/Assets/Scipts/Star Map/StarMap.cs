using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarMap : MonoBehaviour
{
    public int levelWidth = 100;
    public int levelHeight = 25;
    public float levelSpacing = 5;
    Level[,] ShipMap;
    GameObject[,] ObjectMap;
    private int playerX = 0;
    private int playerY = 0;
    private int selectedX = 0;
    private int selectedY = 0;
    private int fuel = 20;
    private GameObject playerShip;
    public Sprite playerShupSprite;
    public Sprite[] derilects;
    private RectTransform[] UIObjects;
    private Canvas bottomBar;
    private Button travelButton;
    private Text levelText;
    public Sprite selectorSprite;
    private GameObject selector;
    private float shipChance = 30;
    private float[] timeLastMoved = { 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        ShipMap = new Level[levelWidth, levelHeight];
        ObjectMap = new GameObject[levelWidth, levelHeight];
        playerY = Mathf.RoundToInt(levelHeight / 2);
        selectedY = playerY;
        generateShipMap();
        playerShip = new GameObject("Player Ship");
        playerShip.transform.parent = this.transform;
        playerShip.AddComponent<SpriteRenderer>().sprite = playerShupSprite;
        playerShip.transform.localPosition = new Vector3((playerX * levelSpacing) - 1, (playerY * levelSpacing) + 1);
        playerShip.GetComponent<SpriteRenderer>().sortingOrder = 1;
        bottomBar = GameObject.FindObjectOfType<Canvas>();
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
        string displayString = travelButton.GetComponentInChildren<Text>().text = "Travel\nCost: " + Mathf.Max(Mathf.Abs(selectedX - playerX), Mathf.Abs(selectedY - playerY)) + " / " + fuel;
        selector.transform.position = Vector3.Lerp(selector.transform.position, new Vector3(selectedX * levelSpacing, selectedY * levelSpacing), Time.deltaTime * 5);
        if (playerShip.transform.localPosition != new Vector3((playerX * levelSpacing) - 1, (playerY * levelSpacing) + 1))
        {
            playerShip.transform.localPosition = Vector3.Lerp(playerShip.transform.localPosition, new Vector3((playerX * levelSpacing) - 1, (playerY * levelSpacing) + 1), Time.deltaTime * 5);
        }
        if (playerX == selectedX && playerY == selectedY)
        {
            if (playerX >= 0 && playerY >= 0 && playerX < levelWidth && playerY < levelHeight)
            {
                if (ShipMap[playerX, playerY] != null)
                {
                    if (ShipMap[playerX, playerY].isVisited == false)
                    {
                        displayString = "Board";
                        validButton = true;
                    }
                }
            }
        }
        else
        {
            if (fuel >= Mathf.Max(Mathf.Abs(selectedX - playerX), Mathf.Abs(selectedY - playerY)))
            {
                validButton = true;
            }

        }
        travelButton.GetComponentInChildren<Text>().text = displayString;
        travelButton.interactable = validButton;
    }

    public void Travel()
    {
        if (playerX == selectedX && playerY == selectedY)
        {
            if (playerX >= 0 && playerY >= 0 && playerX < levelWidth && playerY < levelHeight)
            {
                if (ShipMap[playerX, playerY] != null)
                {
                    if (ShipMap[playerX,playerY].isVisited == false)
                    {
                        ShipMap[playerX, playerY].isVisited = true;
                        ShipMap[playerX, playerY].loadLevel();
                    }
                }
            }
        }
        else
        {
            if (fuel >= Mathf.Max(Mathf.Abs(selectedX - playerX), Mathf.Abs(selectedY - playerY)))
            {
                fuel = fuel - Mathf.Max(Mathf.Abs(selectedX - playerX), Mathf.Abs(selectedY - playerY));
                playerX = selectedX;
                playerY = selectedY;
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
                selectedY = selectedY + 1;
                timeLastMoved[1] = Time.time;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                selectedY = selectedY - 1;
                timeLastMoved[1] = Time.time;
            }
        }
        if (Time.time - timeLastMoved[0] > .25f)
        {
            if (Input.GetKey(KeyCode.D))
            {
                selectedX = selectedX + 1;
                timeLastMoved[0] = Time.time;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                selectedX = selectedX - 1;
                timeLastMoved[0] = Time.time;
            }
        }
        handleTravelButton();
        if (selectedX < 0 || selectedY < 0 || selectedX >= levelWidth || selectedY >= levelHeight)
        {
            levelText.text = "Dead Space Detected";
        }
        else
        {
            if (ShipMap[selectedX, selectedY] != null)
            {
                levelText.text = "Ship Detected\nShip Visited: " + ShipMap[selectedX, selectedY].isVisited + "\nSize: " + Level.valueNames[0, ShipMap[selectedX, selectedY].levelValues[0]] + "\nLife sign Density: " + Level.valueNames[1, ShipMap[selectedX, selectedY].levelValues[1]] + "\nLife Sign Strength: " + Level.valueNames[2, ShipMap[selectedX, selectedY].levelValues[2]] + "\nLife Sign Type: " + Level.factionNames[ShipMap[selectedX, selectedY].levelValues[3]];
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
    
    private void generateShipMap()
    {
        for (int x = 0; x < ShipMap.GetLength(0); x++)
        {
            for (int y = 0; y < ShipMap.GetLength(1); y++)
            {
                if (x != playerX || y != playerY)
                {
                    if (Random.Range(0, 100) < shipChance)
                    {
                        ShipMap[x, y] = new Level().generateLevel(Mathf.RoundToInt(x / ((float)levelWidth / 12)));
                        //Debug.Log("A ship at (" + x + "," + y + ") was created with the stats\nSize: " + ShipMap[x, y].levelValues[0] + "\nDifficulty: " + ShipMap[x, y].levelValues[1] + "\nDensity: " + ShipMap[x, y].levelValues[2] + "\nFaction: " + ShipMap[x, y].levelValues[3]);
                        ObjectMap[x, y] = new GameObject();
                        ObjectMap[x, y].name = x + "," + y;
                        ObjectMap[x, y].transform.parent = this.transform;
                        ObjectMap[x, y].transform.localPosition = new Vector3(x * levelSpacing, y * levelSpacing);
                        ObjectMap[x, y].AddComponent<SpriteRenderer>().sprite = derilects[0];
                    }
                }
            }
        }
    }
}

public class Level
{
    // Size, Difficulty, Density, faction
    public int[] levelValues = new int[4];
    public GameObject displayObject;
    public bool isLevel = true;
    public static string[,] valueNames = { { "Tiny", "Small", "Medium", "Large", "Gargantuan" }, { "Barren", "Sparse", "Lightly Populated", "Populated", "Cramped" }, { "Fragile", "Weak", "Average", "Strong", "Fierce" }  };
    public static string[] factionNames = { "Friendlies", "Insectoid life"};
    public bool isVisited = false;

    public Level generateLevel(int levelScore)
    {
        levelScore = Mathf.Clamp(levelScore, 3, 12);
        float maxCost = 0;
        float minCost = 0;
        int cost = 0;
        for (int x = 0; x < levelValues.Length - 1; x++)
        {
            maxCost = Mathf.Min(4, levelScore - (levelValues.Length - (x + 2)));
            minCost = Mathf.Max(1, levelScore - ((4 * (levelValues.Length - (x + 1))) - 4));
            cost = Mathf.RoundToInt(Random.Range(minCost, maxCost));
            //Debug.Log("Stat: " + x + " | Points Left: " + levelScore + " | Min Points: " + minCost + "  | Max Points: " + maxCost + " | Cost: " + cost);
            levelScore -= cost;
            levelValues[x] = cost;
        }
        levelValues[3] = Mathf.RoundToInt(Random.Range(1, new EnemyGenerator().factionStats.GetLength(0)));

        return this;
    }

    public void loadLevel()
    {
        if (Application.CanStreamedLevelBeLoaded(2))
        {
            PlayerPrefs.SetInt("size", levelValues[0]);
            PlayerPrefs.SetInt("enemyDiffiulty", levelValues[1]);
            PlayerPrefs.SetInt("enemyDensity", levelValues[2]);
            PlayerPrefs.SetInt("faction", levelValues[3]);
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("Scene does not exsits");
        }
    }
}
