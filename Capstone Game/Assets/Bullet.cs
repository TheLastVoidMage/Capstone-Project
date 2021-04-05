using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = gameObject;
        var faction = enemy.GetComponent<Faction>();
        //bullet animation
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //remove bullet after hit
        Destroy(gameObject);
        //removes hit effect after 5 seconds
        Destroy(effect, 5f);
        enemy = collision.gameObject;
        if (faction != null)
        {
            if (faction.factionId != 0)
            {
                faction.doDamage(enemy, damage);
            }
        }
    }

}
