using UnityEngine;

namespace Configs
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerView Prefab;
        public float Acceleration;
        public float MaxSpeed;
        public float RotationSpeed;
        public float DestinationDistance;
    }
}