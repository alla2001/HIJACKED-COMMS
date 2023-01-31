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
        GameManager.startPlaying += () => { RefrenceManager.radioControler.OnDone(); radioUI.SetActive(false); };
        ChangeSong(Song.nothing); 
    }
    public void ChangeSong(Song newSong)
    {
        currentSong = newSong;
        ChangeSongRpc();
        OnChangeSong?.Invoke(currentSong);
       
    }
    [ClientRpc]
    public void ChangeSongRpc()
    {
        
        switch (currentSong)
        {
            case Song.Red:
                audioSource.clip = redSong;
                RefrenceManager.gameManager.planningTime = 10;
                break;
            case Song.Green:
                audioSource.clip = greenSong;
                RefrenceManager.gameManager.planningTime = 20;
                break;
            case Song.yellow:
                audioSource.clip = yellowSong;
                RefrenceManager.gameManager.planningTime = 15;
                break;
            case Song.Blue:
                audioSource.clip = blueSong;
                RefrenceManager.gameManager.planningTime = 5;
                break;
            case Song.nothing:
                audioSource.clip = null;
                RefrenceManager.gameManager.planningTime = 25;
                break;
            default:
                break;
        }
        audioSource.Play();
    }
    [Command]
    public void ChangeSongCommand()
    {
        
        switch (currentSong)
        {
            case Song.Red:
                audioSource.clip = redSong;
                RefrenceManager.gameManager.planningTime = 10;
                break;
            case Song.Green:
                audioSource.clip = greenSong;
                RefrenceManager.gameManager.planningTime = 20;
                break;
            case Song.yellow:
                audioSource.clip = yellowSong;
                RefrenceManager.gameManager.planningTime = 15;
                break;
            case Song.Blue:
                audioSource.clip = blueSong;
                RefrenceManager.gameManager.planningTime = 5;
                break;
            case Song.nothing:
                audioSource.clip = null;
                RefrenceManager.gameManager.planningTime = 25;
                break;
            default:
                break;
        }
        audioSource.Play();
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
