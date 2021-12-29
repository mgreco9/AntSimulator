using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTree
{

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        protected Node parent;
        protected List<Node> children = new List<Node>();
        public Dictionary<String, object> sharedParameters;

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        protected void Attach(Node node)
        {
            children.Add(node);
            node.parent = this;
            node.sharedParameters = sharedParameters;
        }

        public void InitializeSharedParameters(Dictionary<String, object> parameters)
        {
            sharedParameters = parameters;
            foreach (Node child in children)
                child.InitializeSharedParameters(parameters);
        }

        public virtual void Initialize() { }
        public virtual NodeState Evaluate() => NodeState.FAILURE;
        public virtual void Reset() { }
    }
}
