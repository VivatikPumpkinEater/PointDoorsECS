using Components;
using Configs;
using Leopotam.EcsLite;
using Markers;
using UnityEngine;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly PlayerConfig _playerConfig;

        private Transform _playerTransform;
        private PlayerView _playerView;
        
        public PlayerInitSystem(Transform playerTransform, PlayerConfig playerConfig)
        {
            _playerTransform = playerTransform;
            _playerConfig = playerConfig;
            _playerView = _playerTransform.GetComponent<PlayerView>();
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();

            // var deltaTime = systems.GetShared<SharedTime>().DeltaTime;

            world.GetPool<PlayerMarker>().Add(entity);
            
            ref var positions = ref world.GetPool<Position>().Add(entity);
            positions.Value = _playerTransform.position;
            
            ref var moveTo = ref world.GetPool<MoveTo>().Add(entity);
            moveTo.Position = _playerTransform.position;
            moveTo.DestinationDistance = _playerConfig.DestinationDistance;

            ref var rotation = ref world.GetPool<Rotation>().Add(entity);
            rotation.Value = _playerTransform.rotation;

            ref var speedComponent = ref world.GetPool<SpeedComponents>().Add(entity);
            speedComponent.MovementSpeed = 0f;
            speedComponent.Acceleration = _playerConfig.Acceleration;
            speedComponent.MaxMovementSpeed = _playerConfig.MaxSpeed;
            speedComponent.RotationSpeed = _playerConfig.RotationSpeed;
            
            _playerView.SetData(entity, world);
            DataListener<Position>.AddComponent(entity, positions);
            DataListener<Rotation>.AddComponent(entity, rotation);
            DataListener<SpeedComponents>.AddComponent(entity, speedComponent);
        }
    }
}