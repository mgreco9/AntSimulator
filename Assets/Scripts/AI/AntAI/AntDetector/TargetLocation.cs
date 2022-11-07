using BehaviourTreeLibrary;
using UnityEngine;

public class TargetLocation : DecoratorProperty
{

    public enum LocationType
    {
        ANT_BASE,
        RANDOM_FOOD
    }

    public LocationType targetLocationType = LocationType.ANT_BASE;
    public override DecoratorType Type 
    { 
        get { return DecoratorType.START_CONDITION; }
    }

    public override string Description
    {
        get
        {
            return targetLocationType switch
            {
                LocationType.ANT_BASE => "Ant Base",
                LocationType.RANDOM_FOOD => "Random food",
                _ => "",
            };
        }
    }

    public override void BeforeStart()
    {
        switch (targetLocationType)
        {
            case LocationType.ANT_BASE:
                node.blackboard["targetLocation"].B_value = GameObject.Find("AntHill").transform.position;
                break;
            case LocationType.RANDOM_FOOD:
                Transform transform = FoodPool.GetInstance().GetRandomPrefabTransform();
                if(transform is not null)
                    node.blackboard["targetLocation"].B_value = transform.position;
                break;
        }
    }
}
