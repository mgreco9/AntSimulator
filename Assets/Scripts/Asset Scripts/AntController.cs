using Assets.Scripts.Input_Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 1;
    [SerializeField] private float turnSpeed = 1;

    private Rigidbody2D rbody;
    private AgentInputManager cinput;
    private DetectorManager dinput;
    private Transform grabbingPoint;

    private GameObject objectHeld;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<AgentInputManager>();
        if (cinput == null)
            Debug.Log("Input Manager could not be found");

        dinput = GetComponent<DetectorManager>();
        if (dinput == null)
            Debug.Log("Detector Manager could not be found");

        grabbingPoint = transform.GetChild(0);
        if (grabbingPoint == null)
            Debug.Log("Grabbing Point transform could not be found");
    }

    // Update is called once per frame
    void Update()
    {
        // 1 - If no input controller, nothing to do
        if (cinput == null)
            return;
           
        // 2 - Retrieve the command inputs
        AgentInputs commandInputs = cinput.inputs;

        // 3 - Retrieve the detector inputs
        AntDetectorInputs detectorInputs = dinput.inputs;
        
        // 4 - Call the ant's actions
        MoveAction(commandInputs);
        InputGrabAction(commandInputs, detectorInputs);
    }

    private void MoveAction(AgentInputs commandInputs)
    {
        // 1 - Retrieve inputs move and turn values
        float inputForward = commandInputs.Forward;
        float inputTurn = commandInputs.Turn;

        // 2 - Compute new position
        Vector3 newPosition = transform.position + inputForward * forwardSpeed * transform.up;
        rbody.MovePosition(newPosition);

        // 3 - Compute new rotation
        float newRotation = rbody.rotation + inputTurn * turnSpeed;
        rbody.MoveRotation(newRotation);
    }

    private void InputGrabAction(AgentInputs commandInputs, AntDetectorInputs detectorInputs)
    {
        // 1 - Check if the command was pressed
        if (!commandInputs.Grab)
            return;

        // 2 - Check if the ant is not already holding something, if so release the object
        if(objectHeld != null)
        {
            DropAction(detectorInputs);
            return;
        }

        // 3 - Check if an object is in reach, if so grab it
        GameObject foodToGrab = detectorInputs.grabableFood;
        if (foodToGrab != null)
        {
            GrabAction(foodToGrab);
            return;
        }
    }

    private void GrabAction(GameObject foodToGrab)
    {
        foodToGrab.transform.SetParent(grabbingPoint, true);

        foodToGrab.transform.localPosition = Vector3.zero;

        objectHeld = foodToGrab;
    }

    private void DropAction(AntDetectorInputs detectorInputs)
    {
        // 1 - Drop the object
        objectHeld.transform.SetParent(null, true);

        // 2 - Check if object is drop on base, if so notify the anthill that a food has been brought
        if (detectorInputs.anthillBase != null)
        {
            GameObject anthill = detectorInputs.anthillBase;

            anthill.GetComponent<AnthillScorer>().BringFoodToBase(objectHeld);
        }

        // 3 - Remove the object from cache
        objectHeld = null;
    }
}