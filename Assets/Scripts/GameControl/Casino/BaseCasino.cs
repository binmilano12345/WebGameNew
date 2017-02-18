using AppConfig;
using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCasino : MonoBehaviour {
    protected int MaxUser = 4;
    protected List<BasePlayer> ListPlayer = new List<BasePlayer>();
    BasePlayer playerMe;
    [SerializeField]
    Transform[] tf_invite;
    [SerializeField]
    Transform tf_parent_player;
    [SerializeField]
    Text txt_id_table, txt_bet_table, txt_game_name;

    public void OnJoinTablePlaySuccess(Message message) {
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
            int timeTurn = message.reader().ReadInt();
            bool isPlaying = message.reader().ReadBoolean();
            //    PlayerInfo[] pl = new PlayerInfo[len];
            //    int indexmy = 0;
            for (int i = 0; i < len; i++) {
                PlayerData pl = new PlayerData();
                pl.Name = message.reader().ReadUTF();
                pl.DisplaName = message.reader().ReadUTF();
                pl.Avata_Link = message.reader().ReadUTF();
                pl.Avata_Id = message.reader().ReadInt();
                pl.Sit = message.reader().ReadByte();
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
                if (pl.Name.Equals(ClientConfig.UserInfo.NAME)) {
                    playerMe = plUI;
                    Debug.LogError("tao " + i);
                    indexMe = i;
                } else {
                    Debug.LogError(i);
                }
                plUI.SetInfo();
                objPlayer.SetActive(false);
                ListPlayer.Add(plUI);
            }

            onJoinTableSuccess(master);
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

    //protected String[] luatchoi = new String[] { "TÁI GỬI", "KHÔNG TÁI GỬI" };
    public void SetLuatChoi(int rule) {

        //if (screen.game.gameID == GameID.PHOM)
        //    luatChoi.setText(luatchoi[readByte]);
    }

    public void onJoinTableSuccess(String master) {

    }
    int indexMe = 0;
    void SortSitPlayer() {
        if (playerMe != null) {
            playerMe.transform.localPosition = tf_invite[0].localPosition;
            playerMe.gameObject.SetActive(true);
        }
        int indexOther = 0;
        for (int i = 1; i < tf_invite.Length; i++) {
            if (i < ListPlayer.Count) {
                if (indexMe != indexOther) {
                    ListPlayer[indexOther].transform.localPosition = tf_invite[i].localPosition;
                    tf_invite[i].gameObject.SetActive(false);
                    ListPlayer[indexOther].gameObject.SetActive(true);
                    indexOther++;
                }
            }
        }
    }
}
