using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Input_Manager
{
    class UserInput : MonoBehaviour
    {
        private void Awake()
        {
            InputManager cinput = GetComponent<InputManager>();
            cinput.RegisterInputRetriever(RetrieveInputs);
        }

        public GameInputs RetrieveInputs()
        {
            GameInputs inputs = new GameInputs();

            inputs.Turn = Input.GetAxisRaw("Horizontal");
            inputs.Forward = Input.GetAxisRaw("Vertical"); 
            inputs.Grab = Input.GetButtonDown("Grab");

            inputs.MousePosition = mouseScreenPositionToWorldPosition(Input.mousePosition);
            inputs.LeftMouseDown = Input.GetMouseButtonDown(0);
            inputs.LeftMousePressed = Input.GetMouseButton(0);
            inputs.LeftMouseUp = Input.GetMouseButtonUp(0);
            inputs.RightMouseDown = Input.GetMouseButtonDown(1);
            inputs.RightMousePressed = Input.GetMouseButton(1);

            return inputs;
        }

        public Vector3 mouseScreenPositionToWorldPosition(Vector3 localPosition)
        {
            localPosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(localPosition);
            worldPosition.z = 0;

            return worldPosition;
        }
    }
}
