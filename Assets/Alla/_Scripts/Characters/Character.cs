using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;
public class Character : Ticker
{
	public  class CharacterGhost
    {
		public Vector2Int posOnGrid;

	}
	[System.Serializable]
	public class CharacterStats
	{
		[SerializeField] public int Movement;
		[SerializeField] public int Accuracy;
		[SerializeField] public int Range;
		[SerializeField] public int Damage;
		[SerializeField, SyncVar] public int Hp;
		[SerializeField] public int fireRate;
	}
	public CharacterGhost ghost = new CharacterGhost();
	public Vector2Int posOnGrid;
	public int actionPointsLeft;
	public bool finishedActions { get { return _finishedActions; } set { _finishedActions = value; } }
	public Image image;
	[SerializeField] public CharacterStats stats;


	private Queue<GameAction> actions = new Queue<GameAction>();
	[SerializeField]private bool _finishedActions;
	private GameAction currentAction;
	


	private void Start()
	{
		base.Start();
		Vector2Int pos;
		GridManager.instance.WorldToGrid(transform.position,out pos);
		posOnGrid = pos;
		ghost.posOnGrid = pos;
		transform.position = GridManager.instance.GridToWorld(posOnGrid);
		CharacterManager.instance.allCharacters.Add(this);
	}
	
	
	public void MoveTo(Vector2Int pos)
	{

		transform.position= GridManager.instance.GridToWorld(pos);
		posOnGrid = pos;
	}

	public void MoveTo(Vector3 pos)
	{
		Vector2Int temppos;
		GridManager.instance.WorldToGrid(pos, out temppos);
		posOnGrid = temppos;
	}

	public override void OnTick(int Tick)
	{
	
		if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Planning)
			return;
		if (finishedActions) return;
        if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action1)
        {
			if (currentAction == null && actions.Count > 0)
			{
				currentAction = actions.Dequeue();
				currentAction.Start(this, Tick);
				return;
			}
			if (currentAction != null && !currentAction.IsFinished(this))
			{
				if (currentAction.CanDo(this, Tick))
					currentAction.Do(this, Tick);
				return;
			}
			if (currentAction != null)
				currentAction.Removed(this);
			
			CharacterManager.instance.OnCharacterFinishedActions();
			_finishedActions = true;
			return;
		}
		if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2)
		{
			if (currentAction == null && actions.Count > 0)
			{
				currentAction = actions.Dequeue();
				currentAction.Start(this, Tick);
				return;
			}
			if (currentAction != null && !currentAction.IsFinished(this))
			{
				if (currentAction.CanDo(this, Tick))
					currentAction.Do(this, Tick);
				return;
			}
			if (currentAction != null)
				currentAction.Removed(this);
			CharacterManager.instance.OnCharacterFinishedActions();
			_finishedActions = true;

		}
		
	
		
		actions.Clear();
	}
	private void OnDestroy()
	{if(CharacterManager.instance.allCharacters!=null)
		CharacterManager.instance.allCharacters.Remove(this);
	}

	public override void OnStartedPlanning()
	{
	
		ghost.posOnGrid = posOnGrid;
		finishedActions = false;
		actionPointsLeft = CharacterManager.instance.maxActionPoints;
	}
	public void Damage(int value)
    {
		stats.Hp = Mathf.Clamp(stats.Hp - value, 0, 10);
		image.fillAmount=(float)stats.Hp / 10;

	}
	public override void OnStartedPlaying()
	{
		
	}

	public void AddAction(GameAction action)
	{
		
		if (actionPointsLeft >= 0)
		{
			print("action");
			actions.Enqueue(action);
			action.initilize(this);
			actionPointsLeft--;


		}
	}
	public void RemoveAction(GameAction action)
	{
		actions= new Queue<GameAction>(actions.Where(x => x != action));
		action.Removed(this);
        
		actionPointsLeft++;
	}
}