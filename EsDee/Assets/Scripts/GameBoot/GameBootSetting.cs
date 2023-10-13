using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "GameBootSetting", fileName = "GameBootSetting")]
    public class GameBootSetting : ScriptableObject
    {
        [SerializeField]
        GameBootMode bootMode;
        [SerializeField]
        uint maxConnections = 2;

        public uint MaxConnections => maxConnections;

        public GameBootMode BootMode => bootMode;
    }
}
