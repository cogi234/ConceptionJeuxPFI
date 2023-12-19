using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius, force, upModifier = 0;

    float lifetime = 4;

    private IEnumerator Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, force);
        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.AddExplosionForce(force, transform.position, radius, upModifier);
                if (hit.GetComponent<PlayerController>() != null)
                    hit.GetComponent<PlayerController>().ignoreGrounded = true;
            }
        }
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
