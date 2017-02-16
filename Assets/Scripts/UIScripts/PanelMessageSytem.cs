using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PanelMessageSytem : MonoBehaviour {
    public bool isShow { get; set; }
    [SerializeField]
    Text txt_content;
    [SerializeField]
    GameObject btnOK, btnCancel, btnCancelAll;
    public delegate void CallBack();
    CallBack onClickOK, onClickQuit;
    const float posXCenter = 0, posX = 120, posX3 = 190;

    [SerializeField]
    UIPopUp uipopup;

    public void OnShow(string mess) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();

        btnCancel.SetActive(false);
        btnCancelAll.SetActive(false);
        btnOK.SetActive(true);
        txt_content.text = mess;
        setPosBtn(btnOK, posXCenter);

        OnShow();
    }

    public void OnShow2(string mess, CallBack clickOK) {
        onClickOK = null;
        onClickQuit = null;
        uipopup.dialogPopup.transform.DOKill();

        onClickOK = clickOK;

        btnCancel.SetActive(false);
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
        btnCancel.gameObject.SetActive(true);
        btnOK.gameObject.SetActive(true);

        txt_content.text = mess;
        onClickOK = clickOK;
        setPosBtn(btnOK, -posX);
        setPosBtn(btnCancel, posX);

        OnShow();
    }

    public void OnShow(string mess, CallBack clickOK, CallBack clickQuit) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();

        onClickOK = clickOK;
        onClickQuit = clickQuit;

        btnCancelAll.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(true);
        btnOK.gameObject.SetActive(true);

        txt_content.text = mess;
        setPosBtn(btnOK, -posX);
        setPosBtn(btnCancel, posX);

        OnShow();
    }

    public void OnShowCancelAll(string mess, CallBack clickOK) {
        onClickOK = null;
        onClickQuit = null;

        uipopup.dialogPopup.transform.DOKill();

        onClickOK = clickOK;

        btnCancel.gameObject.SetActive(true);
        btnOK.gameObject.SetActive(true);
        btnCancelAll.gameObject.SetActive(true);

        txt_content.text = mess;
        setPosBtn(btnOK, posXCenter);
        setPosBtn(btnCancel, -posX3);

        OnShow();
    }

    public void onClickButtonOK() {
        uipopup.dialogPopup.transform.DOScale(0, 0.1f).OnComplete(delegate {
            if (onClickOK != null) {
                onClickOK.Invoke();
                onClickOK = null;
            }
            onHide();
        });
    }

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

    public void OnClickCancelAll() {

    }
}
