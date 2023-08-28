namespace ETHotfix
{
    /// <summary>
    /// 游戏通道, 通过通道来切换场景
    ///
    /// 1) 负责对应游戏类型的进出;
    /// </summary>
    public abstract class AGameEntry
    {
        public long pToyGameType { private set; get; }

        private GameEntryComponent mToyGameComponent;

        protected GameEntryComponent pToyGameComponent
        {
            get
            {
                if (mToyGameComponent == null)
                {
                    mToyGameComponent = Game.Scene.GetComponent<GameEntryComponent>();
                }

                return mToyGameComponent;
            }
        }

        public virtual void Awake(long gameType)
        {
            pToyGameType = gameType;
        }

        public virtual void StartGame(params object[] objs)
        {
            pToyGameComponent.CurrToyGame = pToyGameType;
        }

        // 一定调  不管调用进入其他游戏 还是调用结算本游戏 先调
        public virtual void EndAndStartOtherGame()
        {
        }

        // 不一定调   玩家调用直接开启其他游戏就不会调 后调
        public virtual async void EndGame()
        {
            pToyGameComponent.StartGame(GameEntryId.Lobby);
        }

    }
}