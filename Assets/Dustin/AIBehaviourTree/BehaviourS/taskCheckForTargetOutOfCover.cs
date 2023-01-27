using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class taskCheckForTargetOutOfCover : BehaviourNode
{
    Character charac;
    List<Character> attackOrder;
    public taskCheckForTargetOutOfCover(Character c)
    {
        charac = c;
        
    }
    public override BehaviourNodeState Evaluate()
    {
        
        if (RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action1 || RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2 || charac.actionPointsLeft <= 0)
        {
            state = BehaviourNodeState.FAILURE;
            return state;
        }
        attackOrder = checkForAllAttackble(charac);
        if (attackOrder.Count <= 0)
        {
            state = BehaviourNodeState.FAILURE;
            return state;
        }
        Shoot shoot = new Shoot();
        Vector2Int randomPosition = attackOrder[Random.Range(0, attackOrder.Count)].posOnGrid;
        shoot.targetCell = randomPosition;
        charac.AddAction(shoot);
        state = BehaviourNodeState.SUCCESS;
        return state;

    }

    public List<Character> checkForAllAttackble(Character c)
    {
        List<Character> goodTargets = new List<Character>();
        List<Character> otherTargets = new List<Character>();
        List<Character> badTargets = new List<Character>();
        List<Character> allCharacters = new List<Character>();
        Character[] allCharactersArray = GameObject.FindObjectsOfType<Character>();
        List<Vector2Int> goodPositionsList = new List<Vector2Int>();
        for (int i = 0; i < allCharactersArray.Length; i++)
        {
            if (allCharactersArray[i].gameObject.GetComponent<PlayerSetup>() != null)
            {
                if (allCharactersArray[i].gameObject.GetComponent<PlayerSetup>().character != null)
                {


                    allCharacters.Add(allCharactersArray[i]);
                }
            }

        }
        foreach (Character target in allCharacters)
        {
            if (Vector2Int.Distance(charac.posOnGrid, target.posOnGrid) > charac.stats.Range)
            {
                badTargets.Add(target); ;
            }
            else
            {
                List<Obstical> obsticals = new List<Obstical>();
                foreach (Vector2Int direction in GridManager.instance.grid.directions)
                {
                    if (Obstical.IsObstacl(target.posOnGrid + direction))
                    {
                        obsticals.Add(Obstical.GetObstacl(target.posOnGrid + direction));
                    }
                }
                if (obsticals.Count <= 0)
                {
                    goodTargets.Add(target);
                }
                else
                {
                    foreach (Obstical cover in obsticals)
                    { //(Vector2Int cell,Vector2Int coverPoint)
                        Vector2Int direction = cover.posOnGrid - charac.posOnGrid;
                        Vector2Int coverDirection = target.posOnGrid - cover.posOnGrid;

                        if (Vector2.Angle(direction, coverDirection) >= 45)
                        {
                            goodTargets.Add(target);
                        }
                        else
                        {
                            otherTargets.Add(target);
                        }
                    }
                }
            }

        }
        if (goodTargets.Count > 0) return goodTargets;
        else return otherTargets;
    }
}
