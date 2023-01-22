using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : GameAction
{
	public Vector2Int targetCell;
	private int shotsleft = 3;
	public GridHilight gh;
	public override bool CanAssigne(Character playerCharacter,Vector2Int selectedCell)
	{
        
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
		Character target = CharacterManager.instance.GetCharacterOnCell(targetCell);
		if (Vector2Int.Distance(owner.posOnGrid, targetCell) <= owner.stats.Range && target != null)
        {
		
           
				target.Damage(1);
				RefrenceManager.notificationManager.NotfyOnCell(targetCell, "-1", Color.yellow);
			
			
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
		Debug.Log("SHOoott");
		return shotsleft<=0;
	}

	public override void Start(Character owner, int Tick)
	{
		
		base.Start(owner, Tick);
		activationFrequency = owner.stats.fireRate;
		return ;
	}

    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
    {
		base.Hilight(playerCharacter, selectedCell);
		if (gh != null) { GridShaderBinder.gridHilights.Remove(gh); }
		 gh = new GridHilight();
		gh.color = new Color(1,0,0);
		int range = playerCharacter.stats.Range;
		Vector2Int startpos = new Vector2Int(playerCharacter.ghost.posOnGrid.x - range, playerCharacter.ghost.posOnGrid.y- range);
		for(int x=0 ; x < range*2; x++)
        {
			for (int y = 0; y < range * 2; y++)
            {
				if(Vector2Int.Distance( new Vector2Int(startpos.x+x, startpos.y + y), playerCharacter.ghost.posOnGrid) < range)
                {
					gh.points.Add(new Vector2Int(startpos.x + x, startpos.y + y));

				}
            }

		}
		GridShaderBinder.gridHilights.Add(gh);
		
	}

    public override void UnHilight()
	{
		base.UnHilight();
		GridShaderBinder.gridHilights.Remove(gh);
	
	}
}
