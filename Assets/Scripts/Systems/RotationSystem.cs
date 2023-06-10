using Components;
using Leopotam.EcsLite;
using Markers;

namespace Systems
{
    public class RotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<Rotation> _rotationPool;
        private EcsPool<Position> _positionPool;
        private EcsPool<NeedRotation> _needRotationPool;
        private EcsPool<MoveTo> _moveToPool;
        private EcsPool<SpeedComponents> _speedPool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<NeedRotation>().Inc<Rotation>().Inc<MoveTo>().Inc<Position>().End();
            
            _rotationPool = world.GetPool<Rotation>();
            _moveToPool = world.GetPool<MoveTo>();
            _positionPool = world.GetPool<Position>();
            _speedPool = world.GetPool<SpeedComponents>();
            _needRotationPool = world.GetPool<NeedRotation>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var position = ref _positionPool.Get(entity);
                ref var moveTo = ref _moveToPool.Get(entity);
                ref var rotation = ref _rotationPool.Get(entity);
                ref var speed = ref _speedPool.Get(entity);

                var (needRotation, nextRotation) = MonoBehaviorUtility.GetNextRotation(rotation.Value, position.Value, moveTo.Position,
                    speed.RotationSpeed);

                if (!needRotation)
                {
                    _needRotationPool.Del(entity);
                }
                
                rotation.Value = nextRotation;
                
                DataListener<Rotation>.UpdateComponent(entity, rotation);
            }
        }
    }
}