using Leopotam.EcsLite;
using Systems;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public Transform PlayerTransform;
    
    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems
            .Add(new PlayerInitSystem(PlayerTransform))
            .Add(new InputSystem())
            .Add(new MovementSystem())
            .Init();
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}