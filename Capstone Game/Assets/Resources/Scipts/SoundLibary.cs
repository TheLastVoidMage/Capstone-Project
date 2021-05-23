using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibary
{

    public string[] gunFire;
    public string[] gunReload;


    public SoundLibary generate()
    {
        gunFire = new string[] { "Sounds/Guns/DefaultShoot"};
        gunReload = new string[] {"Sounds/Guns/DefaultReload"};
        return this;
    }

}
