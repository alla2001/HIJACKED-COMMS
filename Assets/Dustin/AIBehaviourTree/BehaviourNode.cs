using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourNodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class BehaviourNode
    {
        protected BehaviourNodeState state;

        public BehaviourNode parent;
        protected List<BehaviourNode> children = new List<BehaviourNode>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        public BehaviourNode()
        {
            parent = null;
        }
        public BehaviourNode(List<BehaviourNode> children)
        {
            foreach (BehaviourNode child in children)
            {
                _Attach(child);
            }
        }
        private void _Attach(BehaviourNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual BehaviourNodeState Evaluate() => BehaviourNodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            BehaviourNode bNode = parent;
            while (bNode != null) {
                value = bNode.GetData(key);
                if (value != null) return value;
                bNode = bNode.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            BehaviourNode bNode = parent;
            while (bNode != null)
            {
                bool cleared = bNode.ClearData(key);
                if (cleared)
                    return true;
                bNode = bNode.parent;
            }
            return false;
        }
    }
}