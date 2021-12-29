using BehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MoveToLocation : Node
{
    private Func<Transform> GetTargetLocationFunc;

    private Transform agent;
    private Transform targetLocation;

    public MoveToLocation(Transform agent, Transform destination) : base()
    {
        this.agent = agent;
        targetLocation = destination;
    }

    public MoveToLocation(Transform agent, Func<Transform> GetTargetLocationFunc) : base()
    {
        this.agent = agent;
        this.GetTargetLocationFunc = GetTargetLocationFunc;
    }

    public override void Initialize()
    {
        if(GetTargetLocationFunc != null)
            targetLocation = GetTargetLocationFunc();
    }

    public override void Reset()
    {
        if (GetTargetLocationFunc != null)
            targetLocation = GetTargetLocationFunc();
    }

    public override NodeState Evaluate()
    {
        // 0 - If no destination, return failure
        if (targetLocation == null)
            return NodeState.FAILURE;

        // 1 - Compute distance and angle between position and destination
        float distance = Vector3.Distance(agent.position, targetLocation.position);
        float angle = Vector3.SignedAngle(agent.up, targetLocation.position - agent.position, Vector3.forward);

        // 2 - Check if agent is close enough, if so return success
        if (distance <= GameEngineConstant.NEAR_DISTANCE)
            return NodeState.SUCCESS;

        // 3 - Store the inputs in the dictionary
        sharedParameters["Forward"] = Mathf.Clamp(distance, 0, GameEngineConstant.FAR_DISTANCE) / GameEngineConstant.FAR_DISTANCE;
        sharedParameters["Turn"] = angle;

        // 4 - Return running
        return NodeState.RUNNING;
    }
}
