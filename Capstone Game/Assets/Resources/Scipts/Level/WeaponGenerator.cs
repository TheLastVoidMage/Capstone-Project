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

    public Weapon generatePlayerWeapon()
    {
        if (mySoundLibary == null)
        {
            mySoundLibary = new SoundLibary().generate();
        }
        gunSprites = Resources.LoadAll<Sprite>("Images/Guns/");
        meleeSprites = Resources.LoadAll<Sprite>("Images/Melee/");
        return new Weapon(player, gunSprites[0], "TestGun", 1, 1, 1, 1, 1, 1, 1, mySoundLibary.gunFire[0], mySoundLibary.gunFire[0]);
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
        Weapon newWeapon = new Weapon(enemy, selectedSprite, "Enemy Weapon", fireRate, 2/spread, Mathf.RoundToInt(200 / damage), 1, 1, damage, 30 / (200 / damage), mySoundLibary.gunFire[fireSound], mySoundLibary.gunReload[reloadSound], isMelee, false, spread / 5);
        return newWeapon;
    }
}
