using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MusicManager : NetworkBehaviour
{
    public enum Song
    {
       Red,Green,yellow,Blue, nothing
    }
    public GameObject radioUI;
    bool hacking;
    // each clip as a assosiated side efect assosiated to it , create a class for that.
    [SyncVar]
    public Song currentSong;

    public AudioClip redSong;
    public AudioClip greenSong;
    public AudioClip yellowSong;
    public AudioClip blueSong;
    public AudioSource audioSource;
    public delegate void SongDelegate(Song song);

    public SongDelegate OnChangeSong;
    private void Awake()
    {
        if(RefrenceManager.musicManager==null)
        RefrenceManager.musicManager = this;
        else
        Destroy(this);
        RefrenceManager.gameManager.startPlaying += () => { radioUI.SetActive(false); };
        ChangeSong(Song.nothing); 
    }
    public void ChangeSong(Song newSong)
    {
        currentSong = newSong;
        OnChangeSong.Invoke(currentSong);
        ChangeSongCommand();
    }
    [ClientRpc]
    public void ChangeSongRpc()
    {
        OnChangeSong.Invoke(currentSong);
        switch (currentSong)
        {
            case Song.Red:
                audioSource.clip = redSong;
                break;
            case Song.Green:
                audioSource.clip = greenSong;
                break;
            case Song.yellow:
                audioSource.clip = yellowSong;
                break;
            case Song.Blue:
                audioSource.clip = blueSong;
                break;
            case Song.nothing:
                audioSource.clip = null;

                break;
            default:
                break;
        }
    }
    [Command]
    public void ChangeSongCommand()
    {
        
        switch (currentSong)
        {
            case Song.Red:
                audioSource.clip = redSong;
                break;
            case Song.Green:
                audioSource.clip = greenSong;
                break;
            case Song.yellow:
                audioSource.clip = yellowSong;
                break;
            case Song.Blue:
                audioSource.clip = blueSong;
                break;
            case Song.nothing:
                audioSource.clip = null;

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
