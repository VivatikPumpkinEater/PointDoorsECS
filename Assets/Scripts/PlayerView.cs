using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private int _entity;

    private EcsPool<Position> _positionPool;
    private Position _data;

    public void SetData(int entity, EcsWorld world)
    {
        _entity = entity;

        _positionPool = world.GetPool<Position>();
        _data = _positionPool.Get(entity);

        DataListener<Position>.EntityUpdate += UpdatePosition;
    }

    private void UpdatePosition(int entity)
    {
        if (_entity != entity)
            return;
        
        transform.position = _positionPool.Get(entity).Value;
    }
}