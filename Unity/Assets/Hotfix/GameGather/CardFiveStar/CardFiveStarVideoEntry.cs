﻿using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 卡五星游戏入口
    /// </summary>
    [GameEntry(GameEntryId.CardFiveStarVideo)]
    public class CardFiveStarVideoEntry: AGameEntry
    {

        public override void StartGame(params object[] objs)
        {
            //参数1.List<object> 小局操作信息
            base.StartGame();

            Log.Debug("进入游戏卡五星录像");
            FiveStarVideoRoom cardFiveStarRoom = Game.Scene.AddComponent<FiveStarVideoRoom>();
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().Show();
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutWatchVideoInUI();
            UIComponent.GetUiView<CardFiveStarVideoPanelComponent>().Show();
            cardFiveStarRoom._Coroutines[0] = CoroutineMgr.StartCoroutinee(cardFiveStarRoom.StarPlay(objs[0] as List<object>));
        }

        //录像模式进入游戏
        public static void EnterRoom(ParticularMiltaryRecordDataInfo miltaryRecordDataInfo)
        {
            List<object> datas = FiveStarVideoRecordDataDispose.DisposeRecordData(miltaryRecordDataInfo.MiltaryRecordDatas);
            Game.Scene.GetComponent<GameEntryComponent>().StartGame(GameEntryId.CardFiveStarVideo, datas);
        }

        public override void EndAndStartOtherGame()
        {
            base.EndAndStartOtherGame();
            Game.Scene.RemoveComponent<FiveStarVideoRoom>();
        }

        public override async void EndGame()
        {
            base.EndGame();
            UIComponent.GetUiView<MilitaryPanelComponent>().Show();
        }
    }
}