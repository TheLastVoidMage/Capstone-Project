using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    string name = "defualtName";
    float damage = 0.0f;
    float fireRate = 0.0f;
    float spread = 0.0f;
    int clipSize = 0;
    int bulletsFiredOnTriggerPull = 0;
    int projectilesEmitedPerBulletShot = 0;
    bool areaOfEffect = false;

    public Weapons()
    {
        name = "defualtName";
        damage = 0.0f;
        fireRate = 0.0f;
        spread = 0.0f;
        clipSize = 0;
        bulletsFiredOnTriggerPull = 0;
        projectilesEmitedPerBulletShot = 0;
        areaOfEffect = false;
    }
    public Weapons makeGlock()
    {
        name = "Glock";
        damage = 10;
        fireRate = 1;
        spread = 10;
        clipSize = 12;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
        return this;
    }
    public void makeMP5()
    {
        name = "MP5";
        damage = 15;
        fireRate = 15;
        spread = 15;
        clipSize = 30;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeM16()
    {
        name = "M16";
        damage = 30;
        fireRate = 1;
        spread = 15;
        clipSize = 30;
        bulletsFiredOnTriggerPull = 3;
        projectilesEmitedPerBulletShot = 3;
        areaOfEffect = false;
    }
    public void make12Gauge()
    {
        name = "12 Gauge";
        damage = 25;
        fireRate = 1;
        spread = 50;
        clipSize = 4;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 15;
        areaOfEffect = false;
    }
    public void makeLaserBeam()
    {
        name = "Xeno I-15";
        damage = 1;
        fireRate = 50;
        spread = 0;
        clipSize = 200;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeAlienSniper()
    {
        name = "Experiment 82";
        damage = 150;
        fireRate = 1;
        spread = 0;
        clipSize = 1;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeSniper()
    {
        name = "M24";
        damage = 90;
        fireRate = 0.5f;
        spread = 1;
        clipSize = 5;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeRPG()
    {
        name = "RPG";
        damage = 80;
        fireRate = 0.5f;
        spread = 50;
        clipSize = 1;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = true;
    }
    public void makeAK47()
    {
        name = "AK 47";
        damage = 50;
        fireRate = 10;
        spread = 20;
        clipSize = 30;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeM4()
    {
        name = "M4";
        damage = 45;
        fireRate = 12;
        spread = 10;
        clipSize = 30;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = false;
    }
    public void makeGrenadeLauncher()
    {
        name = "G16-L";
        damage = 60;
        fireRate = 1;
        spread = 15;
        clipSize = 6;
        bulletsFiredOnTriggerPull = 1;
        projectilesEmitedPerBulletShot = 1;
        areaOfEffect = true;
    }
    //shooting
    public Transform fireFromHere;
    public GameObject bulletPrefab;
    public float lastFired = 0;

    public float bulletForce = 20f;

    public void Start()
    {
        lastFired = Time.time;
    }
    void shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, fireFromHere.position, fireFromHere.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireFromHere.up * bulletForce, ForceMode2D.Impulse);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }
}