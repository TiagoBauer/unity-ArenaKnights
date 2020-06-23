using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Animations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyAnimation());
    }

    private IEnumerator destroyAnimation()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
