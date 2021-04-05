using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //guns





    //shooting 
    public Transform fireFromHere;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, fireFromHere.position, fireFromHere.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireFromHere.up * bulletForce, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            shoot();
        }
    }
}