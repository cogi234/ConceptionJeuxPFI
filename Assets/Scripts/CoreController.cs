using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionUpModifier;

    [SerializeField] GameObject explosionPrefab;

    private void Awake()
    {
        GetComponent<DamageableComponent>().onDamage.AddListener(Explode);
    }

    private void Explode(int damage)
    {
        Explosion explosion = Instantiate(explosionPrefab, transform.position, transform.rotation).GetComponent<Explosion>();
        explosion.force = explosionForce;
        explosion.radius = explosionRadius;
        explosion.upModifier = explosionUpModifier;
    }
}
