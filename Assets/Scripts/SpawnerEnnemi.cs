using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerEnnemi : MonoBehaviour
{
    [SerializeField] List<GameObject> ennemy;
    [SerializeField] GameObject[] spawns;
    [SerializeField] GameObject Player;
    [SerializeField] float delay = 5;
    int compteurEnnemie=0;
   public bool cinématique = false;
    NavMeshAgent navMeshAgent;
  

   
    void Update()
    {
        if (!cinématique)
        {


            if (delay <= 0)
            {
                SpawnEnnemy();

            }
            delay -= Time.deltaTime;
        }

    }
    void SpawnEnnemy()
    {
       
            GameObject ennemie = ObjectPool.objectPool.GetObject(ennemy[compteurEnnemie]);
        compteurEnnemie++;
        if (compteurEnnemie >= ennemy.Count)
        {
            compteurEnnemie = 0;
        }
        if (ennemie != null)
            {

                NavMeshAgent nav = ennemie.GetComponent<NavMeshAgent>();
            //NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            //int vertexindext = Random.Range(0, triangulation.vertices.Length);
            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(triangulation.vertices[vertexindext], out hit, 2f, 0)){
            //   nav.Warp(hit.position);
            //   
            //}
            ennemie.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;

              nav.Warp(ennemie.transform.position);
            nav.enabled = true;

            ennemie.SetActive(true);
            PoursuivreJoueur mettredestionaion=  ennemie.GetComponent<PoursuivreJoueur>();
            mettredestionaion.destination();
        }
           
            delay = 5;
        }
    }

