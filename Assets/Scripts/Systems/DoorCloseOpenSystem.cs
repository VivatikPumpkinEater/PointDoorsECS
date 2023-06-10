using DefaultNamespace;
using Leopotam.EcsLite;
using Markers;

namespace Systems
{
    public class DoorCloseOpenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _openDoorFilter;
        private EcsFilter _closeDoorFilter;
        
        private EcsPool<DoorComponents> _doorComponentPool;
        private EcsPool<NeedOpen> _needOpenPool;
        private EcsPool<NeedClose> _needClosePool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _openDoorFilter = world.Filter<NeedOpen>().Exc<NeedClose>().End();
            _closeDoorFilter = world.Filter<NeedClose>().Exc<NeedOpen>().End();

            _doorComponentPool = world.GetPool<DoorComponents>();
            _needOpenPool = world.GetPool<NeedOpen>();
            _needClosePool = world.GetPool<NeedClose>();
        }

        public void Run(IEcsSystems systems)
        {
            var deltaTime = systems.GetShared<SharedTime>().DeltaTime;

            foreach (var entity in _openDoorFilter)
            {
                ref var doorComponent = ref _doorComponentPool.Get(entity);

                doorComponent.CurrentPositionY -= doorComponent.Speed * deltaTime;
                
                if (doorComponent.CurrentPositionY <= doorComponent.EndPositionY)
                    _needOpenPool.Del(entity);
                
                DataListener<DoorComponents>.UpdateComponent(entity, doorComponent);
            }

            foreach (var entity in _closeDoorFilter)
            {
                ref var doorComponent = ref _doorComponentPool.Get(entity);

                doorComponent.CurrentPositionY += doorComponent.Speed * deltaTime;
                
                if (doorComponent.CurrentPositionY >= doorComponent.StartPositionY)
                    _needClosePool.Del(entity);
                
                DataListener<DoorComponents>.UpdateComponent(entity, doorComponent);
            }
        }
    }
}