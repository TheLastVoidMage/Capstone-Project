using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private GameObject myParent;
    private EnemyController myParentScript;
    private CircleCollider2D mySight;
    // Start is called before the first frame update
    void Start()
    {
        myParent = this.transform.parent.gameObject;
        myParentScript = myParent.GetComponent<EnemyController>();
        mySight = this.gameObject.AddComponent<CircleCollider2D>();
        myParentScript.sightRadius = myParentScript.sightRadius * (1 / myParentScript.mySize);
        mySight.radius = myParentScript.sightRadius;
        mySight.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (myParentScript.target == null)
        {
            if (other.gameObject.GetComponent<Faction>() != null)
            {
                if (other.gameObject.GetComponent<Faction>().factionId != myParentScript.myFaction.factionId)
                {
                    Vector3 dir = (other.transform.position - this.transform.position).normalized;
                    RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, myParentScript.sightRadius * 2);
                    if (hit != false)
                    {
                        if (hit.collider.gameObject == other.gameObject)
                        {
                            //Debug.Log(this.gameObject.name + " saw " + other.gameObject.name);
                            myParentScript.target = other.gameObject;
                        }
                        else
                        {
                            //Debug.Log("Detected " + hit.collider.gameObject.name);
                        }
                    }
                    else
                    {
                        Debug.Log("Raycast missed");
                    }
                }
            }
        }
    }
}
