using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BehaviourTreeColin
{
    public class DataCooldown : DecoratorNode
    {
        string parameter;

        public DataCooldown(Node child, string parameter) : base(child) { this.parameter = parameter; }

        /// <summary>
        ///  The child should set the cooldown when they end
        /// </summary>
        /// <returns></returns>
        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            if ((float)data[parameter] <= 0)
            {
                NodeState childState = children[0].Evaluate(data);
                return childState;
            }

            data[parameter] = (float)data[parameter] - Time.deltaTime;
            return NodeState.Failure;
        }

        public override object Clone()
        {
            return new DataCooldown((Node)children[0].Clone(), parameter);
        }
    }
}
