using AppConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhomControl : BaseCasino {
    public static PhomControl instace;
    [SerializeField]
    GameObject objBatDau, objSanSang, objDanh, objBoc, objAn, objHaPhom, objXepBai;

    [SerializeField]
    Text txt_num_card_boc;

    int totalCardNoc = 0;
    List<int> ListIdCardAn = new List<int>();
    int cardDanhTruocDo = 0;
    void Awake() {
        instace = this;
    }
    // Use this for initialization
    public new void Start() {
        base.Start();
    }

    internal void SetActiveButton(bool isBatDau, bool isSanSang, bool isDanh, bool isBoc, bool isAn, bool isHaPhom, bool isXepBai) {
        objBatDau.SetActive(isBatDau);
        objSanSang.SetActive(isSanSang);
        objDanh.SetActive(isDanh);
        objBoc.SetActive(isBoc);
        objAn.SetActive(isAn);
        objHaPhom.SetActive(isHaPhom);
        objXepBai.SetActive(isXepBai);
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
        int[] cards = ((PhomPlayer)playerMe).cardTaLaManager.GetCardChooseInHand();
        if (cards.Length <= 0) {
            PopupAndLoadingScript.instance.toast.showToast("Chưa chọn bài");
            return;
        } else if (cards.Length > 1) {
            PopupAndLoadingScript.instance.toast.showToast("Bạn chỉ được đánh mỗi lần 1 lá bài");
            return;
        }
        for (int i = 0; i < cards.Length; i++) {
            if (ListIdCardAn.Contains(cards[i])) {
                PopupAndLoadingScript.instance.toast.showToast("Bạn không được đánh con đã ăn!");
                return;
            }
        }

        SendData.onFireCard(cards[0]);//sua
    }
    public void OnClickBoc() {
        SendData.onSendSkipTurn();
    }

    public void OnClickHaPhom() {
        //int[][] phom = RTL.checkPhom(arr, eatArr);
        //if (phom == null) {
        //    SendData.onHaPhom(null);//tu dong ha
        //} else {
        //    SendData.onHaPhom(phom);//ha thu cong
        //    btn_ha_phom.setVisible(false);
        //}
    }

    public void OnClickAnBai() {
        SendData.onGetCardFromPlayer();
    }
    public void OnClickRule() {
        SendData.onChangeRuleTbl();
    }
    #endregion

    internal override void OnJoinTableSuccess(string master) {
        base.OnJoinTableSuccess(master);
        if (master.Equals(ClientConfig.UserInfo.UNAME)) {
            SetActiveButton(true, false, false, false, false, false, false);
        } else {
            SetActiveButton(false, true, false, false, false, false, false);
        }
    }
    internal override void SetMaster(string nick) {
        base.SetMaster(nick);
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            SetActiveButton(true, false, false, false, false, false, false);
        } else {
            SetActiveButton(false, true, false, false, false, false, false);
        }
    }
    internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.StartTableOk(cardHand, msg, nickPlay);
        totalCardNoc = nickPlay.Length * 4 - 1;
        SetNumCardLoc(totalCardNoc);
        cardDanhTruocDo = 0;
        ListIdCardAn.Clear();

        for (int i = 0; i < nickPlay.Length; i++) {
            PhomPlayer pl = (PhomPlayer)GetPlayerWithName(nickPlay[i]);
            if (pl != null) {
                if (pl.SitOnClient == 0) {
                    pl.cardTaLaManager.SetChiaBai(AutoChooseCard.SortArrCard(cardHand), true);
                } else {
                    pl.cardTaLaManager.SetChiaBai(AutoChooseCard.SortArrCard(cardHand), false);
                }
            }
        }
    }
    internal override void SetTurn(string nick, Message message) {
        base.SetTurn(nick, message);
        try {
            PhomPlayer pl = (PhomPlayer)GetPlayerWithName(nick);
            if (nick.Equals(ClientConfig.UserInfo.UNAME) || string.IsNullOrEmpty(nick)) {
                bool bocbai = false, anbai = false, haphom = false, danhbai = false;
                anbai = false;
                bocbai = true;
                int[] cardMe = pl.cardTaLaManager.GetCardIdCardHand();
                if (cardMe.Length < 10) {
                    bocbai = true;
                } else {
                    bocbai = false;
                }
                if (pl.cardTaLaManager.NumCardFire() >= 4) {
                    haphom = true;
                    danhbai = false;
                    anbai = false;
                    bocbai = false;
                } else {
                    int[] cardPhom = AutoChooseCardTaLa.GetPhomAnDuoc(cardMe, cardDanhTruocDo, ListIdCardAn.ToArray());

                    if (cardPhom != null) {
                        anbai = true;
                    }
                }
                SetActiveButton(false, false, danhbai, bocbai, anbai, haphom, false);//sua check de hien nut an
            } else {
                SetActiveButton(false, false, false, false, false, false, true);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void OnFireCard(string nick, string turnName, int[] card) {
        base.OnFireCard(nick, turnName, card);
        PhomPlayer plTurn = (PhomPlayer)GetPlayerWithName(nick);
        cardDanhTruocDo = card[0];
        if (plTurn != null) {
            plTurn.SetTurn(0);
            plTurn.cardTaLaManager.OnFireCard(card[0], nick.Equals(ClientConfig.UserInfo.UNAME));
            //SetActiveButton(false, false, false, false, false, false, true);
        }
    }
    //Boc bai
    internal void OnGetCardNocSuccess(string nick, int card) {
        PhomPlayer pl = (PhomPlayer)GetPlayerWithName(nick);
        if (pl != null) {
            pl.cardTaLaManager.BocBai(card, nick.Equals(ClientConfig.UserInfo.UNAME));
            totalCardNoc--;
            SetNumCardLoc(totalCardNoc);
        }
    }
    internal void OnEatCardSuccess(string thangBiAn, string thangAn, int card) {
        Card cardAn = null;
        PhomPlayer plThangBiAn = (PhomPlayer)GetPlayerWithName(thangBiAn);
        PhomPlayer plThangAn = (PhomPlayer)GetPlayerWithName(thangAn);

        cardAn = plThangBiAn.cardTaLaManager.ArrayCardFire.GetCardbyIDCard(card);
        if (cardAn != null)
            plThangAn.cardTaLaManager.SetEatCard(card, thangAn.Equals(ClientConfig.UserInfo.UNAME), cardAn);
    }
    internal void OnBalanceCard(string tenThangGuiBai, string guiDenThangNay, int card) {
        try {
            //PhomPlayer plTuThangNay = (PhomPlayer)GetPlayerWithName(tenThangGuiBai);
            //PhomPlayer plDenThangNay = (PhomPlayer)GetPlayerWithName(guiDenThangNay);
            //if (plTuThangNay != null && plDenThangNay != null)
            //    plDenThangNay.cardTaLaManager.GuiBai(new int[] { card }, plTuThangNay, tenThangGuiBai.Equals(ClientConfig.UserInfo.UNAME));
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void OnFinishGame(Message message) {
        base.OnFinishGame(message);
        if (playerMe.IsMaster)
            SetActiveButton(true, false, false, false, false, false, false);
        else
            SetActiveButton(false, true, false, false, false, false, false);
        //sua de tinh diem
        //for (int i = 0; i < 4; i++) {
        //    if (players[i].getName().length() > 0) {
        //        players[i].setInfo(false, false, true, players[i].diem);
        //    }
        //    cardDrop[i].removeAllCard();
        //}
    }
    internal void onDropPhomSuccess(string nick, int[] arrayPhom) {
        PhomPlayer pl = (PhomPlayer)GetPlayerWithName(nick);
        if (pl != null) {
            pl.cardTaLaManager.HaBai(arrayPhom, nick.Equals(ClientConfig.UserInfo.UNAME), ListIdCardAn);
        }
    }
    //Gui bai
    internal void OnAttachCard(string tenThangGuiBai, string guiDenThangNay, int[] phomgui, int[] card) {
        try {
            PhomPlayer plTuThangNay = (PhomPlayer)GetPlayerWithName(tenThangGuiBai);
            PhomPlayer plDenThangNay = (PhomPlayer)GetPlayerWithName(guiDenThangNay);
            if (plTuThangNay != null && plDenThangNay != null)
                plDenThangNay.cardTaLaManager.GuiBai(card, plTuThangNay, tenThangGuiBai.Equals(ClientConfig.UserInfo.UNAME));
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    //========================================================================================================
    internal override void OnStartFail() {
        SetActiveButton(true, false, false, false, false, false, false);
    }
    internal override void OnFinishTurn() {
        base.OnFinishTurn();
    }
    internal override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {//nghi da
        base.InfoCardPlayerInTbl(message, turnName, time, numP);
        try {
            for (int i = 0; i < numP; i++) {
                string name = message.reader().ReadUTF();
                PhomPlayer pl = (PhomPlayer)GetPlayerWithName(name);
                if (pl != null) {
                    pl.IsPlaying = (true);
                    int[] temp = new int[9];
                    for (int j = 0; j < temp.Length; j++) {
                        temp[j] = 52;
                    }
                    //pl.cardTaLaManager.SetCardWithId53();//sua
                    pl.cardTaLaManager.SetChiaBai(temp, false);
                }
            }
            GameConfig.TimerTurnInGame = time;
            BasePlayer plTurn = GetPlayerWithName(turnName);
            if (plTurn != null) {
                plTurn.SetTurn(time);
            }
            if (turnName.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton(false, false, true, true, false, false, false);//kiem tra an dc hay ko
            } else {
                SetActiveButton(false, false, false, false, false, false, false);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    /*internal override void OnInfome(Message message) {
        base.OnInfome(message);
        try {
            GameConfig.TimerTurnInGame = 20;
            playerMe.IsPlaying = (true);
            int sizeCardHand = message.reader().ReadByte();
            int[] cardHand = new int[sizeCardHand];
            for (int i = 0; i < sizeCardHand; i++) {
                cardHand[i] = message.reader().ReadByte();
            }
            playerMe.CardHand.SetCardWithArrID(cardHand);
            playerMe.CardHand.SetActiveCardHand(true);

            int sizeCardFire = message.reader().ReadByte();
            if (sizeCardFire > 0) {
                int[] cardFire = new int[sizeCardFire];
                for (int i = 0; i < sizeCardFire; i++) {
                    cardFire[i] = message.reader().ReadByte();
                }
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
    }
    internal override void OnJoinTablePlaySuccess(Message message) {
        base.OnJoinTablePlaySuccess(message);
        if (isPlaying)
            SetActiveButton(false, false, false, false);
    }
    internal override void OnFireCardFail() {
        base.OnFireCardFail();
        SetActiveButton(false, false, true, true);
        PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("popup_danh_bai_loi"));
    }
    
    internal override void OnStartForView(string[] playingName, Message msg) {
        base.OnStartForView(playingName, msg);
        SetActiveButton(false, false, false, false);
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (ListPlayer[i].IsPlaying) {
                ListPlayer[i].CardHand.SetCardWithId53();
                ListPlayer[i].CardHand.SetActiveCardHand(true);
            }
        }
    }
    */
    public void SetNumCardLoc(int NumCard) {
        if (NumCard <= 0) {
            objBoc.SetActive(false);
            txt_num_card_boc.transform.parent.gameObject.SetActive(false);
        } else {
            txt_num_card_boc.transform.parent.gameObject.SetActive(true);
            totalCardNoc = NumCard;
            txt_num_card_boc.text = "" + NumCard;
        }
    }
}
