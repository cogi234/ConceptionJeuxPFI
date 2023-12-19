using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndeChocDommage : MonoBehaviour
{
    [SerializeField] int degat;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<DamageableComponent>().TakeDamage(degat);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
