using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace BehaviourTreeColin
{
    public class JumpToPlayer : LeafNode
    {
        float timer = 0f;
        bool started = false;
        bool jumping = false;

        Vector3 originalPosition;
        Vector3 targetPosition;

        public JumpToPlayer()
        {

        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            //We only start jumping if the player is far enough
            if (!started)
            {
                if (Vector3.Distance(((Transform)data["playerTransform"]).position, ((Transform)data["bossTransform"]).position) > ((Boss1Controller)data["bossController"]).jumpMinDistance)
                {
                    data["currentAttack"] = "JumpToPlayer";
                    data["canShoot"] = false;
                    started = true;
                }
                else
                    return NodeState.Failure;
            }

            if (jumping)
            {
                timer += Time.deltaTime;

                if (timer >= 2.5f && timer <= 5.5f)
                {
                    ((Transform)data["bossTransform"]).position = Vector3.Slerp(originalPosition, targetPosition, (timer - 2.5f) / 3);
                }

                if (timer >= 5.5f)
                {
                    ((Transform)data["bossTransform"]).position = targetPosition;
                    data["currentAttack"] = "";
                    data["canShoot"] = true;
                    data["attackCooldown"] = 10f + 0.75f;
                    timer = 0f;
                    started = false;
                    return NodeState.Success;
                }
            }
            else
            {
                //Aim for the player
                Vector3 directionToPlayer = ((Transform)data["playerTransform"]).position - ((Transform)data["bossTransform"]).position;
                Vector3 newDirection = Vector3.RotateTowards(((Transform)data["bossTransform"]).forward, directionToPlayer, ((Boss1Controller)data["bossController"]).rotationSpeed * Time.deltaTime, 0);
                ((Transform)data["bossTransform"]).rotation = Quaternion.LookRotation(newDirection);
                /*
                float targetRotation = Mathf.Rad2Deg * Mathf.Asin(((Transform)data["bossTransform"]).InverseTransformPoint(((Transform)data["playerTransform"]).position).normalized.x);
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
                    jumping = true;
                    ((Transform)data["body"]).GetComponent<Animator>().SetTrigger("Jump");
                    float jumpDistance = Mathf.Min(Vector3.Distance(((Transform)data["playerTransform"]).position, ((Transform)data["bossTransform"]).position), ((Boss1Controller)data["bossController"]).jumpMaxDistance);
                    originalPosition = ((Transform)data["bossTransform"]).position;
                    targetPosition = originalPosition + (((Transform)data["bossTransform"]).forward * jumpDistance);
                }
            }

            return NodeState.Running;
        }

        public override object Clone()
        {
            return new JumpToPlayer();
        }
    }
}
