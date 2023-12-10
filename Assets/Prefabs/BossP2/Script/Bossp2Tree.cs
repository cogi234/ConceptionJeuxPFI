using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Anthony
{
    public enum NodeState { Running, Success, Failure }
   
    public abstract class Node
    {
       protected Node root;
        Dictionary<String, object> data = new Dictionary<String, object>();
        public void SetData(string key, object value)
        {
            data[key] = value;
        }
        public object GetData(string key)
        {
            if (data.TryGetValue(key, out object value)) return value;

            if (parent != null)
                return parent.GetData(key);

            return null;
        }

        protected List<Node> children = new();
        protected NodeState State;
        public Node parent;

        protected Node GetRoot()
        {
            Node n = parent;
            while (n.parent != null)
                n = n.parent;

            return n;
        }
        public Node()
        {
            parent = null;
            State = NodeState.Running;
        }
        public Node(List<Node> pChildren)
        {
            parent = null;
            State = NodeState.Running;
            foreach (Node n in pChildren)
            {
                Attach(n);
            }
        }
        protected void Attach(Node n)
        {
            children.Add(n);
            n.parent = this;
        }
        public abstract NodeState Evaluate();
        public bool RemoveData(string key)
        {
            if (data.Remove(key)) { return true; }
            if (parent != null)
                return parent.RemoveData(key);
            return false;
        }
        public void MettreRoot()
        {
            root = GetRoot();
        }
    }

    public class Sequence : Node
    {
        public Sequence(List<Node> n) : base(n) { }
        public override NodeState Evaluate()
        {
            foreach (Node n in children)
            {
                State = n.Evaluate();
                if (State != NodeState.Success)
                    return State;
            }
            State = NodeState.Success;
            return NodeState.Success;
        }
    }

    public class Selector : Node
    {
        public Selector(List<Node> n) : base(n) { }
        public override NodeState Evaluate()
        {
            foreach (Node n in children)
            {
                State = n.Evaluate();
                if (State != NodeState.Failure)
                    return State;
            }
            State = NodeState.Failure;
            return NodeState.Failure;
        }


    }

    public class Inverter : Node
    {
        public Inverter(List<Node> n) : base(n)
        {
            if (n.Count != 1)
            {
                throw new ArgumentException();
            }
        }
        public override NodeState Evaluate()
        {
            NodeState childState = children[0].Evaluate();

            if (childState == NodeState.Failure)
                State = NodeState.Success;
            else if (childState == NodeState.Success)
                State = NodeState.Failure;
            else
                State = NodeState.Running;

            return State;
        }
    }
    public class SequenceAttaque : Node
    {
        int quiAttaque;
     
        public SequenceAttaque(List<Node> n) : base(n) { }
        public override NodeState Evaluate()
        {
            State = children[0].Evaluate();
            // legende de la liste
            //0=cinématique
            //1 = distance pas sur boss
            //2 = cac pas sur boss
            //3= millieu boss
            //4 =surdos boss
            
            if(State != NodeState.Running)
            {
                bool peutAttaquer = (Boolean)root.GetData("peutAttaquer");
                //Debug.Log("estdans attaque");
               
                if (peutAttaquer)

                {
                   
                    quiAttaque = (int)root.GetData("quiAttaquer");

                    State = children[quiAttaque + 1].Evaluate();


                }
            }
           
            return State;
        }
       
    }
    public class cinématique : Node
    {
       public bool cinematique = false;

        public cinématique() : base()
        {

        }

        public override NodeState Evaluate()
        {
          //  Debug.Log("cinemat");
            State = NodeState.Failure;
            if (cinematique)
            {
                State = NodeState.Running;
            }

            return State;
        }

    }
    public class SurBoss : Node
    {
        public bool JoueurSurBoss = false;
 
        public SurBoss() : base()
        {

        }

        public override NodeState Evaluate()
        {
         //   Debug.Log("surboss");
            State = NodeState.Failure;
            if (JoueurSurBoss)
            {
                root.SetData("surboss", true);
                State = NodeState.Success;
            }
            else
            {
                root.SetData("surboss", false);
            }

            return State;
        }
     

    }


    //Partie SolDesAttaque
    public class Distance : Node
    {
        Transform joeur;
        Transform boss;
        float DistanceCac;
        
        public Distance(Transform joeur, Transform boss, float DistanceCac) : base()
        {
            this.joeur = joeur;
            this.boss = boss;
            this.DistanceCac = DistanceCac;
       
        }

        public override NodeState Evaluate()
        {
          
            State = NodeState.Failure;
            if (Vector3.Distance(boss.position, joeur.position) >= DistanceCac)
            {
                root.SetData("distance", true);
                State = NodeState.Success;
            }
            
            return State;
        }
       

    }

    public class WaitTime : Node
    {
        // legende de la liste
        //0 = distance pas sur boss
        //1 = cac pas sur boss
        //2= millieu boss
        //3 =surdos boss
        List<float> ListeTemps;
        List<float> ListeTempDattente;
        int etatPasser =-1;
        int etatEnCour =-1;

      
        public WaitTime(float attente) : base()
        {
            ListeTemps = new List<float> { 0,0,0,0};
            // la s'est la meme chose pour les 4 mais potentielement modifiable pour
            //des temps différents
            ListeTempDattente= new List<float> { attente, attente, attente, attente };

        }

        public override NodeState Evaluate()
        {
          // Debug.Log("WaitTime");
            State = NodeState.Running;
            bool distance =(Boolean)root.GetData("distance");
           bool surboss = (Boolean)root.GetData("surboss");
            bool millieu = (Boolean)root.GetData("millieu");
            bool peutAttaquer = (Boolean)root.GetData("peutAttaquer");
           
            if (!peutAttaquer)
            {
                if (!surboss)
                {

                   
                    if (distance)
                    {
                        etatEnCour = 0;
                        root.SetData("quiAttaquer", etatEnCour);
                    }
                    else
                    {
                     
                        etatEnCour = 1;
                        root.SetData("quiAttaquer", etatEnCour);
                    }

                }
                else
                {

                    if (millieu)
                    {
                        etatEnCour = 2;
                        root.SetData("quiAttaquer", etatEnCour);
                    }
                    else
                    {
                        etatEnCour = 3;
                        root.SetData("quiAttaquer", etatEnCour);
                    }
                }
                if (etatEnCour != etatPasser)
                {
                    etatPasser = etatEnCour;
                    ListeTemps = new List<float> { 0, 0, 0, 0 };
                }
                else
                {
           
                    ListeTemps[etatEnCour] += Time.deltaTime;
                    if (ListeTemps[etatEnCour] > ListeTempDattente[etatEnCour])
                    {
                       
                        ListeTemps[etatEnCour] = 0;
                        root.SetData("peutAttaquer", true);
                    }
                }
            }
           



            return State;
        }
        public void MettreRoots()
        {
            root = GetRoot();
            root.SetData("distance", false);
            root.SetData("surboss", false);
            root.SetData("millieu", false);
            root.SetData("peutAttaquer", false);
            root.SetData("quiAttaquer", -1);
        }

    }

    public class Missile : Node
    {
        Transform pointDépart;
        int nombreMissile;
        Transform[] zonneDeTire;
        int TempsentreMissile;
        float compteurTemps = 0;
        int CompteurNBmissileTirer =0 ;
        int CompteurPositionTire = 0;
        GameObject missile;
       
        Transform joueur;
        public Missile(int nombreMissile, GameObject zoneTire, int TempsentreMissile,
            GameObject Missile, Transform joueur) : base()
        {
            this.nombreMissile = nombreMissile;
            zonneDeTire = zoneTire.GetComponentsInChildren<Transform>();
            this.TempsentreMissile = TempsentreMissile;
            missile = Missile;
            this.joueur = joueur;
            this.pointDépart = zoneTire.transform;
        }

        public override NodeState Evaluate()
        {
            State = NodeState.Running;
            //Debug.Log("estdans Missile");
            State = NodeState.Failure;
            if (CompteurNBmissileTirer != nombreMissile)
            {


                if (compteurTemps > TempsentreMissile)
                {
                    GameObject LeMissile = ObjectPool.objectPool.GetObject(missile);
                    missile scripMissile = LeMissile.GetComponent<missile>();
                    scripMissile.donneCoordoner(zonneDeTire[CompteurPositionTire], joueur);
                    LeMissile.transform.position = pointDépart.position;
                    LeMissile.SetActive(true);

                    CompteurPositionTire++;
                    if (CompteurPositionTire == zonneDeTire.Length)
                    {
                        CompteurPositionTire = 0;
                    }
                    compteurTemps = 0;
                    CompteurNBmissileTirer++;
                }
                else
                {
                    compteurTemps += Time.deltaTime;
                }
             
            }
            else {
                CompteurNBmissileTirer = 0;
                root.SetData("peutAttaquer", false);
            }
            

            return State;
        }
        

    }
    public class BoulleDeFeu : Node
    {

        public BoulleDeFeu() : base()
        {

        }

        public override NodeState Evaluate()
        {

            //potentielement a faire
            return State;
        }

    }

    ////cac = corp a corp
    //public class CAC : Node
    //{

    //    public CAC() : base()
    //    {

    //    }

    //    public override NodeState Evaluate()
    //    {

    //        return State;
    //    }

    //}

    public class ShockWave : Node
    {
       
        float tempsEntreChoc;
        float compteurTempsEntreChoc;
        int nombreDeChoc;
        int CompteurNombreChoc;
        GameObject OndeDeChoc;
        Transform départ;

        public ShockWave(float tempsEntreChoc, GameObject ondeDeChoc,int nombreDeChoc,Transform Source) : base()
        {
            this.tempsEntreChoc = tempsEntreChoc;
            OndeDeChoc = ondeDeChoc;
            this.nombreDeChoc = nombreDeChoc;
            départ = Source;
        }

        public override NodeState Evaluate()
        {

            if (CompteurNombreChoc != nombreDeChoc)
            {


                if (compteurTempsEntreChoc >= tempsEntreChoc)
                {

                    GameObject LeChoc = ObjectPool.objectPool.GetObject(OndeDeChoc);    
                    OndeChoc scripChoc = LeChoc.GetComponent<OndeChoc>();
                    //scripChoc.Grosseur(2);
                    LeChoc.transform.position = départ.position;
                    LeChoc.SetActive(true);
                    CompteurNombreChoc++;
                    compteurTempsEntreChoc = 0;
                }
                else
                {
                    compteurTempsEntreChoc += Time.deltaTime;
                  
                }

            }
            else
            {
                CompteurNombreChoc = 0;
                root.SetData("peutAttaquer", false);
            }

            State = NodeState.Running;
           




            return State;
        }
       

    }


    // Partie SurLeBoss des attaque

    public class Millieu : Node
    {
        public bool DansMillieu;
       
        public Millieu() : base()
        {

        }

        public override NodeState Evaluate()
        {

            State = NodeState.Failure;
            if (DansMillieu)
            {
                root.SetData("millieu", true);
                State = NodeState.Success;
            }
            else
            {
                root.SetData("millieu", false);
            }



            return State;
        }
       

    }


    public class FallBlock : Node
    {

        float tempsEntrebloc;
        float compteurTempsEntrebloc;
        int nombreDebloc;
        int CompteurNombrebloc=0;
        GameObject Blockfall;
        Transform[] départ;
        int CompteurPosition =0;
        public FallBlock(Transform[] Position,GameObject bloc, float tempsEntrebloc,int nombreblock) : base()
        {
            départ = Position;
            Blockfall = bloc;
            this.tempsEntrebloc = tempsEntrebloc;
            this.nombreDebloc = nombreblock;
        }
        public override NodeState Evaluate()
        {


            if (CompteurNombrebloc != nombreDebloc)
            {


                if (compteurTempsEntrebloc >= tempsEntrebloc)
                {

                    GameObject LeChoc = ObjectPool.objectPool.GetObject(Blockfall);

                    Debug.Log(départ.Length);
                    LeChoc.transform.position = départ[CompteurPosition%départ.Length].position;
                    LeChoc.SetActive(true);
                    CompteurPosition++;
                    CompteurNombrebloc++;
                    compteurTempsEntrebloc = 0;
                }
                else
                {
                    compteurTempsEntrebloc += Time.deltaTime;

                }

            }
            else
            {
                CompteurNombrebloc = 0;
                CompteurPosition = 0;
                root.SetData("peutAttaquer", false);
            }

            State = NodeState.Running;





            return State;

        }

    }

    
    public class Spike : Node
    {

        public Spike() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }

    public class SurDos : Node
    {

        public SurDos() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }

    public class BlockTrap : Node
    {

        public BlockTrap() : base()
        {

        }

        public override NodeState Evaluate()
        {
            Debug.Log("dansBlockTrap");
            return State;
        }

    }
    public class SmallMissile : Node
    {

        public SmallMissile() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }
}