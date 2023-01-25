using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Shoot : GameAction
{
	public Vector2Int targetCell;
	private int shotsleft = 1;
	public GridHilight gh;

    public override void Update(Character owner)
    {
    }
    public override bool CanAssigne(Character playerCharacter,Vector2Int selectedCell)
	{
		Character target= CharacterManager.instance.GetCharacterOnCell(selectedCell);
        if (target==null)
        {
			return false;
        }
		
		List<Vector2Int> cells = GridManager.instance.GetCircle(playerCharacter.ghost.posOnGrid, playerCharacter.stats.Range);
		Vector2Int cell= cells.FirstOrDefault((ele)=> { return (ele.x==selectedCell.x && ele.y == selectedCell.y); });
		Debug.Log(cell);
		if (cell==null || cell== Vector2Int.zero)
        {
			
			return false;
		}
		return true; 
	}
    public override bool Removed(Character owner)
    {
		if (gh != null) { GridShaderBinder.gridHilights.Remove(gh); }
		return false;
    }
  

	public override void Do(Character owner, int Tick)
	{
		//owner.stats.Accuracy;

		//owner.stats.Damage;
		
		if (shotsleft <= 0) return;
		int accuracy = owner.stats.Accuracy*5+30;
		int damage = Random.Range(owner.stats.Damage, owner.stats.Damage+ 3);
		Character target = CharacterManager.instance.GetCharacterOnCell(targetCell);
		List<Obstical> covers = target.InCover();

		if (covers.Count>0)
        {
			
			foreach (Obstical cover in covers)
			{
				//cover.HilightPos(target.posOnGrid);
				if (cover.IsCoverdShootingPosition(owner.posOnGrid,target.posOnGrid) )
				{
                    if (cover.coverType==Obstical.Type.Wall)
                    {
						accuracy = (int)(accuracy * 0.5f);
					}
                    else
                    {
						accuracy = (int)(accuracy * 0.25f);
					}
				}
			}
		}
		if(Vector2Int.Distance(owner.posOnGrid, targetCell) > owner.stats.Range)
        {
			accuracy = 0;
        }
		Debug.Log("accuracy : " + accuracy);
		int r = Random.Range(0, 101);
		owner.transform.forward = GridManager.instance.GridToWorld(target.posOnGrid) - owner.transform.position; 
		if (  target != null && r < accuracy)
        {
		
          
				target.Damage(damage);
				RefrenceManager.notificationManager.NotfyOnCell(targetCell, (-damage).ToString(), Color.yellow);
			
			
			
			
        }
        else
        {
			RefrenceManager.notificationManager.NotfyOnCell(targetCell, "Miss", Color.red);
		}
	
		shotsleft--;

		
		return;
	}

    public override void initilize(Character owner)
    {
	
		if (gh != null) { GridShaderBinder.gridHilights.Remove(gh); }
		gh = new GridHilight();
		gh.color = new Color(1, 0, 0);
		gh.points.Add(targetCell);
		GridShaderBinder.gridHilights.Add(gh);
		return;
    }

    public override bool IsFinished(Character owner)
	{
		
		return shotsleft<=0;
	}

	public override void Start(Character owner, int Tick)
	{
		
		base.Start(owner, Tick);
		
		return ;
	}

    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
    {
		base.Hilight(playerCharacter, selectedCell);
		if (gh != null) { GridShaderBinder.gridHilights.Remove(gh); }
		 gh = new GridHilight();
		gh.color = new Color(1,0,0);
	
		List<Vector2Int> cells = GridManager.instance.GetCircle(playerCharacter.ghost.posOnGrid, playerCharacter.stats.Range);
		gh.points.AddRange(cells);

		
		GridShaderBinder.gridHilights.Add(gh);
		
	}

    public override void UnHilight()
	{
		base.UnHilight();
		GridShaderBinder.gridHilights.Remove(gh);
	
	}
}
