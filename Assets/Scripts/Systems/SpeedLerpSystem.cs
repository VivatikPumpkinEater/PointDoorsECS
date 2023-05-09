using System;
using Components;
using DefaultNamespace;
using Leopotam.EcsLite;
using Markers;

namespace Systems
{
    public class SpeedLerpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filterLerp;
        private EcsFilter _filterReset;
        
        private EcsPool<SpeedComponents> _speedPool;
        private EcsPool<NeedReset> _needResetPool;

        private float _deltaTime;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filterLerp = world.Filter<SpeedComponents>().Inc<NeedMove>().Exc<NeedRotation>().End();
            _filterReset = world.Filter<SpeedComponents>().Inc<NeedReset>().Exc<NeedMove>().Exc<NeedRotation>().End();

            _speedPool = world.GetPool<SpeedComponents>();
            _needResetPool = world.GetPool<NeedReset>();
            
            _deltaTime = systems.GetShared<SharedTime>().DeltaTime;
        }
        
        public void Run(IEcsSystems systems)
        {
            _deltaTime = systems.GetShared<SharedTime>().DeltaTime;

            Acceleration();
            Reset();
        }

        private void Acceleration()
        {
            foreach (var entity in _filterLerp)
            {
                ref var speed = ref _speedPool.Get(entity);

                speed.MovementSpeed = Math.Min(speed.MovementSpeed + speed.Acceleration * _deltaTime , speed.MaxMovementSpeed);
                
                DataListener<SpeedComponents>.UpdateComponent(entity, speed);
            }
        }
        
        private void Reset()
        {
            foreach (var entity in _filterReset)
            {
                ref var speed = ref _speedPool.Get(entity);

                speed.MovementSpeed = 0f;
                
                DataListener<SpeedComponents>.UpdateComponent(entity, speed);
                
                _needResetPool.Del(entity);
            }
        }
    }
}