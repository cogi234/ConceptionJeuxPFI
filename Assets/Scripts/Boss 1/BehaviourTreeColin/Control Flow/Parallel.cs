using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public class Parallel : ControlNode
    {
        int treshold;

        /// <param name="treshold">How many successes for the result to be a success</param>
        public Parallel(int treshold ,params Node[] children) : base(children)
        {
            this.treshold = treshold;
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            int successCount = 0, failureCount = 0;

            foreach (Node child in children)
            {
                NodeState childState = child.Evaluate(data);
                switch (childState)
                {
                    case NodeState.Success:
                        successCount++;
                        break;
                    case NodeState.Failure:
                        failureCount++;
                        break;
                    default:
                        break;
                }
            }

            if (successCount >= treshold)
                return NodeState.Success;
            else if (failureCount > children.Count - treshold)
                return NodeState.Failure;
            else
                return NodeState.Running;
        }

        public override object Clone()
        {
            List<Node> children = new List<Node>();
            foreach (Node child in this.children)
                children.Add((Node)child.Clone());

            return new Parallel(treshold, children.ToArray());
        }
    }
}
