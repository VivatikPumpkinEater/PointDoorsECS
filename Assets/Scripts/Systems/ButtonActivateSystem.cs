using Components;
using Leopotam.EcsLite;
using Markers;
using UnityEngine;

namespace Systems
{
    public class ButtonActivateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _nonActiveButtonFilter;
        private EcsFilter _activeButtonFilter;
        private EcsFilter _playerFilter;
        
        private EcsPool<ButtonComponents> _buttonPool;
        private EcsPool<Position> _playerPositionPool;

        private EcsPool<NeedOpen> _needOpenPool;
        private EcsPool<NeedClose> _needClosePool;

        private EcsPool<ActiveButtonMarker> _activeButtonPool;
        private EcsPool<NonActiveButtonMarker> _nonActiveButtonPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _nonActiveButtonFilter = world.Filter<ButtonMarker>().Inc<ButtonComponents>().Inc<NonActiveButtonMarker>().Exc<ActiveButtonMarker>().End();
            _activeButtonFilter = world.Filter<ButtonMarker>().Inc<ButtonComponents>().Inc<ActiveButtonMarker>().Exc<NonActiveButtonMarker>().End();
            _playerFilter = world.Filter<PlayerMarker>().Inc<Position>().End();

            _buttonPool = world.GetPool<ButtonComponents>();
            _playerPositionPool = world.GetPool<Position>();
            _needOpenPool = world.GetPool<NeedOpen>();
            _needClosePool = world.GetPool<NeedClose>();
            _activeButtonPool = world.GetPool<ActiveButtonMarker>();
            _nonActiveButtonPool = world.GetPool<NonActiveButtonMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var buttonEntity in _nonActiveButtonFilter)
            {
                ref var buttonComponent = ref _buttonPool.Get(buttonEntity);
                var position = buttonComponent.Position;
                var buttonRadius = buttonComponent.Radius;

                foreach (var playerEntity in _playerFilter)
                {
                    ref var playerPosition = ref _playerPositionPool.Get(playerEntity);

                    if (MonoBehaviorUtility.Destination(position, playerPosition.Value, buttonRadius))
                        continue;
                    
                    _nonActiveButtonPool.Del(buttonEntity);
                    _activeButtonPool.Add(buttonEntity);
                    
                    _needOpenPool.Add(buttonComponent.DoorEntity);
                    _needClosePool.Del(buttonComponent.DoorEntity);
                }
            }
            
            foreach (var buttonEntity in _activeButtonFilter)
            {
                ref var buttonComponent = ref _buttonPool.Get(buttonEntity);
                var position = buttonComponent.Position;
                var buttonRadius = buttonComponent.Radius;

                foreach (var playerEntity in _playerFilter)
                {
                    ref var playerPosition = ref _playerPositionPool.Get(playerEntity);

                    if (!MonoBehaviorUtility.Destination(position, playerPosition.Value, buttonRadius))
                        continue;
                    
                    _nonActiveButtonPool.Add(buttonEntity);
                    _activeButtonPool.Del(buttonEntity);
                    
                    _needOpenPool.Del(buttonComponent.DoorEntity);
                    _needClosePool.Add(buttonComponent.DoorEntity);
                }
            }
        }
    }
}