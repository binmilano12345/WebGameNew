using AppConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLMNControl : HasMasterCasino {
    public static TLMNControl instace;
    [SerializeField]
    GameObject objDanh, objBoLuot;
    void Awake() {
        instace = this;
    }
    // Use this for initialization
    public new void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }

    internal void SetActiveButton(bool isBatDau, bool isSanSang, bool isDanh, bool isBoLuot) {
        SetActiveBatDauSanSang(isBatDau, isSanSang);
        objDanh.SetActive(isDanh);
        objBoLuot.SetActive(isBoLuot);
    }

    #region Click
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
        //if (BaseInfo.gI().mainInfo.nick.equals(master)) {
        //    btn_batdau.setVisible(true);
        //    btnDatCuoc.setVisible(true);
        //    btn_sansang.setVisible(false);

        //    btnkhoa.setVisible(true);

        //} else {
        //    btnkhoa.setVisible(false);
        //    if (!BaseInfo.gI().isView) {

        //        btn_sansang.setVisible(true);

        //    } else {
        //        btn_sansang.setVisible(false);
        //    }
        //    btn_batdau.setVisible(false);
        //    btnDatCuoc.setVisible(false);

        //}
    }
    internal override void SetTurn(string nick, Message message) {
        base.SetTurn(nick, message);
        try {
            if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton(false, false, true, true);
            } else {
                SetActiveButton(false, false, false, false);
            }
            //    if (nick.toLowerCase().equals(BaseInfo.gI().mainInfo.nick.toLowerCase())) {
            //        btn_danhbai.setVisible(true);
            //        if (tableArrCard != null) {
            //            if (tableArrCard.length > 0) {
            //                btn_boluot.setPosition(xbtn_boluot, ybtn_boluot);
            //                btn_danhbai.setPosition(xbtn_danhbai, ybtn_danhbai);
            //                btn_boluot.setVisible(true);
            //            } else {
            //                btn_danhbai.setPosition(xbtn_danhbai, ybtn_danhbai);
            //                btn_boluot.setVisible(false);
            //            }
            //        } else {
            //            btn_danhbai.setPosition(xbtn_danhbai, ybtn_danhbai);
            //            btn_boluot.setVisible(false);
            //        }
            //    } else {
            //        btn_danhbai.setVisible(false);
            //        btn_boluot.setVisible(false);
            //    }
            //    turnName = nick;
            //    if (turnName.equals(nickFire)) {
            //        finishTurn = true;

            //        tableArrCard1.setArrCard(new int[] { }, false, false, false);
            //        tableArrCard2.setArrCard(new int[] { }, false, false, false);
            //        tableArrCard = null;

            //        btn_boluot.setVisible(false);
            //        btn_danhbai.setPosition(xbtn_danhbai, ybtn_danhbai);

            //        for (int i = 0; i < players.length; i++) {
            //            players[i].cardHand.setAllMo(false);
            //        }
            //    }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.StartTableOk(cardHand, msg, nickPlay);
        for (int i = 0; i < nickPlay.Length; i++) {
            Debug.LogError("Thang nay choi: " + nickPlay[i]);
        }
        //tableArrCard1.removeAllCard();
        //tableArrCard2.removeAllCard();
        //tableArrCard = null;
        //players[0].setCardHand(cardHand, true, false, false);
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (ListPlayer[i].IsPlaying && ListPlayer[i].playerData.SitOnClient == 0) {
                ListPlayer[i].CardHand.ChiaBaiTienLen(cardHand, false);
            }
        }

        //if (!BaseInfo.gI().isView) {
        //    btn_danhbai.setVisible(true);
        //    btn_boluot.setVisible(true);
        //}

        //if (players[0].isMaster()) {
        //    btn_batdau.setVisible(false);
        //    btnDatCuoc.setVisible(false);
        //}
    }
    internal override void OnNickSkip(string nick, string turnname) {
        base.OnNickSkip(nick, turnname);
        // players[getPlayer(nick)].cardHand.setAllMo(true);
        SetTurn(turnname, null);
    }
    internal override void OnFinishTurn() {
        base.OnFinishTurn();
        //finishTurn = true;
        //tableArrCard1.setArrCard(new int[] { }, false, false, false);
        //tableArrCard2.setArrCard(new int[] { }, false, false, false);
        //tableArrCard = null;
        //btn_boluot.setVisible(false);
        //for (int i = 0; i < players.length; i++) {
        //    players[i].cardHand.setAllMo(false);
        //}
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
        SetActiveButton(false, false, false, false);
    }
    internal override void OnJoinTablePlaySuccess(Message message) {
        base.OnJoinTablePlaySuccess(message);
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
        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetTurn(0);
            if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                cardTable.MinhDanh(card, plTurn.CardHand, () => { });
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
                ListPlayer[i].SetMaster(false);
            }
        }

        BasePlayer pl = GetPlayerWithName(nick);
        if (pl != null) {
            pl.SetMaster(true);
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
        //btn_danhbai.setVisible(false);
        //btn_boluot.setVisible(false);
        SetActiveButton(false, true, false, false);
    }
}
