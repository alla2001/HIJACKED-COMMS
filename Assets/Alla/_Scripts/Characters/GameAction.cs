using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GameAction
{
	public bool started;
	public bool highlighted;
	public int activationFrequency = 1;
	int lastTick = 0;

	public abstract bool Removed(Character owner);
	public abstract bool IsFinished(Character owner);
	public virtual bool CanDo(Character owner, int Tick)
	{

		if (Tick - lastTick < activationFrequency)
		{
			return false;
		}
		else
		{
			lastTick = Tick;
			return true;
		}

	}
	public virtual void OnAddClient(Character owner) { }
	public abstract void Do(Character owner, int Tick);
	public abstract void Update(Character owner);
	public virtual void SelectionHilight(Character owner) { }
	public virtual void SelectionUnHilight(){}
	public virtual void Start(Character owner, int Tick)
    {
		lastTick=Tick;
		started = true;
	}
	public abstract void OnAddServer(Character owner);
	public abstract bool CanAssigne(Character playerCharacter,Vector2Int selectedCell);
	public virtual void Hilight(Character playerCharacter, Vector2Int selectedCell)
	{
		
		if (highlighted) UnHilight();
		highlighted = true;

	}
	public virtual void UnHilight()
    {
		if (highlighted == false) return;
		highlighted =false;
	}
}
