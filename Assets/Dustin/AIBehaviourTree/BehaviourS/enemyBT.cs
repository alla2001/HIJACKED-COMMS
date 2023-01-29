using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class enemyBT : BehaveTree
{

    public Character chillum;
    public int rangeForDetection;
    public int Randomness;
  
    protected override BehaviourNode SetupTree()
    {
        BehaviourNode root = new BehaviourSelector(new List<BehaviourNode>
        {
            /*new BehaviourSequence(new List<BehaviourNode>{
            new taskWaitForEnemy(chillum,rangeForDetection),
            }),*/
            new BehaviourSequence(new List<BehaviourNode>
            {

                new taskCheckForInCover(chillum),
                new taskCheckForTargetOutOfCover(chillum),

            }),
           // new taskIdle(),
        }) ;

        return root;
    }
}