using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
public class RadioControler : NetworkBehaviour
{
    Vector3 lastDirection;
    bool clicking;
    public Transform nob;
    int song;
    public TelephoneBooth phone;
    public TextMeshProUGUI text;
    public Image screen;
    public float angle=10;
    public List<Transform> dots= new List<Transform>();

    private void Awake()
    {
        if (RefrenceManager.radioControler ==null)
        RefrenceManager.radioControler = this;
    }


    public void OnDone()
    {
        if (phone != null)
        {
           
            phone.AddSong(song);
            phone.songs.Enqueue((MusicManager.Song)song);
        }
     
        phone=null;

    }
    public void Update()
    {
        if (!clicking) return;
        Vector2 mousePos= InputManager.instance.InputMap.Camera.MousePosition.ReadValue<Vector2>();
        nob.up = mousePos - (Vector2)nob.position;

        song = 4;
        for (int i=0;i<dots.Count;i++)
        {
            Vector3 dir =  dots[i].position- nob.position;
            if (Vector3.Angle(nob.up, dir)<angle)
            {
                song = i;
                break;
            }
        }
      
        text.text = ((MusicManager.Song)song).ToString().ToUpper();
        switch ((MusicManager.Song)song)
        {
            case MusicManager.Song.Red:
                text.color = Color.red;
                screen.color=Color.red;
                break;
            case MusicManager.Song.Green:
                text.color = Color.green;
                screen.color=Color.green;
                break;
            case MusicManager.Song.yellow:
                text.color = Color.yellow;
                screen.color=Color.yellow;
                break;
            case MusicManager.Song.Blue:
                text.color = Color.blue;
                screen.color=Color.blue;
                break;
            case MusicManager.Song.nothing:
                text.color = Color.white;
                screen.color = Color.white;
                break;
            default:
                break;
        }

    }

    public void OnClickDown()
    {
        clicking = true;
    }
    public void OnClickUp()
    {
        clicking = false;
        

    }
  
}
