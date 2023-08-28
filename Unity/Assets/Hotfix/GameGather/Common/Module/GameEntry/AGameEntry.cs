namespace ETHotfix
{
    /// <summary>
    /// 游戏通道, 通过通道来切换场景
    ///
    /// 1) 负责对应游戏类型的进出;
    /// </summary>
    public abstract class AGameEntry
    {
        public long GameId { private set; get; }

        private GameEntryComponent _gameEntryComponent;

        protected GameEntryComponent GameEntryComponent
        {
            get
            {
                if (_gameEntryComponent == null)
                {
                    _gameEntryComponent = Game.Scene.GetComponent<GameEntryComponent>();
                }

                return _gameEntryComponent;
            }
        }

        public virtual void Awake(long gameEntryId)
        {
            GameId = gameEntryId;
        }

        public virtual void StartGame(params object[] objs)
        {
            GameEntryComponent.CurrentGameId = GameId;
        }

        // 一定调  不管调用进入其他游戏 还是调用结算本游戏 先调
        public virtual void EndAndStartOtherGame()
        {
        }

        // 不一定调   玩家调用直接开启其他游戏就不会调 后调
        public virtual async void EndGame()
        {
            GameEntryComponent.StartGame(GameEntryId.Lobby);
        }

    }
}