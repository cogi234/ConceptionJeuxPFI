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
    Node root;
    GameObject joueur;
    [SerializeField] float DistanceCac = 150;
    [SerializeField] float attente = 10;
    [SerializeField] int nombreDeMissile = 5;
    [SerializeField] int TempsentreMissileDistance = 5;
    [SerializeField] int nombreDeChoc= 5;
    [SerializeField] int TempsentreChoc = 5;
    [SerializeField] GameObject Missile;
    [SerializeField] GameObject Choc;
    GameObject Boss;
    GameObject ZoneTire;
    GameObject ZoneChoc;

    private void Awake()
    {
        Boss = GameObject.FindGameObjectWithTag("BossP2");
        joueur = GameObject.FindGameObjectWithTag("Player");
        ZoneTire = GameObject.FindGameObjectWithTag("tireZone");
        ZoneChoc = GameObject.FindGameObjectWithTag("tireChoc");
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
        Node  SurDos = new SurDos();
        Node Seq6SurDos = new Sequence(new List<Node> { SurDos, Wait });


        //trapMillieuSequence
        Node FallBlock = new FallBlock();
        Node spike = new Spike();
        Node Seq7MillieuTrap = new Sequence(new List<Node> { FallBlock, spike });


        //TrapMillieuSequence
        //Node Millieu = new Millieu();
        Node inverterMilieu = new Inverter(new List<Node> { SurDos });
        Node Seq8Millieu= new Sequence(new List<Node> { inverterMilieu, Wait });

        //SelectorSurBoss
        Node SurBossSelector = new Selector(new List<Node>() { Seq8Millieu, Seq6SurDos });

        //SequenceSurBoss
        Node Seq9SurBoss = new Sequence(new List<Node> { SurBoss, SurBossSelector});

        //phase d'attaque
        SequenceAttaque SequenceAttaque = new SequenceAttaque(new List<Node> { Cinema, Seq1Tire,shokWave,Seq7MillieuTrap, Seq5SurDosTrap });

        //Root
        Node RootSelector  =  new Selector(new List<Node>() { SequenceAttaque, Seq4Sol, Seq9SurBoss });
        root = RootSelector;
        Wait.MettreRoot();
        SequenceAttaque.MettreRoot();
        distance.MettreRoot();
        shokWave.MettreRoot();
        misile.MettreRoot();







    }

    void Update()
    {
        root.Evaluate();
    }
   
}
