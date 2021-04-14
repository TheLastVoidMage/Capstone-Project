using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float openRange = 3;
    private BoxCollider2D myBox;
    private CircleCollider2D myCircle;
    private SpriteRenderer mySprite;
    public Sprite openTexture;
    public Sprite closedTexture;
    private List<GameObject> sensedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        myBox = this.gameObject.AddComponent<BoxCollider2D>();
        myCircle = this.gameObject.AddComponent<CircleCollider2D>();
        myCircle.radius = openRange / 2;
        myCircle.isTrigger = true;
        mySprite = this.gameObject.GetComponent<SpriteRenderer>();
        mySprite.sortingOrder = 1;
    }

    public void open()
    {
        isOpen = true;
        updateState();
    }

    public void close()
    {
        isOpen = false;
        updateState();
    }

    public void toggle()
    {
        if (isOpen)
        {
            close();
        }
        else
        {
            open();
        }
    }

    private void updateState()
    {
        if (isOpen)
        {
            myBox.enabled = false;
            mySprite.sprite = openTexture;
        }
        else
        {
            myBox.enabled = true;
            mySprite.sprite = closedTexture;
        }
    }

    public void Update()
    {
        if (sensedObjects.Count == 0)
        {
            close();
        }
        else
        {
            open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Faction>() != null)
        {
            if (sensedObjects.Contains(collision.gameObject) == false)
            {
                Debug.Log(collision.gameObject.name + " entered door range");
                sensedObjects.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Faction>() != null)
        {
            if (sensedObjects.Contains(collision.gameObject) == true)
            {
                Debug.Log(collision.gameObject.name + " left door range");
                sensedObjects.Remove(collision.gameObject);
            }
        }
    }
}
