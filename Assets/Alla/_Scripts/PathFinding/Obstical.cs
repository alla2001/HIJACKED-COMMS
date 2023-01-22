using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class Obstical : GridObject
{

	public static	List<Obstical> obstacles = new List<Obstical>();
	public static bool IsObstacl(Vector2Int pos)
    {
        foreach (Obstical  obs in obstacles)
        {
            if (obs.posOnGrid==pos)
            {
				return true;
            }
        }
		return false;
    }
	public enum Type
	{
		Wall,SemiWall
	}

	public Type obsticalType;


    private void Awake()
    {
        obstacles.Add(this);
    }
    private void OnDestroy()
    {
        obstacles.Remove(this);
    }
    private void Start()
    {
		base.Start();
      
        GridManager.instance.grid.Walls.Add(new GridGraph.GridType{pos= posOnGrid});

        GridShaderBinder.gridHilights.Add(new GridHilight { points = { posOnGrid }, color = new Color(1,0.2f, 0.2f, 1) });

	}
}
