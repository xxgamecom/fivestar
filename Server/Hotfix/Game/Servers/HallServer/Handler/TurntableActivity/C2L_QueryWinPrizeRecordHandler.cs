using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询中奖记录
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_QueryWinPrizeRecordHandler: AMRpcHandler<C2L_QueryWinPrizeRecord, L2C_QueryWinPrizeRecord>
    {
        protected override async void Run(Session session, C2L_QueryWinPrizeRecord message, Action<L2C_QueryWinPrizeRecord> reply)
        {
            var response = new L2C_QueryWinPrizeRecord();
            try
            {
                var winPrizeRecords = await TurntableComponent.Ins.GetWinPrizeRecord(message.QueryUserId);
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