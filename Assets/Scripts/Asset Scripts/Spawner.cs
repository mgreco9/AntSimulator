using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private int numberToSpawn;
    [SerializeField] private float frequency;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnOnInterval());
    }

    private IEnumerator spawnOnInterval()
    {
        for (int spawnedObject = 0; spawnedObject < numberToSpawn; spawnedObject++)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(frequency);
        }
    }
}
