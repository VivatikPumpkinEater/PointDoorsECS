using UnityEngine;

namespace Configs
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerView Prefab;
        public float Speed;
        public float DestinationDisatnce;
    }
}