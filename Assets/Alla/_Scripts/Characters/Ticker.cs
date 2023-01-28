using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public abstract class Ticker : NetworkBehaviour
{
	public void Start()
	{
        if (isServer)
        {
			RefrenceManager.tickManager.Tick += OnTick;
		}
		
		RefrenceManager.gameManager.startPlanning += OnStartedPlanning;
		RefrenceManager.gameManager.startPlaying += OnStartedPlaying;


	}
    public void OnDestroy()
    {
		if (isServer)
		{
			RefrenceManager.tickManager.Tick -= OnTick;
		}
		RefrenceManager.gameManager.startPlanning -= OnStartedPlanning;
		RefrenceManager.gameManager.startPlaying -= OnStartedPlaying;
	}
    public abstract void OnStartedPlanning();
	public abstract void OnStartedPlaying();
	public abstract void OnTick(int Tick);
}