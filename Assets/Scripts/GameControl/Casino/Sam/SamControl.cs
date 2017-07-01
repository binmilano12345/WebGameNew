using AppConfig;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamControl : BaseCasino {
    public static SamControl instace;
    [SerializeField]
    GameObject objBatDau, objSanSang, objDanh, objBoLuot, objBoSam;
    [SerializeField]
    Timer TimeBaoSam;
    //[SerializeField]
    //GameObject txt_wait;
    string nickFire = "";
    public CardTableManager cardTable;
    List<int> ListCardOfMe = new List<int>();
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
        int[] card = ((SamPlayer)playerMe).CardHand.GetCardChoose();
        if (card == null || card.Length < 1) {
            PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("popup_chua_chon_bai"));
        } else {
            SetActiveButton(false, false, false, false);
            SendData.onFireCardTL(card);
            string str = "";
            string str1 = "";
            for (int i = 0; i < card.Length; i++) {
				str += "  " + AutoChooseCardSam.GetValue(card[i]);
                str1 += "  " + card[i];
            }

            Debug.LogError("Danh " + str + "\n" + str1);
        }
    }
    public void OnClickBoLuot() {
        SendData.onSendSkipTurn();
    }
    public void OnClickBaoSam() {
        SendData.baoxam(1);
    }
    public void OnClickBoSam() {
        SendData.baoxam(0);
		objBoSam.SetActive(false);
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
            if (nick.Equals(ClientConfig.UserInfo.UNAME) || string.IsNullOrEmpty(nick)) {
                SetActiveButton(false, false, true, true);
            } else {
                SetActiveButton(false, false, false, false);
            }
            if (nick.Equals(nickFire)) {
                cardTable.UpHetCMNBaiXuong();
				AutoChooseCardSam.CardTrenBan.Clear();
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.StartTableOk(cardHand, msg, nickPlay);
        ListCardOfMe.Clear();
        cardTable.XoaHetCMNBaiTrenBan();
        AutoChooseCardSam.CardTrenBan.Clear();
        nickFire = "";
        for (int i = 0; i < nickPlay.Length; i++) {
            SamPlayer pl = (SamPlayer)GetPlayerWithName(nickPlay[i]);
            if (pl != null) {
                if (pl.SitOnClient == 0) {
					pl.CardHand.ChiaBaiTienLen(AutoChooseCardSam.SortArrCard(cardHand), true);
                    ListCardOfMe.AddRange(cardHand);
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
        SetTurn(turnname, null);
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            ((SamPlayer)playerMe).CardHand.ResetCard();
        }
    }
    internal override void OnFinishTurn() {
        base.OnFinishTurn();
        cardTable.UpHetCMNBaiXuong();
    }
    internal override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl(message, turnName, time, numP);
        try {
            for (int i = 0; i < numP; i++) {
                string nameP = message.reader().ReadUTF();
                sbyte numCard = message.reader().ReadByte();
                SamPlayer pl = (SamPlayer)GetPlayerWithName(nameP);
                if (pl != null) {
                    pl.IsPlaying = (true);
                    int[] temp = new int[numCard];
                    for (int j = 0; j < temp.Length; j++) {
                        temp[j] = 52;
                    }
                    pl.CardHand.SetCardWithId53();
                    pl.CardHand.SetActiveCardHand(true);
                    pl.SetNumCard(numCard);
                }
            }
            GameControl.instance.TimerTurnInGame = time;
            BasePlayer plTurn = GetPlayerWithName(turnName);
            if (plTurn != null) {
                plTurn.SetTurn(time);
            }
            if (turnName.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton(false, false, true, true);
            } else {
                SetActiveButton(false, false, false, false);
            }
            string nickbaoxam = message.reader().ReadUTF();
            OnNickBaoSam(nickbaoxam);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal override void OnInfome(Message message) {
        base.OnInfome(message);
        try {
            //    BaseInfo.gI().isView = false;
            GameControl.instance.TimerTurnInGame = 20;
            //    isStart = true;
            //    players[0].setPlaying(true);
            playerMe.IsPlaying = (true);
            //    disableAllBtnTable();
            int sizeCardHand = message.reader().ReadByte();
            int[] cardHand = new int[sizeCardHand];
            for (int i = 0; i < sizeCardHand; i++) {
                cardHand[i] = message.reader().ReadByte();
            }
            ((SamPlayer)playerMe).CardHand.SetCardWithArrID(cardHand);
            ((SamPlayer)playerMe).CardHand.SetActiveCardHand(true);
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
        if (IsPlaying)
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
        if (IsPlaying)
            SetActiveButton(false, false, false, false);
    }
    internal override void OnFireCardFail() {
        base.OnFireCardFail();
        SetActiveButton(false, false, true, true);
        PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("popup_danh_bai_loi"));
    }
    internal override void OnFireCard(string nick, string turnName, int[] card) {
        base.OnFireCard(nick, turnName, card);
		AutoChooseCardSam.CardTrenBan.Clear();
		AutoChooseCardSam.CardTrenBan.AddRange(card);
        nickFire = nick;
        SamPlayer plTurn = (SamPlayer)GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetTurn(0);
            if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                for (int i = 0; i < card.Length; i++) {
                    ListCardOfMe.Remove(card[i]);
                }
                cardTable.MinhDanh(card, plTurn.CardHand, () => {
                    ((SamPlayer)playerMe).CardHand.SortCardActive();
                });
            } else {
                cardTable.SinhCardGiuaCMNBan(card, plTurn.CardHand.transform);
                int numC = plTurn.NumCard - card.Length;
                plTurn.SetNumCard(numC);
            }
        } else {
            cardTable.SinhCardGiuaCMNBan(card, ((SamPlayer)playerMe).CardHand.transform);
        }

        if (turnName.ToLower().Equals(ClientConfig.UserInfo.UNAME.ToLower())) {
            SetActiveButton(false, false, true, true);
			if (AutoChooseCardSam.CardTrenBan.Count > 0) {
				int[] result = AutoChooseCardSam.ChooseCard(ListCardOfMe.ToArray());
                ((SamPlayer)playerMe).CardHand.SetChooseCard(result);
                //if (result == null) {//sua
                //    playerMe.SetTurn(true, 5);
                //    SetActiveButton(false, false, false, true);
                //    Invoke("KhongDanhDuocThiBo", 5);
                //} else {
                //    if (result.Length <= 0) {
                //        pl.SetTurn(true, 5);
                //        SetActiveButton(false, false, false, true);
                //        Invoke("KhongDanhDuocThiBo", 5);
                //    }
                //}
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
                ((SamPlayer)ListPlayer[i]).CardHand.SetCardWithId53();
                ((SamPlayer)ListPlayer[i]).CardHand.SetActiveCardHand(true);
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

    internal void OnHoiBaoSam(int time) {
        TimeBaoSam.enabled = true;
        objBoSam.SetActive(true);
		TimeBaoSam.transform.localScale = Vector3.one;
		TimeBaoSam.transform.position = Vector3.zero;
        TimeBaoSam.SetTime(time, () => {
            TimeBaoSam.gameObject.SetActive(false);
            objBoSam.SetActive(false);
        });
    }
    internal void OnNickBaoSam(string nick) {
        BasePlayer plBaoSam = GetPlayerWithName(nick);
        if (plBaoSam != null) {
            TimeBaoSam.enabled = false;
            TimeBaoSam.transform.DOMove(plBaoSam.transform.position, 0.4f).OnComplete(delegate {
                TimeBaoSam.transform.DOScale(0.5f, 0.2f);
            });
			objBoSam.SetActive (false);
        }
    }
}
