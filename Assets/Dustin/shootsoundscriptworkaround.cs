using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootsoundscriptworkaround : MonoBehaviour
{
    public AudioSource shootsound;
    void Start()
    {
        
    }
    private void Awake()
    {
        shootsound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (shootsound.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
