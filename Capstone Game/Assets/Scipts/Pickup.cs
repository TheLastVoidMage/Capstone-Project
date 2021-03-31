using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Tells what resource to add to, or if -1 then it's a gun
    public int resourceId = 0;
    public int quantity = 1;
    public Weapons weapon = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int getResourceId()
    {
        return resourceId;
    }

    public int getQuantity()
    {
        return quantity;
    }

    public void destroyPickup()
    {
        Destroy(this.gameObject);
    }

    public Weapons getWeapon()
    {
        return weapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
