using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTreeColin
{
    public class LaserShot : LeafNode
    {
        float rotationSpeed, rotationMargin, shotDuration;
        Vector2 minRotation, maxRotation;
        bool shooting = false;
        Vector2 currentRotation = Vector2.zero;

        float shotTimer = 0;

        public LaserShot(float rotationSpeed, float rotationMargin, Vector2 minRotation, Vector2 maxRotation, float shotDuration)
        {
            this.rotationSpeed = rotationSpeed;
            this.rotationMargin = rotationMargin;
            this.minRotation = minRotation;
            this.maxRotation = maxRotation;
            this.shotDuration = shotDuration;
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            Vector2 targetRotation = Vector2.zero;

            if ((bool)data["canShoot"] && !(bool)data["playerOnBoss"])
            {
                targetRotation.x = Mathf.Rad2Deg * -Mathf.Asin((((Transform)data["playerTransform"]).position - ((Transform)data["coreTarget"]).position).normalized.y);
                targetRotation.y = Mathf.Rad2Deg * Mathf.Asin((((Transform)data["bossTransform"]).worldToLocalMatrix * ((Transform)data["playerTransform"]).position).normalized.x);
            }


            if (shooting)
            {
                shotTimer += Time.deltaTime;
                if (shotTimer >= shotDuration)
                {
                    shooting = false;
                    shotTimer = 0;
                    ((Transform)data["coreTarget"]).GetChild(1).gameObject.SetActive(false);
                    return NodeState.Success;
                }
            }
            else
            {
                Vector2 rotationDiff = targetRotation - currentRotation;

                if (Mathf.Abs(rotationDiff.y) < rotationMargin && Mathf.Abs(rotationDiff.x) < rotationMargin)
                {
                    shooting = true;
                    ((Transform)data["coreTarget"]).GetChild(1).gameObject.SetActive(true);
                }

                if (rotationDiff.y < 0)
                    currentRotation.y += Mathf.Max(-rotationSpeed * Time.deltaTime, rotationDiff.y);
                else
                    currentRotation.y += Mathf.Min(rotationSpeed * Time.deltaTime, rotationDiff.y);
                currentRotation.y = Mathf.Clamp(currentRotation.y, minRotation.y, maxRotation.y);
                ((Transform)data["coreTarget"]).parent.localRotation = Quaternion.Euler(0, currentRotation.y, 0);

                if (rotationDiff.x < 0)
                    currentRotation.x += Mathf.Max(-rotationSpeed * Time.deltaTime, rotationDiff.x);
                else
                    currentRotation.x += Mathf.Min(rotationSpeed * Time.deltaTime, rotationDiff.x);
                currentRotation.x = Mathf.Clamp(currentRotation.x, minRotation.y, maxRotation.y);
                ((Transform)data["coreTarget"]).GetChild(1).localRotation = Quaternion.Euler(currentRotation.x, 0, 0);
            }

            return NodeState.Running;
        }

        public override object Clone()
        {
            return new LaserShot(rotationSpeed, rotationMargin, minRotation, maxRotation, shotDuration);
        }
    }
}
