using AppConfig;
using DataBase;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;
using Beebyte.Obfuscator;

public class BasePlayer : MonoBehaviour {
    [SerializeField]
    Text txt_name, txt_money, txt_effect;
    [SerializeField]
    Image img_avata;
    [SerializeField]
    GameObject master;
    [SerializeField]
    Timer timeTurn;
    [SerializeField]
    Text txt_ready;

    [SerializeField]
    ChatPlayer chatPlayer;
    [SerializeField]
    GameObject objXoay;
    [SerializeField]
    Image imgEffectRank;

    void Init() {
        objXoay.transform.DORotate(new Vector3(0, 0, 120), 0.4f).SetLoops(-1, LoopType.Yoyo);
        imgEffectRank.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.6f).SetLoops(-1, LoopType.Yoyo);
        SetDisableEffectRank();
    }

    public bool IsPlaying { get; set; }
    public bool IsReady { get; set; }
    public bool IsMaster { get; set; }
    public bool IsTurn { get; set; }
    public string NamePlayer { get; set; }
    public int SitOnClient { get; set; }

	public long Money { get; set; }

	public virtual void SetInfo(PlayerData playerData) {
        Init();
		NamePlayer = playerData.NamePlayer;
        if (NamePlayer.Length <= 6) {
            txt_name.text = NamePlayer;
        } else {
            txt_name.text = NamePlayer.Substring(0, 6) + "..";
        }
		SetMoney (playerData.Money);
		IsMaster = playerData.IsMaster;
		IsReady = playerData.IsReady;
		SetShowMaster(IsMaster);
		LoadAssetBundle.LoadSprite(img_avata, BundleName.AVATAS, playerData.Avata_Id + "");
        if (IsMaster)
            SetShowReady(false);
        else
            SetShowReady(IsReady);
    }
    public void SetShowMaster(bool isMaster) {
        master.SetActive(isMaster);
    }
    public void SetShowReady(bool isReady) {
        txt_ready.text = ClientConfig.Language.GetText("ingame_ready");
        txt_ready.gameObject.SetActive(isReady);
    }
    public void SetTurn(float time = 20, bool isDenLuot = true) {
        timeTurn.SetTime(time);
        IsTurn = isDenLuot;
        if (isDenLuot) {
            timeTurn.SetTime(time);
            //if (HighLightViewScript.Instance != null) {
            //    if (!HighLightViewScript.Instance.isShow)
            //        HighLightViewScript.Instance.SetVisibleHighLight(true);
            //    HighLightViewScript.Instance.HighLightTo(gameObject);
            //}
        } else
            timeTurn.SetTime(0);
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
		this.Money = money;
        txt_money.text = MoneyHelper.FormatMoneyNormal(money);
    }

    public void ChatAction() {
        if (NamePlayer.Equals(ClientConfig.UserInfo.UNAME)) return;
//        if (!chatAction.isShow()) {
//            chatAction.OnShowAction();
//        } else {
//            chatAction.OnHideAction();
//        }
    }

    public void SetEnableEffectRank(int rank) {
        CancelInvoke("SetDisableEffectRank");
        imgEffectRank.gameObject.SetActive(true);
        LoadAssetBundle.LoadSprite(imgEffectRank, BundleName.UI, UIName.UI_ANI_WIN[rank]);
        Invoke("SetDisableEffectRank", 3);
    }

[SkipRename]
    public void SetDisableEffectRank() {
        objXoay.SetActive(false);
        imgEffectRank.gameObject.SetActive(false);
    }
	
    public void SetTextBao() {
        txt_ready.text = "Báo";
    }
	/// <summary>
	/// 0 mom, 1 nhat, 2 nhi, 3 ba, 4 bet, 5 u
	/// </summary>
	/// <param name="rank">Rank.</param>
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
				idTR = 6;
			break;
            case 5:
                idTR = 0;
                //        if (pos == 0 && !BaseInfo.gI().isView) {
                //            SoundManager.instance.startUAudio();
                //        }
                objXoay.SetActive(true);
                break;

        }
        if (idTR >= 0 && idTR <= 6) {
			SetEnableEffectRank(idTR);
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
//        chatAction.NamePlayer = NamePlayer;
//        chatAction.SetAnchor(align);
    }
    #endregion
}
