using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Character))]
public class AIBrain : Ticker
{
    public Character character;
    public Character target;
    public override void OnStartedPlanning()
    {


        
       
        if (Vector2Int.Distance(target.posOnGrid, character.posOnGrid) > character.stats.Range)
        {
       
            Move move = new Move {startPosition=character.posOnGrid,targetPosition=target.posOnGrid };
            
            character.AddActionMove(move);
           
        }
        else
        {
          
            Shoot shoot= new Shoot();
            shoot.targetCell=target.posOnGrid;
            character.AddActionShoot(shoot);
        }
    }

    public override void OnStartedPlaying()
    {
        
    }

    public override void OnTick(int Tick)
    {

    }

    private void Start()
    {
        base.Start();
        character = GetComponent<Character>();
    }


}
