using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public class RandomFallback : ControlNode
    {
        static Random rng = new Random();

        public RandomFallback(params Node[] children) : base(children)
        {
            ShuffleChildren();
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            foreach (Node child in children)
            {
                NodeState childState = child.Evaluate(data);
                if (childState == NodeState.Running)
                    return childState;
                else if (childState == NodeState.Success)
                {
                    ShuffleChildren();
                    return childState;
                }
            }

            ShuffleChildren();
            return NodeState.Failure;
        }

        private void ShuffleChildren()
        {
            List<Node> newChildren = new List<Node>();

            while (children.Count > 0)
            {
                int i = rng.Next(children.Count);
                newChildren.Add(children[i]);
                children.RemoveAt(i);
            }

            children = newChildren;
        }

        public override object Clone()
        {
            List<Node> children = new List<Node>();
            foreach (Node child in this.children)
                children.Add((Node)child.Clone());

            return new RandomFallback(children.ToArray());
        }
    }
}
