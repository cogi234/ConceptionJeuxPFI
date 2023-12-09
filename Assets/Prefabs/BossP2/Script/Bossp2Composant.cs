using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthony;
//using Unity.VisualScripting;

public class Bossp2Composant : MonoBehaviour
{
    public SurBoss SurBoss;
    public Distance distance;
    public cinématique Cinema;
    public Millieu DansMilieu;
    Node root;
    GameObject joueur;
    [SerializeField] float DistanceCac = 150;
    [SerializeField] float attente = 10;
    [SerializeField] int nombreDeMissile = 5;
    [SerializeField] int TempsentreMissileDistance = 2;
    [SerializeField] int nombreDeChoc= 5;
    [SerializeField] int TempsentreChoc = 2;
    [SerializeField] int nombreDeBlocMillieu= 5;
    [SerializeField] int TempsentreBlocMillieu = 2;
    [SerializeField] GameObject Missile;
    [SerializeField] GameObject Choc;
    [SerializeField] GameObject MillieuBloc;
    Transform[] millieuPostionBlocDrop;
    GameObject Boss;
    GameObject ZoneTire;
    GameObject ZoneChoc;
    GameObject ZoneMillieu;

    private void Awake()
    {
        Boss = GameObject.FindGameObjectWithTag("BossP2");
        joueur = GameObject.FindGameObjectWithTag("Player");
        ZoneTire = GameObject.FindGameObjectWithTag("tireZone");
        ZoneChoc = GameObject.FindGameObjectWithTag("tireChoc");
        ZoneMillieu = GameObject.FindGameObjectWithTag("tireMillieu");
        millieuPostionBlocDrop = ZoneMillieu.GetComponentsInChildren<Transform>();
        Debug.Log(millieuPostionBlocDrop.Length);
        SetupTree();
    }

   
    private void SetupTree()
    {

         Cinema = new cinématique();

        //Tout les node sol
        //tirenode
        Missile misile = new Missile(nombreDeMissile, ZoneTire, TempsentreMissileDistance, Missile,joueur.transform); 
        Node BouleFeu = new BoulleDeFeu();
       Node Seq1Tire = new Sequence(new List<Node> { misile, BouleFeu });
        //distanceNode
        distance = new Distance(joueur.transform, Boss.transform, DistanceCac);
        WaitTime Wait = new WaitTime(attente);
        Node Seq2SolDistance = new Sequence(new List<Node> { distance, Wait });
        //CacSolNode
       // Node Cac = new CAC();
        Node inverterDistance = new Inverter(new List<Node> { distance});
        ShockWave shokWave = new ShockWave(TempsentreChoc, Choc, nombreDeChoc, ZoneChoc.transform);
        Node Seq3CacSol = new Sequence(new List<Node> { inverterDistance, Wait });
        //selector sol
        Node sel1Sol = new Selector(new List<Node>() { Seq2SolDistance, Seq3CacSol });
        // invertSurSol
        SurBoss = new SurBoss();
        Node Invert1Sol = new Inverter(new List<Node> { SurBoss });
        //Sequencesol
        Node Seq4Sol = new Sequence(new List<Node> { Invert1Sol, sel1Sol });

        // Tout les nodes SurBoss

        //TrapdosSequence
        Node blockTrap = new BlockTrap();
        Node smalMisile = new SmallMissile();
        Node Seq5SurDosTrap = new Sequence(new List<Node> { blockTrap, smalMisile });

        //surDosSequence
        DansMilieu = new Millieu();
        Node inverterMilieu = new Inverter(new List<Node> { DansMilieu });
        Node Seq6SurDos = new Sequence(new List<Node> { inverterMilieu, Wait });


        //trapMillieuSequence
        Node FallBlock = new FallBlock(millieuPostionBlocDrop, MillieuBloc,TempsentreBlocMillieu,nombreDeBlocMillieu);
        Node spike = new Spike();
        Node Seq7MillieuTrap = new Sequence(new List<Node> { FallBlock, spike });


        //TrapMillieuSequence
      
       
        Node Seq8Millieu= new Sequence(new List<Node> { DansMilieu, Wait });

        //SelectorSurBoss
        Node SurBossSelector = new Selector(new List<Node>() { Seq8Millieu, Seq6SurDos });

        //SequenceSurBoss
        Node Seq9SurBoss = new Sequence(new List<Node> { SurBoss, SurBossSelector});

        //phase d'attaque
        SequenceAttaque SequenceAttaque = new SequenceAttaque(new List<Node> { Cinema, Seq1Tire,shokWave,Seq7MillieuTrap, Seq5SurDosTrap });

        //Root
        Node RootSelector  =  new Selector(new List<Node>() { SequenceAttaque, Seq4Sol, Seq9SurBoss });
        root = RootSelector;
        Wait.MettreRoots();
        SequenceAttaque.MettreRoot();
        distance.MettreRoot();
        shokWave.MettreRoot();
        misile.MettreRoot();
        DansMilieu.MettreRoot();
        SurBoss.MettreRoot();
        FallBlock.MettreRoot();






    }

    void Update()
    {
        root.Evaluate();
    }
   
}
