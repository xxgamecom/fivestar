using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 斗地主入口
    /// </summary>
    [GameEntry(GameEntryId.JoyLandlords)]
    public class JoyLandlordsEntry: AGameEntry
    {

        public override void StartGame(params object[] objs)
        {
            base.StartGame();

            Log.Debug("进入游戏欢乐斗地主");
            UIComponent.GetUiView<BaseHallPanelComponent>().ShowChangeBaseHallUI(GameEntryId.JoyLandlords);
        }

        public override async void EndGame()
        {
            // 默认就是进入大厅界面
            base.EndGame();
        }

        public override void EndAndStartOtherGame()
        {
            base.EndAndStartOtherGame();

            if (Game.Scene.GetComponent<JoyLdsGameRoom>() != null)
            {
                Game.Scene.RemoveComponent<JoyLdsGameRoom>();
            }
        }
    }
}