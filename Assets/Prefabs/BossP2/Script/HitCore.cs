using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCore : MonoBehaviour
{

    [SerializeField]int Pv = 2;
    [SerializeField] Transform positionTp;
    GameObject joueur;
    // Start is called before the first frame update
    void Start()
    {
        joueur = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Pv --;
        Tp();
    }
    public void Tp()
    {
        joueur.transform.position = positionTp.position;
    }
    public void charger(SceneStat data)
    {
        Pv = data.VieBoss;
       
    }
    public void sauvegarde(ref SceneStat data)
    {
  
        data.VieBoss = Pv;
       

    }


}
