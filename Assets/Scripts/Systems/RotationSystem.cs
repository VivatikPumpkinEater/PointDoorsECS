using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class RotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<Rotation> _rotationPool;
        private EcsPool<Position> _positionPool;
        private EcsPool<MoveTo> _moveToPool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<Rotation>().End();
            _rotationPool = world.GetPool<Rotation>();
            _moveToPool = world.GetPool<MoveTo>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var position = ref _positionPool.Get(entity);
                ref var moveTo = ref _moveToPool.Get(entity);
                ref var rotation = ref _rotationPool.Get(entity);
                
            }
        }
    }
}