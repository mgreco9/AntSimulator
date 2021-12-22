
using UnityEngine;

public static class MathUtils
{

    public static Vector3 computeNewPositionTransformForward(Transform transform, float inputForward)
    {
        Vector3 forwardDirection = computeYAxisDirection(transform);
        Vector3 moveVector = forwardDirection * inputForward;

        Vector3 newPosition = transform.position + moveVector;

        return newPosition;
    }

    public static Vector3 computeYAxisDirection(Transform transform)
    {
        float rotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        float xValue = -Mathf.Sin(rotation);
        float yValue = Mathf.Cos(rotation);

        Vector3 yAxisDirection = new Vector3(xValue, yValue);

        return yAxisDirection;
    }

}
