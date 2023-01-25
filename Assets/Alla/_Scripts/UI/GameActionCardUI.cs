using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionCardUI : MonoBehaviour
{
    public GameAction action;
    public Character owner;
    public GameObject removeBtn;
    private void Start()
    {
        RefrenceManager.gameManager.startPlanning +=Kill;
        RefrenceManager.gameManager.startPlaying +=OnPlay;
    }
    void Kill()
    {
        RefrenceManager.gameManager.startPlanning -= Kill;
        RefrenceManager.gameManager.startPlaying -= OnPlay;
        Destroy(gameObject);
    }
    public void OnPlay()
    {
        removeBtn.SetActive(false);
    }
    public void RemoveCard()
    {
        ActionManager.instance.RemoveAction(this);
        Kill();
        
    }
    private void OnDestroy()
    {
        Kill();
    }
}
