using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BehaviourTreeColin
{
    public class RandomCooldown : DecoratorNode
    {
        float maxCooldown, minCooldown;
        float timer;

        public RandomCooldown(Node child, float minCooldown, float maxCooldown) : base(child) { this.minCooldown = minCooldown; this.maxCooldown = maxCooldown; }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            if (timer <= 0)
            {
                NodeState childState = children[0].Evaluate(data);
                if (childState == NodeState.Success)
                    timer = Random.Range(minCooldown, maxCooldown);
                return childState;
            }

            timer -= Time.deltaTime;
            return NodeState.Failure;
        }

        public override object Clone()
        {
            return new RandomCooldown((Node)children[0].Clone(), minCooldown, maxCooldown);
        }
    }
}
