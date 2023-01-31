using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionCardUI : MonoBehaviour
{
    public GameAction action;
    public Character owner;
    public GameObject removeBtn;
    public static List<GameActionCardUI> cards= new List<GameActionCardUI> ();
    private void Start()
    {
        GameManager.startPlanning +=Kill;
        GameManager.startPlaying +=OnPlay;
        cards.Add(this);
    }
    public void Kill()
    {
        GameManager.startPlanning -= Kill;
        GameManager.startPlaying -= OnPlay;
        cards.Remove(this);
        Destroy(gameObject);
    }
    public void OnPlay()
    {
       
    }
    public void RemoveCard()
    {
        GameActionManager.instance.RemoveAction(this);
        Kill();
        
    }
    private void OnDestroy()
    {
        Kill();
    }
}
