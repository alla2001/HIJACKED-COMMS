using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class enemyBT : BehaveTree
{

    public Character chillum;
    public int rangeForDetection;
    public int Randomness;
    BehaviourNode root;
    protected override BehaviourNode SetupTree()
    {
         root = new BehaviourSelector(new List<BehaviourNode>
         {
            new BehaviourSequence(new List<BehaviourNode>{
                new taskWaitForEnemy(chillum,rangeForDetection),
                new BehaviourSequence(new List<BehaviourNode>
                {

                    new taskCheckForInCover(chillum),
                    new taskCheckForTargetOutOfCover(chillum),

                }),
            }),
            
         }) ;

        return root;
    }
    private void Update()
    {
        base.Update();
        if (root.state != BehaviourNodeState.RUNNING)
        {
            chillum.ready = true;
        }
    }
}
