using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeColin
{
    public enum NodeState { Running, Success, Failure }

    public abstract class Node : ICloneable
    {
        public Node parent = null;

        public Node Root
        {
            get
            {
                Node root = this;
                while (root.parent != null)
                    root = root.parent;
                return root;
            }
        }

        protected List<Node> children = new List<Node>();

        public abstract NodeState Evaluate(Dictionary<string, object> data);

        public Node(params Node[] children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        protected void Attach(Node n)
        {
            children.Add(n);
            n.parent = this;
        }

        public abstract object Clone();
    }
}