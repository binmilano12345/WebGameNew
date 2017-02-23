using AppConfig;
using DataBase;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class BasePlayer : MonoBehaviour {
    [SerializeField]
    Text txt_name, txt_money, txt_effect;
    [SerializeField]
    RawImage raw_avata;
    [SerializeField]
    GameObject master;
    [SerializeField]
    Timer timeTurn;
    [SerializeField]
    GameObject objReady;
    public ArrayCard CardHand;

    [SerializeField]
    ChatPlayer chatPlayer;
    [SerializeField]
    ChatActionViewScript chatAction;
    [SerializeField]
    GameObject objXoay;

    public PlayerData playerData { get; set; }

    public bool IsPlaying { get; set; }
    public bool IsReady { get; set; }
    public bool IsMaster { get; set; }
    public string NamePlayer { get; set; }

    public void SetInfo() {
        if (playerData.Name.Length <= 6) {
            txt_name.text = playerData.Name;
        } else {
            txt_name.text = playerData.Name.Substring(0, 6) + "..";
        }
        txt_money.text = MoneyHelper.FormatMoneyNormal(playerData.Money);
        IsMaster = playerData.IsMaster;
        IsReady = playerData.IsReady;
        SetShowMaster(playerData.IsMaster);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, playerData.Avata_Id + "");
        if (IsMaster)
            SetShowReady(false);
        else
            SetShowReady(playerData.IsReady);
    }
    public void SetShowMaster(bool isMaster) {
        master.SetActive(isMaster);
    }
    public void SetShowReady(bool isReady) {
        objReady.SetActive(isReady);
    }
    public void SetTurn(float time) {
        timeTurn.SetTime(time);
    }
    public void SetEffect(string msg) {
        txt_effect.text = msg;
        txt_effect.transform.DOKill();
        txt_effect.transform.localPosition = Vector3.zero;
        Vector3 vt = txt_effect.transform.localPosition;
        vt.y -= 20;
        txt_effect.transform.localPosition = vt;
        txt_effect.gameObject.SetActive(true);
        txt_effect.transform.DOLocalMoveY(vt.y + 80, 2f).OnComplete(delegate {
            txt_effect.gameObject.SetActive(false);
        });
    }
    public void SetMoney(long money) {
        txt_money.text = MoneyHelper.FormatMoneyNormal(money);
    }

    public void ChatAction() {
        if (NamePlayer.Equals(ClientConfig.UserInfo.UNAME)) return;
        if (!chatAction.isShow()) {
            chatAction.OnShowAction();
        } else {
            chatAction.OnHideAction();
        }
    }
}
