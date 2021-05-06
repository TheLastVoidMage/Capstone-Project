using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public Level[,] shipMap;
    public int[] playerPos;
    public Weapon[] playerWeapons;
    public int fuel;
    public int characterId;
}
