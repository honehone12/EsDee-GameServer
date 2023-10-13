using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace EsDee
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Singleton { get; private set; }

        [SerializeField]
        float tickRate = 5.0f;
        [SerializeField]
        List<PlayerSpawnLocation> playerSpawnLocationList = new();
        [SerializeField]
        List<GameObject> safetyList = new();

        GameState gameState;
        Queue<ulong> falledPlayerQueue;
        bool isRunning;

        public IReadOnlyCollection<ulong> FalledPlayerIds => falledPlayerQueue;

        void Awake()
        {
            Assert.IsTrue(tickRate > 0f);
            Assert.IsTrue(playerSpawnLocationList.Count >= 
                GameSetting.Singleton.BootSetting.MaxConnections);

            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnNetworkSpawn()
        {
            var networkManager = NetworkManager.Singleton;

            if (IsClient)
            {
                networkManager.OnClientConnectedCallback += OnClientConnectedClientCallback;
                networkManager.OnClientDisconnectCallback += OnClinetDisconnectedClientCallback;
            }

            if (IsServer)
            {
                networkManager.ConnectionApprovalCallback += OnConnectionApprovalCallback;
                networkManager.OnClientConnectedCallback += OnClientConnectedServerCallback;
                networkManager.OnClientDisconnectCallback += OnClientDisconnectedServerCallback;

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

        void OnClientConnectedServerCallback(ulong id)
        {
            
        }

        void OnClientConnectedClientCallback(ulong id)
        {

        }

        void OnClientDisconnectedServerCallback(ulong id)
        {

        }

        void OnClinetDisconnectedClientCallback(ulong id)
        {

        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void RemoveSafetyClientRpc(ClientRpcParams rpcParams = default)
        {
            safetyList.ForEach((go) => go.SetActive(false));
        }
    }
}
