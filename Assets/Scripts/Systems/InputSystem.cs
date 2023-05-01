using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _camera = Camera.main;

        private EcsFilter _filter;
        private EcsPool<MoveTo> _pool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _filter = world.Filter<PlayerMarker>().End();
            _pool = world.GetPool<MoveTo>();
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
                ref var moveTo = ref Utility.GetOrAddComponent(entity, _pool);
                moveTo.Position = hit.point;
            }
        }
    }
}