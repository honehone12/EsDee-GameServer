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
            if (Time.frameCount % 2 != 0)
            {
                return;
            }

            if (IsClient && IsOwner)
            {
                var skillInputBits = characterSkillInput.GetCharacterSkillInput();
                if (skillInputBits != default)
                {
                    var skillSlot = networkSkillSlot.Value;
                    if (skillInputBits.IsBitOn(BitsUtil.MouseRBit))
                    {
                        ClientSideCharacterSkill(skillSlot.charaSkillMouseR, BitsUtil.MouseRBit);
                    }
                    if (skillInputBits.IsBitOn(BitsUtil.MouseLBit))
                    {
                        ClientSideCharacterSkill(skillSlot.charaSkillMouseL, BitsUtil.MouseLBit);
                    }
                }
            }
        }

        void ClientSideCharacterSkill(CharacterSkillCode skillCode, byte skillBits)
        {
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
                            break;
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

        void ServerSideCharacterSkill(CharacterSkillCode skillCode, byte skillBits)
        {
            if (skillIntervalBits.IsBitOn(skillBits))
            {
                var skill = characterSkillList.Find(skillCode, out var ok);
                if (ok)
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
                            break;
                    }

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
        void SetNetworkSkillServerRpc(CharacterSkillCode r, CharacterSkillCode l, ServerRpcParams rpcParams = default)
        {
            if (r == CharacterSkillCode.NotSelected || l == CharacterSkillCode.NotSelected)
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
            if (skillCode != CharacterSkillCode.NotSelected)
            {
                ServerSideCharacterSkill(skillCode, BitsUtil.MouseRBit);
            }
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        void RequestCharaterSkillMouseLServerRpc(ServerRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseL;
            if (skillCode != CharacterSkillCode.NotSelected)
            {
                ServerSideCharacterSkill(skillCode, BitsUtil.MouseLBit);
            }
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        void BroadcastCharacterSkillMouseRClientRpc(ClientRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseR;
            if (skillCode != CharacterSkillCode.NotSelected)
            {
                Debug.Log("skill mouse r");
            }
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        void BroadcastCharacterSkillMouseLClientRpc(ClientRpcParams rpcParams = default)
        {
            var skillCode = networkSkillSlot.Value.charaSkillMouseL;
            if (skillCode != CharacterSkillCode.NotSelected)
            {
                Debug.Log("skill mouse l");
            }
        }
    }
}


