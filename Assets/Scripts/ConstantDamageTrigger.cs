using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ConstantDamageTrigger : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<DamageableComponent>().TakeDamage(damage);
        }
    }
}
