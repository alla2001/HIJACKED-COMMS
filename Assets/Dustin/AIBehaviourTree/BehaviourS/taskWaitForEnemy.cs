using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
public class taskWaitForEnemy : BehaviourNode
{
    Character charac;
    int range;
    bool detected = false;
    public taskWaitForEnemy(Character c, int i)
    {
        charac = c;
        range = i;
    }

    public override BehaviourNodeState Evaluate()
    {
        if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action1 || RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2)
        {

            state = BehaviourNodeState.FAILURE;
            return state;
        }
        if (detected)
        {
            state = BehaviourNodeState.SUCCESS;
            return state;
        }
        if (checkForEnemies())
        {
            detected = true;
            state = BehaviourNodeState.SUCCESS;
            return state;
        }
        else
        {
            state = BehaviourNodeState.FAILURE;
            return state;
        }


    }
    public bool checkForEnemies()
    {
        Character[] allChars = Object.FindObjectsOfType<Character>();
        for (int i = 0; i < allChars.Length; i++)
        {
            Vector2Int testVector = allChars[i].GetComponent<Character>().posOnGrid;
            if (Mathf.Abs(testVector.x - charac.posOnGrid.x) <= range && allChars[i].gameObject.GetComponent<PlayerSetup>()!=null)
            {
                if (Mathf.Abs(testVector.y - charac.posOnGrid.y) <= range)
                {
                    return true;
                }
            }


        }
        return false;
    }

}