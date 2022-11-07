using UnityEngine;

public class ProgramTestInputs : MonoBehaviour
{
    public AgentInputs inputs;

    public float forwardStart = 0f;
    public float turnStart = 0f;

    void Awake()
    {
        AgentInputManager cinput = GetComponent<AgentInputManager>();
        cinput.RegisterInputRetriever(RetrieveInputs);
    }

    void Start()
    {
        inputs.Forward = forwardStart;
        inputs.Turn = turnStart;
    }

    public AgentInputs RetrieveInputs()
    {
        return inputs;
    }
}
