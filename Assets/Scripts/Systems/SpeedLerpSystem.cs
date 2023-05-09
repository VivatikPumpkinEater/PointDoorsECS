using System;
using Components;
using DefaultNamespace;
using Leopotam.EcsLite;
using Markers;

namespace Systems
{
    public class SpeedLerpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<SpeedComponents> _speedPool;

        private float _deltaTime;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<SpeedComponents>().Inc<NeedMove>().Exc<NeedRotation>().End();

            _speedPool = world.GetPool<SpeedComponents>();
            _deltaTime = systems.GetShared<SharedTime>().DeltaTime;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var speed = ref _speedPool.Get(entity);

                var deltaTime = systems.GetShared<SharedTime>().DeltaTime;
                speed.MovementSpeed = Math.Min(speed.MovementSpeed + speed.Acceleration * deltaTime , speed.MaxMovementSpeed);
                
                DataListener<SpeedComponents>.UpdateComponent(entity, speed);
            }
        }
    }
}