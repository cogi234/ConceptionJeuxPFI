using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public class Inverter : DecoratorNode
    {
        public Inverter(Node child) : base(child) { }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            NodeState childState = children[0].Evaluate(data);

            switch (childState)
            {
                case NodeState.Success:
                    return NodeState.Failure;
                case NodeState.Failure:
                    return NodeState.Success;
            }

            return childState;
        }

        public override object Clone()
        {
            return new Inverter((Node)children[0].Clone());
        }
    }
}
