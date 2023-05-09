using Configs;
using DefaultNamespace;
using Leopotam.EcsLite;
using Systems;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;

    public Transform PlayerTransform;

    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world, new SharedTime {DeltaTime = Time.deltaTime});
        _systems
            .Add(new PlayerInitSystem(PlayerTransform, _playerConfig))
            .Add(new InputSystem())
            .Add(new MovementSystem())
            .Add(new RotationSystem())
            .Add(new SpeedLerpSystem())
            .Init();
    }

    private void Update()
    {
        _systems.GetShared<SharedTime>().DeltaTime = Time.deltaTime;
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