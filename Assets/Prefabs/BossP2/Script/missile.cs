using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class missile : MonoBehaviour
{
    Transform Altidute;
    Transform Joueur;
    float speed = 5.0f;
    bool AltiduteAtteint = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //fairevoltige annimation si le Temps
        if (!AltiduteAtteint)
        {
            transform.Translate(Vector3.Normalize(Altidute.position - transform.position) * speed * Time.deltaTime, Space.World);
            if(transform.position.y> Altidute.position.y)
            {
                AltiduteAtteint = true;
            }
        }
        else
        {
            transform.Translate(Vector3.Normalize(Joueur.position - transform.position) * speed * Time.deltaTime, Space.World);
        }
       

    }
    public void donneCoordoner(Transform Altidute,Transform Joueur)
    {
       this.Altidute = Altidute;
        this.Joueur = Joueur;
    }
}
