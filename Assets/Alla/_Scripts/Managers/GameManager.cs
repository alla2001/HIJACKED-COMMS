using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;
public class GameManager : NetworkBehaviour
{
	public int planningTime = 15;
	public int playingTime = 5;
	public UnityAction startPlaying;
	public UnityAction startPlanning;
	public TextMeshProUGUI phaseText;
	public int TimeLeft { get; private set; }
	public enum GamePhase
	{
		Planning, Action1,Action2
	}
	public int playersNeeded = 2;
	[SerializeField]
	private GamePhase _currentPhase = GamePhase.Planning;

	public GamePhase currentPhase
	{ get { return _currentPhase; } private set { _currentPhase = value; } }

	private int lastTransitionTick = 0;
	bool called;
	bool started;
	
    private void Awake()
    {
		if (RefrenceManager.gameManager == null)
			RefrenceManager.gameManager = this;
		else Destroy(this);
    }
    private void Start()
	{
		if (NetworkManager.singleton.numPlayers  >= playersNeeded && isServer)
		{
			RefrenceManager.tickManager.UnPause();
			started = true;
		}
		NetworkManager.singleton.transport.OnServerConnected+= (e) => 
		{
			
			if (NetworkManager.singleton.numPlayers+1 >= playersNeeded && isServer &&!started)
			{
				RefrenceManager.tickManager.UnPause();
				started = true;
			}
		};
      
	
	}

    private void FixedUpdate()
    {
        if (!called)
        {
			startPlanning?.Invoke();
			called= true;

		}
	}
    public void CheckTransferState(int Tick)
	{
		
		switch (currentPhase)
		{
			case GamePhase.Planning:
				if (Tick - lastTransitionTick >= planningTime)
				{
				
					lastTransitionTick = Tick;
					currentPhase = GamePhase.Action1;
					phaseText.text = "Preperation Phase:";
					startPlaying?.Invoke();
					SetState(currentPhase);
					return;
				}
				foreach (Character chara in CharacterManager.instance.allCharacters)
				{
					print(chara.name + " " + chara.ready);
					if (!chara.ready) return;
				}
				lastTransitionTick = Tick;
				currentPhase = GamePhase.Action1;
				phaseText.text = "Playing :";
				startPlaying?.Invoke();
				SetState(currentPhase);
				
				break;

			case GamePhase.Action1:
				if (CharacterManager.instance.HasAllCharactersFinishedActions())
				{
					
					lastTransitionTick = Tick;
					currentPhase = GamePhase.Action2;
					phaseText.text = "Playing :";
					CharacterManager.instance.RestForAction();
					SetState(currentPhase);
				}
				
				
				break;
			case GamePhase.Action2:
				if (CharacterManager.instance.HasAllCharactersFinishedActions())
				{

					lastTransitionTick = Tick;
					currentPhase = GamePhase.Planning;
					phaseText.text = "Planning :";
					startPlanning?.Invoke();
					SetState(currentPhase);
				}


				break;

			default:
				break;
		}
	}
	public void StartPlay()
    {
		

	}
	[ClientRpc]
	public void SetState(GamePhase gamePhase)
    {
		currentPhase = gamePhase;
		switch (currentPhase)
		{
			case GamePhase.Action1:


				phaseText.text = "Action 1 :";
				startPlaying?.Invoke();
					

				break;

			case GamePhase.Action2:

				phaseText.text = "Action 2 :";

				break;
			case GamePhase.Planning:
				
					phaseText.text = "Planning :";
					startPlanning?.Invoke();
				


				break;

			default:
				break;
		}
	}
}