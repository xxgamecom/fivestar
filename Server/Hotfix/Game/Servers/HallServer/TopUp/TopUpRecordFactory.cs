using ETModel;

namespace ETHotfix
{
    public static class TopUpRecordFactory
    {
        public static TopUpRecord Create(string orderId, long userId, Commodity commodity)
        {
            var topUpRecord = ComponentFactory.Create<TopUpRecord>();
            topUpRecord.OrderId = orderId;
            topUpRecord.TopUpUserId = userId;
            topUpRecord.Money = commodity.Price;
            topUpRecord.GoodsId = commodity.CommodityType;
            topUpRecord.GoodsAmount = commodity.Amount;
            topUpRecord.TopUpState = TopUpStateType.NoPay;
            topUpRecord.Time = TimeTool.GetCurrenTimeStamp();
            
            return topUpRecord;
        }
    }
}