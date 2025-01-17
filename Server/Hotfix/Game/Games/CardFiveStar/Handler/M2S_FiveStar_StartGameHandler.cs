﻿using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 匹配服通知游戏服开始一局游戏
    /// </summary>
    [MessageHandler(AppType.CardFiveStar)]
    public class M2S_FiveStar_StartGameHandler: AMHandler<M2S_StartGame>
    {
        protected override async void Run(Session session, M2S_StartGame message)
        {
            var response = new S2M_StartGame();
            try
            {
                Log.Info("卡五星服收到开始游戏消息");
                if (message.RoomConfig.GameEntryId != GameEntryId.CardFiveStar)
                {
                    return;
                }
                
                var fiveStarRoom = await Game.Scene.GetComponent<FiveStarRoomComponent>().StartGame(message);
                response.RoomId = message.RoomId;
                response.RoomActorId = fiveStarRoom.Id;
                session.Send(response);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}