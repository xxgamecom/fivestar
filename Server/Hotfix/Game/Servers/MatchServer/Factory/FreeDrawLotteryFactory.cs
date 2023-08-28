using ETModel;

namespace ETHotfix
{
    public static class FreeDrawLotteryFactory
    {
        public static FreeDrawLottery Create(long userId)
        {
            var freeDrawLottery = ComponentFactory.Create<FreeDrawLottery>();
            freeDrawLottery.UserId = userId;
            freeDrawLottery.Count = 1;
            freeDrawLottery.UpAddFreeDrawLotteryTime = TimeTool.GetCurrenTimeStamp();
            
            return freeDrawLottery;
        }
    }
}