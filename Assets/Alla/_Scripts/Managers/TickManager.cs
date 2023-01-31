using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;
using UnityEngine.UI;
using System;

public class TickManager : NetworkBehaviour
{
	public float tickTimeinSecond = 1f;
	private bool _paused;
	public bool paused
	{ get { return _paused; } private set { _paused = value; } }
	public UnityAction<int> Tick;
	[SerializeField] private int currentTick;
	int lastTick;
	public TextMeshProUGUI timer;
	public Image fill;

	private void Awake()
    {
		if (RefrenceManager.tickManager == null)
			RefrenceManager.tickManager = this;
		else
		Destroy(this);
	}
    public void Start()
	{
		//start Ticking;

		RefrenceManager.gameManager.startPlanning += () => { lastTick = currentTick; };
		RefrenceManager.gameManager.startPlaying += () => { lastTick = currentTick; };
		
	}

	public IEnumerator Ticker()
	{
		yield return new WaitForSeconds(tickTimeinSecond);

		currentTick += 1;
		UpdateTimer(currentTick);
		RefrenceManager.gameManager.CheckTransferState(currentTick);

		StartCoroutine(Ticker());
	}
	
	public void Pause()
	{
		StopAllCoroutines();
		paused = true;
	}
	[ClientRpc]
	public void UpdateTimer(int tick)
    {
		currentTick = tick ;
		if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Planning)
        {
			timer.text =(RefrenceManager.gameManager.planningTime- (tick - lastTick)).ToString();
            if (fill.fillAmount == 1)
            {
				fill.fillAmount = 0;
            }
			float amountToFill = (float)(tick - lastTick) / (float)(RefrenceManager.gameManager.planningTime - 1);
			StartCoroutine("fillImage", amountToFill);


		}

		try
		{
			Tick?.Invoke(tick);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		timer.text = (tick - lastTick).ToString();


	}
	public void UnPause()
	{
		if(!isServer)return;
		print("UNPAUSED");
		StartCoroutine(Ticker());
		paused = false;
	}
	public IEnumerator fillImage(float f)
    {
		float difference = (float)(currentTick - lastTick) / (float)(RefrenceManager.gameManager.planningTime - 1) - fill.fillAmount;
		difference /= 50;
		for(int i = 0; i < 50; i++)
        {
			fill.fillAmount += difference;
			yield return new WaitForSeconds(0.02f);
			
		}
	}
}