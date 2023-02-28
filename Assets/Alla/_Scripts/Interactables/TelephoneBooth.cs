using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TelephoneBooth : Interactable
{
    public Queue<MusicManager.Song> songs = new Queue<MusicManager.Song>();

    [Command(requiresAuthority =false)]
    public void AddSong(int song )
    {
      
        songs.Enqueue((MusicManager.Song)song);
    }
    public override void Interact(Character character)
    {

        RefrenceManager.musicManager.ChangeSong(songs.Dequeue());
    }
    public override void PreInteract(Character character)
    {
        RefrenceManager.musicManager.radioUI.SetActive(true);
        RefrenceManager.radioControler.UI.SetActive(false);
        RefrenceManager.radioControler.phone = this;
    }
}
