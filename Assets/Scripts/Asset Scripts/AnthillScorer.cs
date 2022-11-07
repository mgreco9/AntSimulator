using Assets.Scripts.Utils;
using UnityEngine;

public class AnthillScorer : MonoBehaviour
{
    private FoodPool foodPool;
    private static AnthillScorer _instance;
    private UIManager _uiManager;

    private int score = 0;

    protected void Awake()
    {
        SingletonInstantiation();
    }

    protected void Start()
    {
        _uiManager = UIManager.getInstance();
        if (_uiManager == null)
            CustomLogger.LogMessage("UI Manager could not be found");

        foodPool = FoodPool.GetInstance();
        if (foodPool == null)
            CustomLogger.LogMessage("Food Pool could not be found");
    }

    private void SingletonInstantiation()
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

    public static GameObject GetInstanceGameObject()
    {
        return _instance.gameObject;
    }

    public void BringFoodToBase(GameObject food)
    {
        score++;
        _uiManager.UpdateScore(score);

        foodPool.RequestPrefabDeactivation(food);
    }
}
