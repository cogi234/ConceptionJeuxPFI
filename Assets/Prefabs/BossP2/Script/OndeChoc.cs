using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndeChoc : MonoBehaviour
{
    float speed = 10.0f;
    [SerializeField] float tempsDeVie=20;
    DamageableComponent hitPlayer;
    [SerializeField] int degat =1;
    float compteur;
    // Start is called before the first frame update
    void Start()
    {
        compteur = tempsDeVie;
        hitPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*speed*Time.deltaTime,Space.World);
        compteur -= Time.deltaTime;
        if (compteur < 0)
        {
            compteur = tempsDeVie;
            gameObject.SetActive(false);
          
        }
    }
    public void Grosseur(float grossirDe)
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        hitPlayer.TakeDamage(degat);
        compteur = tempsDeVie;
        gameObject.SetActive(false);
    }
}
