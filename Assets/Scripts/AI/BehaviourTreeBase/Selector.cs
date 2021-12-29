using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        protected int currentChildNodeIdx;

        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override void Initialize()
        {
            // 1 - Initialize idx
            currentChildNodeIdx = 0;

            // 2 - Initialize every child node
            foreach (Node node in children)
            {
                node.Initialize();
            }
        }

        public override NodeState Evaluate()
        {
            // 1 - Iterate over each child node (one node must succeed)
            state = children[currentChildNodeIdx].Evaluate();

            // 2 - If child node has succeeded, return success
            if (state == NodeState.SUCCESS)
                return NodeState.SUCCESS;

            // 3 - If child node has failed, iterate over next node
            if (state == NodeState.FAILURE)
                currentChildNodeIdx++;

            // 4 - If every node have been realized, return failure
            if (currentChildNodeIdx >= children.Count)
                return NodeState.FAILURE;

            // 5 - If still iterating over nodes, return running
            return NodeState.RUNNING;
        }

        public override void Reset()
        {
            // 1 - Reset idx
            currentChildNodeIdx = 0;

            // 2 - Reset each child node
            foreach (Node node in children)
            {
                node.Reset();
            }
        }
    }
}
