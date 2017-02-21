﻿using AppConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLMNControl : BaseCasino {
    public static TLMNControl instace;
    [SerializeField]
    GameObject objBatDau, objSanSang, objDanh, objBoLuot;
    //[SerializeField]
    //GameObject txt_wait;
    string nickFire = "";
    public CardTableManager cardTable;
    void Awake() {
        instace = this;
    }
    // Use this for initialization
    public new void Start() {
        base.Start();
    }

    internal void SetActiveButton(bool isBatDau, bool isSanSang, bool isDanh, bool isBoLuot) {
        objBatDau.SetActive(isBatDau);
        objSanSang.SetActive(isSanSang);
        objDanh.SetActive(isDanh);
        objBoLuot.SetActive(isBoLuot);
    }

    #region Click
    public void OnClickBatDau() {
        bool isAllReady = true;
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (!ListPlayer[i].IsReady) {
                isAllReady = false;
                break;
            }
        }

        if (isAllReady) {
            SendData.onStartGame();
        } else {
            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("popup_con_ng_san_sang"));
        }
    }
    public void OnClickSanSang() {
        SendData.onReady(1);
    }
    public void OnClickDanh() {
        int[] card = playerMe.CardHand.GetCardChoose();
        if (card == null || card.Length < 1) {
            PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("popup_chua_chon_bai"));
        } else {
            SetActiveButton(false, false, false, false);
            SendData.onFireCardTL(card);
        }
    }
    public void OnClickBoLuot() {
        SendData.onSendSkipTurn();
    }
    #endregion

    internal override void OnJoinTableSuccess(string master) {
        base.OnJoinTableSuccess(master);
        if (master.Equals(ClientConfig.UserInfo.UNAME)) {
            SetActiveButton(true, false, false, false);
        } else {
            SetActiveButton(false, true, false, false);
        }
    }
    internal override void SetTurn(string nick, Message message) {
        base.SetTurn(nick, message);
        try {
            if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton(false, false, true, true);
            } else {
                SetActiveButton(false, false, false, false);
            }
            if (nick.Equals(nickFire)) {
                cardTable.UpHetCMNBaiXuong();
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.StartTableOk(cardHand, msg, nickPlay);
        for (int i = 0; i < nickPlay.Length; i++) {
            cardTable.XoaHetCMNBaiTrenBan();
            nickFire = "";
            BasePlayer pl = GetPlayerWithName(nickPlay[i]);
            if (pl != null) {
                if (pl.playerData.SitOnClient == 0) {
                    pl.CardHand.ChiaBaiTienLen(cardHand, true);
                } else {
                    pl.CardHand.ChiaBaiTienLen(cardHand, false);
                }
            }
        }
    }
    internal override void OnStartFail() {
        SetActiveButton(true, false, false, false);
    }
    internal override void OnNickSkip(string nick, string turnname) {
        base.OnNickSkip(nick, turnname);
        // players[getPlayer(nick)].cardHand.setAllMo(true);
        SetTurn(turnname, null);
    }
    internal override void OnFinishTurn() {
        base.OnFinishTurn();
        cardTable.UpHetCMNBaiXuong();
    }
    internal override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl(message, turnName, time, numP);
        try {
            //    String playingName[] = new String[numP];
            for (int i = 0; i < numP; i++) {
                string name = message.reader().ReadUTF();
                sbyte numCard = message.reader().ReadByte();
                BasePlayer pl = GetPlayerWithName(name);
                if (pl != null) {
                    pl.IsPlaying = (true);
                    int[] temp = new int[numCard];
                    for (int j = 0; j < temp.Length; j++) {
                        temp[j] = 52;
                    }
                    pl.CardHand.SetCardWithId53();
                    pl.CardHand.SetActiveCardHand(true);
                    ((TLMNPlayer)pl).SetNumCard(numCard);
                }
                //        players[getPlayer(playingName[i])].setInfo(true, false, false, 0);
            }
            //GameConfig.TimerTurnInGame = time;
            BasePlayer plTurn = GetPlayerWithName(turnName);
            if (plTurn != null) {
                plTurn.SetTurn(time);
            }
        } catch (Exception e) {
        }
    }
    internal override void OnInfome(Message message) {
        base.OnInfome(message);
        try {
            //    BaseInfo.gI().isView = false;
            GameConfig.TimerTurnInGame = 20;
            //    isStart = true;
            //    players[0].setPlaying(true);
            playerMe.IsPlaying = (true);
            //    disableAllBtnTable();
            int sizeCardHand = message.reader().ReadByte();
            int[] cardHand = new int[sizeCardHand];
            for (int i = 0; i < sizeCardHand; i++) {
                cardHand[i] = message.reader().ReadByte();
            }
            playerMe.CardHand.SetCardWithArrID(cardHand);
            playerMe.CardHand.SetActiveCardHand(true);
            //    players[0].setInfo(true, false, false, 0);
            //    players[0].setReady2(true);

            int sizeCardFire = message.reader().ReadByte();
            if (sizeCardFire > 0) {
                int[] cardFire = new int[sizeCardFire];
                for (int i = 0; i < sizeCardFire; i++) {
                    cardFire[i] = message.reader().ReadByte();
                }
                //        tableArrCard = cardFire;
                //        tableArrCard2.setArrCard(cardFire);
                //sua
            }
            string turnName = message.reader().ReadUTF();
            int turnTime = message.reader().ReadInt();
            BasePlayer plTurn = GetPlayerWithName(turnName);
            if (plTurn != null) {
                plTurn.SetTurn(turnTime);
            }

            if (turnName.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton(false, false, true, sizeCardFire > 0);
            } else {
                SetActiveButton(false, false, false, false);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void OnJoinTableSuccess(Message message) {
        //resetData();
        base.OnJoinTableSuccess(message);
        if (isPlaying)
            SetActiveButton(false, false, false, false);
    }
    internal override void OnReady(string nick, bool ready) {
        base.OnReady(nick, ready);
        if (nick.Equals(ClientConfig.UserInfo.UNAME) && !playerMe.IsMaster) {
            if (ready)
                SetActiveButton(false, false, false, false);
            else
                SetActiveButton(false, true, false, false);
        }

        Debug.LogError(ready + "     " + nick + " ==Ready== " + ClientConfig.UserInfo.UNAME);
    }
    internal override void OnJoinTablePlaySuccess(Message message) {
        base.OnJoinTablePlaySuccess(message);
        if (isPlaying)
            SetActiveButton(false, false, false, false);
    }
    internal override void OnFireCardFail() {
        base.OnFireCardFail();
        //btn_boluot.setVisible(boluot);
        //btn_danhbai.setVisible(true);

        SetActiveButton(false, false, true, true);
    }
    internal override void OnFireCard(string nick, string turnName, int[] card) {
        base.OnFireCard(nick, turnName, card);
        nickFire = nick;
        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetTurn(0);
            if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                cardTable.MinhDanh(card, plTurn.CardHand, () => {
                    playerMe.CardHand.SapXepLaiBaiSauKhiDanh();
                });
            } else {
                cardTable.SinhCardGiuaCMNBan(card, plTurn.CardHand.transform);
            }
        }
    }
    internal override void SetMaster(string nick) {
        //masterID = nick;
        //if (players != null) {
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (ListPlayer[i] != null) {
                // tat ca nhung nguoi con lai khong phai chu ban
                ListPlayer[i].SetShowMaster(false);
            }
        }

        BasePlayer pl = GetPlayerWithName(nick);
        if (pl != null) {
            pl.SetShowMaster(true);
        }
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            SetActiveButton(true, false, false, false);
        }
        //}
    }
    internal override void OnStartForView(string[] playingName, Message msg) {
        base.OnStartForView(playingName, msg);
        SetActiveButton(false, false, false, false);
        //btn_sansang.setVisible(false);
        //tableArrCard1.removeAllCard();
        //tableArrCard2.removeAllCard();
        //tableArrCard = null;//sua
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (ListPlayer[i].IsPlaying) {
                ListPlayer[i].CardHand.SetCardWithId53();
                ListPlayer[i].CardHand.SetActiveCardHand(true);
            }
        }
    }
    internal override void OnFinishGame(Message message) {
        base.OnFinishGame(message);
        if (playerMe.IsMaster)
            SetActiveButton(true, false, false, false);
        else
            SetActiveButton(false, true, false, false);
        cardTable.XoaHetCMNBaiTrenBan();
    }
}
