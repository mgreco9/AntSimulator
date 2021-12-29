using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MouseInputs
{
    // Food spawner controls
    public Vector3 MousePosition;
    public bool LeftMouseDown;
    public bool LeftMousePressed;
    public bool LeftMouseUp;

    // Camera controls
    public bool RightMouseDown;
    public bool RightMousePressed;
    public bool RightMouseUp;
    public float ScrollWheelDelta;
    public Vector3 MouseShift;
}

public class MouseInputManager : MonoBehaviour
{
    public MouseInputs inputs;

    private Func<MouseInputs> inputRetriever;

    public void RegisterInputRetriever(Func<MouseInputs> inputRetriever)
    {
        this.inputRetriever = inputRetriever;
    }

    void Update()
    {
        if(inputRetriever != null)
            inputs = inputRetriever();
    }
}
