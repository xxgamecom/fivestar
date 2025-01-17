﻿using ETModel;

namespace ETHotfix
{
    public static class MatchPlayerInfoFactory
    {
        public static MatchPlayerInfo Create(User user, long sessionActorId, int seatIndex, bool isAI = false)
        {
            var matchPlayerInfo = ComponentFactory.Create<MatchPlayerInfo>();
            matchPlayerInfo.SeatIndex = seatIndex;
            matchPlayerInfo.User = user;
            matchPlayerInfo.SessionActorId = sessionActorId;
            matchPlayerInfo.IsAI = isAI;
            
            return matchPlayerInfo;
        }
    }
}