using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Shapes;
using Random = System.Random;
using Mathf = System.Math;
public class Entrerp2 : MonoBehaviour
{
    Random random = new Random();
  
    GameObject Boss;
    List<BlocDownfall> BlocDownfallList = new List<BlocDownfall>();
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("BossP2");
        int compteur = Boss.transform.childCount;
        for (int i =0; i<compteur; i++)
        {
            GameObject objet = Boss.transform.GetChild(i).gameObject;
            int compteur2 = objet.transform.childCount;

            for (int j = 0; j < compteur2; j++)
            {

                BlocDownfallList.Add(objet.transform.GetChild(j).GetComponent<BlocDownfall>());
            }

       
        }
      
    }

    bool Tp = true;
    BlocDownfall chute;

    private void Awake()
    {

        
    }

    [SerializeField] float BPM = 1;
    float ConteurBPM = 0;
    [SerializeField] float vitesseBPM = 10;
    private void Update()
    {
        if (Tp)
        {

            foreach (BlocDownfall t in BlocDownfallList)
            {
                t.tp();
            }
            Tp = false;
        }
        if (BlocDownfallList.Count != 0)
        {


            if (ConteurBPM >= BPM)
            {
                int pointeur;
                pointeur = random.Next(0, BlocDownfallList.Count);
                BlocDownfallList[pointeur].Tomber(vitesseBPM);
                BlocDownfallList.Remove(BlocDownfallList[pointeur]);
                ConteurBPM = 0;
                vitesseBPM += 2;
                BPM -= 0.02f;
            }
            else
            {
                ConteurBPM += Time.deltaTime;
            }
        }




    }



}
