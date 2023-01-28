
using System.Collections.Generic;

namespace BehaviourTree
{
    public class BehaviourSequence : BehaviourNode
    {
        public BehaviourSequence() : base() { }
        public BehaviourSequence(List<BehaviourNode> children) : base(children) { }
       public override BehaviourNodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            foreach (BehaviourNode bNode in children)
            {
                switch(bNode.Evaluate())
                    {
                    case BehaviourNodeState.FAILURE:
                        state = BehaviourNodeState.FAILURE;
                        return state;
                    case BehaviourNodeState.SUCCESS:
                        continue;
                    case BehaviourNodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = BehaviourNodeState.SUCCESS;
                        return state;
                    }
            }
            state = anyChildIsRunning ? BehaviourNodeState.RUNNING : BehaviourNodeState.SUCCESS;
            return state;
        }
    }
}
