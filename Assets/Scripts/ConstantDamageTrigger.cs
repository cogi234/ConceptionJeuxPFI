using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDamageTrigger : MonoBehaviour
{
    [SerializeField] float damageInterval;
    [SerializeField] int damagePerInterval;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > damageInterval)
        {
            timer -= damageInterval;

            RaycastHit[] hits = Physics.SphereCastAll(transform.parent.position, 0.75f, transform.parent.forward, 200);
            
            foreach (RaycastHit hit in hits)
            {
               if (hit.collider.GetComponent<DamageableComponent>() == null)
                    continue;

                Debug.Log($"Take damage {hit.collider.gameObject.name}!");
                hit.collider.GetComponent<DamageableComponent>().TakeDamage(damagePerInterval);
            }
        }
    }
}
