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

	}
	public CharacterGhost ghost = new CharacterGhost();
	public Vector2Int posOnGrid;
	public int actionPointsLeft;
	public Transform center;
	public bool finishedActions { get { return _finishedActions; } set { _finishedActions = value; } }
	public Image image;
	[SerializeField] public CharacterStats stats;


	private Queue<GameAction> actions = new Queue<GameAction>();
	[SerializeField]private bool _finishedActions;
	private GameAction currentAction;
	float speed;
	Vector2Int targetPos;
	public Animator animator;
	private void Start()
	{
		base.Start();
		Vector2Int pos;
		GridManager.instance.WorldToGrid(transform.position,out pos);
		posOnGrid = pos;
		ghost.posOnGrid = pos;
		transform.position = GridManager.instance.GridToWorld(posOnGrid);
		CharacterManager.instance.allCharacters.Add(this);


		
		//GridHilight gh = new GridHilight();
		//gh.color = new Color(1, 0, 0);

		//List<Vector2Int> cells = GridManager.instance.GetCircle(posOnGrid, stats.Range);
		//gh.points.AddRange(cells);


		//GridShaderBinder.gridHilights.Add(gh);
	}
	public void SetTarget(Vector2Int pos)
    {
		targetPos = pos;

	}

	
	public List<Obstical> InCover()
    {

		List<Obstical> obsticals = new List<Obstical>();
        foreach (Vector2Int direction in GridManager.instance.grid.directions)
        {
            if (Obstical.IsObstacl(posOnGrid + direction))
            {
				obsticals.Add(Obstical.GetObstacl(posOnGrid + direction));


			}
        }
		return obsticals;
    }
	public void MoveTo(Vector2Int pos, float _speed)
	{
		speed = _speed;
		
		posOnGrid = pos;
	}

	public void MoveTo(Vector3 pos)
	{
		Vector2Int temppos;
		GridManager.instance.WorldToGrid(pos, out temppos);
		posOnGrid = temppos;
	}
	public float DistanceToTarget()
    {
		return Vector3.Distance(transform.position, GridManager.instance.GridToWorld(posOnGrid));
    }
    private void Update()
    {
		if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Planning)
			return;
        if (DistanceToTarget()>0.1f)
        {
			animator.SetBool("Moving", true);
        }
        else
        {
			animator.SetBool("Moving", false);
		}
		Move();
	

		if (finishedActions) return;
		if (currentAction == null)
			return;
		currentAction.Update(this);
	}
    public void Move()
    {
	
		transform.position = Vector3.MoveTowards(transform.position, GridManager.instance.GridToWorld(posOnGrid), speed*Time.deltaTime);
		Vector3 dir = GridManager.instance.GridToWorld(posOnGrid) - transform.position;
		if (dir!=Vector3.zero)
		transform.forward = GridManager.instance.GridToWorld(posOnGrid) - transform.position;

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
			currentAction = null;
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
			currentAction = null;
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