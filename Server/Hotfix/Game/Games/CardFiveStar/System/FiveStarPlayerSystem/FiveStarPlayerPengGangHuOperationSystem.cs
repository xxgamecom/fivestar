using ETModel;

namespace ETHotfix
{
    public static class FiveStarPlayerPengGangHuOperationSystem
    {
        // 玩家执行操作
        public static void ExecuteOperate(this FiveStarPlayer self, FiveStarOperateInfo operateInfo)
        {
            self.boolData = false;
            if (operateInfo.OperateType == FiveStarOperateType.MingGang)
            {
                if (!self.canGangCards.ContainsKey(operateInfo.Card))
                {
                    Log.Error("玩家要杠的牌 不在可杠列表里面" + operateInfo.Card);
                    return;
                }
                operateInfo.OperateType = self.canGangCards[operateInfo.Card]; //玩家只发明杠 需要服务器判断是什么杠
            }

            if (self.IsLiangDao) //如果玩家亮倒了 可以胡 却选择不胡 强制胡
            {
                if (operateInfo.OperateType == FiveStarOperateType.None && self.canOperateLists.Contains(FiveStarOperateType.FangChongHu))
                {
                    operateInfo.OperateType = FiveStarOperateType.FangChongHu;
                }
            }

            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.None:
                    self.boolData = true;
                    break;
                case FiveStarOperateType.Peng:
                    operateInfo.Card = self.FiveStarRoom.CurrChuPaiCard;
                    self.boolData = self.PengOrMingGangOrAnGang(self.FiveStarRoom.CurrChuPaiCard, 2, operateInfo.OperateType);
                    break;
                case FiveStarOperateType.MingGang:
                    operateInfo.Card = self.FiveStarRoom.CurrChuPaiCard;
                    self.boolData = self.PengOrMingGangOrAnGang(self.FiveStarRoom.CurrChuPaiCard, 3, operateInfo.OperateType);
                    break;
                case FiveStarOperateType.AnGang:
                    self.boolData = self.PengOrMingGangOrAnGang(operateInfo.Card, 4, operateInfo.OperateType); //暗杠是可以有多个选择 要客户端传
                    break;
                case FiveStarOperateType.CaGang:
                    self.boolData = self.CaGang(operateInfo.Card); //擦杠是可以有多个选择 要客户端传
                    break;
                case FiveStarOperateType.FangChongHu:
                case FiveStarOperateType.ZiMo:
                    operateInfo.Card = 0;
                    operateInfo.OperateType = FiveStarOperateType.ZiMo;
                    if (self.Hands.Count % 3 == 1)
                    {
                        operateInfo.Card = self.FiveStarRoom.CurrChuPaiCard;
                        operateInfo.OperateType = FiveStarOperateType.FangChongHu;
                    }
                    self.boolData = self.HuPai(operateInfo.Card, self.FiveStarRoom.CurrChuPaiIndex);
                    break;
            }
            if (!self.boolData)
            {
                Log.Error("操作错误 视为放弃操作");
                operateInfo.OperateType = FiveStarOperateType.None;
            }
        }

        //操作完成后续
        public static void OperateFinishFollow(this FiveStarPlayer self, FiveStarOperateInfo operateInfo)
        {
            //执行碰 明杠 放冲胡 玩家 最后打牌要被数组移除
            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.Peng:
                case FiveStarOperateType.MingGang:
                case FiveStarOperateType.FangChongHu:
                    self.FiveStarRoom.FiveStarPlayerDic[self.FiveStarRoom.CurrChuPaiIndex].PlayCardByEatOff(); //玩家打出牌被吃掉
                    break;
            }
            //操作完成后续
            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.None: //玩家不操作
                    if (self.FiveStarRoom.QiOperateNextStep == FiveStarOperateType.MoCard)
                    {
                        self.FiveStarRoom.PlayerMoPai(); //可出牌的人 和当前出牌的是同一个 证明 刚刚摸牌玩家已经出牌了 所以按正常流程摸牌
                    }
                    else if (self.FiveStarRoom.QiOperateNextStep == FiveStarOperateType.ChuCard)
                    {
                        self.FiveStarRoom.FiveStarPlayerDic[self.FiveStarRoom.LastMoPaiSeatIndex].CanChuPai(); //最后摸牌的玩家可以出牌
                    }
                    break;
                case FiveStarOperateType.Peng:
                    self.SendNewestHands(); //发送玩家最新的手牌信息
                    self.CanChuPai();       //碰了就可以出牌
                    break;
                case FiveStarOperateType.MingGang:
                case FiveStarOperateType.AnGang:
                case FiveStarOperateType.CaGang:
                    self.SendNewestHands();                                  //发送玩家最新的手牌信息
                    self.FiveStarRoom.PlayerMoPai(self.SeatIndex); //杠的话就摸一张牌
                    break;
            }
        }

        //玩家碰 明杠 暗杠
        public static bool PengOrMingGangOrAnGang(this FiveStarPlayer self, int card, int count, int operateType)
        {
            if (self.RemoveCardCount(card, count))
            {
                FiveStarOperateInfo fiveStarOperateInfo = FiveStarOperateInfoFactory.Create(card, operateType, self.FiveStarRoom.CurrChuPaiIndex);
                self.OperateInfos.Add(fiveStarOperateInfo);
                return true;
            }
            return false;
        }

        //玩家擦杠
        public static bool CaGang(this FiveStarPlayer self, int card)
        {
            for (int i = 0; i < self.OperateInfos.Count; i++)
            {
                if (self.OperateInfos[i].OperateType == FiveStarOperateType.Peng && self.OperateInfos[i].Card == card)
                {
                    if (self.RemoveCardCount(card, 1))
                    {
                        self.OperateInfos[i].OperateType = FiveStarOperateType.CaGang;
                        return true;
                    }
                }
            }
            return false;
        }

        //玩家胡牌
        public static bool HuPai(this FiveStarPlayer self, int card = 0, int playCardIndex = 0)
        {
            if (self.IsCanHu(card, playCardIndex))
            {
                if (card > 0)
                {
                    self.Hands.Add(card);
                }
                return true;
            }
            return false;
        }
    }

}