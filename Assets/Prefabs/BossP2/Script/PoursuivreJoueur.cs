using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoursuivreJoueur : MonoBehaviour
{
   GameObject player;
    NavMeshAgent agent;
    bool activer = false;
    int degat = 1;

    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        GetComponent<DamageableComponent>().onDamage.AddListener(TakeDamage);
    }
    bool mort = false;
    void Update()
    {
        if (!mort)
        {
            agent.destination = player.transform.position;
        }
       
    }
    public void destination()
    {
        activer = true;
    }

    public void TakeDamage(int dommage)
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        mort = true;
        GetComponent<AudioSource>().Play();
        agent.enabled = false;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<DamageableComponent>().TakeDamage(degat);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
