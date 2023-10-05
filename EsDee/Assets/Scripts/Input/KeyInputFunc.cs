using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Input/KyeInputFunc", fileName = "KeyInputFunc")]
    public class KeyInputFunc : DigitalInputFunc
    {
        [SerializeField]
        KeyCode keyCode = KeyCode.None;

        public override bool IsOn => Input.GetKey(keyCode);
    }
}
