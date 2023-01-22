using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
   public Vector2Int posOnGrid;

    public void Start()
    {
        Vector2Int pos;
        GridManager.instance.WorldToGrid(transform.position, out pos);
        transform.position = GridManager.instance.GridToWorld(pos);
        posOnGrid = pos;
    }
}
