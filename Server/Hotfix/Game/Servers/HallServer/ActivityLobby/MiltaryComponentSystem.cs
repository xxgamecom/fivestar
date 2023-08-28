using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class MiltaryComponentSystem
    {
        //获取大局记录信息
        public static async Task<List<Miltary>> GetMiltary(this MiltaryComponent miltaryComponent, long userId, int friendCircleId)
        {
            List<Miltary> miltaries = null;
            
            // 一个月之前的时间
            long beforeMoths = TimeTool.GetCurrenTimeStamp() - TimeTool.MonthsTime;
            if (friendCircleId == 0)
            {
                miltaries = await miltaryComponent.dbProxyComponent.Query<Miltary>(miltary => miltary.PlayerUserIds.Contains(userId) && miltary.Time > beforeMoths);
            }
            else if (userId == 0)
            {
                miltaries = await miltaryComponent.dbProxyComponent.Query<Miltary>(miltary => miltary.FriendCircleId == friendCircleId && miltary.Time > beforeMoths);
            }
            else
            {
                miltaries = await miltaryComponent.dbProxyComponent.Query<Miltary>(miltary => miltary.PlayerUserIds.Contains(userId) && miltary.FriendCircleId == friendCircleId && miltary.Time > beforeMoths);
            }

            return miltaries;
        }

        // 获取大局里面的所有小局记录信息
        public static async Task<MiltarySmallInfo> GetMiltarySmallInfo(this MiltaryComponent miltaryComponent, int miltaryId)
        {
            var miltarySmallInfos = await miltaryComponent.dbProxyComponent.Query<MiltarySmallInfo>(smllInfo => smllInfo.MiltaryId == miltaryId);
            if (miltarySmallInfos.Count > 0)
            {
                return miltarySmallInfos[0];
            }

            return null;
        }

        // 获取一局游戏里面 游戏过程数据
        public static async Task<ParticularMiltaryRecordDataInfo> GetParticularMiltaryRecordData(this MiltaryComponent miltaryComponent, int dataId)
        {
            var dataInfos = await miltaryComponent.dbProxyComponent.Query<ParticularMiltaryRecordDataInfo>(data => data.DataId == dataId);
            if (dataInfos.Count > 0)
            {
                return dataInfos[0];
            }

            return null;
        }
    }
}