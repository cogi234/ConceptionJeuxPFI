using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDamageTrigger : MonoBehaviour
{
    [SerializeField] float damageInterval;
    [SerializeField] int damagePerInterval;

    float timer;
    Collider myCollider;
    List<GameObject> overlappingObjects;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        overlappingObjects = new List<GameObject>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > damageInterval)
        {
            timer -= damageInterval;
            
            foreach (GameObject obj in overlappingObjects)
            {
                if (obj == null || obj.GetComponent<DamageableComponent>() == null)
                    continue;

                obj.GetComponent<DamageableComponent>().TakeDamage(damagePerInterval);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        overlappingObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        overlappingObjects.Remove(other.gameObject);
    }
}
