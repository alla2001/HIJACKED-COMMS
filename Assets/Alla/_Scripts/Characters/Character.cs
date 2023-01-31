using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;
public class Character : Ticker
{
	[System.Serializable]
	public  class CharacterGhost
    {
		public Vector2Int posOnGrid { get; protected set; }
		public GameObject Ghost;
		public void Move(Vector2Int pos)
        {
			posOnGrid = pos;
			if(Ghost!=null)
			Ghost.transform.position = GridManager.instance.GridToWorld(pos);
        }
		
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
	[SyncVar]
	public Vector2Int posOnGrid;
	public int actionPointsLeft;
	public Transform center;
	public bool finishedActions { get { return _finishedActions; } set { _finishedActions = value; } }
	public Image image;
	[SerializeField] public CharacterStats stats;

	public Queue<GameAction> clientActions= new Queue<GameAction>();
	private Queue<GameAction> actions = new Queue<GameAction>();
	[SerializeField]private bool _finishedActions;
	private GameAction currentAction;
	float speed;
	Vector2Int targetPos;
	public Animator animator;
	
	public LineRenderer lazer;
	[SyncVar]
	public MusicManager.Song nextSong;
	[HideInInspector]public bool ready;

	[Command]
	public void ReadyCommand()
    {
		ready=true;
      

	}
	
	private void Start()
	{
		base.Start();
		Vector2Int pos;
		GridManager.instance.WorldToGrid(transform.position,out pos);
		posOnGrid = pos;
		ghost.Move(pos);
		
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
	public void Lazer(Character target)
    {
		if (lazer == null) return;
		Vector3[] points = { center.position, target.center.position };

		lazer.SetPositions(points);

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
	public void ShootAnimationsServer()
    {
		animator.SetTrigger("Fire");
		ShootAnimationRPC();
	}
	[ClientRpc]
	private void ShootAnimationRPC()
    {
		animator.SetTrigger("Fire");
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
		if (currentAction == null || !currentAction.started)
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
			if (isOwned)
			{
				CharacterFinished();
			}
			else
			{
				CharacterFinishedOnServer();

			}

		
			currentAction = null;

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
            if (isOwned)
            {
				CharacterFinished();
			}
            else
            {
				CharacterFinishedOnServer();

			}
			currentAction = null;


		}
		
	
		
		actions.Clear();
	}


	[Command]
	public void CharacterFinished()
    {
		CharacterManager.instance.OnCharacterFinishedActions();
		_finishedActions = true;
	}

	public void CharacterFinishedOnServer()
	{
		CharacterManager.instance.OnCharacterFinishedActions();
		_finishedActions = true;
	}
	private void OnDestroy()
	{
		base.OnDestroy();
		if(CharacterManager.instance.allCharacters!=null)
		CharacterManager.instance.allCharacters.Remove(this);
	}

	public override void OnStartedPlanning()
	{
        foreach (GameAction action in clientActions)
        {
			action.Removed(this);
        }
		actions.Clear();
		ghost.Move(posOnGrid);
		finishedActions = false;
		actionPointsLeft = CharacterManager.instance.maxActionPoints;
		ready = false;
	}
	
	
	public void Damage(int value)
    {
		stats.Hp = Mathf.Clamp(stats.Hp - value, 0, 10);
		image.fillAmount=(float)stats.Hp / 10;
		if (stats.Hp <= 0 && isServer)
		{
			Destroy(gameObject);
			//DeadCMD();
		}


	}

	
	private void DeadCMD()
    {
		Debug.Log("Dead");
        if (isServer)
        {
			DeadRPC();
        }
        
    }
	[ClientRpc]
	private void DeadRPC()
    {
		Destroy(gameObject);
	}
	public override void OnStartedPlaying()
	{
		
	}

	public void AddActionClient(GameAction action)
    {
       
			
			clientActions.Enqueue(action);
			action.OnAddClient(this);
		
		
	}
	[Command]
	public void AddActionMove(Move move)
	{
		print("Move : " + move.targetPosition +"  start :" +move.startPosition);
		
			print("action");
			actions.Enqueue(move);
			move.OnAddServer(this);
			actionPointsLeft--;




	}
	[Command]
	public void AddActionShoot(Shoot shoot)
	{


	
			actions.Enqueue(shoot);
			shoot.OnAddServer(this);
			actionPointsLeft--;



	}
	[Command]
	public void AddActionGuard(Guard guard)
	{

		
			
			actions.Enqueue(guard);
			guard.OnAddServer(this);
			actionPointsLeft--;



	}
	[Command]
	public void RemoveAction()
	{
		//actions= new Queue<GameAction>(actions.Where(x => x != action));
		GameAction action = actions.ElementAt(actions.Count - 1);

		action.Removed(this);


		actionPointsLeft++;

	}
	[Command]
	public void ClearActionsServer()
    {
		if (actions.Count < 0) return;
		for (int i = 0; i < actions.Count + 1; i++)
		{
			GameAction action = actions.ElementAt(actions.Count - 1);

			action.Removed(this);

			
			actionPointsLeft++;
		}
		actions.Clear();

	}
	public void AddActionAIServer(GameAction action)
    {
        if (isServer)
        {
			actions.Enqueue(action);
			action.OnAddServer(this);
			actionPointsLeft--;
		}
		
	}
	public void ClearActions()
    {
		ClearActionsServer();
		for (int i = 0; i < clientActions.Count; i++)
		{
			clientActions.ElementAt(i).Removed(this);

		}
		ghost.Move(posOnGrid);
		actionPointsLeft = CharacterManager.instance.maxActionPoints;
		clientActions.Clear();
		
	}

}