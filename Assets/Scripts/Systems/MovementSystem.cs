using Components;
using Leopotam.EcsLite;

public class MovementSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<Position> _position;
    private EcsPool<MoveTo> _poolMoveTo;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<PlayerMarker>().Inc<MoveTo>().End();

        _position = world.GetPool<Position>();
        _poolMoveTo = world.GetPool<MoveTo>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var position = ref _position.Get(entity);
            ref var moveTo = ref _poolMoveTo.Get(entity);

            if (!MonoBehaviorUtility.Destination(position.Value, moveTo.Position, 0.5f))
            {
                _poolMoveTo.Del(entity);
                return;
            }
            
            var nextPosition = MonoBehaviorUtility.GetNextPosition(position.Value, moveTo.Position, 0.01f);
            position.Value = nextPosition;
            
            DataListener<Position>.UpdateComponent(entity, position);
        }
    }
}