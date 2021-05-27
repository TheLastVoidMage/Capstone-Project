using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelGenerator : MonoBehaviour
{
    // Sizes: Tiny, Small, Medium, Large, Gargantuan
    // How many room models across and tall a map could be
    private int[,] levelSizes = new int[,] { { 5, 7, 9, 11, 15 }, { 3, 5, 7, 7, 9} };
    public int selectedLevelSize = 0;
    // Enemy Desity: Barren, Sparse, Populated, Cramped
    // How many enemies there are before levelSize is taken into account
    private int[] enemyDensity = new int[] { 3, 5, 9, 15, 25};
    public int selectedLevelDensity = 0;
    // Enemy Difficulty: Fragile, Weak, Average, Strong, Fierce
    // How many points each enemy feads into the enemy generator
    private int[] enemyDifficulty = new int[] { 8, 13, 18, 27, 35};
    public int selectedEnemyDifficulty = 0;
    public int faction = 1;
    private Room[,] levelMap;
    private EnemyGenerator myEnemyGenerator;
    private WeaponGenerator myWeaponGenerator;
    public Sprite[] regularFloors;
    public Sprite[] regularWalls;
    public Sprite[] specialFloors;
    public Sprite[] specialWalls;
    // Floor, Wall, Door closed, Door Open
    public Sprite[] textures;
    public int specialId = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (selectedLevelSize == -1)
        {
            selectedLevelSize = PlayerPrefs.GetInt("size");
        }
        if (selectedEnemyDifficulty == -1)
        {
            selectedEnemyDifficulty = PlayerPrefs.GetInt("enemyDiffiulty");
        }
        if (selectedLevelDensity == -1)
        {
            selectedLevelDensity = PlayerPrefs.GetInt("enemyDensity");
        }
        if (faction == -1)
        {
            faction = PlayerPrefs.GetInt("faction");
        }
        if (specialId == 0)
        {
            specialId = -1;
        }
        if (specialId == -1)
        {
            specialId = PlayerPrefs.GetInt("specialId");
        }
        regularFloors = Resources.LoadAll<Sprite>("Images/LevelSprites/RegularFloor/");
        regularWalls = Resources.LoadAll<Sprite>("Images/LevelSprites/RegularWall/");
        myEnemyGenerator = this.GetComponent<EnemyGenerator>();
        myWeaponGenerator = this.gameObject.AddComponent<WeaponGenerator>();
        generateMap(selectedLevelSize);
        generateEnemies(selectedLevelDensity, selectedEnemyDifficulty, faction);
        generatePickUps(selectedLevelDensity + selectedEnemyDifficulty + 3);
        AstarPath.active.Scan();

    }
    private void generateEnemies(int density, int difficulty, int faction)
    {
        int enemiesWanted = enemyDensity[density] + Random.Range(0, 2);
        int enemiesGenerated = 0;
        int x = 0;
        int y = 0;
        int iterations = 0;
        Vector3 enemyPos = new Vector3(1, 1);
        if (levelMap != null)
        {
            while (enemiesGenerated < enemiesWanted && iterations < 1000)
            {
                x = Random.Range(0, levelMap.GetLength(0));
                y = Random.Range(0, levelMap.GetLength(1));
                if (levelMap[x, y] != null)
                {
                    if (x != levelMap.GetLength(0) / 2 && y != 0) // If not spawn
                    {
                        enemyPos = new Vector3(this.transform.position.x + (x * 14) + 6 + Random.Range(-4, 4), this.transform.position.y + (y * 14) + 6 + Random.Range(-4, 4));
                        myEnemyGenerator.generateNewEnemy(enemyPos, enemyDifficulty[difficulty], faction, specialId);
                        enemiesGenerated++;
                    }
                }
                iterations++;
            }
        }
    }
    private void generatePickUps(int maxCrates)
    {
        int x = 0;
        int y = 0;
        bool[,] used = new bool[levelMap.GetLength(0), levelMap.GetLength(1)];
        int spawnedCrates = 0;
        int iterations = 0;
        int maxIterations = 1000;
        Vector3 spawnLocation = new Vector3(0, 0);
        if (levelMap != null)
        {
            while (spawnedCrates < maxCrates && iterations < maxIterations)
            {
                x = Random.Range(0, levelMap.GetLength(0));
                y = Random.Range(0, levelMap.GetLength(1));
                if (levelMap[x, y] != null)
                {
                    if (x != levelMap.GetLength(0) / 2 && y != 0) // If not spawn
                    {
                        if (used[x, y] == false)
                        {
                            used[x, y] = true;
                            spawnLocation = new Vector3(this.transform.position.x + (x * 14) + 6, this.transform.position.y + (y * 14) + 6);
                            GameObject newCrate = new GameObject("Crate");
                            newCrate.transform.position = spawnLocation;
                            int id = 0;
                            int chance = Random.Range(0, 99);
                            if (chance < 40)
                            {
                                id = 0;
                            }
                            else if (chance < 80)
                            {
                                id = 1;
                            }
                            else
                            {
                                id = 2;
                            }
                            newCrate.AddComponent<Pickup>().generateCrate(id, myWeaponGenerator.generatePlayerWeapon());

                            spawnedCrates++;
                        }
                    }
                }
                iterations++;
            }
        }
    }
    private void generateMap(int size)
    {
        textures = new Sprite[4];
        if (specialId == -1)
        {
            textures[0] = regularFloors[Random.Range(0, regularFloors.Length - 1)];
            textures[1] = regularWalls[Random.Range(0, regularWalls.Length - 1)];
        }
        else
        {
            textures[0] = specialFloors[specialId];
            textures[1] = specialWalls[specialId];
        }
        int iterations = 0;
        levelMap = new Room[levelSizes[0, size], levelSizes[1, size]];
        Debug.Log("The level is " + levelMap.GetLength(0) + " X " + levelMap.GetLength(1));
        this.gameObject.transform.position = new Vector3((-(levelMap.GetLength(0) * 14) / 2) + 1, -6);
        levelMap[levelMap.GetLength(0) / 2, 0] = new Room();
        levelMap[levelMap.GetLength(0) / 2, 0].makeRoom(this.gameObject, "Spawn", new Vector3(levelMap.GetLength(0) / 2, 0), textures, new int[4] { 0, 0, 3, 0 }, true);
        while (checkValidMap() == false && iterations < 1000)
        {
            for (int xpos = 0; xpos < levelMap.GetLength(0); xpos++)
            {
                for (int ypos = 0; ypos < levelMap.GetLength(1); ypos++)
                {
                    if (levelMap[xpos, ypos] == null)
                    {
                        levelMap[xpos, ypos] = addRoom(xpos, ypos);
                    }
                }
            }
            iterations++;
            if (iterations == 1000)
            {
                Debug.Log("Level Generator Broke out");
            }
        }
    }
    private int[] setDoors(int x, int y)
    {
        int[] doors = new int[4] { 0, 0, 0, 0 };
        int tempNumber = -2;
        // Left
        if (x > 0)
        {
            if (levelMap[x - 1, y] != null)
            {
                doors[3] = levelMap[x - 1, y].doorSizes[1];
            }
            else
            {
                tempNumber = -2;
                while (tempNumber % 2 != 1 && tempNumber != 0)
                {
                    tempNumber = Random.Range(0, 6);
                }
                doors[3] = tempNumber;
            }
        }
        else
        {
            doors[3] = 0;
        }
        // Right
        if (x < levelMap.GetLength(0) - 1)
        {
            if (levelMap[x + 1, y] != null)
            {
                doors[1] = levelMap[x + 1, y].doorSizes[3];
            }
            else
            {
                tempNumber = -2;
                while (tempNumber % 2 != 1 && tempNumber != 0)
                {
                    tempNumber = Random.Range(0, 6);
                }
                doors[1] = tempNumber;
            }
        }
        else
        {
            doors[1] = 0;
        }
        // Down
        if (y > 0)
        {
            if (levelMap[x, y - 1] != null)
            {
                doors[0] = levelMap[x, y - 1].doorSizes[2];
            }
            else
            {
                tempNumber = -2;
                while (tempNumber % 2 != 1 && tempNumber != 0)
                {
                    tempNumber = Random.Range(0, 6);
                }
                doors[0] = tempNumber;
            }
        }
        else
        {
            doors[0] = 0;
        }
        // Up
        if (y < levelMap.GetLength(1) - 1)
        {
            if (levelMap[x, y + 1] != null)
            {
                doors[2] = levelMap[x, y + 1].doorSizes[0];
            }
            else
            {
                tempNumber = -2;
                while (tempNumber % 2 != 1 && tempNumber != 0)
                {
                    tempNumber = Random.Range(0, 6);
                }
                doors[2] = tempNumber;
            }
        }
        else
        {
            doors[2] = 0;
        }



        return doors;
    }
    private Room addRoom(int x, int y)
    {
        Room newRoom = null;
        if (checkValidRoomPlacement(x, y))
        {
            newRoom = new Room();
            newRoom.makeRoom(this.gameObject, x + "," + y, new Vector3(x, y), textures, setDoors(x, y));
        }
        return newRoom;
    }
    private bool checkValidRoomPlacement(int x, int y)
    {
        bool isValid = false;
        if (x > 0)
        {
            if (levelMap[x - 1, y] != null)
            {
                if (levelMap[x - 1, y].doorSizes[1] != 0)
                {
                    isValid = true;
                }
            }
        }
        if (x < levelMap.GetLength(0) - 1)
        {
            if (levelMap[x + 1, y] != null)
            {
                if (levelMap[x + 1, y].doorSizes[3] != 0)
                {
                    isValid = true;
                }
            }
        }
        if (y > 0)
        {
            if (levelMap[x, y - 1] != null)
            {
                if (levelMap[x, y - 1].doorSizes[2] != 0)
                {
                    isValid = true;
                }
            }
        }
        if (y < levelMap.GetLength(1) - 1)
        {
            if (levelMap[x, y + 1] != null)
            {
                if (levelMap[x, y + 1].doorSizes[0] != 0)
                {
                    isValid = true;
                }
            }
        }
        return isValid;
    }
    private bool checkValidMap()
    {
        bool isValid = true;
        for (int x = 0; x < levelMap.GetLength(0); x++)
        {
            for (int y = 0; y < levelMap.GetLength(1); y++)
            {
                if (levelMap[x,y] == null)
                {
                    isValid = false;
                }
            }
        }
        return isValid;
    }

}

