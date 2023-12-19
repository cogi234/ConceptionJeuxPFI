using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Shapes;
using Random = System.Random;
using Mathf = System.Math;
using Unity.VisualScripting;
using Anthony;

public class Entrerp2 : MonoBehaviour
{
    Random random = new Random();

    GameObject Boss;
    List<BlocDownfall> BlocDownfallList = new List<BlocDownfall>();
    List<BlocDownfall2> BlocDownfallList2 = new List<BlocDownfall2>();

    BlocDownfall[] BlocDownfallListsTab = new BlocDownfall[0];
    BlocDownfall2[] BlocDownfallListsTab2 = new BlocDownfall2[0];
    Animator animator;
    AudioSource audioSource;
    cinématique cinematique;
    SpawnerEnnemi spawner;
    int ListeRequis;
    List<Camera> cameras = new List<Camera>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] li;
         li = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject go in li)
        {
            cameras.Add(go.GetComponent<Camera>());
        }
        cameras[1].enabled = false;
        cameras[0].enabled = true;
        spawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<SpawnerEnnemi>();
        spawner.cinématique = true;
        Boss = GameObject.FindGameObjectWithTag("BossP2");
        animator= Boss.GetComponent<Animator>();
        audioSource = Boss.GetComponent<AudioSource>();
        BlocDownfallListsTab = Boss.GetComponentsInChildren<BlocDownfall>();
        BlocDownfallListsTab2 = Boss.GetComponentsInChildren<BlocDownfall2>();
        cinematique = Boss.GetComponent<Bossp2Composant>().Cinema;
        for (int i = 0; i < BlocDownfallListsTab.Length; i++)
        {
            BlocDownfallList.Add(BlocDownfallListsTab[i]);  
        }
        for (int j = 0; j < BlocDownfallListsTab2.Length; j++)
        {
            BlocDownfallList2.Add(BlocDownfallListsTab2[j]);
        }

        cinematique.cinematique = true;
        GameObject.Find("Player").GetComponent<PlayerController>().immobile = true;
    }

    bool Tp = true;
    BlocDownfall chute;

    private void Awake()
    {

        
    }

    [SerializeField] float BPM = 1;
    float ConteurBPM = 0;
    [SerializeField] float vitesseBPM = 10;
    [SerializeField] int blocParBPM=5;
    [SerializeField] int augmentationDuTauxNBDrop =1;
    int compteurAugmentation;
      int pointeur;
    bool reset =true;
    bool finDeEntrer = false;
    [SerializeField] float compteurAvantCri=0.5f;
    float compteurCri;
    bool triggerAnimation = true;
  

  [SerializeField]  float tempsAnimation = 2;
    private void Update()
    {

       
        if (!finDeEntrer)
        {
            
        

         if (Tp)
         {

            foreach (BlocDownfall t in BlocDownfallList)
            {
                t.tp();
            }
            foreach (BlocDownfall2 t in BlocDownfallList2)
            {
                t.tp();
            }
            Tp = false;
         }
          if (BlocDownfallList.Count != 0)
         {


            if (ConteurBPM >= BPM)
            {
                for (int i = 0; i < blocParBPM && BlocDownfallList.Count != 0; i++)
                {

                    pointeur = random.Next(0, BlocDownfallList.Count);

                    BlocDownfallList[pointeur].Tomber(vitesseBPM);
                    BlocDownfallList.Remove(BlocDownfallList[pointeur]);
                    ConteurBPM = 0;


                }
                if (vitesseBPM + 2 < 130)
                {
                    vitesseBPM += 2;
                }
               

                if (BPM - 0.02f > 0.2f)
                {
                    BPM -= 0.02f;
                }
                else
                {
                    if (compteurAugmentation > augmentationDuTauxNBDrop)
                    {
                        blocParBPM += 1;
                        compteurAugmentation = 0;
                    }
                    else
                    {
                        compteurAugmentation++;
                    }

                }
            }
            else
            {

                ConteurBPM += Time.deltaTime;
            }
              }
         else if (BlocDownfallList2.Count != 0)
          {
            if (reset)
            {
                reset = !reset;
                ConteurBPM = 0;
                BPM = 0.4f;
                blocParBPM= 5;
                vitesseBPM = 50;
            }
            if (ConteurBPM >= BPM)
            {
                for (int j = 0; j < blocParBPM && BlocDownfallList2.Count != 0; j++)
                {

                    pointeur = random.Next(0, BlocDownfallList2.Count);

                    BlocDownfallList2[pointeur].Tomber(vitesseBPM);
                    BlocDownfallList2.Remove(BlocDownfallList2[pointeur]);
                    ConteurBPM = 0;


                }
                if (vitesseBPM + 2 < 130)
                {
                    vitesseBPM += 2;
                }

                if (BPM - 0.02f > 0.2f)
                {
                    BPM -= 0.02f;
                }
                else
                {
                    if (compteurAugmentation > augmentationDuTauxNBDrop)
                    {
                        blocParBPM += 1;
                        compteurAugmentation = 0;
                    }
                    else
                    {
                        compteurAugmentation++;
                    }

                }
            }
            else
            {

                ConteurBPM += Time.deltaTime;
            }




         }
         if(BlocDownfallList2.Count == 0 && BlocDownfallList.Count == 0)
         {
                if (tempsAnimation<0)
                {


                    if (triggerAnimation)
                    {
                        animator.SetTrigger("DebutCri");
                    }
                    if (compteurCri > compteurAvantCri)
                    {
                        audioSource.Play();
                        finDeEntrer = true;
                    }
                    else
                    {
                        compteurCri += Time.deltaTime;
                    }

                }
                else
                {
                    tempsAnimation -= Time.deltaTime;
                }
          }

        }
        //apres la fin d'entrer
        else
        {
            cinematique.cinematique = false;
            GameObject.Find("Player").GetComponent<PlayerController>().immobile = false;
            spawner.cinématique = false;
            cameras[1].enabled = true ;
            cameras[0].enabled = false;
        }

    }
    public void charger(SceneStat data)
    {
        cinematique.cinematique = data.CinématiqueenCour;

    }
    public void sauvegarde(ref SceneStat data)
    {

        data.CinématiqueenCour = cinematique.cinematique;


    }
}
