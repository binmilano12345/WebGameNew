using AppConfig;
using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public abstract class BaseCasino : MonoBehaviour {
    protected int MaxUser = 4;
    protected List<BasePlayer> ListPlayer = new List<BasePlayer>();
    internal BasePlayer playerMe;
    [SerializeField]
    Transform[] tf_invite;
    [SerializeField]
    Transform tf_parent_player;
    [SerializeField]
    Text txt_id_table, txt_bet_table, txt_game_name;

    internal bool isPlaying = false;
    internal void Start() {
        GameControl.instance.UnloadSubScene();
        GameControl.instance.UnloadScene(SceneName.SCENE_ROOM);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadScene(SceneName.SCENE_MAIN);
        GameControl.instance.UnloadSubScene();
        for (int i = 1; i < tf_invite.Length; i++) {
            tf_invite[i].GetComponent<UIButton>()._onClick.AddListener(OnClickInvite);
        }
    }

    void OnClickInvite() {
        Debug.LogError("Hien moi ban");
    }
    #region Xu li trong game
    internal virtual void OnJoinTablePlaySuccess(Message message) {
        short idTable = message.reader().ReadShort();
        long betMoney = message.reader().ReadLong();
        long needMoney = message.reader().ReadLong();
        long maxMoney = message.reader().ReadLong();

        txt_id_table.text = "Bàn " + idTable;
        txt_bet_table.text = "Mức cược " + "<color=yellow>" + betMoney + "</color>";
        txt_game_name.text = GameConfig.GameName[(int)GameConfig.CurrentGameID];

        //BaseInfo.gI().isView = false;
        //BaseInfo.gI().isOutTable = false;
        //BaseInfo.gI().startVaobanAudio();
        try {
            int luatPhom = message.reader().ReadByte();
            SetLuatChoi(luatPhom);
            string master = message.reader().ReadUTF();
            //    masterID = master;
            int len = message.reader().ReadByte();
            GameConfig.TimerTurnInGame = message.reader().ReadInt();
            isPlaying = message.reader().ReadBoolean();
            //    PlayerInfo[] pl = new PlayerInfo[len];
            //    int indexmy = 0;
            for (int i = 0; i < len; i++) {
                PlayerData pl = new PlayerData();
                pl.Name = message.reader().ReadUTF();
                pl.DisplaName = message.reader().ReadUTF();
                pl.Avata_Link = message.reader().ReadUTF();
                pl.Avata_Id = message.reader().ReadInt();
                pl.SitOnSever = message.reader().ReadByte();
                pl.Money = message.reader().ReadLong();
                pl.IsReady = message.reader().ReadBoolean();
                pl.FolowMoney = message.reader().ReadLong();
                pl.IsMaster = pl.Name.Equals(master);
                if (isPlaying) {
                    pl.IsReady = false;
                }
                GameObject objPlayer = Instantiate(GameControl.instance.objPlayer);
                objPlayer.transform.SetParent(tf_parent_player);
                BasePlayer plUI = objPlayer.GetComponent<BasePlayer>();
                plUI.playerData = pl;
                if (pl.Name.Equals(ClientConfig.UserInfo.UNAME)) {
                    playerMe = plUI;
                    indexMe = i;
                }
                plUI.SetInfo();
                objPlayer.SetActive(false);
                ListPlayer.Add(plUI);
            }

            OnJoinTableSuccess(master);
            //    if (!isPlaying
            //            && BaseInfo.gI().isAutoReady
            //            && !BaseInfo.gI().mainInfo.nick.equals(master)
            //            && !BaseInfo.gI().checkHettien()
            //            && (CasinoStage.this instanceof TLMNStage || CasinoStage.this instanceof PhomStage || CasinoStage.this instanceof XamStage)) {
            //        btn_sansang.setVisible(false);
            //        SendData.onReady(1);// san sang }
            //    }
            SortSitPlayer();
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    internal virtual void OnFireCardFail() {
    }

    internal virtual void OnFinishTurn() {
    }

    internal virtual void OnInfome(Message message) {
    }

    internal virtual void OnFinishGame(Message message) {
        isPlaying = false;
        int total = message.reader().ReadByte();
        //      BaseInfo.gI().infoWin.clear();
        //      for (int i = 0; i < nUsers; i++) {
        //          players[i].setSoChip(0);
        //          players[i].setTurn(false, 0);
        //      }
        for (int i = 0; i < total; i++) {
            string nick = message.reader().ReadUTF();
            //if (nick.equals(BaseInfo.gI().mainInfo.nick)) {
            //    MainInfo.setPlayingUser(false);
            //}
            int rank = message.reader().ReadByte();
            if (rank != 1 && rank != 5) {
            }
            long money = message.reader().ReadLong();
            string dau = "";
            if (rank == 1 || rank == 5) {
                dau = "+";
            }
            BasePlayer pl = GetPlayerWithName(nick);
            if (pl != null) {
                pl.SetEffect(dau + MoneyHelper.FormatMoneyNormal(money));
            }
            //if (getPlayer(nick) == 0) {
            //    BaseInfo.gI().infoWin.add(new InfoWin(dau, nick, money, true));
            //} else {
            //    BaseInfo.gI().infoWin.add(new InfoWin(dau, nick, money, false));
            //}
            //nickFire = "";

            //for (int j = 0; j < nUsers; j++) {
            //    if (players[j].isPlaying() && players[j].getName().equals(nick)) {
            //        players[j].setRank(rank);
            //        players[j].setReady(false);
            //        break;
            //    }
            //}
        }
        //OnJoinTableSuccess(masterID);
        //      for (int j = 0; j < nUsers; j++) {
        //          if (players[j].isPlaying()) {
        //              players[j].setPlaying(false);
        //          } else {
        //              players[j].diem = 555;
        //          }
        //          players[j].setTurn(false, 0);
        //      }
        //      BaseInfo.gI().media_countdown.pause();

        //      if (!MainInfo.isAutoOutTable()
        //              && BaseInfo.gI().isAutoReady
        //              && !BaseInfo.gI().isView
        //              && !BaseInfo.gI().checkHettien()
        //              && (CasinoStage.this instanceof TLMNStage || CasinoStage.this instanceof PhomStage || CasinoStage.this instanceof XamStage)

        //                  && !BaseInfo.gI().mainInfo.nick.equals(masterID)) {
        //          btn_sansang.setVisible(false);
        //          SendData.onReady(1);
        //      }

        //  } catch (Exception ex) {

        //          ex.printStackTrace();
        //}
    }

    //protected String[] luatchoi = new String[] { "TÁI GỬI", "KHÔNG TÁI GỬI" };
    internal void SetLuatChoi(int rule) {

        //if (screen.game.gameID == GameID.PHOM)
        //    luatChoi.setText(luatchoi[readByte]);
    }

    internal virtual void OnJoinTableSuccess(string master) {

    }
    internal virtual void OnJoinTableSuccess(Message message) {

    }

    public void OnJoinView(Message message) {
        try {
            //    BaseInfo.gI().isView = true;
            //    resetData();
            //    for (int i = 0; i < players.length; i++) {
            //        players[i].setExit();
            //    }

            int rule = message.reader().ReadByte();
            SetLuatChoi(rule);
            string master = message.reader().ReadUTF();
            //    masterID = master;
            int len = message.reader().ReadByte();
            GameConfig.TimerTurnInGame = message.reader().ReadInt();
            isPlaying = message.reader().ReadBoolean();
            for (int i = 0; i < len; i++) {
                PlayerData pl = new PlayerData();
                pl.Name = message.reader().ReadUTF();
                pl.DisplaName = message.reader().ReadUTF();
                pl.Avata_Link = message.reader().ReadUTF();
                pl.Avata_Id = message.reader().ReadInt();
                pl.SitOnSever = message.reader().ReadByte();
                pl.Money = message.reader().ReadLong();
                pl.IsReady = message.reader().ReadBoolean();
                pl.FolowMoney = message.reader().ReadLong();
                pl.IsMaster = pl.Name.Equals(master);
                if (isPlaying) {
                    pl.IsReady = false;
                }

                GameObject objPlayer = Instantiate(GameControl.instance.objPlayer);
                objPlayer.transform.SetParent(tf_parent_player);
                BasePlayer plUI = objPlayer.GetComponent<BasePlayer>();
                plUI.playerData = pl;
                if (pl.Name.Equals(ClientConfig.UserInfo.UNAME)) {
                    playerMe = plUI;
                    indexMe = i;
                }
                plUI.SetInfo();
                objPlayer.SetActive(false);
                ListPlayer.Add(plUI);
                OnJoinTableSuccess(master);
                //    screen.dialogWaitting.onShow();
                //    if (len < BaseInfo.gI().numberPlayer)
                //        SendData.onJoinTable(BaseInfo.gI().mainInfo.nick, BaseInfo.gI().idTable, "", -1);

                //} catch (IOException ex) {
                //    ex.printStackTrace();
                //}
                //try {
                //    setTableName("Phòng ");
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    internal void OnUserExitTable(string nick, string master) {
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            //LoadAssetBundle.LoadScene(SceneName.SCENE_ROOM, SceneName.SCENE_ROOM, () => {
            GameControl.instance.SetCurrentCasino(null);
            //});
        } else {
            BasePlayer pl = GetPlayerWithName(nick);
            if (pl != null) {
                tf_invite[pl.playerData.SitOnClient].gameObject.SetActive(true);
                Destroy(pl.gameObject);
                ListPlayer.Remove(pl);
            }
            BasePlayer plMaster = GetPlayerWithName(master);
            if (plMaster != null) {
                plMaster.IsMaster = true;
                plMaster.SetShowMaster(true);
            }
        }
    }
    internal void InfoCardPlayerInTbl(Message message) {
        try {
            string turnName = message.reader().ReadUTF();
            int time = message.reader().ReadInt();
            sbyte numP = message.reader().ReadByte();
            InfoCardPlayerInTbl(message, turnName, time, numP);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    internal virtual void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
        for (int i = 0; i < ListPlayer.Count; i++) {
            ListPlayer[i].IsPlaying = false;
        }
    }
    internal virtual void OnReady(string nick, bool ready) {
        BasePlayer pl = GetPlayerWithName(nick);
        if (pl != null) {
            pl.IsReady = ready;
            pl.SetShowReady(ready);
        }
    }
    internal virtual void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        GameConfig.TimerTurnInGame = 20;
        for (int i = 0; i < nickPlay.Length; i++) {
            BasePlayer pl = GetPlayerWithName(nickPlay[i]);
            if (pl != null) {
                pl.IsReady = false;
                pl.SetShowReady(false);
                pl.IsPlaying = true;
            }
        }
    }
    internal virtual void OnStartForView(string[] playingName, Message msg) {
        for (int i = 0; i < ListPlayer.Count; i++) {
            ListPlayer[i].SetShowReady(false);
            ListPlayer[i].IsPlaying = false;
        }
        for (int i = 0; i < playingName.Length; i++) {
            BasePlayer pl = GetPlayerWithName(playingName[i]);
            if (pl != null) {
                pl.IsPlaying = (true);
            }
            //if (playingName[i].equals(BaseInfo.gI().mainInfo.nick)) {
            //    MainInfo.setPlayingUser(false);
            //}
        }
    }
    internal virtual void OnStartFail() { }
    internal virtual void SetTurn(string nick, Message message) {
        if (string.IsNullOrEmpty(nick)) {
            return;
        }
        for (int i = 0; i < ListPlayer.Count; i++) {
            ListPlayer[i].SetTurn(0);
        }
        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetTurn(GameConfig.TimerTurnInGame);
        }
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            playerMe.CardHand.ResetCard();
        }
    }
    internal virtual void OnFireCard(string nick, string turnName, int[] card) {
        //nickFire = nick;
        //finishTurn = false;
        //if (CasinoStage.this instanceof TLMNStage) {
        //    // players[getPlayer(nick)].cardHand.onfireCard(card,getPlayer(nick));
        //    players[getPlayer(nick)].cardHand.onfireCard(card);
        //} else {
        //    players[getPlayer(nick)].cardHand.onfireCard(card);
        //}sua
        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetTurn(0);
        }
        //if (getPlayer(nick) != 0) {
        //    int sobai = Integer.parseInt(players[getPlayer(nick)].lbl_SoBai.getText().toString()) - card.length;
        //    if (sobai < 0) {
        //        sobai = 0;
        //    }
        //    players[getPlayer(nick)].setSobai(sobai);
        //}
        SetTurn(turnName, null);
    }
    internal virtual void OnNickSkip(string nick, string turnname2) {
        //players[getPlayer(nick)].setAction(Res.AC_BOLUOT);

        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetEffect(ClientConfig.Language.GetText("ingame_leave"));
            plTurn.SetTurn(0);
            //BaseInfo.gI().media_countdown.pause();
        }
    }
    internal void OnNickSkip(string nick, Message msg) {
        BasePlayer plTurn = GetPlayerWithName(nick);
        if (plTurn != null) {
            plTurn.SetEffect(ClientConfig.Language.GetText("ingame_leave"));
            plTurn.SetTurn(0);
            //BaseInfo.gI().media_countdown.pause();
        }
    }
    internal virtual void SetMaster(string nick) {
        for (int i = 0; i < ListPlayer.Count; i++) {
            ListPlayer[i].IsMaster = false;
            ListPlayer[i].SetShowMaster(false);
        }
        BasePlayer plMaster = GetPlayerWithName(nick);
        if (plMaster != null) {
            plMaster.IsMaster = true;
            plMaster.IsReady = true;
            plMaster.SetShowMaster(true);
            playerMe.SetShowReady(false);
        }
    }
    internal void OnUpdateMoneyTbl(Message message) {
        try {
            int size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                string name = message.reader().ReadUTF();
                long money = message.reader().ReadLong();
                long folowMoney = message.reader().ReadLong();
                bool isGetMoney = message.reader().ReadBoolean();
                BasePlayer pl = GetPlayerWithName(name);
                pl.SetMoney(folowMoney);
                if (!isGetMoney) {
                    pl.SetEffect(MoneyHelper.FormatMoneyNormal(folowMoney));
                }
                if (name.Equals(ClientConfig.UserInfo.UNAME)) {
                    ClientConfig.UserInfo.CASH_FREE = money;
                    pl.SetMoney(money);
                }
            }
        } catch (Exception ex) {
            Debug.LogError(ex);
        }

    }
    public void AllCardFinish(string nick, int[] card) {
        card = RTL.sort(card);
        BasePlayer pl = GetPlayerWithName(nick);
        if (pl != null) {
            pl.CardHand.SetCardKhiKetThucGame(card);
        }
        //if (players[getPlayer(nick)].isPlaying()) {
        //    players[getPlayer(nick)].setCardHandInFinishGame(card);
        //}

    }
    #endregion
    #region Xep cho ngoi
    int indexMe = 0;
    void SortSitPlayer() {
        if (playerMe != null) {
            playerMe.transform.localPosition = tf_invite[0].localPosition;
            playerMe.playerData.SitOnClient = 0;
            playerMe.gameObject.SetActive(true);
        }
        int j = 1;
        for (int i = indexMe + 1; i < ListPlayer.Count; i++) {
            ListPlayer[i].transform.localPosition = tf_invite[j].localPosition;
            tf_invite[j].gameObject.SetActive(false);
            ListPlayer[i].gameObject.SetActive(true);
            ListPlayer[i].playerData.SitOnClient = j;
            j++;
        }
        j = tf_invite.Length - 1;
        for (int i = indexMe - 1; i >= 0; i--) {
            if (j <= 0) break;
            ListPlayer[i].transform.localPosition = tf_invite[j].localPosition;
            tf_invite[j].gameObject.SetActive(false);
            ListPlayer[i].gameObject.SetActive(true);
            ListPlayer[i].playerData.SitOnClient = j;
            j--;
        }
        InitPlayerTLMN();
    }
    void InitPlayerTLMN() {
        for (int i = 0; i < ListPlayer.Count; i++) {
            BasePlayer pl = ListPlayer[i];
            pl.CardHand.CardCount = 13;
            if (pl.playerData.SitOnClient == 0) {
                pl.CardHand.isSmall = false;
                pl.CardHand.isTouched = true;
                pl.CardHand.align_Anchor = Align_Anchor.CENTER;
                pl.CardHand.MaxWidth = 900;

                pl.CardHand.Init();
                pl.CardHand.SetInputChooseCard();
            } else if (pl.playerData.SitOnClient == 1) {
                pl.CardHand.isSmall = true;
                pl.CardHand.isTouched = false;
                pl.CardHand.align_Anchor = Align_Anchor.RIGHT;
                pl.CardHand.MaxWidth = 500;
                pl.CardHand.Init();
            } else {
                pl.CardHand.isSmall = true;
                pl.CardHand.isTouched = false;
                pl.CardHand.align_Anchor = Align_Anchor.LEFT;
                pl.CardHand.MaxWidth = 500;
                pl.CardHand.Init();
            }

            pl.CardHand.SetPositonCardHand();
        }
    }
    #endregion
    internal BasePlayer GetPlayerWithName(string nick) {
        for (int i = 0; i < ListPlayer.Count; i++) {
            if (ListPlayer[i].playerData.Name.Equals(nick)) {
                return ListPlayer[i];
            }
        }
        return null;
    }
    #region Button Click
    public void OnClickBack() {
        PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("popup_quittable"), () => {
            SendData.onOutTable();
            PopupAndLoadingScript.instance.ShowLoading();
        });
    }
    public void OnClickChat() {

    }
    public void OnClickSetting() {

    }
    #endregion
}
