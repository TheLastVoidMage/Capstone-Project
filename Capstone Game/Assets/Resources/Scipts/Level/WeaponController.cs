using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon[] heldWeapons;
    private SoundLibary mySoundLibary;
    public int selectedWeapon = 0;
    private bool isReloading = false;
    private AudioSource myFireAudioPlayer;
    private AudioSource myReloadAudioPlayer;
    private SpriteRenderer myRenderer;
    private playerController myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GameObject.FindObjectOfType<playerController>();
        mySoundLibary = new SoundLibary().generate();
        if (heldWeapons == null)
        {
            heldWeapons = new Weapon[4] { new Weapon(this.gameObject, null, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]), new Weapon(this.gameObject, null, "Rocket Launcher", 3, 1, 1, 1, 1, 100, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 3), new Weapon(this.gameObject, null, "Boomstick", 3, 2, 2, 1, 10, 5, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1), null };
        }
        else if (heldWeapons.Length == 0)
        {
            heldWeapons = new Weapon[4] { new Weapon(this.gameObject, null, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0]), new Weapon(this.gameObject, null, "Rocket Launcher", 3, 1, 1, 1, 1, 100, 3, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 3), new Weapon(this.gameObject, null, "Boomstick", 3, 2, 2, 1, 10, 5, 1, mySoundLibary.gunFire[0], mySoundLibary.gunReload[0], false, true, 1), null };
        }
        myFireAudioPlayer = this.gameObject.AddComponent<AudioSource>();
        myReloadAudioPlayer = this.gameObject.AddComponent<AudioSource>();
        myRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        myRenderer.sortingOrder = -1;
        this.gameObject.transform.localPosition = new Vector3(0, 1);
        foreach (Weapon w in heldWeapons)
        {
            if (w != null)
            {
                w.timeLastFired = Time.time;
                w.bulletsInClip = w.clipSize;
            }
        }
        changeWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Weapon changeWeapon(int newWeapon)
    {
        selectedWeapon = newWeapon;
        myRenderer.sprite = heldWeapons[selectedWeapon].displaySprite();
        myFireAudioPlayer.clip = heldWeapons[selectedWeapon].getFireClip();
        myReloadAudioPlayer.clip = heldWeapons[selectedWeapon].getReloadClip();
        heldWeapons[selectedWeapon].timeLastFired = Time.time;
        myPlayer.updateUI();
        return heldWeapons[selectedWeapon];
    }

    public void fire()
    {
        if (heldWeapons[selectedWeapon].timeLastFired > Time.time)
        {
            heldWeapons[selectedWeapon].timeLastFired = 0;
        }
        if (heldWeapons[selectedWeapon].bulletsInClip < heldWeapons[selectedWeapon].bulletsUsedInShot)
        {
            reload();
        }
        else
        {
            if (heldWeapons[selectedWeapon].timeLastFired + (1 / heldWeapons[selectedWeapon].fireRate) < Time.time && isReloading == false)
            {
                heldWeapons[selectedWeapon].timeLastFired = Time.time;
                heldWeapons[selectedWeapon].bulletsInClip--;
                myFireAudioPlayer.Play();
                heldWeapons[selectedWeapon].shoot(this.gameObject);
            }
        }
    }

    public WeaponController pickUpWeapon(Weapon newWeapon)
    {
        for (int x = 0; x < heldWeapons.Length; x++)
        {
            if (heldWeapons[x] == null)
            {
                heldWeapons[x] = newWeapon;
                return this;
            }
        }
        heldWeapons[selectedWeapon] = newWeapon;
        myPlayer.updateUI();
        return this;
    }

    public void reload()
    {
        if (heldWeapons[selectedWeapon].clipSize != heldWeapons[selectedWeapon].bulletsInClip && isReloading == false)
        {
            isReloading = true;
            myReloadAudioPlayer.Play();
            StartCoroutine(finishReload(heldWeapons[selectedWeapon].reloadTime, heldWeapons[selectedWeapon]));
        }
    }

    public IEnumerator finishReload(float time, Weapon reloadingWeapon)
    {
        yield return new WaitForSeconds(time);
        reloadingWeapon.bulletsInClip = reloadingWeapon.clipSize;
        isReloading = false;
        if (this.gameObject.transform.parent.transform.parent.GetComponent<playerController>() != null)
        {
            this.gameObject.transform.parent.transform.parent.GetComponent<playerController>().updateUI();
        }
    }
}
