using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibary
{

    public AudioClip[] gunFire;
    public AudioClip[] gunReload;


    public SoundLibary generate()
    {
        gunFire = new AudioClip[] { Resources.Load<AudioClip>("Sounds/Guns/DefaultShoot") };
        gunReload = new AudioClip[] { Resources.Load<AudioClip>("Sounds/Guns/DefaultReload") };
        return this;
    }

}
