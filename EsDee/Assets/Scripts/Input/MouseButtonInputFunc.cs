using UnityEngine;

namespace EsDee
{
    public enum MouseButtonCode : byte
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    [CreateAssetMenu(menuName = "Input/MouseButtonInputFunc", fileName = "MouseButtonInputFunc")]
    public class MouseButtonInputFunc : DigitalInputFunc
    {
        [SerializeField]
        MouseButtonCode buttonCode;

        public override bool IsOn => Input.GetMouseButton((int)buttonCode);
    }
}


