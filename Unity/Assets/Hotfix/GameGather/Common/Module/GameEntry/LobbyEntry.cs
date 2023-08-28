using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 大厅通道
    /// </summary>
    [GameEntry(GameEntryId.Lobby)]
    public class LobbyEntry: AGameEntry
    {

        /// <summary>
        /// 在StartGame(GameEntryId.Lobby)时执行
        /// </summary>
        /// <param name="objs"></param>
        public override void StartGame(params object[] objs)
        {
            base.StartGame();
            
            Log.Debug("进入大厅");
            UIComponent.GetUiView<FiveStarLobbyPanelComponent>().Show();
        }

        public override void EndGame()
        {
            //base.EndGame();
            GameEntryComponent.StartGame(GameEntryId.Login);
        }
    }
}