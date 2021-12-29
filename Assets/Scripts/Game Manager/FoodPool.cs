using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    private static FoodPool _instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int numberOfInstances;

    private List<GameObject> activePool = new List<GameObject>();
    private Queue<GameObject> inactivePool = new Queue<GameObject>();

    public void Awake()
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

    void Start()
    {
        GeneratePrefabsInInactivePool(numberOfInstances);
    }

    private void GeneratePrefabsInInactivePool(int nb)
    {
        for(int i = 0; i < nb; i++)
        {
            GameObject oneInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            oneInstance.SetActive(false);
            inactivePool.Enqueue(oneInstance);
        }
    }

    public GameObject getPrefab()
    {
        return prefab;
    }

    public GameObject RequestPrefabActivation(Vector3 position)
    {
        if(inactivePool.Count == 0)
        {
            GeneratePrefabsInInactivePool(numberOfInstances);
            numberOfInstances *= 2;
        }

        GameObject prefabToActivate = inactivePool.Dequeue();

        prefabToActivate.SetActive(true);
        prefabToActivate.transform.position = position;

        activePool.Add(prefabToActivate);

        return prefabToActivate;
    }

    public void RequestPrefabDeactivation(GameObject prefabToDeactivate)
    {
        activePool.Remove(prefabToDeactivate);

        prefabToDeactivate.SetActive(false);

        inactivePool.Enqueue(prefabToDeactivate);
    }

    public Transform GetRandomActivePrefab()
    {
        if (activePool.Count == 0)
            return null;

        int randomIdx = Random.Range(0, activePool.Count);
        return activePool[randomIdx].transform;
    }
}