class Room
{
    public Animator leaveLevelAnimation;
    private GameObject room;
    private GameObject[,] roomMap = new GameObject[7, 7];
    public float size = 2;
    public int[] doorSizes = new int[4] { 0, 0, 0, 0 };
    public void makeRoom(GameObject parent, string name, Vector3 position, Sprite[] sprites, int[] doors, bool hasExit = false)
    {
        int[,] roomPlan = new int[7, 7];
        room = new GameObject();
        room.transform.parent = parent.transform;
        room.transform.localPosition = position * (size * roomMap.GetLength(1));
        room.name = name;
        int[] tempWall = new int[7];

        tempWall = addDoor(doors[0]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[x, 0] = tempWall[x];
            if (tempWall[x] != 1)
            {
                doorSizes[0]++;
            }
        }
        tempWall = addDoor(doors[1]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[roomPlan.GetLength(1) - 1, x] = tempWall[x];
            if (tempWall[x] != 1)
            {
                doorSizes[1]++;
            }
        }
        tempWall = addDoor(doors[2]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[x, roomPlan.GetLength(1) - 1] = tempWall[x];
            if (tempWall[x] != 1)
            {
                doorSizes[2]++;
            }
        }
        tempWall = addDoor(doors[3]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[0, x] = tempWall[x];
            if (tempWall[x] != 1)
            {
                doorSizes[3]++;
            }
        }
        

        for (int x = 0; x < roomMap.GetLength(0); x++)
        {
            for (int y = 0; y < roomMap.GetLength(0); y++)
            {
                roomMap[x, y] = new GameObject();
                roomMap[x, y].transform.parent = room.transform;
                roomMap[x, y].name = x + ":" + y;
                roomMap[x, y].transform.localScale = new Vector3(size, size);
                roomMap[x, y].transform.localPosition = new Vector3(x * size, y * size);
                SpriteRenderer mySprite = roomMap[x, y].AddComponent<SpriteRenderer>();
                mySprite.sprite = sprites[roomPlan[x, y]];
                mySprite.sortingOrder = -10;
                if (roomPlan[x, y] == 1)
                {
                    roomMap[x, y].AddComponent<BoxCollider2D>();
                    if (hasExit == false || y > 0 || x < 2 || x > 4)
                    {
                        GameObject shadowObject = new GameObject("ShadowObject");
                        shadowObject.transform.parent = roomMap[x, y].transform;
                        shadowObject.transform.localPosition = new Vector3();
                        shadowObject.AddComponent<ShadowCaster2D>();
                        shadowObject.GetComponent<ShadowCaster2D>().useRendererSilhouette = false;
                        shadowObject.GetComponent<ShadowCaster2D>().selfShadows = true;
                        shadowObject.transform.localScale = new Vector3(1, 1, 1);
                        if (x < roomPlan.GetLength(0) / 2)
                        {
                            shadowObject.transform.localPosition += new Vector3(-.2f, 0);
                        }
                        else if (x > roomPlan.GetLength(0) / 2)
                        {
                            shadowObject.transform.localPosition += new Vector3(.2f, 0);
                        }
                        if (y < roomPlan.GetLength(1) / 2)
                        {
                            shadowObject.transform.localPosition += new Vector3(0, -.2f);
                        }
                        else if (y > roomPlan.GetLength(1) / 2)
                        {
                            shadowObject.transform.localPosition += new Vector3(0, .2f);
                        }
                        if (x == roomPlan.GetLength(0) / 2)
                        {
                            shadowObject.transform.localScale += new Vector3(.4f, 0);
                        }
                        if (y == roomPlan.GetLength(1) / 2)
                        {
                            shadowObject.transform.localScale += new Vector3(0, .4f);
                        }
                    }
                    roomMap[x, y].layer = 6;
                }
            }
        }
        if (hasExit)
        {
            GameObject exit = GameObject.FindObjectOfType<AirlockController>().gameObject;
            exit.transform.parent = room.transform;
            exit.name = "Exit";
            exit.transform.localPosition = new Vector3(size, size);
            exit.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LevelSprites/Other/ControlPanel");
            exit.AddComponent<BoxCollider2D>();
            roomMap[2,0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LevelSprites/Other/ControlPanel");
            roomMap[2, 0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LevelSprites/Other/AirlockLeft");
            roomMap[3, 0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LevelSprites/Other/AirlockMiddle");
            roomMap[4, 0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LevelSprites/Other/AirlockRight");
        }
    }

    int[] addDoor(int doorSize)
    {
        int[] wall = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        for (int y = 0; y < wall.Length; y++)
        {
            if (y < (wall.Length / 2) - (doorSize / 2) || y > (wall.Length / 2) + ((doorSize - .5) / 2))
            {
                 wall[y] = 1;
            }
            else
            {
                 wall[y] = 0;
            }
        }
        return wall;
    }
}