using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GodModeAI : BehaviourTreeBase
{
    private AgentInputs inputs;
    private DetectorManager dinput;
    private Transform baseLocation;

    private void Awake()
    {
        AgentInputManager cinput = GetComponent<AgentInputManager>();
        cinput.RegisterInputRetriever(RetrieveInputs);

        dinput = GetComponent<DetectorManager>();
        if (dinput == null)
            Debug.Log("Detector Manager could not be found");

        baseLocation = GameObject.Find("AntHill").transform;
    }

    protected override Node SetupTree()
    {
        Node moveToFood = new MoveToLocation(transform, FoodPool.GetInstance().GetRandomActivePrefab);
        Node dontStopMovingToFood = new Negator(moveToFood);
        Node checkDetectFood = new CheckDetectPrefab(dinput, DetectorType.FOOD_GRAB_DETECTOR);
        Node moveUntilFood = new ParallelSelector(new List<Node>() { dontStopMovingToFood, checkDetectFood });

        Node grabFood = new GrabCommand();

        Node moveToBase = new MoveToLocation(transform, baseLocation);
        Node checkDetectBase = new CheckDetectPrefab(dinput, DetectorType.BASE_DETECTOR);
        Node moveUntilBase = new ParallelSelector(new List<Node>() { moveToBase, checkDetectBase });

        Node dropFood = new GrabCommand();

        Node root = new Sequence(new List<Node>() { moveUntilFood, grabFood, moveUntilBase, dropFood});
        root.InitializeSharedParameters(sharedParameters);

        return root;
    }

    public AgentInputs RetrieveInputs()
    {
        AgentInputs inputs = new AgentInputs();

        if(sharedParameters.ContainsKey("Forward"))
            inputs.Forward = (float) sharedParameters["Forward"];

        if (sharedParameters.ContainsKey("Turn"))
            inputs.Turn = (float) sharedParameters["Turn"];

        if (sharedParameters.ContainsKey("Grab"))
            inputs.Grab = (bool) sharedParameters["Grab"];

        return inputs;
    }
}
