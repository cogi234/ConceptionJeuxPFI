using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public abstract class ControlNode : Node
    {
        public ControlNode(params Node[] children) : base(children) {
            if (children == null)
                throw new ArgumentNullException("children");
            else if (children.Length == 0)
                throw new ArgumentException("There should be at least one child");
        }
    }
}
