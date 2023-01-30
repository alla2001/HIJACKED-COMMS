using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public float time = 1;
    private void Start()
    {
        StartCoroutine(WaitFade());
    }
    IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
