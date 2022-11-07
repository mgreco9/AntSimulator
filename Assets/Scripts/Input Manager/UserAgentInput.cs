using UnityEngine;

namespace Assets.Scripts.Input_Manager
{
    class UserAgentInput : MonoBehaviour
    {
        protected private void Awake()
        {
            AgentInputManager cinput = GetComponent<AgentInputManager>();
            cinput.RegisterInputRetriever(RetrieveInputs);
        }

        public AgentInputs RetrieveInputs()
        {
            AgentInputs inputs = new AgentInputs();

            inputs.Turn = Input.GetAxisRaw("Horizontal");
            inputs.Forward = -Input.GetAxisRaw("Vertical");
            inputs.Grab = Input.GetButtonDown("Grab");

            return inputs;
        }
    }
}
