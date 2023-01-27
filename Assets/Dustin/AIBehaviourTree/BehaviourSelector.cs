using System.Collections.Generic;

namespace BehaviourTree
{
    public class BehaviourSelector : BehaviourNode
    {
        public BehaviourSelector() : base() { }
        public BehaviourSelector(List<BehaviourNode> children) : base(children) { }
        public override BehaviourNodeState Evaluate()
        {
            foreach (BehaviourNode bNode in children)
            {
                switch (bNode.Evaluate())
                {
                    case BehaviourNodeState.FAILURE:
                        continue;
                    case BehaviourNodeState.SUCCESS:
                        state = BehaviourNodeState.SUCCESS;
                        return state;
                    case BehaviourNodeState.RUNNING:
                        state = BehaviourNodeState.RUNNING;
                        return state;
                    default:
                        return state;
                }
            }
            state = BehaviourNodeState.FAILURE;
            return state;
        }
    }
}
