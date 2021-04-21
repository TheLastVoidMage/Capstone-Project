using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    string displayName = "";
    float fireRate = 0; // Firerate measured in bullets per second
    float spread = 0; // Degrees of deviation
    int clipSize = 0; // Max clip size
    int bulletsInClip = 0; // Ammo left in clip
    int bulletsUsedInShot = 1; // How much ammo to subtract per shot
    int pelletCount = 1;
    float damagePerPellet = 0;
    float reloadTime = 0; // Measured in seconds
    float AOERange = 0;
    bool isMelee = false;
    bool pelletsExplode = false;
    bool isReloading = false;

    private float timeLastFired = 0;
    private float timeOfReload = 0;

    public Weapon()
    {
        this.displayName = "Glock";
        this.fireRate = 3;
        this.spread = 1;
        this.clipSize = 30;
        this.bulletsInClip = clipSize;
        this.bulletsUsedInShot = 1;
        this.pelletCount = 1;
        this.damagePerPellet = 25;
        this.reloadTime = 1;
        this.isMelee = false;
        this.pelletsExplode = false;
        this.timeLastFired = 0;
        this.timeOfReload = 0;
        this.AOERange = 0;
    }
    public Weapon(string displayName, float fireRate, float spread, int clipSize, int bulletsUsedInShot, int pelletCount, float damagePerPellet, float reloadTime, bool isMelee = false, bool pelletsExplode = false, float explosionRange = 0)
    {
        this.displayName = displayName;
        this.fireRate = fireRate;
        this.spread = spread;
        this.clipSize = clipSize;
        this.bulletsInClip = clipSize;
        this.bulletsUsedInShot = bulletsUsedInShot;
        this.pelletCount = pelletCount;
        this.damagePerPellet = damagePerPellet;
        this.reloadTime = reloadTime;
        this.isMelee = isMelee;
        this.pelletsExplode = pelletsExplode;
        this.timeLastFired = 0;
        this.timeOfReload = 0;
        this.AOERange = explosionRange;
    }



    public void shoot(GameObject parent)
    {
        if (bulletsInClip < bulletsUsedInShot)
        {
            startReload();
            if (this.timeOfReload + this.reloadTime < Time.time)
            {
                doReload();
            }
        }
        else
        {
            if (timeLastFired + (1 / fireRate) < Time.time && isReloading == false)
            {
                timeLastFired = Time.time;
                bulletsInClip--;
                Debug.Log(this.displayName + " has fired");
                if (isMelee == false)
                {
                    firepellets(parent);
                }
                else
                {
                    meleeStrike(parent);
                }
            }
        }
    }

    private void firepellets(GameObject parent)
    {
        Debug.Log("Fired from: (" + parent.transform.position.x + "," + parent.transform.position.y + ")");
        Vector3[] dirs = new Vector3[this.pelletCount];
        float diviation = 0;
        RaycastHit2D[] hits = new RaycastHit2D[this.pelletCount];
        for (int x = 0; x < dirs.Length;x++)
        {
            dirs[x] = parent.GetComponentInChildren<SpriteRenderer>().gameObject.transform.up;
            diviation = UnityEngine.Random.Range(-this.spread, this.spread) / 6;
            dirs[x] = new Vector3(dirs[x].x + diviation, dirs[x].y + diviation);
            hits[x] = Physics2D.Raycast(parent.transform.position, dirs[x], 1000);
            Debug.DrawRay(parent.transform.position, dirs[x], Color.red, 3);
            if (hits[x] != false)
            {
                Debug.Log("You hit " + hits[x].transform.gameObject.name);
                if (hits[x].transform.gameObject.GetComponent<Faction>() != null)
                {
                    hits[x].transform.gameObject.GetComponent<Faction>().doDamage(hits[x].transform.gameObject, this.damagePerPellet, parent);
                }
                if (this.pelletsExplode)
                {
                    calculateAOE(hits[x].point, parent);
                }
            }
            else
            {
                Debug.Log("You missed");
            }
        }
    }
    private void calculateAOE(Vector3 AOEPoint, GameObject parent)
    {
        GameObject parentDisplay = parent.GetComponentInChildren<SpriteRenderer>().gameObject;
        bool hitWall = false;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        List<GameObject> objectsHit = new List<GameObject>();
        for (int x = -10; x < 11; x++)
        {
            for (int y = -9; y < 10; y++)
            {
                hitWall = false;
                Debug.DrawRay(AOEPoint, new Vector3((float)x / 10, (float)y / 9), Color.red, 3);
                hits = Physics2D.RaycastAll(AOEPoint, new Vector3((float)x / 10, (float)y / 9), this.AOERange);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit != false)
                    {
                        if (hit.transform.gameObject.GetComponent<Faction>() != null)
                        {
                            if (hitWall == false)
                            {
                                if (objectsHit.Contains(hit.transform.gameObject) == false)
                                {
                                    objectsHit.Add(hit.transform.gameObject);
                                }
                            }
                        }
                        else
                        {
                            hitWall = true;
                        }
                    }
                }
            }
        }
        foreach (GameObject o in objectsHit)
        {
            if (o != false)
            {
                Debug.Log("Explosion Hit: " + o.name);
                o.GetComponent<Faction>().doDamage(o.transform.gameObject, this.damagePerPellet, parent);

            }
        }
    }

    private void meleeStrike(GameObject parent)
    {
        calculateAOE(parent.transform.position, parent);
    }

    public void startReload()
    {
        if (this.clipSize != this.bulletsInClip && isReloading == false)
        {
            isReloading = true;
            timeOfReload = Time.time;
            Debug.Log(this.displayName + " started reloading");
        }

    }

    private void doReload()
    {
        this.bulletsInClip = this.clipSize;
        this.isReloading = false;
        Debug.Log(this.displayName + " finished reloading");
    }
}
