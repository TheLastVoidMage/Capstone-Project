using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibary
{

    public string[] gunFire;
    public string[] gunReload;
    public string playerHurt;
    public string playerHeal;

    public SoundLibary generate()
    {
        gunFire = new string[] { "Sounds/Guns/DefaultShoot"};
        gunReload = new string[] {"Sounds/Guns/DefaultReload"};
        playerHurt = "Sounds/Player/Player Hurt";
        playerHeal = "Sounds/Player/Player Heal";
        return this;
    }

}
