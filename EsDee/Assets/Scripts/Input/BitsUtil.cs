namespace EsDee
{
    public static class BitsUtil
    {
        public const byte ZeroBits = 0b0000_0000;

        public const byte ForwardBit = 0b1000_0000;
        public const byte BackBit = 0b0100_0000;
        public const byte RightBit = 0b0010_0000;
        public const byte LeftBit = 0b0001_0000;

        public const byte JumpBit = 0b0000_1000;
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
    }
}
