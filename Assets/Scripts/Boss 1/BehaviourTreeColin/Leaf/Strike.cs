using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace BehaviourTreeColin
{
    public class Strike : LeafNode
    {
        float timer = 0f;
        bool started = false;
        bool aiming = false;

        string name;
        float offset;

        public Strike(string name, float offset)
        {
            this.name = name;
            this.offset = offset;
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            //We only start aiming if the player is at the right distance
            if (!started)
            {
                float distance = Vector3.Distance(((Transform)data["playerTransform"]).position, ((Transform)data["bossTransform"]).position);
                if (distance > 5 && distance < 20)
                {
                    data["currentAttack"] = $"Strike{name}";
                    started = true;
                    aiming = true;
                }
                else
                    return NodeState.Failure;
            }

            if (aiming)
            {
                //Aim for the player
                Vector3 directionToPlayer = (((Transform)data["playerTransform"]).position + new Vector3(offset, 0, 0)) - ((Transform)data["bossTransform"]).position;
                Vector3 newDirection = Vector3.RotateTowards(((Transform)data["bossTransform"]).forward, directionToPlayer, ((Boss1Controller)data["bossController"]).rotationSpeed * Time.deltaTime, 0);
                ((Transform)data["bossTransform"]).rotation = Quaternion.LookRotation(newDirection, Vector3.up);

                if (Vector3.Angle(((Transform)data["bossTransform"]).forward, directionToPlayer) < 5f)
                {
                    aiming = false;
                    ((Transform)data["body"]).GetComponent<Animator>().SetTrigger($"Strike {name}");
                }
            } else
            {
                timer += Time.deltaTime;

                if (timer >= 10)
                {
                    data["currentAttack"] = "";
                    data["attackCooldown"] = 10f;
                    timer = 0f;
                    started = false;
                    return NodeState.Success;
                }
            }

            return NodeState.Running;
        }

        public override object Clone()
        {
            return new Strike(name, offset);
        }
    }
}
