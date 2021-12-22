using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnthillScorer : MonoBehaviour
{
    private FoodPool foodPool;
    private static AnthillScorer _instance;
    private UIManager _uiManager;

    private int score = 0;

    public void Awake()
    {
        singletonInstantiation();
    }

    public void Start()
    {
        _uiManager = UIManager.getInstance();
        if (_uiManager == null)
            Debug.Log("UI Manager could not be found");

        foodPool = FoodPool.getInstance();
        if (foodPool == null)
            Debug.Log("Food Pool could not be found");
    }

    private void singletonInstantiation()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static GameObject getInstanceGameObject()
    {
        return _instance.gameObject;
    }

    public void BringFoodToBase(GameObject food)
    {
        score++;
        _uiManager.UpdateScore(score);

        foodPool.requestPrefabDeactivation(food);
    }
}
