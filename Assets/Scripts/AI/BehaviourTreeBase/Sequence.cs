using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        protected int currentChildNodeIdx;
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

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
            // 1 - Iterate over each child node (all node must succeed)
            foreach (Node node in children)
            {
                // 1 - Iterate over each child node (one node must succeed)
                state = children[currentChildNodeIdx].Evaluate();

                // 2 - If child node has failed, return failure
                if (state == NodeState.FAILURE)
                    return NodeState.FAILURE;

                // 3 - If child node has succeeded, iterate over next node
                if (state == NodeState.SUCCESS)
                    currentChildNodeIdx++;

                // 4 - If every node have been realized, return success
                if (currentChildNodeIdx >= children.Count)
                    return NodeState.SUCCESS;

                // 5 - If still iterating over nodes, return running
                return NodeState.RUNNING;
            }

            // 2 - No child node failed, return success
            state = NodeState.SUCCESS;
            return state;
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
