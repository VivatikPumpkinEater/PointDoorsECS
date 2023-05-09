using Components;
using Leopotam.EcsLite;
using Markers;
using UnityEngine;

namespace Systems
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _camera = Camera.main;

        private EcsWorld _world;
        
        private EcsFilter _filter;
        private EcsPool<MoveTo> _moveToPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world.Filter<PlayerMarker>().End();
            _moveToPool = _world.GetPool<MoveTo>();
        }
        
        public void Run(IEcsSystems systems)
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(ray, out var hit))
                return;

            foreach (var entity in _filter)
            {
                ref var moveTo = ref Utility.GetOrAddComponent(entity, _moveToPool);
                moveTo.Position = hit.point;

                Utility.GetOrAddComponent(entity, _world.GetPool<NeedMove>());
                Utility.GetOrAddComponent(entity, _world.GetPool<NeedRotation>());
            }
        }
    }
}