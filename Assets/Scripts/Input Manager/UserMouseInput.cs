using UnityEngine;

namespace Assets.Scripts.Input_Manager
{
    class UserMouseInput : MonoBehaviour
    {
        Vector3 mousePreviousPosition;

        private void Awake()
        {
            MouseInputManager cinput = GetComponent<MouseInputManager>();
            cinput.RegisterInputRetriever(RetrieveInputs);
        }

        public MouseInputs RetrieveInputs()
        {
            MouseInputs inputs = new MouseInputs();

            inputs.MousePosition = MouseScreenPositionToWorldPosition(Input.mousePosition);
            inputs.LeftMouseDown = Input.GetMouseButtonDown(0);
            inputs.LeftMousePressed = Input.GetMouseButton(0);
            inputs.LeftMouseUp = Input.GetMouseButtonUp(0);

            inputs.RightMouseDown = Input.GetMouseButtonDown(1);
            inputs.RightMousePressed = Input.GetMouseButton(1);
            inputs.RightMouseUp = Input.GetMouseButtonUp(1);

            inputs.ScrollWheelDelta = Input.mouseScrollDelta.y;

            inputs.MouseShift = Input.mousePosition - mousePreviousPosition;
            mousePreviousPosition = Input.mousePosition;

            return inputs;
        }

        public static Vector3 MouseScreenPositionToWorldPosition(Vector3 localPosition)
        {
            localPosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(localPosition);
            worldPosition.z = 0;

            return worldPosition;
        }
    }
}
