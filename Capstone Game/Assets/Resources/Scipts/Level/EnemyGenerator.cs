using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    private WeaponGenerator myWeaponGenerator;
    // Factions: Player, Bug
    // Stats: Health, Speed, Firerate, Range, Damage
    private float[,] minAndMaxStats = new float[,]{ { 25, 2, .5f, 5, 5 }, { 150, 5, 5, 15, 50} };
    private float[] statScorePerPoint = new float[5];
    // This stores how much each faction values each stat
    public float[,] factionStats = new float[,] { { 100,100,100,100,100}, { 0.5f, 3f, 1, .75f, 1}, { .75f, 1, .5f, 10, 1} };
    public float[,] specialFactionStats = new float[,] { { 1.5f, 3, 1.5f, 3, 1.5f} };
    private string enemyFolder = "Images/Enemies/";
    public string[] factionNames = new string[] { "Friendly", "Bugs", "Bots"};
    public string[] specialFactionNames = new string[] { "Cow"};
    private string[] imageNames = new string[] { "Health", "Speed", "Firerate", "Range", "Damage"};
    private Sprite[,] allFactionSprites = null;
    public Sprite[] factionSprites;

    public GameObject generateNewEnemy(Vector3 position, int points, int factionID, int specialId = -1)
    {
        if (allFactionSprites == null)
        {
            allFactionSprites = new Sprite[,] { { null, null, null, null, null }, 
                { Resources.Load<Sprite>(enemyFolder + factionNames[1] + "/" + imageNames[0]), Resources.Load<Sprite>(enemyFolder + factionNames[1] + "/" + imageNames[1]), Resources.Load<Sprite>(enemyFolder + factionNames[1] + "/" + imageNames[2]), Resources.Load<Sprite>(enemyFolder + factionNames[1] + "/" + imageNames[3]), Resources.Load<Sprite>(enemyFolder + factionNames[1] + "/" + imageNames[4])},
                { Resources.Load<Sprite>(enemyFolder + factionNames[2] + "/" + imageNames[0]), Resources.Load<Sprite>(enemyFolder + factionNames[2] + "/" + imageNames[1]), Resources.Load<Sprite>(enemyFolder + factionNames[2] + "/" + imageNames[2]), Resources.Load<Sprite>(enemyFolder + factionNames[2] + "/" + imageNames[3]), Resources.Load<Sprite>(enemyFolder + factionNames[2] + "/" + imageNames[4])}};
        }
        factionSprites = new Sprite[] { allFactionSprites[factionID, 0], allFactionSprites[factionID, 1] , allFactionSprites[factionID, 2] , allFactionSprites[factionID, 3], allFactionSprites[factionID, 4] };
        if (specialId != -1)
        {
            specialId -= 1;
            factionID = 1;
            factionSprites = new Sprite[] { Resources.Load<Sprite>(enemyFolder + specialFactionNames[0] + "/" + imageNames[0]), Resources.Load<Sprite>(enemyFolder + specialFactionNames[0] + "/" + imageNames[1]), Resources.Load<Sprite>(enemyFolder + specialFactionNames[0] + "/" + imageNames[2]), Resources.Load<Sprite>(enemyFolder + specialFactionNames[0] + "/" + imageNames[3]), Resources.Load<Sprite>(enemyFolder + specialFactionNames[0] + "/" + imageNames[4]) };
        }
        GameObject newEnemy = null;
        if (this.gameObject.GetComponent<WeaponGenerator>() == null)
        {
            myWeaponGenerator = this.gameObject.AddComponent<WeaponGenerator>();
        }
        else
        {
            myWeaponGenerator = this.gameObject.GetComponent<WeaponGenerator>();
        }
        if (enemyPrefab != null)
        {
            newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            EnemyController enemyStats = newEnemy.GetComponent<EnemyController>();
            // Health - Speed - Size
            // Firerate - Range - Damage
            float[] selectedFactionStats = new float[factionStats.GetLength(1)];
            for (int x = 0; x < selectedFactionStats.Length; x++)
            {
                if (specialId == -1)
                {
                    selectedFactionStats[x] = factionStats[factionID, x];
                }
                else
                {
                    Debug.Log("Special Id: " + specialId);
                    selectedFactionStats[x] = specialFactionStats[specialId, x];
                }
            }
            int pointsToSpend = points;
            int cost = 0;
            float[] stats = new float[minAndMaxStats.GetLength(1)];
            float minCost = 0;
            float maxCost = 0;
            int highestStatId = 0;
            int highestStatValue = -1;
            for (int x = 0; x < minAndMaxStats.GetLength(1); x++)
            {
                maxCost = Mathf.Min(10, pointsToSpend - (minAndMaxStats.GetLength(1) - (x + 1)));
                minCost = Mathf.Max(1, pointsToSpend - ((10 * (minAndMaxStats.GetLongLength(1) - x)) - 10));
                statScorePerPoint[x] = (minAndMaxStats[1, x] - minAndMaxStats[0, x]) / 10;
                cost = Mathf.RoundToInt(Random.Range(minCost, maxCost));
                if (highestStatValue < cost)
                {
                    highestStatValue = cost;
                    highestStatId = x;
                }
                pointsToSpend -= cost;
                stats[x] = minAndMaxStats[0, x] + ((statScorePerPoint[x] * cost) * selectedFactionStats[x]);
                //Debug.Log("Stat " + (x + 1) + ": " + minAndMaxStats[0, x] + " + " + (statScorePerPoint[x] * cost) + " * " + factionStats[factionID, x] + " = " + stats[x]);
            }
            enemyStats.health = stats[0];
            enemyStats.movementSpeed = stats[1];
            //enemyStats.fireRate = stats[2];
            enemyStats.range = stats[3];
            enemyStats.sightRadius = Mathf.Max(5, stats[3] + 2);
            enemyStats.mySize = Mathf.Max(Mathf.Min(1, (stats[0] / 25) / stats[1]), .5f);
            enemyStats.factionId = factionID;
            enemyStats.myImage = factionSprites[highestStatId];
            enemyStats.myWeapons = new GameObject("Gun").AddComponent<WeaponController>();
            enemyStats.myWeapons.gameObject.transform.parent = enemyStats.transform;
            enemyStats.myWeapons.gameObject.transform.localPosition = new Vector3(0, 0);
            enemyStats.myWeapons.heldWeapons = new Weapon[1] { myWeaponGenerator.generateEnemyWeapon(enemyStats.gameObject, stats[2], stats[3], stats[4])};
            if (enemyStats.myWeapons.heldWeapons[enemyStats.myWeapons.selectedWeapon].isMelee)
            {
                enemyStats.range = enemyStats.myWeapons.heldWeapons[enemyStats.myWeapons.selectedWeapon].AOERange;
            }
            if (enemyStats.range < 10)
            {
                enemyStats.range = 3;
            }
        }

        return newEnemy;
    }
}
