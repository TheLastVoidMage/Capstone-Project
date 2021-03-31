using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myBody;
    private GameObject mySprite;
    private Vector3 newPosition;
    private float maxPlayerZoom = 15;
    private float minPlayerZoom = 5;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        mySprite = this.gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        newPosition = this.transform.position;
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
        mySprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        float newZoom = Camera.main.orthographicSize + (Input.GetAxis("Mouse ScrollWheel") * -3);
        if (newZoom >= minPlayerZoom && newZoom <= maxPlayerZoom)
        {
            Camera.main.orthographicSize = newZoom;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        playerLook();
    }
}
