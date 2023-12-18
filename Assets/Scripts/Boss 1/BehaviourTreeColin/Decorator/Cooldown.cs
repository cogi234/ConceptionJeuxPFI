using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BehaviourTreeColin
{
    public class Cooldown : DecoratorNode
    {
        float cooldown;
        float timer;

        public Cooldown(Node child, float cooldown) : base(child) { this.cooldown = cooldown; }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            if (timer <= 0)
            {
                NodeState childState = children[0].Evaluate(data);
                if (childState == NodeState.Success)
                    timer = cooldown;
                return childState;
            }

            timer -= Time.deltaTime;
            return NodeState.Failure;
        }

        public override object Clone()
        {
            return new Cooldown((Node)children[0].Clone(), cooldown);
        }
    }
}
