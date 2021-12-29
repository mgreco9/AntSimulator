using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestAntController
{
    private GameObject antObject;
    private ProgramTestInputs testInputs;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        antObject = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TestPrefabs/AntProgramTest.prefab"));
        yield return null;
        testInputs = antObject.GetComponent<ProgramTestInputs>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(antObject);
    }

    [UnityTest]
    public IEnumerator TestControlAntMoveForward()
    {
        // 1 - Retrieve current position
        Vector3 prevPosition = antObject.transform.position;

        // 2 - program forward input
        testInputs.inputs.Forward = 1;
        testInputs.inputs.Turn = 0;

        // 3 - wait next frame 
        yield return new WaitForSeconds(0.05f);

        // 4 - Retrieve new position
        Vector3 currPosition = antObject.transform.position;

        // 5 - Assert ant has moved forward
        Assert.IsTrue(currPosition.y > prevPosition.y);
    }

    [UnityTest]
    public IEnumerator TestControlAntMoveBackward()
    { 
        // 1 - Retrieve current position
        Vector3 prevPosition = antObject.transform.position;

        // 2 - program forward input
        testInputs.inputs.Forward = -1;
        testInputs.inputs.Turn = 0;

        // 3 - wait next frame 
        yield return new WaitForSeconds(0.05f);

        // 4 - Retrieve new position
        Vector3 currPosition = antObject.transform.position;

        // 5 - Assert ant has moved forward
        Assert.IsTrue(currPosition.y < prevPosition.y);
    }

    [UnityTest]
    public IEnumerator TestControlAntMoveAlongSelfAxis()
    {
        // 1 - Rotate game object
        antObject.transform.Rotate(0, 0, -90);

        // 2 - Retrieve current position
        Vector3 prevPosition = antObject.transform.position;

        // 3 - program forward input
        testInputs.inputs.Forward = 1;
        testInputs.inputs.Turn = 0;

        // 4 - wait next frame 
        yield return new WaitForSeconds(0.05f);

        // 5 - Retrieve new position
        Vector3 currPosition = antObject.transform.position;

        // 6 - Assert ant has moved forward
        Assert.IsTrue(currPosition.x > prevPosition.x);
    }

    [UnityTest]
    public IEnumerator TestControlAntTurnRight()
    { 
        // 1 - Retrieve current position
        Quaternion prevRotation = antObject.transform.rotation;

        // 2 - program turn right
        testInputs.inputs.Forward = 0;
        testInputs.inputs.Turn = -1;

        // 3 - wait next frame 
        yield return new WaitForSeconds(0.05f);

        // 4 - Retrieve new position
        Quaternion currRotation = antObject.transform.rotation;

        // 5 - Assert ant has moved forward
        Assert.IsTrue(currRotation.z < prevRotation.z);
    }

    [UnityTest]
    public IEnumerator TestControlAntTurnLeft()
    { 
        // 1 - Retrieve current position
        Quaternion prevRotation = antObject.transform.rotation;

        // 2 - program turn right
        testInputs.inputs.Forward = 0;
        testInputs.inputs.Turn = 1;

        // 3 - wait next frame 
        yield return new WaitForSeconds(0.05f);

        // 4 - Retrieve new position
        Quaternion currRotation = antObject.transform.rotation;

        // 5 - Assert ant has moved forward
        Assert.IsTrue(currRotation.z > prevRotation.z);
    }

    [UnityTest]
    public IEnumerator TestAntGrab()
    {
        // 1 - Instantiate food out of reach of detector
        GameObject food = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Food.prefab"), new Vector3(0, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.05f);

        // 2 - Try to grab the object
        testInputs.inputs.Grab = true;
        yield return null;
        testInputs.inputs.Grab = false;
        yield return null;

        // 3 - Check object is grabbed
        Assert.AreEqual(antObject.GetInstanceID(), food.transform.parent.parent.gameObject.GetInstanceID());

        // 4 - Try to release the object
        testInputs.inputs.Grab = true;
        yield return null;
        testInputs.inputs.Grab = false;
        yield return null;

        // 5 - Check object is no longer grabbed
        Assert.Null(food.transform.parent);

        // 6 - Destroy food object
        Object.Destroy(food);
    }
}
