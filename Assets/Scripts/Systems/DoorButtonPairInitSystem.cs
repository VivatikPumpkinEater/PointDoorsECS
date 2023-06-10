using Leopotam.EcsLite;
using Markers;

namespace Systems
{
    public class DoorButtonPairInitSystem : IEcsInitSystem
    {
        private readonly DoorButtonPair[] _doorButtonPairs;
        
        public DoorButtonPairInitSystem(DoorButtonPair[] doorButtonPair)
        {
            _doorButtonPairs = doorButtonPair;
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach (var doorButtonPair in _doorButtonPairs)
            {
                var door = doorButtonPair.Door;
                var button = doorButtonPair.Button;
                
                var doorEntity = world.NewEntity();
                var buttonEntity = world.NewEntity();

                ref var doorComponent = ref world.GetPool<DoorComponents>().Add(doorEntity);
                doorComponent.CurrentPositionY= door.StartPositionY;
                doorComponent.StartPositionY = door.StartPositionY;
                doorComponent.EndPositionY = door.EndPositionY;
                doorComponent.Speed = 1f;

                ref var buttonComponent = ref world.GetPool<ButtonComponents>().Add(buttonEntity);
                buttonComponent.DoorEntity = doorEntity;
                buttonComponent.Radius = button.Radius;
                buttonComponent.Position = button.Position;

                world.GetPool<DoorMarker>().Add(doorEntity);
                world.GetPool<ButtonMarker>().Add(buttonEntity);
                world.GetPool<NonActiveButtonMarker>().Add(buttonEntity);
                
                door.SetData(doorEntity, world);
                DataListener<DoorComponents>.AddComponent(doorEntity, doorComponent);
            }
        }
    }
}