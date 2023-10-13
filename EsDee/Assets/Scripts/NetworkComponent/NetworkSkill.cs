using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;
using EsDee.Extensions;

namespace EsDee
{
    public class NetworkSkill : NetworkBehaviour
    {
        [Header("Input")]
        [SerializeField]
        CharacterSkillInput characterSkillInput;
        [Header("SkillList")]
        [SerializeField]
        CharacterSkillList characterSkillList;
        [Header("OriginList")]
        [SerializeField]
        SkillOriginList skillOriginList = new();

        uint skillIntervalBits = 0b1111_1111_1111_1111;
        NetworkVariable<SkillSlot> networkSkillSlot = new();

        void Awake()
        {
            Assert.IsNotNull(characterSkillInput);
            Assert.IsNotNull(characterSkillList);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && IsOwner)
            {
                var slot = UserProfile.Singleton.SkillSlot;
                SetNetworkSkillServerRpc(slot.charaSkillMouseR, slot.charaSkillMouseL);
            }
        }

        void FixedUpdate()
        {

            if (IsClient && IsOwner)
            {
                var skillInputBits = characterSkillInput.GetCharacterSkillInput();
                if (skillInputBits != default)
                {
                    var skillSlot = networkSkillSlot.Value;
                    if (skillInputBits.IsBitOn(BitsUtil.MouseRBit))
                    {
                        ClientSideTriggerCharacterSkill(skillSlot.charaSkillMouseR, BitsUtil.MouseRBit);
                    }
                    if (skillInputBits.IsBitOn(BitsUtil.MouseLBit))
                    {
                        ClientSideTriggerCharacterSkill(skillSlot.charaSkillMouseL, BitsUtil.MouseLBit);
                    }
                }
            }
        }

        void ClientSideTriggerCharacterSkill(SkillCode skillCode, byte skillBits)
        {
            if (skillCode == SkillCode.NotSelected || skillBits == default)
            {
                return;
            }

            if (skillIntervalBits.IsBitOn(skillBits))
            {
                var skill = characterSkillList.Find(skillCode, out var ok);
                if (ok)
                {
                    switch (skillBits)
                    {
                        case BitsUtil.MouseRBit:
                            RequestCharacterSkillMouseRServerRpc();
                            break;
                        case BitsUtil.MouseLBit:
                            RequestCharaterSkillMouseLServerRpc();
                            break;
                        default:
                            return;
                    }

                    if (IsHost && IsOwner)
                    {
                        return;
                    }

                    skillIntervalBits &= ~(uint)skillBits;
                    StartCoroutine(CoolingSkillInterval(skill.intervalSec, skillBits));                    
                }
            }
        }

        void ClientSideFireCharacterSkill(SkillCode skillCode)
        {
            if (skillCode == SkillCode.NotSelected)
            {
                return;
            }

            var skill = characterSkillList.Find(skillCode, out var skillOK);
            var skillOrigin = skillOriginList.Find(skillCode, out var originOK);
            if (skillOK && originOK)
            {
                var originTransform = skillOrigin.origin;
                var skillParams = new SkillParams(originTransform.position, originTransform.forward);
                skill.skillFunc.ClientFire(in skillParams);
            }
        }

        void ServerSideCharacterSkill(SkillCode skillCode, byte skillBits)
        {
            if (skillCode == SkillCode.NotSelected || skillBits == default)
            {
                return;
            }

            if (skillIntervalBits.IsBitOn(skillBits))
            {
                var skill = characterSkillList.Find(skillCode, out var skillOK);
                var skillOrigin = skillOriginList.Find(skillCode, out var originOK);
                if (skillOK && originOK)
                {
                    switch (skillBits)
                    {
                        case BitsUtil.MouseRBit:
                            BroadcastCharacterSkillMouseRClientRpc();
                            break;
                        case BitsUtil.MouseLBit:
                            BroadcastCharacterSkillMouseLClientRpc();
                            break;
                        default:
                            return;
                    }

                    var originTransform = skillOrigin.origin;
                    var skillParams = new SkillParams(originTransform.position, originTransform.forward);
                    skill.skillFunc.ServerFire(in skillParams);

                    skillIntervalBits &= ~(uint)skillBits;
                    StartCoroutine(CoolingSkillInterval(skill.intervalSec, skillBits));
                }
            }
        }

        IEnumerator CoolingSkillInterval(float sec, byte skillBit)
        {
            Assert.IsTrue(sec > 0.0f);
            yield return new WaitForSeconds(sec);
            skillIntervalBits |= skillBit;
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        void SetNetworkSkillServerRpc(SkillCode r, SkillCode l, ServerRpcParams rpcParams = default)
        {
            if (r == SkillCode.NotSelected || l == SkillCode.NotSelected)
            {
                NetworkManager.Singleton.DisconnectClient(rpcParams.Receive.SenderClientId);
            }

            networkSkillSlot.Value = new SkillSlot
            {
                charaSkillMouseR = r,
                charaSkillMouseL = l
            };
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        void RequestCharacterSkillMouseRServerRpc(ServerRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseR;
            ServerSideCharacterSkill(skillCode, BitsUtil.MouseRBit);
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        void RequestCharaterSkillMouseLServerRpc(ServerRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseL;
            ServerSideCharacterSkill(skillCode, BitsUtil.MouseLBit);
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        void BroadcastCharacterSkillMouseRClientRpc(ClientRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseR;
            ClientSideFireCharacterSkill(skillCode);
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        void BroadcastCharacterSkillMouseLClientRpc(ClientRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseL;
            ClientSideFireCharacterSkill(skillCode);
        }
    }
}
