using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndeChoc : MonoBehaviour
{
    float speed = 10.0f;
    [SerializeField] float tempsDeVie=20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*speed*Time.deltaTime,Space.World);
        tempsDeVie -= Time.deltaTime;
        if (tempsDeVie < 0)
        {
            gameObject.SetActive(false);
          
        }
    }
    public void Grosseur(float grossirDe)
    {
        
    }
}
