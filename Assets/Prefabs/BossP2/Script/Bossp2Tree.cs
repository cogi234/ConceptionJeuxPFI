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
    public class cinématique : Node
    {

        public cinématique() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }
    public class SurBoss : Node
    {

        public SurBoss() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }


    //Partie SolDesAttaque
    public class Distance : Node
    {

        public Distance() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }

    public class WaitTime : Node
    {

        public WaitTime() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
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

    //cac = corp a corp
    public class CAC : Node
    {

        public CAC() : base()
        {

        }

        public override NodeState Evaluate()
        {

            return State;
        }

    }

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