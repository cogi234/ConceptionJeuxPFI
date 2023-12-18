using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace BehaviourTreeColin
{
    public class MoveToPlayer : LeafNode
    {
        bool advancing = false;

        public MoveToPlayer()
        {
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
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
            //Advance if not in ideal range
            float distance = Vector3.Distance(((Transform)data["playerTransform"]).position, ((Transform)data["bossTransform"]).position);

            if (advancing)
                ((Transform)data["bossTransform"]).Translate(Vector3.forward * ((Boss1Controller)data["bossController"]).movementSpeed * Time.deltaTime, Space.Self);

            if (!advancing && distance >= 20)
            {
                //Advance
                ((Transform)data["body"]).GetComponent<Animator>().SetFloat("Speed", ((Boss1Controller)data["bossController"]).movementSpeed);
                advancing = true;

            } else if (advancing && distance <= 5)
            {
                ((Transform)data["body"]).GetComponent<Animator>().SetFloat("Speed", 0);
                advancing = false;
            }

            return NodeState.Running;
        }

        public override object Clone()
        {
            return new MoveToPlayer();
        }
    }
}
