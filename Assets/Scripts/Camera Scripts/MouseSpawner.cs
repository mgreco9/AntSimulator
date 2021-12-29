using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRange = 1f;
    [SerializeField] private float frequency = 0.1f;

    private FoodPool foodPool;
    private MouseInputManager cinput;
    private List<GameObject> gameObjectsToAvoid;
    private float lastSpawnTimeStamp;

    private bool mouseDownOverWorldFlag = false;

    // Start is called before the first frame update
    void Awake ()
    {
        cinput = GetComponent<MouseInputManager>();
        if (cinput == null)
            Debug.Log("Input Manager could not be found");
    }

    void Start()
    {
        foodPool = FoodPool.GetInstance();
        if (foodPool == null)
            Debug.Log("Food Pool could not be found");

        gameObjectsToAvoid = new List<GameObject>();
        gameObjectsToAvoid.Add(AnthillScorer.getInstanceGameObject());

        lastSpawnTimeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // 1 - Retrieve the command inputs
        MouseInputs commandInputs = cinput.inputs;

        // 2 - Check if spawn input is identified
        if (!CheckIfSpawnInput(commandInputs))
            return;

        // 3 - Compute the position where to spawn the prefab
        Vector3 spawnPosition = ComputeSpawnPosition(commandInputs);

        // 4 - Check if the position is valid
        if (!CheckPositionValid(spawnPosition))
            return;

        // 5 - Spawn the prefab at the specific position
        SpawnObject(spawnPosition);
    }

    private bool CheckIfSpawnInput(MouseInputs commandInputs)
    {
        // 1 - Retrieve the mouse inputs
        bool leftMouseDown = commandInputs.LeftMouseDown;
        bool leftMousePressed = commandInputs.LeftMousePressed;
        bool leftMouseUp = commandInputs.LeftMouseUp;
        bool mouseOverUIElement = EventSystem.current.IsPointerOverGameObject();

        // 2 - If the left click is pressed, it must not be over an ui, start spawn sequence
        if (leftMouseDown && !mouseOverUIElement)
        {
            mouseDownOverWorldFlag = true;
        }

        // 3 - When left click is up, stop spawn sequence
        if (leftMouseUp)
        {
            mouseDownOverWorldFlag = false;
        }

        // 4 - If left mouse is pressed (but not over ui) return true
        return (leftMousePressed && mouseDownOverWorldFlag && Time.time > (lastSpawnTimeStamp + frequency));
    }

    private Vector3 ComputeSpawnPosition(MouseInputs commandInputs)
    {
        // 1 - Retrieve the mouse position
        Vector3 mousePosition = commandInputs.MousePosition;

        // 2 - Compute the local position from the reference
        float range = Random.Range(0, spawnRange);
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float xPos = range*Mathf.Cos(angle);
        float yPos = range*Mathf.Sin(angle);

        Vector3 randomDiff = new Vector3(xPos, yPos);

        // 3 - Return the global position
        return mousePosition + randomDiff;
    }

    private bool CheckPositionValid(Vector3 position)
    {
        // 1 - Compute the prefab radius
        float radius = foodPool.getPrefab().transform.localScale.x/2;

        // 2 - For each game object to avoid
        foreach(GameObject avoid in gameObjectsToAvoid)
        {
            // 2.1 - Compute the object to avoid radius and positio,
            float avoidRadius = avoid.transform.localScale.x/2;
            Vector3 avoidPosition = avoid.transform.localPosition;

            // 2.2 - If the prefab and the object to avoid intersect, return false
            if (Vector3.Distance(position, avoidPosition) < avoidRadius + radius)
                return false;
        }

        // 3 - If prefab do not intersect with any object to avoid, return true;
        return true;
    }

    private void SpawnObject(Vector3 position)
    {
        foodPool.RequestPrefabActivation(position);

        lastSpawnTimeStamp = Time.time;
    }
}
