using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Shapes;
using Random = System.Random;
using Mathf = System.Math;
using Unity.VisualScripting;

public class Entrerp2 : MonoBehaviour
{
    Random random = new Random();
  
    GameObject Boss;
    List<BlocDownfall> BlocDownfallList = new List<BlocDownfall>();
    BlocDownfall[] BlocDownfallListsTab = new BlocDownfall[0];
    int ListeRequis;
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("BossP2");
        BlocDownfallListsTab = Boss.GetComponentsInChildren<BlocDownfall>();
        for (int i = 0; i < BlocDownfallListsTab.Length; i++)
        {
         
           
            BlocDownfallList.Add(BlocDownfallListsTab[i]);  
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
