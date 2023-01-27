using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeAI : Ticker
{
    private Character character;


    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public enum FakeActionEnum
    {
        Move,Shoot
    }
    [System.Serializable]
     public class FakeAction
    {
        public Vector2Int pos;
        public FakeActionEnum action;
    }
    public List<FakeAction> fakeActions ;
    int index=0;
    public override void OnStartedPlanning()
    {




        
    }

    public override void OnStartedPlaying()
    {
        switch (fakeActions[index].action)
        { case FakeActionEnum.Shoot:
                character.AddActionShoot(new Shoot { targetCell = fakeActions[index].pos });
                break;
            case FakeActionEnum.Move:
                character.AddActionMove(new Move { startPosition=character.posOnGrid,targetPosition= fakeActions[index].pos });
                break;
         
        }
        index++;
    }

    public override void OnTick(int Tick)
    {
        //throw new System.NotImplementedException();
    }
}
