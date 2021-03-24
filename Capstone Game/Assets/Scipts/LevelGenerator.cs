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
    // Start is called before the first frame update
    void Start()
    {
        myEnemyGenerator = this.GetComponent<EnemyGenerator>();
        //myEnemyGenerator.generateNewEnemy(new Vector3(10, 10), 15, 1);
        generateMap(0);
    }

    private void generateMap(int size)
    {
        levelMap = new Room[levelSizes[0,size],levelSizes[1,size]];
        Debug.Log("The level is " + levelMap.GetLength(0) + " X " + levelMap.GetLength(1));
        levelMap[levelMap.GetLength(0) / 2, 0] = new Room();
        levelMap[levelMap.GetLength(0) / 2, 0].createRoom("Spawn", 0, new Vector3(levelMap.GetLength(0) / 2, 0), 0, new Sprite[] { floorTexture, wallTexture });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class Room
{
    private GameObject room;
    private GameObject[,] roomMap = new GameObject[7, 7];
    public float size = 2;
    private int[,,] roomTemplates = new int[3,7,7]{ 
    { { 1,1,1,1,1,1,1}, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,0 }, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1 }, { 1,1,1,1,1,1,1 } },
    { { 1,1,1,1,1,1,1}, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1}, { 0,0,0,0,0,0,0 }, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1 }, { 1,1,1,1,1,1,1} },
    { { 1,1,1,1,1,1,1}, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,0 }, { 1,0,0,0,0,0,1}, { 1,0,0,0,0,0,1 }, { 1,1,1,0,1,1,1} }
    };
    public void createRoom(string name, int roomId, Vector3 position, float roomRotation, Sprite[] sprites)
    {
        room = new GameObject();
        room.transform.position = position * (size * roomTemplates.GetLength(1));
        room.transform.RotateAround(new Vector3(room.transform.position.x + (3 * size), room.transform.position.y + (3 * size)), Vector3.forward, roomRotation);
        room.name = name;

        for (int x = 0; x < roomMap.GetLength(1); x++)
        {
            for (int y = 0; y < roomMap.GetLength(1); y++)
            {
                roomMap[x, y] = new GameObject();
                roomMap[x, y].transform.parent = room.transform;
                roomMap[x, y].name = x + ":" + y;
                roomMap[x, y].transform.localScale = new Vector3(size, size);
                roomMap[x, y].transform.localPosition = new Vector3(x * size, y * size);
                SpriteRenderer mySprite = roomMap[x, y].AddComponent<SpriteRenderer>();
                mySprite.sprite = sprites[roomTemplates[roomId, x, y]];
                mySprite.sortingOrder = -1;
                if (roomTemplates[roomId, x, y] == 1)
                {
                    roomMap[x, y].AddComponent<BoxCollider2D>();
                }
            }
        }
    }

}