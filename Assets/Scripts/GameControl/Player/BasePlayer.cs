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
    Text txt_ready;
    public ArrayCard CardHand;

    [SerializeField]
    ChatPlayer chatPlayer;
    [SerializeField]
    ChatActionViewScript chatAction;
    [SerializeField]
    GameObject objXoay;
    [SerializeField]
    Image imgEffectRank;

    //public PlayerData playerData { get; set; }

    void Init() {
        objXoay.transform.DORotate(new Vector3(0, 0, 120), 0.4f).SetLoops(-1, LoopType.Yoyo);
        imgEffectRank.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.6f).SetLoops(-1, LoopType.Yoyo);
        SetDisableEffectRank();
    }

    public bool IsPlaying { get; set; }
    public bool IsReady { get; set; }
    public bool IsMaster { get; set; }
    public string NamePlayer { get; set; }
    public int SitOnClient { get; set; }

    public void SetInfo(string name, long money, bool isMaster, bool isReady, int avata_id) {
        Init();
        NamePlayer = name;
        if (name.Length <= 6) {
            txt_name.text = name;
        } else {
            txt_name.text = name.Substring(0, 6) + "..";
        }
        txt_money.text = MoneyHelper.FormatMoneyNormal(money);
        IsMaster = isMaster;
        IsReady = isReady;
        SetShowMaster(isMaster);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, avata_id + "");
        if (IsMaster)
            SetShowReady(false);
        else
            SetShowReady(isReady);
    }
    public void SetShowMaster(bool isMaster) {
        master.SetActive(isMaster);
    }
    public void SetShowReady(bool isReady) {
        txt_ready.text = ClientConfig.Language.GetText("ingame_ready");
        txt_ready.gameObject.SetActive(isReady);
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

    public void SetEnableEffectRank(int rank) {
        CancelInvoke("SetDisableEffectRank");
        imgEffectRank.gameObject.SetActive(true);
        LoadAssetBundle.LoadSprite(imgEffectRank, BundleName.UI, UIName.UI_ANI_WIN[rank]);
        Invoke("SetDisableEffectRank", 3);
    }

    public void SetDisableEffectRank() {
        objXoay.SetActive(false);
        imgEffectRank.gameObject.SetActive(false);
    }
    public void SetTextBao() {
        txt_ready.text = "Báo";
    }
    
    public virtual void SetRank(int rank) {
        // 0 mom, 1 nhat, 2 nhi, 3 ba, 4 bet, 5 u

        //Debug.Log("setRank============ > " + rank);
        int idTR = -1;
        switch (rank) {
            case 0:
                idTR = 3;
                //        if (pos == 0 && !BaseInfo.gI().isView) {
                //            SoundManager.instance.startMomAudio();
                //        }
                break;
            case 1:
                idTR = 1;
                //        if (pos == 0 && !BaseInfo.gI().isView) {
                //            SoundManager.instance.startWinAudio();
                //        }

                objXoay.SetActive(true);
                break;
            case 2:
            case 3:
                //        if (pos == 0) {
                //            SoundManager.instance.startBaAudio();
                //        }
                break;
            case 4:
                //        if (pos == 0 && !BaseInfo.gI().isView) {
                //            SoundManager.instance.startLostAudio();
                //        }
                break;
            case 5:
                idTR = 0;
                //        if (pos == 0 && !BaseInfo.gI().isView) {
                //            SoundManager.instance.startUAudio();
                //        }
                objXoay.SetActive(true);
                break;

        }
        if (idTR >= 0 && idTR <= 5) {
            SetEnableEffectRank(rank);
        }
    }


    #region Chat Action
    public void SetChat(string msg) {
        chatPlayer.SetText(msg);
    }
    public void SetPositionChatLeft(bool isLeft) {
        chatPlayer.SetPosition(isLeft);
    }
    public void SetPositionChatAction(Align_Anchor align) {
        chatAction.namePlayer = NamePlayer;
        chatAction.SetAnchor(align);
    }
    #endregion
}
