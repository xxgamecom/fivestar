using ETModel;

namespace ETHotfix
{
    public class UserInFriendsCircleFactory
    {
        public static UserInFriendsCircle Create(long userId)
        {
            var userInFriendsCircle = ComponentFactory.Create<UserInFriendsCircle>();
            userInFriendsCircle.UserId = userId;
            return userInFriendsCircle;
        }
    }
}