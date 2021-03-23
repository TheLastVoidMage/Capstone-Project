using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private EnemyGenerator myEnemyGenerator;
    // Start is called before the first frame update
    void Start()
    {
        myEnemyGenerator = this.GetComponent<EnemyGenerator>();
        myEnemyGenerator.generateNewEnemy(new Vector3(10, 10), 15, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
