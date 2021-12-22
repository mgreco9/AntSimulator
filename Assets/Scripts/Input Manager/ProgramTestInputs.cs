using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramTestInputs : MonoBehaviour
{
    public GameInputs inputs;

    public float forwardStart = 0f;
    public float turnStart = 0f;

    void Awake()
    {
        InputManager cinput = GetComponent<InputManager>();
        cinput.RegisterInputRetriever(RetrieveInputs);
    }

    void Start()
    {
        inputs.Forward = forwardStart;
        inputs.Turn = turnStart; 
    }

    public GameInputs RetrieveInputs()
    {
        return inputs;
    }
}
