using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourTreeColin
{
    public abstract class DecoratorNode : Node
    {
        public DecoratorNode(Node child) : base(child)
        {
            if (child == null)
                throw new ArgumentNullException("children");
        }
    }
}
