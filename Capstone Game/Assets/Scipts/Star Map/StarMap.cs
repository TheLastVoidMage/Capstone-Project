using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarMap : MonoBehaviour
{
    public int levelWidth = 100;
    public int levelHeight = 25;
    public float levelSpacing = 5;
    public Level[,] ShipMap;
    GameObject[,] ObjectMap;
    public Sprite[] derilects;
    public Sprite[] specialDerilects;

    // Level Chances - Regular, Cow
    private float[] shipChances = { 30, 1};

    // Start is called before the first frame update
    void Start()
    {
        ShipMap = new Level[levelWidth, levelHeight];
        ObjectMap = new GameObject[levelWidth, levelHeight];
        generateShipMap();
    }

    private void generateShipMap()
    {
        for (int x = 0; x < ShipMap.GetLength(0); x++)
        {
            for (int y = 0; y < ShipMap.GetLength(1); y++)
            {
                if (x != 0 || y != Mathf.RoundToInt(levelHeight / 2))
                {
                    if (Random.Range(0, 100) < shipChances[0])
                    {
                        ShipMap[x, y] = new Level().generateLevel(Mathf.RoundToInt(x / ((float)levelWidth / 12)));
                        //Debug.Log("A ship at (" + x + "," + y + ") was created with the stats\nSize: " + ShipMap[x, y].levelValues[0] + "\nDifficulty: " + ShipMap[x, y].levelValues[1] + "\nDensity: " + ShipMap[x, y].levelValues[2] + "\nFaction: " + ShipMap[x, y].levelValues[3]);
                        ObjectMap[x, y] = new GameObject();
                        ObjectMap[x, y].name = x + "," + y;
                        ObjectMap[x, y].transform.parent = this.transform;
                        ObjectMap[x, y].transform.localPosition = new Vector3(x * levelSpacing, y * levelSpacing);
                        ObjectMap[x, y].AddComponent<SpriteRenderer>().sprite = derilects[Mathf.RoundToInt(Random.Range(0, derilects.Length))];
                    }
                    else if (Random.Range(0, 100) < shipChances[1])
                    {
                        ShipMap[x, y] = new Level().generateLevel(Mathf.RoundToInt(x / ((float)levelWidth / 12)), 1);
                        //Debug.Log("A ship at (" + x + "," + y + ") was created with the stats\nSize: " + ShipMap[x, y].levelValues[0] + "\nDifficulty: " + ShipMap[x, y].levelValues[1] + "\nDensity: " + ShipMap[x, y].levelValues[2] + "\nFaction: " + ShipMap[x, y].levelValues[3]);
                        ObjectMap[x, y] = new GameObject();
                        ObjectMap[x, y].name = x + "," + y;
                        ObjectMap[x, y].transform.parent = this.transform;
                        ObjectMap[x, y].transform.localPosition = new Vector3(x * levelSpacing, y * levelSpacing);
                        ObjectMap[x, y].AddComponent<SpriteRenderer>().sprite = specialDerilects[0];
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
    public static string[] factionNames = { "Friendlies", "Insectoid life", "Pirates"};
    public bool isVisited = false;
    private int specialId = 0;

    public Level generateLevel(int levelScore, int specialId = -1)
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
        this.specialId = specialId;
        if (this.specialId == -1)
        {
            levelValues[3] = Mathf.RoundToInt(Random.Range(1, new EnemyGenerator().factionStats.GetLength(0)));
        }
        else
        {
            // Will be replaced later
            levelValues[3] = 0;
        }

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
            PlayerPrefs.SetInt("specialId", specialId);
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("Scene does not exsits");
        }
    }
}
