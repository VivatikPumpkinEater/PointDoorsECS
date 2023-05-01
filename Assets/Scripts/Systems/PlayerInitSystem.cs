using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private Transform _playerTransform;
        private PlayerView _playerView;
        
        public PlayerInitSystem(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            _playerView = _playerTransform.GetComponent<PlayerView>();
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();

            world.GetPool<PlayerMarker>().Add(entity);
            
            ref var positions = ref world.GetPool<Position>().Add(entity);
            positions.Value = _playerTransform.position;
            ref var moveTo = ref world.GetPool<MoveTo>().Add(entity);
            moveTo.Position = _playerTransform.position;
            
            _playerView.SetData(entity, world);
            DataListener<Position>.AddComponent(entity, positions);
        }
    }
}