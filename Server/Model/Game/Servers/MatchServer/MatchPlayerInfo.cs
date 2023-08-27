﻿using ETModel;

namespace ETHotfix
{
    public partial class MatchPlayerInfo: Entity
    {
        public override void Dispose()
        {
            SeatIndex = 0;
            User = null;
            SessionActorId = 0;
            base.Dispose();
        }
    }
}