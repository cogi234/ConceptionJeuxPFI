using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ConstantDamageTrigger : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<DamageableComponent>() != null && other.gameObject.name != "Core")
        {
            other.GetComponent<DamageableComponent>().TakeDamage(damage);
        }
    }
}
