using Leopotam.EcsLite;
using UnityEngine;

public class DoorView : MonoBehaviour
{
    private int _entity;
    private EcsPool<DoorComponents> _doorPool;

    public float StartPositionY => transform.position.y;
    public float EndPositionY => transform.position.y - transform.localScale.y;
    private Transform Transform => transform;

    public void SetData(int entity, EcsWorld world)
    {
        _entity = entity;

        _doorPool = world.GetPool<DoorComponents>();

        DataListener<DoorComponents>.EntityUpdate += UpdatePositionY;
    }

    private void UpdatePositionY(int entity)
    {
        if (_entity != entity)
            return;

        var positionY = _doorPool.Get(entity).CurrentPositionY;
        var lastPosition = Transform.position;
        lastPosition.y = positionY;
        Transform.position = lastPosition;
    }

    private void OnDestroy()
    {
        DataListener<DoorComponents>.EntityUpdate -= UpdatePositionY;
    }
}