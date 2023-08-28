using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取中奖记录
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetWinPrizeRecordHandler: AMRpcHandler<C2L_GetWinPrizeRecord, L2C_GetWinPrizeRecord>
    {
        protected override async void Run(Session session, C2L_GetWinPrizeRecord message, Action<L2C_GetWinPrizeRecord> reply)
        {
            var response = new L2C_GetWinPrizeRecord();
            try
            {
                var winPrizeRecords = await TurntableComponent.Ins.GetWinPrizeRecord(message.UserId);
                response.Records.Add(winPrizeRecords.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}