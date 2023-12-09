using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerEnnemi : MonoBehaviour
{
    [SerializeField] GameObject ennemy;
    [SerializeField] GameObject[] spawns;
    [SerializeField] GameObject Player;
    float delay = 5;
  
    NavMeshAgent navMeshAgent;
    float lastSpeed = 0.2f;

   
    void Update()
    {
        SpawnEnnemy();
        delay -= Time.deltaTime;

    }
    void SpawnEnnemy()
    {
        if (delay <= 0)
        {
            GameObject tempEnnemy = ObjectPool.objectPool.GetObject(ennemy);
            
            if (tempEnnemy != null)
            {

                NavMeshAgent nav = tempEnnemy.GetComponent<NavMeshAgent>();
                tempEnnemy.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
               
                nav.enabled = true;
                tempEnnemy.SetActive(true);
            }
            lastSpeed += 0.1f;
            delay = 5;
        }
    }
}
