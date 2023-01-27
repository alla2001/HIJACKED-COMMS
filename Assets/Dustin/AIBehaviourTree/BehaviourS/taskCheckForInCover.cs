using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using BehaviourTree;

public class taskCheckForInCover : BehaviourNode
{
    public List<Vector2Int> allMovable = new List<Vector2Int>();
    Character charac;
    public taskCheckForInCover(Character c)
    {

        charac = c;
        
    }

    public override BehaviourNodeState Evaluate()
    {
       
        if (RefrenceManager.gameManager.currentPhase==GameManager.GamePhase.Action1 || RefrenceManager.gameManager.currentPhase == GameManager.GamePhase.Action2 ||charac.actionPointsLeft<=1)
        {
            state = BehaviourNodeState.RUNNING;
            return state;
        }
        if (!inCover())
        {
            allMovable = checkMovable(charac.posOnGrid);
            allMovable = goodPositions(allMovable);
        }
        else
        {
            state = BehaviourNodeState.SUCCESS;
            return state;
        }//else if random skip

        Move moveaction = new Move();
        if (allMovable.Count > 0)
        {


            Vector2Int randomPosition = allMovable[Random.Range(0, allMovable.Count)];
            if (randomPosition != null)
            {
                moveaction.startPosition = charac.posOnGrid;
                moveaction.targetPosition = randomPosition;
                charac.AddAction(moveaction);
            }
        }
        state = BehaviourNodeState.FAILURE;
        return state;
    }
    private List<Vector2Int> checkMovable(Vector2Int v2)
    {
        List<Vector2Int> tmpV2List = new List<Vector2Int>();

        //suboptimal lol

        for (int i = -charac.stats.Movement; i <= charac.stats.Movement; i++)
        {

            for (int j = -charac.stats.Movement; j <= charac.stats.Movement; j++)
            {

                Vector2Int tmpV2 = new Vector2Int(i + charac.posOnGrid.x, j + charac.posOnGrid.y);
                if (Mathf.Abs(i) + Mathf.Abs(j) <= charac.stats.Movement)
                {

                    if (!tmpV2List.Contains(tmpV2))
                    {

                        if (!Obstical.IsObstacl(tmpV2))
                        {
                            List<Obstical> obsticals = new List<Obstical>();
                            foreach (Vector2Int direction in GridManager.instance.grid.directions)
                            {
                                if (Obstical.IsObstacl(tmpV2 + direction))
                                {
                                    obsticals.Add(Obstical.GetObstacl(tmpV2 + direction));


                                }
                            }
                            if (obsticals.Count > 0) tmpV2List.Add(tmpV2);
                        }

                    }
                }
            }
        }

        return tmpV2List;
    }

    private List<Vector2Int> goodPositions(List<Vector2Int> listV2)
    {

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
        foreach (Vector2Int v2 in listV2)
        {
            List<Obstical> obsticals = new List<Obstical>();
            foreach (Vector2Int direction in GridManager.instance.grid.directions)
            {
                if (Obstical.IsObstacl(v2 + direction))
                {
                    obsticals.Add(Obstical.GetObstacl(v2 + direction));
                }
            }
            foreach (Character c in allCharacters)
            {
                Vector2Int shooterPosition = c.posOnGrid;

                foreach (Obstical cover in obsticals)
                {
                    Vector2Int direction = cover.posOnGrid - shooterPosition;
                    Vector2Int coverDirection = v2 - cover.posOnGrid;
                    float d1 = Vector2Int.Distance(shooterPosition, cover.posOnGrid);
                    float d2 = Vector2Int.Distance(shooterPosition, v2);

                    if ((Vector2.Angle(direction, coverDirection) <= 45) && d1 < d2)
                    {
                        goodPositionsList.Add(v2);
                    }
                }
            }


        }
        return goodPositionsList;
    }

    private bool inCover()
    {
        List<Character> allCharacters = new List<Character>();
        Character[] allCharactersArray = GameObject.FindObjectsOfType<Character>();
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
        List<Obstical> obsticals = new List<Obstical>();
        foreach (Vector2Int direction in GridManager.instance.grid.directions)
        {
            if (Obstical.IsObstacl(charac.posOnGrid + direction))
            {
                obsticals.Add(Obstical.GetObstacl(charac.posOnGrid + direction));
            }
        }
        foreach (Character c in allCharacters)
        {
            Vector2Int shooterPosition = c.posOnGrid; // = c.gridpos

            foreach (Obstical cover in obsticals)
            {
                Vector2Int direction = cover.posOnGrid - shooterPosition;
                Vector2Int coverDirection = charac.posOnGrid - cover.posOnGrid;
                float d1 = Vector2Int.Distance(shooterPosition, cover.posOnGrid);
                float d2 = Vector2Int.Distance(shooterPosition, charac.posOnGrid);

                if ((Vector2.Angle(direction, coverDirection) <= 45) && d1 < d2)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

    

