using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MauBinhPlayer : BasePlayer {
    public CardMauBinh cardMauBinh;
	[SerializeField]
	Text txt_typeAction;
	public void SetSap3Chi(long money){
//		txt_typeAction.gameObject.SetActive (true);
//		txt_typeAction.text = "Sập 3 chi!";
		SetTypeActionEffect ("Sập 3 chi");
	}
	public void SetLung(long money){
//		txt_typeAction.gameObject.SetActive (true);
//		txt_typeAction.text = "Lủng";

		SetTypeActionEffect ("Lủng");
	}

	 void SetTypeActionEffect(string msg) {
		txt_typeAction.text = msg;
		txt_typeAction.transform.DOKill();
		txt_typeAction.transform.localPosition = Vector3.zero;
		Vector3 vt = txt_typeAction.transform.localPosition;
		vt.y -= 20;
		txt_typeAction.transform.localPosition = vt;
		txt_typeAction.gameObject.SetActive(true);
		txt_typeAction.transform.DOLocalMoveY(vt.y + 80, 2f).OnComplete(delegate {
			txt_typeAction.gameObject.SetActive(false);
		});
	}
}
