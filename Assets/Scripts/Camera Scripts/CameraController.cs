using Assets.Scripts.Utils;
using UnityEngine;
using static Assets.Scripts.Utils.CustomLogger;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float MinZoom = 5;
    [SerializeField] private float MaxZoom = 50;

    [SerializeField] private float ZoomSpeed = 1f;
    [SerializeField] private float DragSpeed = 1f;

    private Camera cam;
    private MouseInputManager cinput;

    protected void Awake()
    {
        cinput = GetComponent<MouseInputManager>();
        if (cinput == null)
            CustomLogger.LogMessage("Input Manager could not be found", LogFlag.Camera);

        cam = Camera.main;
        if (cam == null)
            CustomLogger.LogMessage("Camera could not be found", LogFlag.Camera);
    }

    // Update is called once per frame
    protected void Update()
    {
        // 1 - Retrieve the command inputs
        MouseInputs commandInputs = cinput.inputs;

        DragCameraCommand(commandInputs);
        ZoomControlCamera(commandInputs);
    }

    private void DragCameraCommand(MouseInputs inputs)
    {
        if (!CheckCameraIsDragged(inputs))
            return;

        transform.Translate(-inputs.MouseShift * DragSpeed);
    }

    private bool CheckCameraIsDragged(MouseInputs inputs)
    {
        if (inputs.RightMouseDown || inputs.RightMouseUp)
            return false;

        if (inputs.RightMousePressed)
            return true;

        return false;
    }

    private void ZoomControlCamera(MouseInputs inputs)
    {
        float orthographicSize = cam.orthographicSize - inputs.ScrollWheelDelta * ZoomSpeed;

        orthographicSize = Mathf.Max(orthographicSize, MinZoom);
        orthographicSize = Mathf.Min(orthographicSize, MaxZoom);

        cam.orthographicSize = orthographicSize;
    }
}
