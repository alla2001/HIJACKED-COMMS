using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControler : SingletonMonoBehaviour<RadioControler>
{
    Vector3 lastDirection;
    bool clicking;
    int song;
    public void ChangeSong()
    {
        RefrenceManager.musicManager.ChangeSong((MusicManager.Song)song);
    }

    public void Update()
    {
        if (!clicking) return;
        Vector2 mousePos= InputManager.instance.InputMap.Camera.MousePosition.ReadValue<Vector2>();
        transform.up = mousePos - (Vector2)transform.position;
        float angle = Vector3.SignedAngle(transform.up, Vector3.up,Vector3.forward);
        angle += 45;
        if (angle < 0) angle = angle + 360;

      
        song = (int)(angle / 90);
       
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
