using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;   

public  class Obstical : GridObject
{
	public static	List<Obstical> obstacles = new List<Obstical>();
    public static float angle = 45;
    public CoverType coverType;
    public MoveType nextMove;
    GameAction move;
    Vector2Int targetCell;
    public static bool finishedMoving;
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
        Vector2Int direction = cell- posOnGrid  ;
        Vector2Int coverDirection = posOnGrid - coverPoint;

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
    public enum CoverType
	{
		Wall,SemiWall
	}
    public enum MoveType
    {
       Left,Right
    }
    public void Move()
    {
       
        switch (nextMove)
        {
            case MoveType.Left:
                targetCell = posOnGrid + new Vector2Int(-2,0);
                break;
            case MoveType.Right:
                targetCell = posOnGrid + new Vector2Int(2, 0);
                break;
            default:
                break;
        }
       
    }
    [ClientRpc]
    public void MoveRPC()
    {
        transform.position = GridManager.instance.GridToWorld(posOnGrid);
    }
    
    public void Do()
    {
        if (posOnGrid == targetCell) { finishedMoving = true; return; } 
        Vector2Int direction= Vector2Int.zero;
        switch (nextMove)
        {
            case MoveType.Left:
                posOnGrid += new Vector2Int(-1, 0);
                direction= new Vector2Int(-1, 0);
                break;
            case MoveType.Right:
                posOnGrid += new Vector2Int(1, 0);
               direction = new Vector2Int(1, 0);
                break;
            default:
                break;
        }
        Character chara = CharacterManager.instance.GetCharacterOnCell(posOnGrid);

        if (chara != null)
        {
            chara.posOnGrid += direction;
            chara.transform.position = GridManager.instance.GridToWorld(chara.posOnGrid);
        }
        transform.position= GridManager.instance.GridToWorld(posOnGrid);
        MoveRPC();
        if (posOnGrid == targetCell)
        {
            switch (nextMove)
            {
                case MoveType.Left:
                    nextMove = MoveType.Right;
                    break;
                case MoveType.Right:
                    nextMove = MoveType.Left;
                    break;
                default:
                    break;
            }
        }
          
    }


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
        targetCell = posOnGrid;
        
    

        GridShaderBinder.gridHilights.Add(new GridHilight { points = { posOnGrid }, color = new Color(1,0.2f, 0.2f, 1) });
        if (isServer)
        {
            RefrenceManager.musicManager.OnChangeSong += (song) =>
            {
                if (song == MusicManager.Song.yellow)
                {
                    finishedMoving = false;
                    Move();
                }
            };
            RefrenceManager.gameManager.startPlaying += () =>
            {
                if (RefrenceManager.musicManager.currentSong == MusicManager.Song.yellow)
                {
                    finishedMoving = false;
                    Move();
                }
            };
            RefrenceManager.tickManager.Tick += (tick) =>
            {
                Do();
            };
        }
       

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
