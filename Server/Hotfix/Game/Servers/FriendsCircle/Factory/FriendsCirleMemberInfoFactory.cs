using ETModel;

namespace ETHotfix
{
    public class FriendsCirleMemberInfoFactory
    {
        public static FriendsCirleMemberInfo Create(int friendsCirleId)
        {
            var friendsCirleMemberInfo = ComponentFactory.Create<FriendsCirleMemberInfo>();
            friendsCirleMemberInfo.FriendsCircleId = friendsCirleId;
            return friendsCirleMemberInfo;
        }
    }
}