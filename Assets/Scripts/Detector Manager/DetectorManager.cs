using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectorType
{
    FOOD_GRAB_DETECTOR,
    BASE_DETECTOR
}

public struct AntDetectorInputs
{
    public GameObject grabableFood;
    public GameObject anthillBase;
}

public class DetectorManager : MonoBehaviour
{
    public AntDetectorInputs inputs;

    private Dictionary<DetectorType, Func<GameObject>> detectors = new Dictionary<DetectorType, Func<GameObject>>();

    public void RegisterDetector(DetectorType detectorType, Func<GameObject> detectObject)
    {
        if (!detectors.ContainsKey(detectorType))
            detectors[detectorType] = detectObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 1 - Check if any reachable food is detected
        if(detectors.ContainsKey(DetectorType.FOOD_GRAB_DETECTOR))
            inputs.grabableFood = detectors[DetectorType.FOOD_GRAB_DETECTOR]();

        // 2 - Check if food can be drop in base
        if (detectors.ContainsKey(DetectorType.BASE_DETECTOR))
            inputs.anthillBase = detectors[DetectorType.BASE_DETECTOR]();
    }
}
