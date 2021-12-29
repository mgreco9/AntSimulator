using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourTree;
using UnityEngine;

class GrabCommand : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("GrabCommand");
        // 1 - Send grab input
        sharedParameters["Grab"] = true;

        // 2 - Return success
        return NodeState.SUCCESS;
    }
}
