using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthony;

public class Bossp2Composant : MonoBehaviour
{
  
    Node root;

    private void Awake()
    {
       
        SetupTree();
    }

   
    private void SetupTree()
    {
        
       Node Cinema = new cinématique();

        //Tout les node sol
        //tirenode
        Node misile = new Missile(); 
        Node BouleFeu = new BoulleDeFeu();
       Node Seq1Tire = new Sequence(new List<Node> { misile, BouleFeu });
        //distanceNode
        Node distance = new Distance();
        Node Wait = new WaitTime();
        Node Seq2SolDistance = new Sequence(new List<Node> { distance, Wait, Seq1Tire });
        //CacSolNode
        Node Cac = new CAC();
        Node shokWave = new ShockWave();
        Node Seq3CacSol = new Sequence(new List<Node> { Cac, Wait, shokWave });
        //selector sol
        Node sel1Sol = new Selector(new List<Node>() { Seq2SolDistance, Seq3CacSol });
        // invertSurSol
        Node SurBoss = new SurBoss();
        Node Invert1Sol = new Inverter(new List<Node> { SurBoss });
        //Sequencesol
        Node Seq4Sol = new Sequence(new List<Node> { Invert1Sol, sel1Sol });

        // Tout les nodes SurBoss

        //TrapdosSequence
        Node blockTrap = new BlockTrap();
        Node smalMisile = new SmallMissile();
        Node Seq5Trap = new Sequence(new List<Node> { blockTrap, smalMisile });

        //surDosSequence
        Node  SurDos = new SurDos();
        Node Seq6SurDos = new Sequence(new List<Node> { SurDos, Wait, Seq5Trap });


        //trapMillieuSequence
        Node FallBlock = new FallBlock();
        Node spike = new Spike();
        Node Seq7MillieuTrap = new Sequence(new List<Node> { FallBlock, spike });


        //TrapMillieuSequence
        Node Millieu = new Millieu();
        Node Seq8Millieu= new Sequence(new List<Node> { Millieu, Wait, Seq7MillieuTrap });

        //SelectorSurBoss
        Node SurBossSelector = new Selector(new List<Node>() { Seq8Millieu, Seq6SurDos });

        //SequenceSurBoss
        Node Seq9SurBoss = new Sequence(new List<Node> { SurBoss, SurBossSelector});

        //Root
        Node RootSelector  =  new Selector(new List<Node>() { Cinema, Seq4Sol, Seq9SurBoss });
        root = RootSelector;







    }

    void Update()
    {
        root.Evaluate();
    }
}
