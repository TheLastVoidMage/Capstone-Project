using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarMap : MonoBehaviour
{
    public float levelWidth = 10;
    public float levelHeight = 5;
    Level[,] ShipMap = new Level[5, 10];
    private int playerX = 0;
    private int playerY = 0;
    private GameObject playerShip;
    public Sprite playerShupSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerY = Mathf.RoundToInt(levelHeight / 2);
        generateShipMap();
        playerShip = new GameObject("Player Ship");
        playerShip.transform.parent = this.transform;
        playerShip.AddComponent<SpriteRenderer>().sprite = playerShupSprite;
        playerShip.transform.localPosition = new Vector3(playerX, playerY);
        Camera.main.transform.parent = playerShip.transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void generateShipMap()
    {
        for (int x = 0; x < ShipMap.GetLength(0); x++)
        {
            for (int y = 0; y < ShipMap.GetLength(1); y++)
            {
                if (x != playerX && y != playerY)
                {
                    ShipMap[x, y] = new Level().generateLevel(y);
                    Debug.Log("A ship at (" + x + "," + y + ") was created with the stats\nSize: " + ShipMap[x, y].levelValues[0] + "\nDifficulty: " + ShipMap[x, y].levelValues[1] + "\nDensity: " + ShipMap[x, y].levelValues[2] + "\nFaction: " + ShipMap[x, y].levelValues[3]);
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

    public Level generateLevel(int levelScore)
    {
        levelScore = Mathf.Max(levelScore, 3);
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
