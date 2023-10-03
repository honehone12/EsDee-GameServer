using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "GameBootSetting", fileName = "GameBootSetting")]
    public class GameBootModeSetting : ScriptableObject
    {
        [SerializeField]
        GameBootMode bootMode;

        public GameBootMode BootMode => bootMode;
    }
}
