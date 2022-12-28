using Assets.Scripts.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.Utils.CustomLogger;

public class AntController : MonoBehaviour
{
    // The 
    [SerializeField] private float forwardSpeed = 1;
    [SerializeField] private float turnSpeed = 1;

    // 
    private Rigidbody2D rbody;
    private AgentInputManager cinput;
    private DetectorManager dinput;
    private Transform grabbingPoint;

    private GameObject objectHeld;

    public GameObject action;
    public DetectorManager detector;
    public Transform truc;

    protected void Awake()
    {
        if (!TryGetComponent(out rbody))
            LogMessage("Rigid body could not be found", LogFlag.AntController);

        if (!TryGetComponent(out cinput))
            LogMessage("Input Manager could not be found", LogFlag.AntController);

        if (!TryGetComponent(out dinput))
            LogMessage("Detector Manager could not be found", LogFlag.AntController);

        grabbingPoint = transform.GetChild(0);
        if (grabbingPoint == null)
            LogMessage("Grabbing Point transform could not be found", LogFlag.AntController);
    }

    protected void Update()
    {
        // 1 - Retrieve the command inputs
        AgentInputs commandInputs = cinput.inputs;

        // 2 - Retrieve the detector inputs
        AntDetectorInputs detectorInputs = dinput.inputs;

        // 3 - Call the ant's actions
        MoveAction(commandInputs);
        InputGrabAction(commandInputs, detectorInputs);
    }

    /// <summary>
    /// Move the Ant object based on the command inputs
    /// </summary>
    /// <param name="commandInputs">The command inputs containing the instructions sent by the user</param>
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

    /// <summary>
    /// Determine if the grab input has been pressed
    /// If so, determine if the agent needs to drop the current object or grab the one in reach
    /// Do nothing if none are possible
    /// </summary>
    /// <param name="commandInputs">The command inputs containing the instructions sent by the user</param>
    /// <param name="detectorInputs">The detector inputs containing the information from the agent's detector</param>
    private void InputGrabAction(AgentInputs commandInputs, AntDetectorInputs detectorInputs)
    {
        // 1 - Check if the command was pressed
        if (!commandInputs.Grab)
            return;

        // 2 - Check if the ant is not already holding something, if so release the object
        if (objectHeld != null)
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

    /// <summary>
    /// Grab the object specified in input
    /// </summary>
    /// <param name="foodToGrab">The object to grab</param>
    private void GrabAction(GameObject foodToGrab)
    {
        // 1 - Set the food as local to the ant (grab it)
        foodToGrab.transform.SetParent(grabbingPoint, true);
        foodToGrab.transform.localPosition = Vector3.zero;

        // 2 - Change the layer of the food so other ants can't detect it
        foodToGrab.layer = 0;

        // 3 - Set object held in the cache
        objectHeld = foodToGrab;
    }

    /// <summary>
    /// Drop the currently held object.
    /// If detector 
    /// </summary>
    /// <param name="detectorInputs"></param>
    private void DropAction(AntDetectorInputs detectorInputs)
    {
        // 1 - Drop the object
        objectHeld.transform.SetParent(null, true);
        objectHeld.layer = 3;

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