using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AppConfig;

public class MauBinhPlayer : BasePlayer {
    public CardMauBinh cardMauBinh;
	[SerializeField]
	Text txt_typeAction;
//	void Update(){
//		if (Input.GetKeyDown (KeyCode.L)) {
//			SetLung (1234);
//		}if (Input.GetKeyDown (KeyCode.M)) {
//			SetSap3Chi (1234);
//		}if (Input.GetKeyDown (KeyCode.N)) {
//			SetXepXong (true);
//		}if (Input.GetKeyDown (KeyCode.B)) {
//			SetTypeCard ("thung pha sanh", Vector3.zero, true);
//		}
//	}
	public void SetSap3Chi(){
		txt_typeAction.gameObject.SetActive(true);
		txt_typeAction.transform.localPosition = Vector3.zero;

		txt_typeAction.text = "Sập 3 chi";
		txt_typeAction.SetNativeSize ();
	}
	public void SetLung(){
		txt_typeAction.gameObject.SetActive(true);
		txt_typeAction.transform.localPosition = Vector3.zero;

		txt_typeAction.text = "Lủng";
		txt_typeAction.SetNativeSize ();
	}

	void SetTypeActionEffect(string msg, bool isHide = true) {
		txt_typeAction.text = msg;

		txt_typeAction.SetNativeSize ();
		txt_typeAction.transform.DOKill();
		Vector3 vt = txt_typeAction.transform.localPosition;
		vt.x = 0;
		vt.y = -20;
		txt_typeAction.transform.localPosition = vt;
		txt_typeAction.gameObject.SetActive(true);
		txt_typeAction.transform.DOLocalMoveY(vt.y + 80, 2f).OnComplete(delegate {
			if(isHide)
			txt_typeAction.gameObject.SetActive(false);
		});
	}

	public void SetTypeCard(string str, Vector3 lcPositon, bool isMe = false) {
		if (string.IsNullOrEmpty(str)) {
			txt_typeAction.gameObject.SetActive(false);
			return;
		}

		txt_typeAction.transform.localPosition = lcPositon;
		txt_typeAction.gameObject.SetActive(true);
		txt_typeAction.text = str.ToUpper();

		txt_typeAction.SetNativeSize ();
	}

	public void SetXepXong(bool isXong){
		txt_typeAction.gameObject.SetActive(isXong);
		if (isXong) {
			txt_typeAction.transform.localPosition = Vector3.zero;

			txt_typeAction.text = "Xếp xong";
			txt_typeAction.SetNativeSize ();
		}
	}
	public void SetMauBinh(int type) {
		if (type < 0 || type > 6) {
			return;
		}
		Debug.LogError (type + "Thang trang  " + GameConfig.STR_THANG_TRANG[type]);
		SetTypeActionEffect (GameConfig.STR_THANG_TRANG[type]);
//		BaseInfo.gI().startMaubinhAudio();
	}

	public void SetDisableAction(){
		txt_typeAction.gameObject.SetActive (false);
	}
}
