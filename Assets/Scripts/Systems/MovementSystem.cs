using Components;
using Leopotam.EcsLite;
using Markers;

public class MovementSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<Position> _position;
    private EcsPool<MoveTo> _poolMoveTo;
    private EcsPool<NeedMove> _poolNeedMove;
    private EcsPool<SpeedComponents> _speedPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<NeedMove>().Inc<MoveTo>().Exc<NeedRotation>().End();

        _position = world.GetPool<Position>();
        _poolMoveTo = world.GetPool<MoveTo>();
        _speedPool = world.GetPool<SpeedComponents>();
        _poolNeedMove = world.GetPool<NeedMove>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var position = ref _position.Get(entity);
            ref var moveTo = ref _poolMoveTo.Get(entity);
            ref var speed = ref _speedPool.Get(entity);

            if (!MonoBehaviorUtility.Destination(position.Value, moveTo.Position, 0.5f))
            {
                speed.MovementSpeed = 0f;
                _poolNeedMove.Del(entity);
                return;
            }
            
            var nextPosition = MonoBehaviorUtility.GetNextPosition(position.Value, moveTo.Position, speed.MovementSpeed);
            position.Value = nextPosition;
            
            DataListener<Position>.UpdateComponent(entity, position);
        }
    }
}