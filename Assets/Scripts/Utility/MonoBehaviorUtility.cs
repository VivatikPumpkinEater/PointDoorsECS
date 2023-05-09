using UnityEngine;

public static class MonoBehaviorUtility
{
    public static Vector3 GetNextPosition(Vector3 startPosition, Vector3 targetPosition, float speed)
    {
        return Vector3.MoveTowards(startPosition, targetPosition, speed);
    }

    public static bool Destination(Vector3 currentPosition, Vector3 endPosition, float endReachDistance)
    {
        return Vector3.Distance(currentPosition, endPosition) > endReachDistance;
    }

    public static (bool, Quaternion) GetNextRotation(Quaternion startRotation, Vector3 position, Vector3 target, float speed)
    {
        var targetDirection = target - position;
        targetDirection.y = 0f;
        
        var lookRotation = Quaternion.LookRotation(targetDirection);

        return (NeedRotation(startRotation, lookRotation), Quaternion.RotateTowards(startRotation, lookRotation, speed));
    }

    public static bool NeedRotation(Quaternion rotation, Quaternion targetRotation)
    {
        var angle = Quaternion.Angle(rotation, targetRotation);

        return angle > 0.5f;
    }
}