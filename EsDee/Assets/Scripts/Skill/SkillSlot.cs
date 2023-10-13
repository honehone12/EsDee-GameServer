using Unity.Netcode;

namespace EsDee
{
    [System.Serializable]
    public struct SkillSlot : INetworkSerializable 
    {
        public SkillCode charaSkillMouseR;
        public SkillCode charaSkillMouseL;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref charaSkillMouseR);
            serializer.SerializeValue(ref charaSkillMouseL);
        }
    }
}
