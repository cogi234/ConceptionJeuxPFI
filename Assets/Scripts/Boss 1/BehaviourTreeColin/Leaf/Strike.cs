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
                    data["canShoot"] = false;
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
                ((Transform)data["bossTransform"]).rotation = Quaternion.LookRotation(newDirection);

                /*
                float targetRotation = Mathf.Rad2Deg * Mathf.Asin((((Transform)data["bossTransform"]).worldToLocalMatrix * directionToPlayer).normalized.x);
                float currentRotation = ((Transform)data["bossTransform"]).rotation.eulerAngles.y;
                float rotationDifference;

                float y1 = currentRotation % 360, y2 = targetRotation % 360;
                if (Mathf.Abs(y2 - y1) < Mathf.Abs(y2 - y1 + 360) && Mathf.Abs(y2 - y1) < Mathf.Abs(y2 - y1 - 360))
                    rotationDifference = y2 - y1;
                else if (Mathf.Abs(y2 - y1 + 360) < Mathf.Abs(y2 - y1 - 360))
                    rotationDifference = y2 - y1 + 360;
                else
                    rotationDifference = y2 - y1 - 360;

                float rotationToDo;

                if (rotationDifference < 0)
                    rotationToDo = Mathf.Max(-((Boss1Controller)data["bossController"]).rotationSpeed * Time.deltaTime, rotationDifference);
                else
                    rotationToDo = Mathf.Min(((Boss1Controller)data["bossController"]).rotationSpeed * Time.deltaTime, rotationDifference);

                ((Transform)data["bossTransform"]).Rotate(new Vector3(0, rotationToDo, 0));
                */

                if (Vector3.Angle(((Transform)data["bossTransform"]).forward, directionToPlayer) < 1.5f)
                {
                    aiming = false;
                    ((Transform)data["body"]).GetComponent<Animator>().SetTrigger($"Strike {name}");
                }
            } else
            {
                timer += Time.deltaTime;

                if (timer >= 7)
                {
                    data["currentAttack"] = "";
                    data["canShoot"] = true;
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
