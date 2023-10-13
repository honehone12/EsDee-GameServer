namespace EsDee
{
    public abstract class GameState
    {
        public virtual void OnStateEnter() { }

        public virtual void OnGameMangerTick() { }

        public virtual void OnStateExit() { }
    }
}

