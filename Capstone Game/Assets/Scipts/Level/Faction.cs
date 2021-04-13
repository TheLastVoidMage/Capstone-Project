// Samuel Watson
// 3/16/21
// This script handles interactions between different factioned objects (Player, and enemies)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public int factionId = -1; // Determines faction. -1 is netrual
    public EnemyController ec = null;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponent<EnemyController>() != null)
        {
            ec = this.gameObject.GetComponent<EnemyController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doDamage(GameObject target, float damage, GameObject sender = null)
    {
        if (target.GetComponent<Faction>() != null)
        {
            if (target.GetComponent<EnemyController>() != null)
            {
                target.GetComponent<EnemyController>().takeDamage(damage, sender);
            }
            else if (target.GetComponent<playerController>() != null)
            {

            }
        }
    }
}
