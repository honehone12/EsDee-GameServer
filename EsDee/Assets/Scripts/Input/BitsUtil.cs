namespace EsDee
{
    public static class BitsUtil
    {
        public const byte ZeroBits = 0b0000_0000;

        public const byte ForwardBit = 0b0000_0001;
        public const byte BackBit = 0b0000_0010;
        public const byte RightBit = 0b0000_0100;
        public const byte LeftBit = 0b0000_1000;

        public const byte JumpBit = 0b0001_0000;

        public const byte MouseRBit = 0b0000_0001;
        public const byte MouseLBit = 0b0000_0010;
    }
}

namespace EsDee.Extensions
{
    public static class BitsExtension
    {
        public static bool IsBitOn(this byte bits, byte compare)
        {
            return (bits & compare) == compare;
        }

        public static bool IsBitOn(this uint bits, uint compare)
        {
            return (bits & compare) == compare;
        }
    }
}
