using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDetector
{
    private GameObject detectorGameObject;
    private Detector detector;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        detectorGameObject = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TestPrefabs/DetectorGameObject.prefab"), Vector3.zero, Quaternion.identity);
        yield return null;
        detector = detectorGameObject.GetComponent<Detector>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(detectorGameObject);
    }

    [UnityTest]
    public IEnumerator TestDetectFoodAsDefault()
    {
        // 1 - Check the detector return nothing when nothing instantiated
        GameObject objectDetected = detector.DetectObject();
        Assert.Null(objectDetected);

        // 2 - Instantiate food out of reach of detector
        GameObject food = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Food.prefab"), new Vector3(0, 3), Quaternion.identity);
        yield return new WaitForSeconds(0.05f);

        // 3 - Check detector return nothing when out of reach (above max scope)
        objectDetected = detector.DetectObject();
        Assert.Null(objectDetected);

        // 4 - Set food object in range of detection
        food.transform.position = new Vector3(0, 1);
        yield return new WaitForSeconds(0.05f);

        // 5 - Check detector return same food object when in range
        objectDetected = detector.DetectObject();
        Assert.AreEqual(food.GetInstanceID(), objectDetected.GetInstanceID());

        // 6 - Set food object out of 
        food.transform.position = new Vector3(0, -1);
        yield return new WaitForSeconds(0.05f);

        // 7 - Check detector return nothing when out of reach (below min scope)
        objectDetected = detector.DetectObject();
        Assert.Null(objectDetected);

        // 8 - Destroy food object
        Object.Destroy(food);
    }

    [UnityTest]
    public IEnumerator TestDetectorDifferentObjectType()
    {
        // 1 - Instantiate Anthill in front of detector
        GameObject canvas = MonoBehaviour.Instantiate(new GameObject("Canvas"));
        GameObject anthill = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/AntHill.prefab"), new Vector3(0, 1), Quaternion.identity);
        yield return new WaitForSeconds(0.05f);

        // 2 - Check detector return nothing (incorrect type)
        GameObject objectDetected = detector.DetectObject();
        Assert.Null(objectDetected);

        // 3 - Change layer mask to detect anthill instead
        LayerMask layer = new LayerMask
        {
            value = 1 << 6
        };
        detector.setDetectorLayerMask(layer);

        // 4 - Check detector return same anthill object
        objectDetected = detector.DetectObject();
        Assert.AreEqual(anthill.GetInstanceID(), objectDetected.GetInstanceID());

        // 5 - Reset layer mask to detect food
        layer = new LayerMask
        {
            value = 1 << 3
        };
        detector.setDetectorLayerMask(layer);

        // 6 - Check detector return nothing
        objectDetected = detector.DetectObject();
        Assert.Null(objectDetected);

        // 7 - Destroy anthill object
        Object.Destroy(anthill);
        Object.Destroy(canvas);
    }
}
