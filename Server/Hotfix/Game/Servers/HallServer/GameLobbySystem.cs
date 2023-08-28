using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyAwakeSystem: AwakeSystem<GameLobby>
    {

        public override async void Awake(GameLobby self)
        {
            long nextModayGapStamp = TimeTool.GetNextModayGapTimeStamp();
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(TimeTool.TicksConvertMillisecond(nextModayGapStamp));
            var friendsSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.FriendsCircle); //获取亲友圈 Session

            while (true)
            {
                // 调用整点刷新
                self.WeekRefreshAction?.Invoke();

                // 现在 每周刷新 只有亲友圈服务需要
                friendsSession.Send(new L2S_WeekRefresh());
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(TimeTool.TicksConvertMillisecond(TimeTool.WeekTime));
            }
        }
    }

    public static class GameLobbySystem
    {

    }
}