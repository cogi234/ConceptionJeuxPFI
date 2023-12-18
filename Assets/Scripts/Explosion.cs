using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius, force, upModifier = 0;

    float lifetime = 4;

    private void Awake()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, force);
        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(force, transform.GetChild(0).position, radius, upModifier);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
