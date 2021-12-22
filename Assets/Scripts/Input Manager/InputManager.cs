using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameInputs
{
    public float Forward;
    public float Turn;
    public bool Grab;

    public Vector3 MousePosition;
    public bool LeftMouseDown;
    public bool LeftMousePressed;
    public bool LeftMouseUp;
    public bool RightMouseDown;
    public bool RightMousePressed;
}

public class InputManager : MonoBehaviour
{
    public GameInputs inputs;

    private Func<GameInputs> inputRetriever;

    public void RegisterInputRetriever(Func<GameInputs> inputRetriever)
    {
        this.inputRetriever = inputRetriever;
    }

    void Update()
    {
        if(inputRetriever != null)
            inputs = inputRetriever();
    }
}
