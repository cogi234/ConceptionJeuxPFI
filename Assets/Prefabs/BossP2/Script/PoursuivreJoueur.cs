using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoursuivreJoueur : MonoBehaviour
{
   GameObject player;
    NavMeshAgent agent;
    bool activer = false;
    // Start is called before the first frame update
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
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
}
