using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public Rigidbody2D myBody;
    private GameObject mySpriteObject;
    private Sprite mySprite;
    private Vector3 newPosition;
    private float maxPlayerZoom = 15;
    private float minPlayerZoom = 5;
    private GameObject selectedPickup;
    private float interactRange = 3;
    public float speed = 3;

    public WeaponController myWeapons;
    private int fuel;
    private float maxHealth = 150;
    private float health = 150;

    public bool isPaused = false;

    private LevelTransition myTransition;

    private AudioSource myHurtSound;
    private AudioSource myHealSound;

    // HUD elements
    private Text ammoCounter;
    private Text gunName;
    private Text healthText;
    private Text fuelText;
    
    // Start is called before the first frame update
    void Start()
    {
        myWeapons = this.gameObject.GetComponentInChildren<WeaponController>();
        LoadGame();
        myTransition = GameObject.FindObjectOfType<LevelTransition>();
        myBody = this.GetComponent<Rigidbody2D>();
        mySpriteObject = this.gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        mySpriteObject.GetComponent<SpriteRenderer>().sprite = mySprite;
        newPosition = this.transform.position;
        Text[] texts = GetComponentsInChildren<Text>();
        SoundLibary mySoundLibary = new SoundLibary().generate();
        myHurtSound = this.gameObject.AddComponent<AudioSource>();
        myHealSound = this.gameObject.AddComponent<AudioSource>();
        myHurtSound.clip = Resources.Load<AudioClip>(mySoundLibary.playerHurt);
        myHealSound.clip = Resources.Load<AudioClip>(mySoundLibary.playerHeal);
        foreach (Text t in texts)
        {
            if (t.name == "Ammo")
            {
                ammoCounter = t;
            }
            else if (t.name == "Gun Type")
            {
                gunName = t;
            }
            else if (t.name == "Health")
            {
                healthText = t;
            }
            else if (t.name == "FuelText")
            {
                fuelText = t;
            }
        }
        updateUI();
    }

    public void pickUpFuel(int amount)
    {
        fuel += amount;
        updateUI();
    }

    public void heal(float amount)
    {
        myHealSound.Play();
        if (maxHealth - health >= Mathf.RoundToInt(maxHealth / 10))
        {
            amount = maxHealth / 10;
        }
        health += amount;
        if (health > maxHealth)
        {
            maxHealth = health;
        }
        updateUI();
    }

    private Save CreateSaveGameObject()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();
        save.fuel = fuel;
        save.playerWeapons = myWeapons.heldWeapons;
        save.maxHealth = this.maxHealth;
        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved");
    }

    public bool LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.save"))
        {
            Debug.Log("File found");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            myWeapons.heldWeapons = save.playerWeapons;
            fuel = save.fuel;
            mySprite = Resources.LoadAll<Sprite>("Images/Player/")[save.characterId];
            if (myWeapons.heldWeapons == null)
            {
                myWeapons.heldWeapons = new Weapon[4] { new WeaponGenerator().generateStartingWeapon(), new Weapon(null, "Rocket Launcher", 3, 1, 1, 1, 1, 100, 3, null, null, false, true, 3), new Weapon(null, "Boomstick", 3, 2, 2, 1, 10, 5, 1, null, null, false, true, 1), null };
            }
            myWeapons.heldWeapons[myWeapons.selectedWeapon].bulletsInClip = myWeapons.heldWeapons[myWeapons.selectedWeapon].clipSize;
            maxHealth = save.maxHealth;
            health = maxHealth;
            return true;
        }
        return false;
    }

    public void updateUI()
    {
        if (healthText != null && myWeapons.heldWeapons != null)
        {
            if (maxHealth > 999)
            {
                healthText.fontSize = 50;
            }
            if (gunName.text.Length > 30)
            {
                gunName.fontSize = 20;
            }
            else
            {
                gunName.fontSize = 34;
            }
            healthText.text = health + "/" + maxHealth;
            gunName.text = myWeapons.heldWeapons[myWeapons.selectedWeapon].displayName;
            ammoCounter.text = myWeapons.heldWeapons[myWeapons.selectedWeapon].bulletsInClip + "/" + myWeapons.heldWeapons[myWeapons.selectedWeapon].clipSize;
            fuelText.text = fuel + "";
        }
    }

    public void takeDamage(float amount)
    {
        if (isPaused == false)
        {
            myHurtSound.Play();
            health -= amount;
            Debug.Log("Damage: " + amount);
            if (health <= 0)
            {
                isPaused = true;
                myBody.velocity = new Vector3(0, 0);
                health = 0;
                myTransition.LoadLevel(1, "A. V. A.:\nLife Signs Lost...");
                myTransition.setDoors(Resources.Load<Sprite>("Images/LoadingScreens/GameoverScreenLeft"), Resources.Load<Sprite>("Images/LoadingScreens/GameoverScreenright"));
            }
            updateUI();
        }
    }

    void handleGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (myWeapons.heldWeapons[0] != null)
            {
                myWeapons.changeWeapon(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (myWeapons.heldWeapons[1] != null)
            {
                myWeapons.changeWeapon(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (myWeapons.heldWeapons[2] != null)
            {
                myWeapons.changeWeapon(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (myWeapons.heldWeapons[3] != null)
            {
                myWeapons.changeWeapon(3);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (myWeapons.heldWeapons[myWeapons.selectedWeapon] != null)
            {
                myWeapons.fire();
                updateUI();
            }
        }
        else if (Input.GetKey(KeyCode.R))
        {
            if (myWeapons.heldWeapons[myWeapons.selectedWeapon] != null)
            {
                myWeapons.reload();
                updateUI();
            }
        }
    }

    void handleMovement()
    {
        newPosition = this.transform.position;
        myBody.velocity = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            myBody.AddForce(Vector2.up * speed * Time.deltaTime * 2500);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            myBody.AddForce(-Vector2.up * speed * Time.deltaTime * 2500);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myBody.AddForce(Vector2.right * speed * Time.deltaTime * 2500);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            myBody.AddForce(-Vector2.right * speed * Time.deltaTime * 2500);
        }
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime);
    }

    void playerLook()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float angle = Mathf.Atan2(mousePosition.y - this.transform.position.y, mousePosition.x - this.transform.position.x) * Mathf.Rad2Deg;
        mySpriteObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        float newZoom = Camera.main.orthographicSize + (Input.GetAxis("Mouse ScrollWheel") * -3);
        if (newZoom >= minPlayerZoom && newZoom <= maxPlayerZoom)
        {
            Camera.main.orthographicSize = newZoom;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Pickup item = other.GetComponent<Pickup>();
        if (item != null)
        {
            selectedPickup = other.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            handleMovement();
            playerLook();
            handleGuns();
        }
    }

    //Ammo Script
    public static void ammoCount()
    {

    }
}
