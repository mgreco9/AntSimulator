using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    private static FoodPool _instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int numberOfInstances;

    private List<GameObject> activePool = new List<GameObject>();
    private Queue<GameObject> inactivePool = new Queue<GameObject>();

    protected void Awake()
    {
        SingletonInstantiation();
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
    public static FoodPool GetInstance()
    {
        return _instance;
    }

    protected void Start()
    {
        GeneratePrefabsInInactivePool(numberOfInstances);
    }

    /// <summary>
    /// Generate the number of instances set as inactive
    /// </summary>
    /// <param name="numberOfInstances">The number of instances to generate</param>
    private void GeneratePrefabsInInactivePool(int numberOfInstances)
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject oneInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            oneInstance.SetActive(false);
            inactivePool.Enqueue(oneInstance);
        }
    }

    /// <summary>
    /// Get the model prefab used to generate the instances
    /// </summary>
    /// <returns>The model prefab</returns>
    public GameObject GetPrefab()
    {
        return prefab;
    }

    /// <summary>
    /// Request for a deactivated prefab to be activated at a specific location
    /// </summary>
    /// <param name="position">The position where the prefab needs to be activated</param>
    /// <returns>The activated object</returns>
    public GameObject RequestPrefabActivation(Vector3 position)
    {
        CustomLogger.LogMessage("RequestPrefabActivation : " + activePool.Count);
        // 1 - Check if any prefab is available, if not double the number of prefabs
        if (inactivePool.Count == 0)
        {
            GeneratePrefabsInInactivePool(numberOfInstances);
            numberOfInstances *= 2;
        }

        // 2 - Retrieve one of the non active prefab
        GameObject prefabToActivate = inactivePool.Dequeue();

        // 3 - Set the prefab as active and set the position
        prefabToActivate.transform.position = position;
        prefabToActivate.SetActive(true);

        // 4 - Add the prefab to the active pool list
        activePool.Add(prefabToActivate);

        // 5 - Return the activated prefab
        return prefabToActivate;
    }

    /// <summary>
    /// Request for an activated prefab to be deactivated
    /// </summary>
    /// <param name="prefabToDeactivate">The prefab to deactivate</param>
    public void RequestPrefabDeactivation(GameObject prefabToDeactivate)
    {
        CustomLogger.LogMessage("RequestPrefabDeactivation : " + activePool.Count);
        // 1 - Remove the prefab to deactivate from the active list
        if (!activePool.Remove(prefabToDeactivate))
            return;

        // 2 - Deactivate the prefab
        prefabToDeactivate.SetActive(false);

        // 3 - Add the prefab to the inactive pool
        inactivePool.Enqueue(prefabToDeactivate);
    }

    /// <summary>
    /// Return a random active prefab transform
    /// </summary>
    /// <returns>An active prefab transform</returns>
    public Transform GetRandomPrefabTransform()
    {
        // 1 - Check that there are at least one active prefab
        if (activePool.Count == 0)
            return null;

        // 2 - Select randomly a prefab and return it
        int randomIdx = Random.Range(0, activePool.Count);
        return activePool[randomIdx].transform;
    }
}
