using Unity.Netcode;

namespace EsDee
{
    public class StandBy : GameState
    {
        readonly GameManager gameManager;
        readonly GameSetting gameSetting;
        readonly NetworkManager networkManager;

        public StandBy(
            GameManager gameManager, 
            GameSetting gameSetting,
            NetworkManager networkManager) 
        {
            this.gameManager = gameManager;
            this.gameSetting = gameSetting;
            this.networkManager = networkManager;
        }

        public override void OnGameMangerTick()
        {     
            var numConnected = networkManager.ConnectedClientsIds.Count;
            if (numConnected == gameSetting.BootSetting.MaxConnections)
            {
                gameManager.SetNextState(new Fighting(gameManager));
            }
        }
    }
}
