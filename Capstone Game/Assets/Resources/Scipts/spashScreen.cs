using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class spashScreen : MonoBehaviour
{
    public Sprite[] spashScreens;
    public float splashTime;
    private float startTime;
    private Image splashImage;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        splashImage = GameObject.FindObjectOfType<Image>();
        if (spashScreens.Length > 0)
        {
            splashImage.sprite = spashScreens[Random.Range(0, spashScreens.Length - 1)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > splashTime)
        {
            if (Application.CanStreamedLevelBeLoaded(1))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
