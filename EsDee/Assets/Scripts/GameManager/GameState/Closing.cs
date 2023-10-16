using UnityEngine.Assertions;

namespace EsDee
{
    public class Closing : GameState
    {
        readonly GameManager gameManager;

        public Closing(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public override void OnStateEnter()
        {
            Assert.IsTrue(gameManager.FalledPlayerIds.Count > 0);
            gameManager.CloseBattle();
        }
    }
}
