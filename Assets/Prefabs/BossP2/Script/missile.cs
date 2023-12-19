using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class missile : MonoBehaviour
{
    Transform Altidute;
    Transform Joueur;
    Vector3 ancienPositionJoueur;
    [SerializeField] int degat;
    [SerializeField] float tempsDeVie = 500;
    float compteur=0;
    private Vector3 directionNormalize;
    private Vector3 _direction;
    //
    float speed =70.0f;
    bool AltiduteAtteint = false;

    void Update()
    {
        compteur -= Time.deltaTime;
        //fairevoltige annimation si le Temps
        if (!AltiduteAtteint)
        {
            transform.Translate(Vector3.Normalize(Altidute.position - transform.position) * speed*2 * Time.deltaTime, Space.World);
            if(transform.position.y> Altidute.position.y)
            {
                
                ancienPositionJoueur = new Vector3(Joueur.position.x, Joueur.position.y, Joueur.position.z);
                directionNormalize = Vector3.Normalize(ancienPositionJoueur - transform.position);
                AltiduteAtteint = true;
            }
        }
        else
        {
            transform.Translate(directionNormalize * speed*2 * Time.deltaTime, Space.World);
        }
        if(compteur < 0)
        {
            compteur = tempsDeVie;
            gameObject.SetActive(false);
        }
       

    }
    void FixedUpdate()
    {
        if (AltiduteAtteint)
        {


            _direction = (ancienPositionJoueur - transform.position).normalized;

            Vector3 gravityUp = -_direction.normalized;
            transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        }

    }
    public void donneCoordoner(Transform Altidute,Transform Joueur)
    {
       this.Altidute = Altidute;
        this.Joueur = Joueur;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<DamageableComponent>().TakeDamage(degat);
            compteur = 0;
            gameObject.SetActive(false);
        }
    }
}
