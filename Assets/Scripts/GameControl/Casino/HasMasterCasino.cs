using AppConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasMasterCasino : BaseCasino {
    [SerializeField]
    GameObject objBatDau, objSanSang;
    [SerializeField]
    Text txt_wait;

    public CardTableManager cardTable;

    internal void SetActiveBatDauSanSang(bool isBatDau, bool isSanSang) {

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
    #endregion

    internal override void OnJoinTablePlaySuccess(Message message) {
        base.OnJoinTablePlaySuccess(message);
        //if (players[0].getName().equals(BaseInfo.gI().mainInfo.nick)) {
        if (isPlaying) {
            SetActiveBatDauSanSang(false, false);
        }
        //    lblXinCho.setVisible(isPlaying);

        txt_wait.gameObject.SetActive(isPlaying);
        //}
    }
    internal override void OnJoinTableSuccess(Message message) {
        base.OnJoinTableSuccess(message);
        //if (players[0].getName().equals(BaseInfo.gI().mainInfo.nick)) {
        if (isPlaying)
            SetActiveBatDauSanSang(false, false);
        txt_wait.gameObject.SetActive(isPlaying);
        //}
    }
    internal new void OnReady(string nick, bool ready) {
        base.OnReady(nick, ready);
        if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
            SetActiveBatDauSanSang(false, false);
        }
    }
    internal override void OnFinishGame(Message message) {
        base.OnFinishGame(message);
        txt_wait.gameObject.SetActive(isPlaying);
    }
    internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.StartTableOk(cardHand, msg, nickPlay);
        txt_wait.gameObject.SetActive(isPlaying);
    }
    internal override void OnInfome(Message message) {
        base.OnInfome(message);
        txt_wait.gameObject.SetActive(false);
    }
}
