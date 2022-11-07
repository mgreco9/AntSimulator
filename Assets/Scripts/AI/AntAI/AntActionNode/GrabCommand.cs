using BehaviourTreeLibrary;
using UnityEngine;

class GrabCommand : ActionNode
{
    public override void OnStart()
    {
    }

    public override NodeState OnUpdate()
    {
        // 1 - Send grab input
        blackboard["grab"].B_value = true;

        // 2 - Return success
        return NodeState.SUCCESS;
    }

    public override void OnStop()
    {
    }
}
