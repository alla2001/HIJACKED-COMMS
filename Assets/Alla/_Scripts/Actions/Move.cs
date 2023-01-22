using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GameAction
{
	public Vector2Int targetPosition;
	public Vector2Int startPosition;
	public List<Node> setPath;
	private Node currentNode;
	private int index;
	private GridHilight hilight;
	
    public override bool Removed(Character owner)
    {
		owner.ghost.posOnGrid = startPosition;
		return GridShaderBinder.gridHilights.Remove(hilight);
	}
    public override void Do(Character owner, int Tick)
	{
		if (index + 1 > setPath.Count ) return;
		if (GridManager.instance.CanMove(setPath[index].Position)) return;
		owner.MoveTo(setPath[index].Position);
		//if (Interactable.IsInteractable(setPath[index].Position))
  //      {
			

		//}
	
		
		index++;

		
	}

	public override bool IsFinished(Character owner)
	{
		if (owner.posOnGrid == targetPosition || index + 1 > setPath.Count )
		{
			return true;
		}
		return false;
	}

	public void CalculatePath()
	{
		
		GridManager.instance.PathBetween(startPosition, targetPosition, out setPath);
	}

	public override void Start(Character owner, int Tick)
	{
		base.Start(owner,Tick);
		index = 0;
		currentNode = setPath[index];
	}

	public override bool CanAssigne(Character playerCharacter,Vector2Int selectedCell)
	{
        if (Vector2Int.Distance(playerCharacter.posOnGrid, selectedCell)>playerCharacter.stats.Movement)
        {
			return false;
		}
        if (!GridManager.instance.PathBetween(playerCharacter.ghost.posOnGrid, selectedCell, out setPath))
        {
			return false;
		}
		return true;
       
		
	}

    public override void initilize(Character owner)
    {
        CalculatePath();
		Hilight(owner, targetPosition);
		owner.ghost.posOnGrid = targetPosition;
		Interactable inter = Interactable.IsInteractable(targetPosition);
		if (inter!=null)
        {
			inter.Interact();
        }
		

	}

    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
    {
		
		if (Obstical.IsObstacl(selectedCell)) return;
		base.Hilight(playerCharacter, selectedCell);

		GridManager.instance.PathBetween(playerCharacter.ghost.posOnGrid, selectedCell, out setPath);
		hilight = new GridHilight();
		hilight.color = Color.yellow;
		foreach (Node node in setPath)
		{
			hilight.points.Add(node.Position);
		}
		GridShaderBinder.gridHilights.Add(hilight);
	}

    public override void UnHilight()
    {   
		base.UnHilight();
		if(setPath!=null)
		setPath.Clear();
		GridShaderBinder.gridHilights.Remove(hilight);
	}
}