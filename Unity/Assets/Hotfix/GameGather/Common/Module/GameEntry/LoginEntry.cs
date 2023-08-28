using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 负责登录的是登录通道
    /// </summary>
    [GameEntry(ToyGameId.Login)]
    public class LoginEntry: AGameEntry
    {

        public override void StartGame(params object[] objs)
        {
            base.StartGame();

            Log.Debug("进入登陆界面");
            //EventMsgMgr.AllRemoveEvent();
            Game.Scene.GetComponent<KCPUseManage>().InitiativeDisconnect();
            //  Game.Scene.GetComponent<UIComponent>().RemoveAll();
            Game.Scene.GetComponent<UIComponent>().Show(UIType.LoginPanel);
        }

        public override void EndGame()
        {
            //base.EndGame();
            // Log.Error("处于登陆界面无法退出");
            //Game.Scene.GetComponent<UIComponent>().Show(UIType.LobbyPanel);
        }
    }
}