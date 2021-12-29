using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
public class TestAnthillScorer
{
    private GameObject antObject;
    private GameObject antHillObject;

    private TMP_Text scoreText;
    private ProgramTestInputs testInputs;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Canvas.prefab"));
        MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Generator.prefab"));
        antObject = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TestPrefabs/AntProgramTest.prefab"), Vector3.zero, Quaternion.identity);
        antHillObject = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/AntHill.prefab"), new Vector3(0,1), Quaternion.identity);

        yield return null;

        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        testInputs = antObject.GetComponent<ProgramTestInputs>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(antObject);
        Object.Destroy(antHillObject);
    }

    [UnityTest]
    public IEnumerator TestDetectFoodAsDefault()
    {
        // 1 - Check the detector return nothing when nothing instantiated
        Assert.AreEqual("Score : 0", scoreText.text);

        // 2 - Instantiate food out of reach of detector
        GameObject food = MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Food.prefab"), new Vector3(0, 0.8f), Quaternion.identity);
        yield return new WaitForSeconds(0.05f);

        // 3 - Grab the object
        testInputs.inputs.Grab = true;
        yield return null;
        testInputs.inputs.Grab = false;
        yield return null;

        // 4 - Drop the object in the base
        testInputs.inputs.Grab = true;
        yield return null;
        testInputs.inputs.Grab = false;
        yield return null;

        // 5 - Assert the score has been altered and food is destroyed
        Assert.AreEqual("Score : 1", scoreText.text);
        Assert.False(food.gameObject.activeSelf);
    }
}
