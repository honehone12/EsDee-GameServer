using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using Unity.Collections;

namespace EsDee
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Singleton { get; private set; }

        [SerializeField]
        float tickRate = 5.0f;
        [Header("PlayerSpawn")]
        [SerializeField]
        List<PlayerSpawnLocation> playerSpawnLocationList = new();
        [Header("BattleArea")]
        [SerializeField]
        List<GameObject> safetyList = new();
        [Header("UI")]
        [SerializeField]
        GameObject winUI;
        [SerializeField]
        GameObject loseUI;

        GameState gameState;
        Queue<ulong> falledPlayerQueue;
        bool isRunning;

        public IReadOnlyCollection<ulong> FalledPlayerIds => falledPlayerQueue;

        void Awake()
        {
            Assert.IsTrue(tickRate > 0f);
            Assert.IsTrue(playerSpawnLocationList.Count >= 
                GameSetting.Singleton.BootSetting.MaxConnections);
            Assert.IsNotNull(winUI);
            Assert.IsNotNull(loseUI);

            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnServerBoot()
        {
            var networkManager = NetworkManager.Singleton;
            Assert.IsNotNull(networkManager);
            networkManager.ConnectionApprovalCallback += OnConnectionApprovalCallback;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                falledPlayerQueue = new();
                isRunning = true;
                SetNextState(new StandBy(this, GameSetting.Singleton, NetworkManager.Singleton));
                _ = StartCoroutine(Tick());
            }
        }

        IEnumerator Tick()
        {
            if (!IsServer)
            {
                yield break;
            }

            var waitSec = 1.0f / tickRate;
            var ticker = new WaitForSeconds(waitSec);

            while (isRunning)
            {
                yield return ticker;

                gameState?.OnGameMangerTick();
            }
        }

        public void SetNextState(GameState nextState)
        {
            if (!IsServer || nextState == null)
            {
                return;
            }

            gameState?.OnStateExit();
            gameState = nextState;
            gameState.OnStateEnter();
        }

        public void EnqueueFalledPlayer(ulong id)
        {
            if (!IsServer)
            {
                return;
            }

            falledPlayerQueue.Enqueue(id);
        }

        void OnConnectionApprovalCallback(
            NetworkManager.ConnectionApprovalRequest request, 
            NetworkManager.ConnectionApprovalResponse response)
        {
            var numConnected = NetworkManager.Singleton.ConnectedClientsIds.Count;
            if (numConnected >= GameSetting.Singleton.BootSetting.MaxConnections ||
                numConnected >= playerSpawnLocationList.Count)
            {
                response.CreatePlayerObject = false;
                response.Approved = false;
                return;
            }

            var nextSpawnLocationTransform = playerSpawnLocationList[numConnected].transform;
            response.Position = nextSpawnLocationTransform.position;
            response.Rotation = nextSpawnLocationTransform.rotation;
            response.CreatePlayerObject = true;
            response.Approved = true;
        }

        public void StartBattle()
        {
            RemoveSafetyClientRpc();
        }

        public void CloseBattle()
        {
            if (falledPlayerQueue.TryDequeue(out var loseId))
            {
                var loseIds = new NativeArray<ulong>(1, Allocator.Temp);
                loseIds[0] = loseId;

                EnableLoseUiClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIdsNativeArray = loseIds
                    }
                });

                loseIds.Dispose();

                if (!falledPlayerQueue.TryDequeue(out var winId))
                {
                    winId = NetworkManager.Singleton.ConnectedClientsIds.
                        First((id) => id != loseId);
                }

                var winIds = new NativeArray<ulong>(1, Allocator.Temp);
                winIds[0] = winId;

                EnableWinUiClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIdsNativeArray = winIds
                    }
                });

                winIds.Dispose();
            }
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void RemoveSafetyClientRpc(ClientRpcParams rpcParams = default)
        {
            safetyList.ForEach((go) => go.SetActive(false));
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void EnableWinUiClientRpc(ClientRpcParams rpcParams = default)
        {
            winUI.SetActive(true);
        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void EnableLoseUiClientRpc(ClientRpcParams rpcParams = default)
        {
            loseUI.SetActive(true);
        }
    }
}
