using ETModel;

namespace ETHotfix
{
    public class ApplyFriendsCirleInfoFactory
    {
        public static ApplyFriendsCirleInfo Create(int friendsCirleId)
        {
            var applyFriendsCirleInfo = ComponentFactory.Create<ApplyFriendsCirleInfo>();
            applyFriendsCirleInfo.FriendsCirleId = friendsCirleId;

            return applyFriendsCirleInfo;
        }
    }
}