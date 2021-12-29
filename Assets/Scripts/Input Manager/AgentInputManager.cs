using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AgentInputs
{
    // Ant controls
    public float Forward;
    public float Turn;
    public bool Grab;
}

public class AgentInputManager : MonoBehaviour
{
    public AgentInputs inputs;

    private Func<AgentInputs> inputRetriever;

    public void RegisterInputRetriever(Func<AgentInputs> inputRetriever)
    {
        this.inputRetriever = inputRetriever;
    }

    void Update()
    {
        if(inputRetriever != null)
            inputs = inputRetriever();
    }
}
