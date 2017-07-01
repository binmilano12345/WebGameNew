using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AppConfig;
//using Beebyte.Obfuscator;
using Us.Mobile.Utilites;

public class TaiXiuTouchMoveControl : MonoBehaviour,
								IBeginDragHandler,
								IDragHandler,
								IEndDragHandler {
	public static TaiXiuTouchMoveControl instance;
	float xLeft, xRight, xCenter;
	float imgWidth;
	bool isDrag = false;

	[SerializeField]
	Image img;
	[SerializeField]
	TimeCountDown timeCountDown;
	[SerializeField]
	GameObject objCanCua, objDatCua, obj_show_one;
	[SerializeField]
	Text txt_tai_or_xiu;
	[SerializeField]
	Text txt_money_win;
	[SerializeField]
	Text txt_effect_money_win;
	[SerializeField]
	Text txt_effect_msg;

	void Awake() {
		instance = this;
	}

	void Start() {
		//img.DOFade(0.4f, 1f).SetDelay(1);

		xLeft = transform.InverseTransformPoint(new Vector3(0, 0, 0)).x;
		xRight = transform.InverseTransformPoint(new Vector3(Screen.width, 0, 0)).x;
		xCenter = transform.InverseTransformPoint(new Vector3(Screen.width / 2, 0, 0)).x;
		imgWidth = img.rectTransform.sizeDelta.x;
		OnEndDrag(null);
	}

	#region IBeginDragHandler implement
	public void OnBeginDrag(PointerEventData eventData) {
		isDrag = true;
	}
	#endregion

	#region IDragHandler implement
	public void OnDrag(PointerEventData eventData) {
		transform.position = Input.mousePosition;

	}
	#endregion

	#region IEndDragHandler implement
	public void OnEndDrag(PointerEventData eventData) {
		isDrag = false;
		if (transform.localPosition.x > xCenter) {
			transform.DOLocalMoveX(xRight - imgWidth, 0.6f);
		} else {
			transform.DOLocalMoveX(xLeft + imgWidth, 0.6f);
		}
	}
	#endregion

	//[SkipRename]
	public void OnClick() {
		if (!isDrag) {
			LoadAssetBundle.LoadScene(SceneName.GAME_TAIXIU, SceneName.GAME_TAIXIU);
			//var json = new JObject();
			//json["evt"] = "highlowinfo1";
			//GameControl.instance.cubeia.sendService(json);
		}
	}

	public void SetState(float time, string strTaiXiu, bool isCanCua, bool isDatCua) {
		SetTime(time);
		SetTX(strTaiXiu);
		objCanCua.SetActive(isCanCua);
		objDatCua.SetActive(isDatCua);
	}
	public void SetFinish(string result, float time) {
		objCanCua.SetActive(false);
		objDatCua.SetActive(false);
		SetTX(result);
		StartCoroutine(SetTimeHaveWait(time));
	}

	IEnumerator SetTimeHaveWait(float time) {
		yield return new WaitForSeconds(time / 2);
		SetTime(time / 2);
		SetTX("");
	}
	public void SetTime(float time) {
		timeCountDown.SetTime(time);
		obj_show_one.SetActive(false);
	}
	public void SetTX(string strTaiXiu) {
		if (string.IsNullOrEmpty(strTaiXiu)) {
			txt_tai_or_xiu.transform.parent.gameObject.SetActive(false);
			return;
		}

		obj_show_one.SetActive(false);
		txt_tai_or_xiu.transform.parent.gameObject.SetActive(true);
		txt_tai_or_xiu.text = strTaiXiu;
	}

	public void SetMoneyWin(long moneuWin) {
		if (moneuWin == 0) {
			txt_money_win.gameObject.SetActive(false);
			return;
		}
		txt_money_win.gameObject.SetActive(true);
		string strTemp = "";
		if (moneuWin > 0) {
			txt_money_win.color = Color.yellow;
			strTemp = "+" + MoneyHelper.FormatMoneyNormal(moneuWin);
		} else {
			txt_money_win.color = Color.red;
			strTemp = MoneyHelper.FormatMoneyNormal(moneuWin);
		}

		txt_money_win.text = strTemp;
	}

	public void SetMessageTaiXiu(long money_refund, string cua_refund) {
		txt_effect_msg.gameObject.SetActive(true);
		txt_effect_msg.text = "Cân cửa trả lại " + MoneyHelper.FormatMoneyNormal(money_refund) + " Gold cửa " + cua_refund;

		txt_effect_msg.transform.localPosition = new Vector3(0, -600, 0);
		txt_effect_msg.transform.DOLocalMoveY(-100, 3).OnComplete(() => {
			txt_effect_msg.DOFade(1, 0.5f).SetDelay(1).OnComplete(() => {
				txt_effect_msg.color = new Color32(255, 255, 255, 255);
				txt_effect_msg.gameObject.SetActive(false);
			});
		});
	}

	public void SetMoneyWinTaiXiu(long moneuWin) {
		txt_effect_money_win.gameObject.SetActive(true);
		string strTemp = "";
		if (moneuWin > 0) {
			txt_effect_money_win.color = Color.yellow;
			strTemp = "+" + MoneyHelper.FormatMoneyNormal(moneuWin);
		} else {
			txt_effect_money_win.color = Color.red;
			strTemp = MoneyHelper.FormatMoneyNormal(moneuWin);
		}

		txt_effect_money_win.text = strTemp;

		txt_effect_money_win.transform.localPosition = new Vector3(0, -600, 0);
		txt_effect_money_win.transform.DOLocalMoveY(-150, 3).OnComplete(() => {
			txt_effect_money_win.DOFade(1, 0.5f).SetDelay(1).OnComplete(() => {
				txt_effect_money_win.color = new Color32(255, 255, 255, 255);
				txt_effect_money_win.gameObject.SetActive(false);
			});
		});
	}
}

