using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Sizes: Tiny, Small, Medium, Large, Gargantuan
    // How many room models across and tall a map could be
    private int[,] levelSizes = new int[,] { { 5, 7, 9, 11, 15 }, { 3, 5, 7, 7, 9} };
    // Enemy Desity: Barren, Sparse, Populated, Cramped
    // How many enemies there are before levelSize is taken into account
    private int[] enemyDensity = new int[] { 3, 5, 7, 9, 13};
    // Enemy Difficulty: Fragile, Weak, Average, Strong, Fierce
    // How many points each enemy feads into the enemy generator
    private int[] enemyDifficulty = new int[] { 8, 13, 18, 27, 35};
    private int faction = 1;
    private Room[,] levelMap;
    private EnemyGenerator myEnemyGenerator;
    public Sprite wallTexture;
    public Sprite floorTexture;
    private Sprite[] textures;
    // Start is called before the first frame update
    void Start()
    {
        myEnemyGenerator = this.GetComponent<EnemyGenerator>();
        textures = new Sprite[] { floorTexture, wallTexture };
        generateMap(4);
    }

    private void generateMap(int size)
    {
        int iterations = 0;
        levelMap = new Room[levelSizes[0, size], levelSizes[1, size]];
        Debug.Log("The level is " + levelMap.GetLength(0) + " X " + levelMap.GetLength(1));
        levelMap[levelMap.GetLength(0) / 2, 0] = new Room();
        levelMap[levelMap.GetLength(0) / 2, 0].makeRoom(this.gameObject, "Spawn", new Vector3(levelMap.GetLength(0) / 2, 0), textures, new int[4] { 0, 0, 1, 0 });
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
                Debug.Log("Broke out");
            }
        }
    }
    private int[] setDoors(int x, int y)
    {
        // Sometimes left and right doors don't generate rooms
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
    private GameObject room;
    private GameObject[,] roomMap = new GameObject[7, 7];
    public float size = 2;
    public int[] doorSizes = new int[4] { 0, 0, 0, 0 };
    public void makeRoom(GameObject parent, string name, Vector3 position, Sprite[] sprites, int[] doors)
    {
        int[,] roomPlan = new int[7, 7];
        room = new GameObject();
        room.transform.parent = parent.transform;
        room.transform.position = position * (size * roomMap.GetLength(1));
        room.name = name;
        int[] tempWall = new int[7];
        tempWall = addDoor(doors[0]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[x, 0] = tempWall[x];
            if (tempWall[x] == 0)
            {
                doorSizes[0]++;
            }
        }
        tempWall = addDoor(doors[1]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[roomPlan.GetLength(1) - 1, x] = tempWall[x];
            if (tempWall[x] == 0)
            {
                doorSizes[1]++;
            }
        }
        tempWall = addDoor(doors[2]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[x, roomPlan.GetLength(1) - 1] = tempWall[x];
            if (tempWall[x] == 0)
            {
                doorSizes[2]++;
            }
        }
        tempWall = addDoor(doors[3]);
        for (int x = 0; x < tempWall.Length; x++)
        {
            roomPlan[0, x] = tempWall[x];
            if (tempWall[x] == 0)
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
                mySprite.sortingOrder = -1;
                if (roomPlan[x, y] == 1)
                {
                    roomMap[x, y].AddComponent<BoxCollider2D>();
                }
            }
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