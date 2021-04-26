using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public GameObject target = null;
    public Faction myFaction;
    private AIDestinationSetter myPathSetter;
    private AIPath myAiPath;
    private SpriteRenderer mySprite;
    private GameObject myDetector;


    // Variables that will be assigned by level generator
    public float sightRadius = 5;
    public float movementSpeed = 1;
    public float health = 100;
    public float damage = 10;
    public float range = 3;
    public Sprite myImage = null;
    public Color myColor = Color.white;
    public float mySize = 1;
    public int factionId = 1;
    public Weapon myGun;

    public void takeDamage(float damage, GameObject attacker)
    {
        if (target == null)
        {
            target = attacker;
        }
        health = health - damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myFaction = this.gameObject.GetComponent<Faction>();
        myFaction.factionId = factionId;
        this.GetComponent<CircleCollider2D>().radius *= 2;
        myPathSetter = this.GetComponent<AIDestinationSetter>();
        myAiPath = this.GetComponent<AIPath>();
        myAiPath.maxSpeed = movementSpeed;
        myAiPath.maxAcceleration = movementSpeed;
        myDetector = new GameObject();
        myDetector.transform.parent = this.transform;
        myDetector.layer = 2;
        myDetector.name = "Detection";
        myDetector.transform.localPosition = new Vector3(0, 0);
        myDetector.AddComponent<EnemyDetection>();
        mySprite = this.GetComponentInChildren<SpriteRenderer>();
        if (mySprite != null)
        {
            if (myImage != null)
            {
                mySprite.sprite = myImage;
            }
            if (myColor != null)
            {
                mySprite.color = myColor;
            }
        }
        this.transform.localScale = new Vector3(mySize, mySize, 1);
    }

    void handleAttacking()
    {
        // Point toward enemy
        float angle = Mathf.Atan2(target.transform.position.y - this.transform.position.y, target.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        // Attack target
        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, range);
        if (hit != false && target != false)
        {
            if (hit.transform.gameObject == target.gameObject)
            {
                    myGun.shoot(this.gameObject);
            }
        }
    }

    void handleMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.forward, 0);
        Vector3 dir = new Vector3(0,0,0);
        if (target != null)
        {
            myPathSetter.target = target.transform;
            dir = (target.transform.position - this.transform.position).normalized;
            hit = Physics2D.Raycast(this.transform.position, dir, range);
        }
        if (hit == true) // If you can see the enemy and is in range
        {
            if (hit.transform.gameObject == target)
            {
                Debug.Log("Stoped to hit target");
                myPathSetter.target = this.transform;
                handleAttacking();
            }
        }
    }


    void FixedUpdate()
    {
        handleMovement();
    }

}
