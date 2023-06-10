using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _entity;
    private EcsPool<Position> _positionPool;
    private EcsPool<Rotation> _rotationPool;
    private EcsPool<SpeedComponents> _speedPool;
    
    private readonly int _speedKey = Animator.StringToHash("Speed");

    public void SetData(int entity, EcsWorld world)
    {
        _entity = entity;

        _positionPool = world.GetPool<Position>();
        _rotationPool = world.GetPool<Rotation>();
        _speedPool = world.GetPool<SpeedComponents>();

        DataListener<Position>.EntityUpdate += UpdatePosition;
        DataListener<Rotation>.EntityUpdate += UpdateRotation;
        DataListener<SpeedComponents>.EntityUpdate += BlendAnimation;
    }

    private void UpdatePosition(int entity)
    {
        if (_entity != entity)
            return;
        
        var position = _positionPool.Get(entity).Value;
        transform.position = position;
    }

    private void BlendAnimation(int entity)
    {
        if (_entity != entity)
            return;

        ref var t = ref _speedPool.Get(entity).MovementSpeed;
        
        var a = Mathf.Min(t, 1f);
        _animator.SetFloat(_speedKey, a);
    }

    private void UpdateRotation(int entity)
    {
        if (_entity != entity)
            return;

        var rotation = _rotationPool.Get(entity).Value;
        transform.rotation = rotation;
    }

    private void OnDestroy()
    {
        DataListener<Position>.EntityUpdate -= UpdatePosition;
        DataListener<Rotation>.EntityUpdate -= UpdateRotation;
        DataListener<SpeedComponents>.EntityUpdate -= BlendAnimation;
    }
}