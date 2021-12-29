using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public class Negator : Node
    {
        public Negator(Node child)
        {
            Attach(child);
        }

        public override void Initialize()
        {
            children[0].Initialize();
        }
        public override NodeState Evaluate()
        {
            // 1 - Iterate over each child node (one node must succeed)
            state = children[0].Evaluate();

            // 2 - If child node has succeeded, return failure
            if (state == NodeState.SUCCESS)
                return NodeState.FAILURE;

            // 3 - If child node has failed, return failure
            if (state == NodeState.FAILURE)
                return NodeState.FAILURE;

            // 4 - If still iterating over nodes, return running
            return NodeState.RUNNING;
        }

        public override void Reset()
        {
            children[0].Reset();
        }
    }
}