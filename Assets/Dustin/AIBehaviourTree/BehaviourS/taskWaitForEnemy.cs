using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
public class taskWaitForEnemy : BehaviourNode
{
    Character charac;
    int range;
    bool detected = true;
    public taskWaitForEnemy(Character c, int i)
    {
        charac = c;
    }

    public override BehaviourNodeState Evaluate()
    {
        if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action1 || RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2)
        {

            state = BehaviourNodeState.SUCCESS;
            return state;
        }
        if (checkForEnemies())
        {
            state = BehaviourNodeState.SUCCESS;
        }
        state = BehaviourNodeState.FAILURE;
        return state;

    }
    public bool checkForEnemies()
    {
        return true;
    }
}
