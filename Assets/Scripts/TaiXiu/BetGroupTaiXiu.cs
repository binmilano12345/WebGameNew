using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Beebyte.Obfuscator;
using UnityEngine.UI;
using Us.Mobile.Utilites;
using AppConfig;

public class BetGroupTaiXiu : MonoBehaviour {
	public bool isShow { get; set; }
	//long[] betTX = new long[6];
	[SerializeField]
	Text txt_tai_xiu, txt_money;
	[SerializeField]
	Text[] txt_bet;
	[SerializeField]
	UIButton[] btn_bet;
	[SerializeField]
	GameObject obj_arrow;
	long current_bet = 0;
	bool isTai = false;

	void Start() {
		//int minPos = 0;
		//for (int i = 0; i < GameControl.instance.ListBetTaiXiu.Count; i++) {
		//	if (GameControl.instance.ListBetTaiXiu[i] * 2000 <= ClientConfig.UserInfo.CASH_FREE) {
		//		minPos = i;
		//	} else {
		//		break;
		//	}
		//}

		//if (GameControl.instance.ListBetTaiXiu.Count - minPos < 6) {
		//	minPos = GameControl.instance.ListBetTaiXiu.Count - 6;
		//}
		//for (int i = 0; i <  btn_bet.Length; i++) {
		//	btn_bet[i].name = (i + minPos) + "";
		//	txt_bet[i].text = MoneyHelper.FormatMoneyNormal(GameControl.instance.ListBetTaiXiu[i + minPos]);
		//	GameObject obj = btn_bet[i].gameObject;
		//	btn_bet[i]._onClick.AddListener(delegate {
		//		OnClickBet(obj);
		//	});
		//}
		for (int i = 0; i < GameControl.instance.ListBetTaiXiu.Count; i++) {
			if (i < btn_bet.Length) {
				btn_bet[i].name = i + "";
				txt_bet[i].text = MoneyHelper.FormatMoneyNormal(GameControl.instance.ListBetTaiXiu[i]);
				GameObject obj = btn_bet[i].gameObject;
				btn_bet[i]._onClick.AddListener(delegate {
					OnClickBet(obj);
				});
			}
		}
	}

	void OnClickBet(GameObject obj) {
		int index = int.Parse(obj.name);
		current_bet += GameControl.instance.ListBetTaiXiu[index];
		if (current_bet >= TaiXiuViewScript.instance.moneyKhaDung) {
			current_bet = TaiXiuViewScript.instance.moneyKhaDung;
		}
		txt_money.text = MoneyHelper.FormatMoneyNormal(current_bet);
		if (!obj_arrow.activeSelf)
			obj_arrow.SetActive(true);
	}

	public void OnShow(bool isTai) {
		//		Debug.LogError ("-=-=-=-=-=-=====  " + isShow);
		this.isTai = isTai;
		txt_tai_xiu.text = isTai ? "Tài" : "Xỉu";
		current_bet = 0;
		if (isShow == false) {
			gameObject.SetActive(true);
			isShow = true;
			transform.localScale = Vector3.zero;
			transform.DOScale(1, 0.2f);
		}

		txt_money.text = current_bet + "";
	}

	//[SkipRename]
	public void OnHide() {
		transform.DOScale(0, 0.2f).OnComplete(() => {
			obj_arrow.SetActive(false);
			gameObject.SetActive(false);
			isShow = false;
		});
		TaiXiuViewScript.instance.HideEffect();
	}

	//[SkipRename]
	public void OnClickXacNhan() {
		TaiXiuViewScript.instance.SendBetCuoc(current_bet, isTai ? 2 : 1);
		OnHide();
	}
	//[SkipRename]
	public void OnClickHuy() {
		current_bet = 0;
		txt_money.text = current_bet + "";
		obj_arrow.SetActive(false);
	}
}
