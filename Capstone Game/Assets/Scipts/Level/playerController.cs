using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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

    public Weapon[] heldWeapons;
    private SoundLibary mySoundLibary;
    private int selectedWeapon = 0;
    private int fuel;

    public bool isPaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
        mySoundLibary = new SoundLibary().generate();;
        heldWeapons = new Weapon[4] { new Weapon(this.gameObject, null, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]), new Weapon(this.gameObject, null, "Rocket Launcher", 3, 1, 1, 1, 1, 100, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 3), new Weapon(this.gameObject, null, "Boomstick", 3, 2, 2, 1, 10, 5, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1), null };
        myBody = this.GetComponent<Rigidbody2D>();
        mySpriteObject = this.gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        mySpriteObject.GetComponent<SpriteRenderer>().sprite = mySprite;
        newPosition = this.transform.position;
    }

    private Save CreateSaveGameObject()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();
        save.fuel = fuel;
        save.playerWeapons = heldWeapons;
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
            heldWeapons = save.playerWeapons;
            fuel = save.fuel;
            mySprite = Resources.LoadAll<Sprite>("Images/Player/")[save.characterId];
            return true;
        }
        return false;
    }

    void handleGuns()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (heldWeapons[0] != null)
            {
                selectedWeapon = 0;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            if (heldWeapons[1] != null)
            {
                selectedWeapon = 1;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            if (heldWeapons[2] != null)
            {
                selectedWeapon = 2;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            if (heldWeapons[3] != null)
            {
                selectedWeapon = 3;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (heldWeapons[selectedWeapon] != null)
            {
                heldWeapons[selectedWeapon].shoot(this.gameObject);
            }
        }
        else if (Input.GetKey(KeyCode.R))
        {
            if (heldWeapons[selectedWeapon] != null)
            {
                heldWeapons[selectedWeapon].startReload();
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
}
