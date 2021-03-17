using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public GameObject target = null;
    private CircleCollider2D mySight;
    private Faction myFaction;
    private AIDestinationSetter myPathSetter;
    private AIPath myAiPath;


    // Variables that will be assigned by level generator
    public float sightRadius = 5;
    public float movementSpeed = 1;
    public float health = 100;
    public float damage = 10;
    public float range = 3;
    public float fireRate = 1; // Attacks per second


    // Start is called before the first frame update
    void Start()
    {
        myFaction = this.gameObject.GetComponent<Faction>();
        mySight = this.gameObject.AddComponent<CircleCollider2D>();
        mySight.radius = sightRadius;
        mySight.isTrigger = true;
        myPathSetter = this.GetComponent<AIDestinationSetter>();
        myAiPath = this.GetComponent<AIPath>();
        myAiPath.maxSpeed = movementSpeed;
        myAiPath.maxAcceleration = movementSpeed;
    }

    void handleMovement()
    {
        if (target != null)
        {
            myPathSetter.target = target.transform;
        }
        if (1 != 1) // If you can see the enemy and is in range
        {
            myPathSetter.target = null;
        }
    }


    void FixedUpdate()
    {
        handleMovement();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Faction>() != null)
        {
            if (other.gameObject.GetComponent<Faction>().factionId != myFaction.factionId)
            {
                Vector3 dir = (this.transform.position - other.transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, other.transform.position, sightRadius);
                if (hit.collider.gameObject == other.gameObject || 1 == 1) // Need tto fix later
                {
                    Debug.Log(this.gameObject.name + " saw " + other.gameObject.name);
                    target = other.gameObject;
                }
                else if (hit != false)
                {
                    Debug.Log(this.gameObject.name);
                }
                else
                {
                    Debug.Log("Raycast missed");
                }
            }
        }
    }
}
