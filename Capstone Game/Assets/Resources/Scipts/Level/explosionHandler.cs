using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionHandler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Resources.Load<GameObject>("Objects/Explosion"), this.transform.position, Quaternion.identity).transform.parent = this.gameObject.transform;
        StartCoroutine(remove(.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator remove(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
