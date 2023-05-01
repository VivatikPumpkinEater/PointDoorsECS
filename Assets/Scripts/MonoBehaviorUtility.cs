using UnityEngine;

public static class MonoBehaviorUtility
{
    public static Vector3 GetNextPosition(Vector3 startPosition, Vector3 targetPosition, float speed)
    {
        return startPosition + (targetPosition - startPosition).normalized * speed;
    }

    public static bool Destination(Vector3 currentPosition, Vector3 endPosition, float endReachDistance)
    {
        return Vector3.Distance(currentPosition, endPosition) > endReachDistance;
    }
}