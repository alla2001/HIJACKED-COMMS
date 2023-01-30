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
	bool interacting;
	public int Song;
	bool end=false;
    public override bool Removed(Character owner)
    {
		owner.ghost.Move(startPosition);

		return GridShaderBinder.gridHilights.Remove(hilight);
	}
    public override void Do(Character owner, int Tick)
	{

		
	}

	public override bool IsFinished(Character owner)
	{
		if (end) return true;
		
        if (index + 1 > setPath.Count)
        {
			return true;

		}
		//List<Node> tempPath;
		//if (!GridManager.instance.PathBetween(startPosition, targetPosition, out tempPath))
		//{
		//	return true;
		//}
		//if (tempPath == null)
  //      {
		//	return true;
		//}
		if (Obstical.finishedMoving && !GridManager.instance.CanMove(setPath[index].Position))
		{
			return true;
		}
		Debug.Log("Notfinished");
		return false;
	}

	public void CalculatePath()
	{
		
		GridManager.instance.PathBetween(startPosition, targetPosition, out setPath);
	}
    public override void SelectionHilight(Character owner)
    {
		CircleHilighter.instance.HilightCircle(GridManager.instance.GridToWorld(owner.ghost.posOnGrid), owner.stats.Movement * GridManager.instance.gridSize);
	}
    public override void SelectionUnHilight()
    {
		CircleHilighter.instance.ClearHilight();

	}
    public override void Start(Character owner, int Tick)
	{
       
		base.Start(owner,Tick);
		index = 0;
		currentNode = setPath[index];
		if (owner.posOnGrid != startPosition)
		{
			end = true;
			return;

		}

	}
	public override  void Update(Character owner)
    {
		if (end) return;
		if (owner.DistanceToTarget() > 0.1f)
			return;
		if (index + 1 > setPath.Count) return;
		if (!GridManager.instance.CanMove(setPath[index].Position)) return;
		Interactable inter = Interactable.IsInteractable(setPath[index].Position);
		if (inter != null&& interacting)
		{
			
			inter.Interact(owner);
		}
		float speed =(float) RefrenceManager.gameManager.playingTime * RefrenceManager.tickManager.tickTimeinSecond;
		owner.MoveTo(setPath[index].Position,speed);
		index++;
	}
	public override bool CanAssigne(Character playerCharacter,Vector2Int selectedCell)
	{
        if (Vector2Int.Distance(playerCharacter.ghost.posOnGrid, selectedCell)>playerCharacter.stats.Movement)
        {
			return false;
		}
        if (!GridManager.instance.PathBetween(playerCharacter.ghost.posOnGrid, selectedCell, out setPath))
        {
			return false;
		}
        if (setPath.Count>playerCharacter.stats.Movement+5)
        {
			return false;
		}
		if (Obstical.GetObstacl(selectedCell))
        {
			return false;
        }
		return true;
       
		
	}

    public override void OnAddServer(Character owner)
    {
        CalculatePath();
		Interactable inter = Interactable.IsInteractable(targetPosition);
		if (inter != null)
		{
			interacting = true;
			
		}




	}
	public override void OnAddClient(Character owner)
    {
		Hilight(owner, targetPosition);
		Interactable inter = Interactable.IsInteractable(targetPosition);
		if (inter != null)
		{
			interacting = true;
			inter.PreInteract(owner);
		}
		owner.ghost.Move(targetPosition);
	}
    public override void Hilight(Character playerCharacter, Vector2Int selectedCell)
	{
		UnHilight();
		if (Vector2Int.Distance(playerCharacter.ghost.posOnGrid, selectedCell) > playerCharacter.stats.Movement) return;
		if (Obstical.IsObstacl(selectedCell)) return;
	

		GridManager.instance.PathBetween(playerCharacter.ghost.posOnGrid, selectedCell, out setPath);
		if (setPath == null) return;
		base.Hilight(playerCharacter, selectedCell);
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
		CircleHilighter.instance.ClearHilight();
	}
}