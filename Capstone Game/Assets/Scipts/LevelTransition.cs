using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public Animator myAnimator;
    private float transitionTime = 5f;
    
    public void LoadLevel(int levelID)
    {
        StartCoroutine(TransitionLevel(levelID));
    }

    IEnumerator TransitionLevel(int levelID)
    {
        myAnimator.SetTrigger("onExitLevel");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelID);
    }
}
