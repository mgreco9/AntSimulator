using static Assets.Scripts.Utils.CustomLogger;
using UnityEngine;
using System.Collections.Generic;
using BehaviourTreeLibrary;

public class GodModeAI : BehaviourTreeRunner
{
    private AgentInputs inputs = new AgentInputs();
    private DetectorManager dinput;

    protected override void Start()
    {
        AgentInputManager cinput = GetComponent<AgentInputManager>();
        cinput.RegisterInputRetriever(RetrieveInputs);

        dinput = GetComponent<DetectorManager>();
        if (dinput == null)
            LogMessage("Detector Manager could not be found", LogFlag.BehaviorTree);

        behaviourTree.blackboard["dinput"].B_value = dinput;
        behaviourTree.blackboard["agent"].B_value = transform;
        behaviourTree.Bind();
    }

    public override void ResetInputs()
    {
        Dictionary<string, BlackboardEntry> blackboard = behaviourTree.blackboard;

        blackboard["forward"].B_value = 0f;
        blackboard["turn"].B_value = 0f;
        blackboard["grab"].B_value = false;
    }

    public AgentInputs RetrieveInputs()
    {
        Dictionary<string, BlackboardEntry> blackboard = behaviourTree.blackboard;

        inputs.Forward = (float)blackboard["forward"].B_value;

        inputs.Turn = (float)blackboard["turn"].B_value;

        inputs.Grab = (bool)blackboard["grab"].B_value;

        return inputs;
    }
}
 