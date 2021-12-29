using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public class ParallelSelector : Node
    {
        public ParallelSelector() : base() { }
        public ParallelSelector(List<Node> children) : base(children) { }

        public override void Initialize()
        {
            // 1 - Initialize every child node
            foreach (Node node in children)
            {
                node.Initialize();
            }
        }

        public override NodeState Evaluate()
        {
            bool stillRunning = false;

            // 1 - Iterate over each child node (one node must succeed)
            foreach (Node child in children)
            {
                state = child.Evaluate();

                // 2 - If child node has succeeded, return success
                if (state == NodeState.SUCCESS)
                    return NodeState.SUCCESS;

                // 3 - If child node is still running, mark selector as running
                if (state == NodeState.RUNNING)
                    stillRunning = true;
            }

            // 4 - If still iterating over nodes, return running
            if(stillRunning)
                return NodeState.RUNNING;

            // 5 - If every node failed, return failure
            return NodeState.FAILURE;
        }

        public override void Reset()
        {
            // 1 - Reset each child node
            foreach (Node node in children)
            {
                node.Reset();
            }
        }
    }
}
