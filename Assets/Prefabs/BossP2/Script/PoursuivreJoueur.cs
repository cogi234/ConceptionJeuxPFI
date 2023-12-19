using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoursuivreJoueur : MonoBehaviour
{
   GameObject player;
    NavMeshAgent agent;
    bool activer = false;

    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        GetComponent<DamageableComponent>().onDamage.AddListener(TakeDamage);
    }

    void Update()
    {
        if (activer)
        {
            
        }
        agent.destination = player.transform.position;
    }
    public void destination()
    {
        activer = true;
    }

    public void TakeDamage(int dommage)
    {
        gameObject.SetActive(false);
    }
}
