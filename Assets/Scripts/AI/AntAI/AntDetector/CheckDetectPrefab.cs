using BehaviourTreeLibrary;
using System.Collections.Generic;
using UnityEngine;

class CheckDetectPrefab : DecoratorProperty
{
    [HideInInspector] public DetectorManager dinput;
    public DetectorType detectorType = DetectorType.BASE_DETECTOR;

    private bool hasDetectedPrefab = false;

    public override DecoratorType Type
    {
        get { return DecoratorType.STOP_CONDITION; }
    }

    public override string Description
    {
        get
        {
            return detectorType switch
            {
                DetectorType.BASE_DETECTOR => "Ant Hill",
                DetectorType.FOOD_GRAB_DETECTOR => "Food",
                _ => "",
            };
        }
    }

    public override void Load()
    {
        dinput = node.blackboard["dinput"].B_value as DetectorManager;
    }

    public override void AfterUpdate()
    {
        if (dinput.GetDetectedPrefab(detectorType) is not null)
        {
            hasDetectedPrefab = true;
            node.state = NodeState.SUCCESS;
        }
    }

    public override void AfterStop()
    {
        if (!hasDetectedPrefab)
            node.state = NodeState.FAILURE;
    }
}
