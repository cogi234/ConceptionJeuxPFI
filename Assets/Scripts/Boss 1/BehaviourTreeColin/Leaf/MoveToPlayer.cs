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
            ((Transform)data["bossTransform"]).rotation = Quaternion.LookRotation(newDirection, Vector3.up);

            //Advance if not in ideal range
            float distance = Vector3.Distance(((Transform)data["playerTransform"]).position, ((Transform)data["bossTransform"]).position);

            if (advancing)
                ((Transform)data["bossTransform"]).Translate(Vector3.forward * ((Boss1Controller)data["bossController"]).movementSpeed * Time.deltaTime, Space.Self);

            if (!advancing && distance >= 25)
            {
                //Advance
                ((Transform)data["body"]).GetComponent<Animator>().SetFloat("Speed", 1);
                advancing = true;

            } else if (advancing && distance <= 10)
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
