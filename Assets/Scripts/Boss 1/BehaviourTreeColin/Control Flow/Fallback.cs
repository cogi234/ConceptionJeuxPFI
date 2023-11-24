using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public class Fallback : ControlNode
    {
        public Fallback(params Node[] children) : base(children) { }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            foreach (Node child in children)
            {
                NodeState childState = child.Evaluate(data);
                if (childState != NodeState.Failure)
                {
                    return childState;
                }
            }

            return NodeState.Failure;
        }

        public override object Clone()
        {
            List<Node> children = new List<Node>();
            foreach (Node child in this.children)
                children.Add((Node)child.Clone());

            return new Fallback(children.ToArray());
        }
    }
}
