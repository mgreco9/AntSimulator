using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private float minScope;
    [SerializeField] private float maxScope;
    [SerializeField] private LayerMask detectorLayerMask;
    [SerializeField] private DetectorType detectorType;

    [SerializeField] private bool debugDraw;

    private void Awake()
    {
        DetectorManager dinput = GetComponent<DetectorManager>();
        dinput.RegisterDetector(detectorType, DetectObject);
    }

    public GameObject DetectObject()
    {
        // 1 - Compute the front direction of the object to check
        Vector3 rayDirection = MathUtils.computeYAxisDirection(transform);

        // 2 - Compute the origin
        Vector3 origin = transform.position + rayDirection * minScope;

        // 3 - Compute the target
        Vector3 target = transform.position + rayDirection * maxScope;

        // 4 - Instantiate the raycasting
        RaycastHit2D hit = Physics2D.Linecast(origin, target, detectorLayerMask);

        // 5 - If nothing was found
        Color debugColor = Color.red;
        GameObject gameObject = null;

        // 6 - If the raycasting has hit unto something
        if (hit.collider != null)
        {
            debugColor = Color.green;
            gameObject = hit.collider.gameObject;
        }

        if(debugDraw)
            Debug.DrawLine(origin, target, debugColor);
        return gameObject;
    }

    public void setDetectorLayerMask(LayerMask layerMask)
    {
        detectorLayerMask = layerMask;
    }
}
