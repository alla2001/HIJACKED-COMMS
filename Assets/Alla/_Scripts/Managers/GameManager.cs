using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;
public class GameManager : NetworkBehaviour
{
	public int planningTime = 15;
	public UnityAction startPlaying;
	public UnityAction startPlanning;
	public TextMeshProUGUI phaseText;
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
		}
		NetworkManager.singleton.transport.OnServerConnected+= (e) => 
		{
			
			if (NetworkManager.singleton.numPlayers+1 >= playersNeeded && isServer)
			{
				RefrenceManager.tickManager.UnPause();
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
					phaseText.text = "Playing :";
					startPlaying?.Invoke();
				}

				break;

			case GamePhase.Action1:
				if (CharacterManager.instance.HasAllCharactersFinishedActions())
				{

					lastTransitionTick = Tick;
					currentPhase = GamePhase.Action2;
					phaseText.text = "Playing :";
					CharacterManager.instance.RestForAction();

				}
				
				
				break;
			case GamePhase.Action2:
				if (CharacterManager.instance.HasAllCharactersFinishedActions())
				{

					lastTransitionTick = Tick;
					currentPhase = GamePhase.Planning;
					phaseText.text = "Planning :";
					startPlanning?.Invoke();
				}


				break;

			default:
				break;
		}
	}
}