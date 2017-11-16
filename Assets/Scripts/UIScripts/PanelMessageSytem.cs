using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using AppConfig;
using Beebyte.Obfuscator;

public class PanelMessageSytem : MonoBehaviour {
    public bool isShow { get; set; }
    [SerializeField]
    Text txt_content;
    [SerializeField]
    GameObject btnOK, btnCancelAll;
    public delegate void CallBack();
    CallBack onClickOK, onClickQuit;
    const float posXCenter = 0, posX = 120;

    [SerializeField]
    UIPopUp uipopup;

    public void OnShow(string mess) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();
        
        btnCancelAll.SetActive(false);
        btnOK.SetActive(true);
        txt_content.text = mess;
        setPosBtn(btnOK, posXCenter);

        OnShow();
    }
    
    public void OnShow(string mess, CallBack clickOK) {
        onClickOK = null;
        onClickQuit = null;
        uipopup.dialogPopup.transform.DOKill();
        onClickOK = clickOK;

        btnCancelAll.gameObject.SetActive(false);
        btnOK.gameObject.SetActive(true);

        txt_content.text = mess;
        onClickOK = clickOK;
        setPosBtn(btnOK, posXCenter);

        OnShow();
    }

    public void OnShow(string mess, CallBack clickOK, CallBack clickQuit) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();

        onClickOK = clickOK;
        onClickQuit = clickQuit;

        btnCancelAll.gameObject.SetActive(false);
        btnOK.gameObject.SetActive(true);

        txt_content.text = mess;
        setPosBtn(btnOK, posXCenter);

        OnShow();
    }

    public void OnShowCancelAll(string mess, CallBack clickOK) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();

        onClickOK = clickOK;
        
        btnOK.gameObject.SetActive(true);
        btnCancelAll.gameObject.SetActive(true);

        txt_content.text = mess;
        setPosBtn(btnOK, posX);
        setPosBtn(btnCancelAll, -posX);

        OnShow();
    }

[SkipRename]
    public void onClickButtonOK() {
        uipopup.dialogPopup.transform.DOScale(0, 0.1f).OnComplete(delegate {
            if (onClickOK != null) {
                onClickOK.Invoke();
                onClickOK = null;
            }
            onHide();
        });
    }

[SkipRename]
    public void ClickCancel() {
        uipopup.dialogPopup.transform.DOScale(0, 0.1f).OnComplete(delegate {
            if (onClickQuit != null) {
                onClickQuit.Invoke();
                onClickQuit = null;
            }
            onHide();
        });
    }

    void setPosBtn(GameObject btn, float pos) {
        Vector3 vt = btn.transform.localPosition;
        vt.x = pos;
        btn.transform.localPosition = vt;
    }

    public void onHide() {
        isShow = false;
        uipopup.HideDialog();
    }

    public void OnShow() {
        isShow = true;
        uipopup.ShowDialog();
    }

[SkipRename]
    public void OnClickCancelAll() {
		SettingConfig.IsInvite = false;
        onHide();
    }
}
