using ETModel;

namespace ETHotfix
{
    public class RanKingPlayerInfoFactory
    {
        public static RanKingPlayerInfo Create(int friendsCircleId, long userId)
        {
            var friendsCircle = ComponentFactory.Create<RanKingPlayerInfo>();
            friendsCircle.FriendsCircleId = friendsCircleId;
            friendsCircle.UserId = userId;
            return friendsCircle;
        }
    }
}