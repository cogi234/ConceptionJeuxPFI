using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            DamageableComponent dmg = other.GetComponent<DamageableComponent>();
            if (dmg != null)
                dmg.TakeDamage(damage);
        }
    }
}
