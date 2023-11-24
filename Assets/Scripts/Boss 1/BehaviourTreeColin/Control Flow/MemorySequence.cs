using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public class MemorySequence : ControlNode
    {
        bool[] childrenProcessed;

        public MemorySequence(params Node[] children) : base(children)
        {
            childrenProcessed = new bool[children.Length];
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (!childrenProcessed[i])
                {
                    NodeState childState = children[i].Evaluate(data);
                    if (childState == NodeState.Running)
                        return childState;
                    else if (childState == NodeState.Failure)
                    {
                        ResetProcessed();
                        return childState;
                    }
                    else
                        childrenProcessed[i] = true;
                }
            }

            ResetProcessed();
            return NodeState.Success;
        }

        private void ResetProcessed()
        {
            for (int i = 0; i < childrenProcessed.Length; i++)
                childrenProcessed[i] = false;
        }

        public override object Clone()
        {
            List<Node> children = new List<Node>();
            foreach (Node child in this.children)
                children.Add((Node)child.Clone());

            return new MemorySequence(children.ToArray());
        }
    }
}
