using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
public class taskIdle : BehaviourNode
{

    public taskIdle()
    {

    }

    public override BehaviourNodeState Evaluate()
    {
        
        if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action1 || RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2)
        {
            state = BehaviourNodeState.SUCCESS;
            return state;
        }
        state = BehaviourNodeState.RUNNING;
        return state;
        
    }
}
