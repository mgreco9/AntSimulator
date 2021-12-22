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
        singletonInstantiation();
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
    public static FoodPool getInstance()
    {
        return _instance;
    }

    void Start()
    {
        generatePrefabsInInactivePool(numberOfInstances);
    }

    private void generatePrefabsInInactivePool(int nb)
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

    public GameObject requestPrefabActivation(Vector3 position)
    {
        if(inactivePool.Count == 0)
        {
            generatePrefabsInInactivePool(numberOfInstances);
            numberOfInstances *= 2;
        }

        GameObject prefabToActivate = inactivePool.Dequeue();

        prefabToActivate.SetActive(true);
        prefabToActivate.transform.position = position;

        activePool.Add(prefabToActivate);

        return prefabToActivate;
    }

    public void requestPrefabDeactivation(GameObject prefabToDeactivate)
    {
        activePool.Remove(prefabToDeactivate);

        prefabToDeactivate.SetActive(false);

        inactivePool.Enqueue(prefabToDeactivate);
    }
}
