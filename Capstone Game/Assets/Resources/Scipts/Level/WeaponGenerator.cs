using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    private SoundLibary mySoundLibary;
    private Sprite[] gunSprites;
    private Sprite[] meleeSprites;
    private Sprite selectedSprite;
    private int gunId = 0;
    private GameObject player;
    private Weapon startingWeapon;
    private Weapon[] commonWeapons = new Weapon[6];
    private Weapon[] rareWeapons = new Weapon[4];

    // Start is called before the first frame update
    void Start()
    {
        mySoundLibary = new SoundLibary().generate();
        gunSprites = Resources.LoadAll<Sprite>("Images/Guns/");
        meleeSprites = Resources.LoadAll<Sprite>("Images/Melee/");
        player = GameObject.FindObjectOfType<playerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Weapon generateStartingWeapon()
    {
        Sprite myImage = Resources.Load<Sprite>("Images/Guns/Special/Pistol");
        if (mySoundLibary == null)
        {
            mySoundLibary = new SoundLibary().generate();
        }
        return new Weapon(myImage, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]);
    }
    public Weapon generatePlayerWeapon()
    {
        if (mySoundLibary == null)
        {
            mySoundLibary = new SoundLibary().generate();
        }
        gunSprites = Resources.LoadAll<Sprite>("Images/Guns/");
        meleeSprites = Resources.LoadAll<Sprite>("Images/Melee/");
        commonWeapons = new Weapon[] {
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "M16 (Assault Rifle)", 1, 1, 30, 1, 3, 10, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "MP5 (Submachine Gun)", 5, 1f, 30, 1, 1, 15, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "12 Gauge (Shotgun)", 1, 2, 8, 1, 10, 5, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "M24 (Sniper Rifle)", .5f, 0, 10, 1, 1, 90, 1.5f, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "AK 47 (Assault Rifle)", 5, 1.5f, 30, 1, 1, 10, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "M4 (Assault Rifle)", 7, .9f, 30, 1, 1, 15, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0])
        };
        rareWeapons = new Weapon[]
        {
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "Xeno I-15 (???)", 50, 0, 200, 1, 1, 1, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "Experiment 82 (???)", 1, 0, 1, 1, 1, 150, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "RPG (Rocket Propelled Grenade)", .5f, .5f, 1, 1, 1, 80, 2, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "G16-L (Grenade Launcher)", 1, 1, 1, 1, 1, 60, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1),
            new Weapon(gunSprites[Random.Range(0, gunSprites.Length - 1)], "Boomstick", 3, 2, 2, 1, 10, 5, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1)
        };
        Weapon newWeapon = null;
        if (Random.Range(0, 99) < 40)
        {
            newWeapon = commonWeapons[Random.Range(0, commonWeapons.Length - 1)];
        }
        else
        {
            newWeapon = rareWeapons[Random.Range(0, rareWeapons.Length - 1)];
        }
        return newWeapon;
    }

    public Weapon generateEnemyWeapon(GameObject enemy, float fireRate, float spread, float damage)
    {
        gunId++;
        int fireSound = 0;
        int reloadSound = 0;
        bool isMelee = false;
        if (mySoundLibary == null)
        {
            mySoundLibary = new SoundLibary().generate();
        }
        if (spread < 10)
        {
            isMelee = true;
        }
        gunSprites = Resources.LoadAll<Sprite>("Images/Guns/");
        meleeSprites = Resources.LoadAll<Sprite>("Images/Melee/");
        if (isMelee)
        {
            selectedSprite = meleeSprites[Random.Range(0, meleeSprites.Length - 1)];
        }
        else
        {
            selectedSprite = gunSprites[Random.Range(0, gunSprites.Length - 1)];
        }
        if (this.gameObject.GetComponent<LevelGenerator>().faction == 1)
        {
            Debug.Log("Removed selected sprite");
            selectedSprite = null;
        }
        Weapon newWeapon = new Weapon(selectedSprite, "Enemy Weapon", fireRate, 2/spread, Mathf.RoundToInt(200 / damage), 1, 1, damage, 30 / (200 / damage), mySoundLibary.gunFire[fireSound], mySoundLibary.gunReload[reloadSound], isMelee, false, spread / 5);
        return newWeapon;
    }
}
