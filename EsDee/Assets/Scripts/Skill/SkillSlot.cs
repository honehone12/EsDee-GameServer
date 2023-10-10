using Unity.Netcode;

namespace EsDee
{
    [System.Serializable]
    public struct SkillSlot : INetworkSerializable 
    {
        public CharacterSkillCode charaSkillMouseR;
        public CharacterSkillCode charaSkillMouseL;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref charaSkillMouseR);
            serializer.SerializeValue(ref charaSkillMouseL);
        }
    }
}
