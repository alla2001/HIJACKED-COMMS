using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : GridObject
{
 
    Vector3 target;
    Vector3 startpos;
    public BoxCollider collider;

    Vector3 bottomLeft;
    Vector3 topRight;
    public List<Vector2Int> points = new List<Vector2Int>();
    public static List<Door> doors=new List<Door>();
    public bool active;
    public float speed=3f;
    private void Awake()
    {
        doors.Add(this);
        startpos = transform.position;
        if (active)
        {
            target = startpos;

        }
        else
        {
            target = startpos - new Vector3(0, 5, 0);
        }
    }
    private void OnDestroy()
    {
        doors.Remove(this);
    }
    private void Start()
    {
        base.Start();
        bottomLeft = new Vector3(collider.bounds.center.x - collider.bounds.extents.x, collider.center.y, collider.bounds.center.z - collider.bounds.extents.z);
        topRight = new Vector3(collider.bounds.center.x + collider.bounds.extents.x, collider.center.y, collider.bounds.center.z + collider.bounds.extents.z);
        Vector2Int blgrid;
        Vector2Int trgrid;
        GridManager.instance.WorldToGrid(bottomLeft, out blgrid);
        GridManager.instance.WorldToGrid(topRight, out trgrid);
        int ydiff = trgrid.y - blgrid.y + 1;
        int xdiff = trgrid.x - blgrid.x + 1;



        for (int x = 0; x < xdiff; x++)
        {
            for (int y = 0; y < ydiff; y++)
            {
                points.Add(posOnGrid+new Vector2Int(x,y));
            }

        }
        if (isServer)
        {
            RefrenceManager.musicManager.OnChangeSong += (song) =>
            {
            
                if (song == MusicManager.Song.Green)
                {

                    active = !active;
                    if (active)
                    {
                        target = startpos;

                    }
                    else
                    {
                        target = startpos - new Vector3(0, 5, 0);
                    }
                    if (isServer)
                    {
                        ChangeState(active);
                    }
                }
            };
            GameManager.startPlaying += () =>
            {
                if (RefrenceManager.musicManager.currentSong == MusicManager.Song.Green)
                {

                    active = !active;
                    if (active)
                    {
                        target = startpos;

                    }
                    else
                    {
                        target = startpos - new Vector3(0, 5, 0);
                    }
                    if (isServer)
                    {
                        ChangeState(active);
                    }
                }
            };
            
        }

    }
 
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, speed*Time.deltaTime);
    }
  
    [ClientRpc]
    public void ChangeState(bool value)
    {
        active = value;
        if (active)
        {
            target = startpos;

        }
        else
        {
            target = startpos - new Vector3(0, 5, 0);
        }
    }
  
    

    
}
