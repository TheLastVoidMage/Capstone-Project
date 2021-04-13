using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] musicList;
    private AudioSource mySource;
    private int factionId = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindObjectOfType<LevelGenerator>() != null)
        {
            factionId = GameObject.FindObjectOfType<LevelGenerator>().faction;
        }
        mySource = this.gameObject.AddComponent<AudioSource>();
        mySource.loop = true;
        if (musicList.Length > 0)
        {
            if (factionId != -1)
            {
                mySource.clip = musicList[factionId - 1];
            }
            else
            {
                mySource.clip = musicList[Mathf.RoundToInt(Random.Range(0, musicList.Length))];
            }
        }
        mySource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
