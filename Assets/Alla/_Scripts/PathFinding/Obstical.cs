using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class Obstical : GridObject
{
	public static	List<Obstical> obstacles = new List<Obstical>();
    public static float angle = 45;
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
    
    public bool IsCoverdShootingPosition(Vector2Int cell,Vector2Int coverPoint)
    {
        Vector2Int direction = posOnGrid - cell;
        Vector2Int coverDirection = coverPoint- posOnGrid ;

        if (Vector2.Angle(direction,coverDirection)<=angle)
        {
            return true;
        }
        return false;   
    }
    public static Obstical GetObstacl(Vector2Int pos)
    {
        foreach (Obstical obs in obstacles)
        {
            if (obs.posOnGrid == pos)
            {
                return obs;
            }
        }
        return null;
    }
    public enum Type
	{
		Wall,SemiWall
	}

	public Type coverType;


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
    public void HilightPos(Vector2Int targetPos)
    {

        GridHilight gh = new GridHilight();
        gh.color = new Color(1, 0, 0);

        List<Vector2Int> cells = new List<Vector2Int>();




        for (int x = 0; x < GridManager.instance.gridSizeX; x++)
        {
            for (int y = 0; y < GridManager.instance.gridSizeZ; y++)
            {
                if (IsCoverdShootingPosition(new Vector2Int(x, y), targetPos))
                {
                    cells.Add(new Vector2Int(x, y));
                }
            }
        }
        gh.points.AddRange(cells);
        GridShaderBinder.gridHilights.Add(gh);
    }
}
