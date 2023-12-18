using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTreeColin
{
    public class LaserSweep : LeafNode
    {
        float timer = 0f;

        public LaserSweep()
        {

        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            if (timer == 0f)
            {
                data["currentAttack"] = "LaserSweep";
                ((Transform)data["body"]).GetComponent<Animator>().SetTrigger("Laser Sweep");
                data["canShoot"] = false;
            } else if (timer >= 17f)
            {
                data["currentAttack"] = "";
                data["canShoot"] = true;
                data["attackCooldown"] = 10f;
                timer = 0f;
                return NodeState.Success;
            }

            timer += Time.deltaTime;

            return NodeState.Running;
        }

        public override object Clone()
        {
            return new LaserSweep();
        }
    }
}
