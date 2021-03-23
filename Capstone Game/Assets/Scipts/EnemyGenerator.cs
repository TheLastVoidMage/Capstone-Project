using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Factions: Player, Bug
    // Stats: Health, Speed, Firerate, Range, Damage
    private float[,] minAndMaxStats = new float[,]{ { 10, 2, .5f, 1, 5 }, { 300, 5, 5, 5, 90} };
    private float[] statScorePerPoint = new float[5];
    // This stores how much each faction values each stat
    private float[,] factionStats = new float[,] { { 100,100,100,100,100}, { 0.5f, 1.5f, 1, .1f, 1}};
    public GameObject generateNewEnemy(Vector3 position, int points, int factionID)
    {
        GameObject newEnemy = null;
        if (enemyPrefab != null)
        {
            newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            EnemyController enemyStats = newEnemy.GetComponent<EnemyController>();
            // Health - Speed - Size
            // Firerate - Range - Damage
            int pointsToSpend = points;
            int cost = 0;
            float[] stats = new float[minAndMaxStats.GetLength(1)];
            float minCost = 0;
            float maxCost = 0;
            for (int x = 0; x < minAndMaxStats.GetLength(1); x++)
            {
                maxCost = Mathf.Min(10, pointsToSpend - (minAndMaxStats.GetLength(1) - (x + 1)));
                minCost = Mathf.Max(0, pointsToSpend - ((10 * (minAndMaxStats.GetLongLength(1) - x)) - 10));
                statScorePerPoint[x] = (minAndMaxStats[1, x] - minAndMaxStats[0, x]) / 10;
                cost = Mathf.RoundToInt(Random.Range(minCost, maxCost));
                pointsToSpend -= cost;
                stats[x] = minAndMaxStats[0, x] + ((statScorePerPoint[x] * cost) * factionStats[factionID, x]);
            }
            enemyStats.health = stats[0];
            enemyStats.movementSpeed = stats[1];
            enemyStats.fireRate = stats[2];
            enemyStats.range = stats[3];
            enemyStats.sightRadius = stats[3] + 2;
            enemyStats.damage = stats[4];
        }

        return newEnemy;
    }
}
