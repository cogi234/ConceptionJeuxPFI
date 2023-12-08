using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anthony
{
    public enum NodeState { Running, Success, Failure }

    public abstract class Node
    {
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
        Node root;
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
            
            if(State != NodeState.Success)
            {
                bool peutAttaquer = (Boolean)root.GetData("peutAttaquer");

                if (peutAttaquer)
                {
                    quiAttaque = (int)root.GetData("quiAttaquer");

                    State = children[quiAttaque + 1].Evaluate();


                }
            }
           
            return State;
        }
        public void MettreRoot()
        {
            root = GetRoot();
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
            State = NodeState.Failure;
            if (cinematique)
            {
                State = NodeState.Success;
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
            State = NodeState.Failure;
            if (JoueurSurBoss)
            {
                State = NodeState.Success;
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

        Node root;
        public WaitTime(float attente) : base()
        {
            ListeTemps = new List<float> { 0,0,0,0};
            // la s'est la meme chose pour les 4 mais potentielement modifiable pour
            //des temps différents
            ListeTempDattente= new List<float> { attente, attente, attente, attente };

        }

        public override NodeState Evaluate()
        {
           State = NodeState.Running;
            bool distance =(Boolean)root.GetData("distance");
           bool surboss = (Boolean)root.GetData("surboss");
            bool millieu = (Boolean)root.GetData("millieu");
            bool peutAttaquer = (Boolean)root.GetData("peutAttaquer");
            if (!peutAttaquer)
            {
                if (surboss)
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
                        peutAttaquer = true;
                    }
                }
            }
           



            return State;
        }
        public void MettreRoot()
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

        public Missile() : base()
        {

        }

        public override NodeState Evaluate()
        {

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

        public ShockWave() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }


    // Partie SurLeBoss des attaque

    public class Millieu : Node
    {

        public Millieu() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }


    public class FallBlock : Node
    {

        public FallBlock() : base()
        {

        }

        public override NodeState Evaluate()
        {

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