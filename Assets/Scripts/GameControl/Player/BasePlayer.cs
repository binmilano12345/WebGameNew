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

    public PlayerData playerData { get; set; }

    public bool IsPlaying { get; set; }
    public bool IsReady { get; set; }

    public void SetInfo() {
        txt_name.text = playerData.Name;
        txt_money.text = MoneyHelper.FormatMoneyNormal(playerData.Money);
        SetMaster(playerData.IsMaster);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, playerData.Avata_Id + "");
        SetMaster(playerData.IsMaster);
        SetReady(playerData.IsReady);
    }

    public void SetMaster(bool isMaster) {
        master.SetActive(isMaster);
    }

    public void SetReady(bool isReady) {
        objReady.SetActive(isReady);
        IsReady = isReady;
    }

    public void SetTurn(float time) {
        timeTurn.SetTime(time);
    }

    public void SetEffect(string msg) {
        txt_effect.text = msg;
        txt_effect.transform.DOKill();
        Vector3 vt = txt_effect.transform.localPosition;
        vt.y -= 20;
        txt_effect.transform.localPosition = vt;
        txt_effect.gameObject.SetActive(true);
        txt_effect.transform.DOLocalMoveY(vt.y + 80, 2f).OnComplete(delegate {
            txt_effect.gameObject.SetActive(false);
        });
    }

    public void setMoney(long money) {

    }
}
