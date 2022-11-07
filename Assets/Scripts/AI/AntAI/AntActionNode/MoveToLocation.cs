using BehaviourTreeLibrary;
using System;
using UnityEngine;
using static Assets.Scripts.Utils.CustomLogger;
using System.Collections.Generic;

class MoveToLocation : ActionNode
{
    [HideInInspector] public Transform agent;
    [HideInInspector] public Vector3 targetLocation;

    public override void Load()
    {
        agent = blackboard["agent"].B_value as Transform;
    }

    public override void OnStart()
    {
        targetLocation = (Vector3)blackboard["targetLocation"].B_value;
    }

    public override NodeState OnUpdate()
    {
        // 1 - Store the default inputs in the dictionary
        blackboard["forward"].B_value = 0f;
        blackboard["turn"].B_value = 0f;

        // 2 - If no destination, return failure
        if (targetLocation == null)
        {
            return NodeState.FAILURE;
        }

        // 3 - Compute distance and angle between position and destination
        float distance = Vector3.Distance(agent.position, targetLocation);
        float angle = Vector3.SignedAngle(agent.up, targetLocation - agent.position, Vector3.forward);
        LogMessage("Moving node, distance : " + distance + " angle : " + angle, LogFlag.BehaviorTree);

        // 4 - Check if agent is close enough, if so return success
        if (distance <= GameEngineConstant.NEAR_DISTANCE)
            return NodeState.SUCCESS;

        // 5 - Store the inputs in the dictionary
        blackboard["forward"].B_value = Mathf.Clamp(distance, 0, GameEngineConstant.FAR_DISTANCE) / GameEngineConstant.FAR_DISTANCE;
        blackboard["turn"].B_value = angle;

        // 6 - Return running
        return NodeState.RUNNING;
    }

    public override void OnStop()
    {
        blackboard["forward"].B_value = 0f;
        blackboard["turn"].B_value = 0f;
    }
}
