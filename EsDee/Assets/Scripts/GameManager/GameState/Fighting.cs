namespace EsDee
{
    public class Fighting : GameState
    {
        readonly GameManager gameManager;

        public Fighting(GameManager gameManager) 
        {
            this.gameManager = gameManager;
        }

        public override void OnStateEnter()
        {
            gameManager.StartBattle();
        }

        public override void OnGameMangerTick()
        {
            if (gameManager.FalledPlayerIds.Count > 0)
            {
                gameManager.SetNextState(new Closing(gameManager));
            }
        }
    }
}


