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
        RefrenceManager.gameManager.startPlaying += () => { removeBtn.SetActive(false); };
    }
    void Kill()
    {
        RefrenceManager.gameManager.startPlanning -= Kill;
        Destroy(gameObject);
    }
    public void RemoveCard()
    {
        ActionManager.instance.RemoveAction(this);
        Destroy(gameObject);
    }
}
