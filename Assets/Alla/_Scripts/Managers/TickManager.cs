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

		GameManager.startPlanning += () => { lastTick = currentTick; };
		GameManager.startPlaying += () => { lastTick = currentTick; };
		
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
	float amountToFill;
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
			 amountToFill = (float)(tick - lastTick) / (float)(RefrenceManager.gameManager.planningTime - 1);
		


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
    private void Update()
    {
		fill.fillAmount = Mathf.MoveTowards(fill.fillAmount, amountToFill, 0.5f* Time.deltaTime);
    }
    public void UnPause()
	{
		if(!isServer)return;
		print("UNPAUSED");
		StartCoroutine(Ticker());
		paused = false;
	}
	
}