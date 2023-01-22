using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MusicManager : NetworkBehaviour
{
    public enum Song
    {
        Red,Green,yellow,Blue
    }
    public GameObject radioUI;
    bool hacking;
    // each clip as a assosiated side efect assosiated to it , create a class for that.
    [SyncVar]
    public Song currentSong;
    private void Awake()
    {
        if(RefrenceManager.musicManager==null)
        RefrenceManager.musicManager = this;
        else
        Destroy(this);
        RefrenceManager.gameManager.startPlaying += () => { radioUI.SetActive(false); };
    }
    public void ChangeSong(Song newSong)
    {
        currentSong = newSong;
        switch (newSong)
        {
            case Song.Red:
                break;
            case Song.Green:
                break;
            case Song.yellow:
                break;
            case Song.Blue:
                break;
            default:
                break;
        }
    }
    public void StartHacking()
    {
        hacking= true;
    }
    public void Stophacking()
    {
        hacking = false;
    }
}
