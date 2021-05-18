using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    // Tells what resource to add to, or if -1 then it's a gun
    public int resourceId = 0;
    public int quantity = 1;
    public Weapon weapon = null;
    private bool playerIsColliding = false;
    private KeyCode pickUpKey = KeyCode.E;

    private SpriteRenderer myRenderer;
    private TextMesh myPickupOverlay;
    private playerController myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (myRenderer == null)
        {
            myRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        }
        myPlayer = FindObjectOfType<playerController>();
        generateCrate(resourceId, weapon);
    }

    public Pickup generateCrate(int id = 0, Weapon newWeapon = null)
    {
        resourceId = id;
        // Text
        GameObject myTextObject = new GameObject("myLabel");
        myTextObject.transform.parent = this.transform;
        myTextObject.transform.localPosition = new Vector3(0, 0);
        myTextObject.layer = 5;
        myPickupOverlay = myTextObject.AddComponent<TextMesh>();
        myPickupOverlay.offsetZ = -1;
        myPickupOverlay.characterSize = .5f;
        myPickupOverlay.alignment = TextAlignment.Center;
        myPickupOverlay.anchor = TextAnchor.MiddleCenter;
        myPickupOverlay.text = "Press " + pickUpKey + " to pick up\n";
        myPickupOverlay.gameObject.SetActive(false);
        // Visual
        if (myRenderer == null)
        {
            myRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            myRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        myRenderer.sortingOrder = -1;
        if (resourceId == 0)
        {
            myRenderer.sprite = Resources.Load<Sprite>("Images/Pickups/FuelCrate");
            quantity = Random.Range(1, 4);
            myPickupOverlay.text = myPickupOverlay.text + quantity + " Fuel";
        }
        else if (resourceId == 1)
        {
            myRenderer.sprite = Resources.Load<Sprite>("Images/Pickups/HealthCrate");
            quantity = Random.Range(25, 51);
            myPickupOverlay.text = myPickupOverlay.text + quantity + " Health";
        }
        else
        {
            myRenderer.sprite = Resources.Load<Sprite>("Images/Pickups/GunCrate");
            weapon = newWeapon;
            if (weapon != null)
            {
                myPickupOverlay.text = myPickupOverlay.text + weapon.displayName;
            }
            else
            {
                myPickupOverlay.text = myPickupOverlay.text + "None";
            }
        }
        // Collider
        this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;

        return this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsColliding && Input.GetKeyDown(pickUpKey))
        {
            pickUpCrate();
        }
    }

    private void pickUpCrate()
    {
        if (resourceId == 0)
        {
            myPlayer.pickUpFuel(quantity);
        }
        else if (resourceId == 1)
        {
            myPlayer.heal(quantity);
        }
        else if (weapon != null)
        {
            myPlayer.myWeapons.pickUpWeapon(weapon);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() != null)
        {
            playerIsColliding = true;
            myPickupOverlay.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() != null)
        {
            playerIsColliding = false;
            myPickupOverlay.gameObject.SetActive(false);
        }
    }
}
