using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StarMap : MonoBehaviour
{
    public int levelWidth = 100;
    public int levelHeight = 25;
    public float levelSpacing = 5;
    public Level[,] ShipMap;
    GameObject[,] ObjectMap;
    public Sprite[] derilects;
    public Sprite[] specialDerilects;
    private ShipController myShipController;

    // Level Chances - Regular, Cow
    private float[] shipChances = { 30, .5f };

    // Start is called before the first frame update
    void Start()
    {
        myShipController = this.gameObject.GetComponent<ShipController>();
        ShipMap = new Level[levelWidth, levelHeight];
        ObjectMap = new GameObject[levelWidth, levelHeight];
        generateShipMap();
    }

    private Save CreateSaveGameObject(int cId)
    {
        Save save = new Save();
        if (cId >= 0) // New Save
        {
            save = new Save();

            save.fuel = myShipController.fuel;
            save.shipMap = this.ShipMap;
            save.playerPos = myShipController.playerCoordinates;
            save.characterId = cId;
            save.playerWeapons = new Weapon[4] { new WeaponGenerator().generateStartingWeapon(), null, null, null};
            save.maxHealth = 150;
        }
        else // Edit Save
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.save", FileMode.Open);
            save = (Save)bf.Deserialize(file);
            file.Close();
            save.fuel = myShipController.fuel;
            save.shipMap = this.ShipMap;
            save.playerPos = myShipController.playerCoordinates;
        }
        

        return save;
    }

    public void SaveGame(int characterId = -1)
    {
        Save save = CreateSaveGameObject(characterId);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.save");
        bf.Serialize(file, save);
        file.Close();

        //Debug.Log("Save Game Method: X: " + myShipController.playerCoordinates[0] + "\nY: " + myShipController.playerCoordinates[1]);
        Debug.Log("Game Saved");
    }

    public bool LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.save") && PlayerPrefs.GetInt("newGame") == 0)
        {
            Debug.Log("File found");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            myShipController.fuel = save.fuel;
            this.ShipMap = save.shipMap;
            myShipController.onLoadSetPositions(save.playerPos);
            //Debug.Log("Load Game Method: X: " + myShipController.playerCoordinates[0] + "\nY: " + myShipController.playerCoordinates[1]);
            return true;
        }
        return false;
    }

    private void generateShipMap()
    {
        if (LoadGame() == false)
        {
            for (int x = 0; x < ShipMap.GetLength(0); x++)
            {
                for (int y = 0; y < ShipMap.GetLength(1); y++)
                {
                    if (x != 0 || y != Mathf.RoundToInt(levelHeight / 2))
                    {
                        if (Random.Range(0, 100) < shipChances[0])
                        {
                            ShipMap[x, y] = new Level().generateLevel(Mathf.RoundToInt(x / ((float)levelWidth / 12)), x, y, Mathf.RoundToInt(Random.Range(0, derilects.Length)));
                            //Debug.Log("A ship at (" + x + "," + y + ") was created with the stats\nSize: " + ShipMap[x, y].levelValues[0] + "\nDifficulty: " + ShipMap[x, y].levelValues[1] + "\nDensity: " + ShipMap[x, y].levelValues[2] + "\nFaction: " + ShipMap[x, y].levelValues[3]);
                            ObjectMap[x, y] = new GameObject();
                            ObjectMap[x, y].name = x + "," + y;
                            ObjectMap[x, y].transform.parent = this.transform;
                            ObjectMap[x, y].transform.localPosition = new Vector3(x * levelSpacing, y * levelSpacing);
                            ObjectMap[x, y].AddComponent<SpriteRenderer>().sprite = derilects[Mathf.RoundToInt(Random.Range(0, derilects.Length))];
                        }
                        else if (Random.Range(0, 100) < shipChances[1])
                        {
                            ShipMap[x, y] = new Level().generateLevel(Mathf.RoundToInt(x / ((float)levelWidth / 12)), x, y, Mathf.RoundToInt(Random.Range(0, derilects.Length)), 1);
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
            ShipMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2] = null;
            if (ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2] == null)
            {
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2] = new GameObject();
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2].name = "Gate";
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2].transform.parent = this.transform;
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2].transform.localPosition = new Vector3((ShipMap.GetLength(0) - 1) * levelSpacing, (ShipMap.GetLength(1) / 2) * levelSpacing);
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ships/Gate");
            }
            else
            {
                ObjectMap[ShipMap.GetLength(0) - 1, ShipMap.GetLength(1) / 2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ships/Gate");
            }
            SaveGame(Random.Range(0, Resources.LoadAll<Sprite>("Images/Player/").Length - 1));
        }
        else
        {
            for (int x = 0; x < ShipMap.GetLength(0); x++)
            {
                for (int y = 0; y < ShipMap.GetLength(1); y++)
                {
                    if (x != 0 || y != Mathf.RoundToInt(levelHeight / 2))
                    {
                        if (ShipMap[x, y] != null)
                        {
                            ObjectMap[x, y] = new GameObject();
                            ObjectMap[x, y].name = x + "," + y;
                            ObjectMap[x, y].transform.parent = this.transform;
                            ObjectMap[x, y].transform.localPosition = new Vector3(x * levelSpacing, y * levelSpacing);
                            ObjectMap[x, y].AddComponent<SpriteRenderer>().sprite = derilects[ShipMap[x, y].imageId];
                            if (ShipMap[x, y].specialId > 0)
                            {
                                ObjectMap[x, y].GetComponent<SpriteRenderer>().sprite = specialDerilects[ShipMap[x, y].specialId - 1];
                            }
                        }
                    }
                }
            }
        }
    }

}

[System.Serializable]
public class Level
{
    // Size, Difficulty, Density, faction
    public int[] levelValues = new int[4];
    public bool isLevel = true;
    public static string[,] valueNames = { { "Tiny", "Small", "Medium", "Large", "Gargantuan" }, { "Barren", "Sparse", "Lightly Populated", "Populated", "Cramped" }, { "Fragile", "Weak", "Average", "Strong", "Fierce" } };
    public static string[] factionNames = { "Unknown", "Insectoid life", "Cybernetic life", "Single cellular life" };
    public bool isVisited = false;
    public int specialId = 0;
    public int[] coordinates = new int[2];
    public int imageId = 0;

    public Level generateLevel(int levelScore, int xPos, int yPos, int imageId, int specialId = -1)
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
            levelValues[3] = Random.Range(1, new EnemyGenerator().factionNames.GetLength(0));
        }
        else
        {
            // Will be replaced later
            levelValues[3] = 0;
        }
        coordinates = new int[2]{ xPos, yPos};
        this.imageId = imageId;
        return this;
    }

    public void loadLevel()
    {
        if (Application.CanStreamedLevelBeLoaded(3))
        {
            PlayerPrefs.SetInt("size", levelValues[0]);
            PlayerPrefs.SetInt("enemyDiffiulty", levelValues[1]);
            PlayerPrefs.SetInt("enemyDensity", levelValues[2]);
            PlayerPrefs.SetInt("faction", levelValues[3]);
            PlayerPrefs.SetInt("specialId", specialId);
            GameObject.FindObjectOfType<LevelTransition>().LoadLevel(3);
        }
        else
        {
            Debug.Log("Scene does not exsits");
        }
    }
}
