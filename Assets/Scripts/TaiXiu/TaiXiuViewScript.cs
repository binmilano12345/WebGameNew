using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using Beebyte.Obfuscator;
using Us.Mobile.Utilites;
using DG.Tweening;
using System;
using Beebyte.Obfuscator;

public class TaiXiuViewScript : MonoBehaviour {
	public static TaiXiuViewScript instance;

	#region UI

	[SerializeField]
	Text txt_sum_player_tai, txt_sum_player_xiu;
	[SerializeField]
	Text txt_sum_money_tai, txt_sum_money_xiu;
	[SerializeField]
	Text txt_me_money_tai, txt_me_money_xiu;
	[SerializeField]
	Text txt_me_money_current;
	[SerializeField]
	Text txt_phien;

	[SerializeField]
	Transform tf_parent_his;

	[SerializeField]
	GameObject effect_win_tai, effect_win_xiu;
	[SerializeField]
	GameObject effect_select_tai, effect_select_xiu;

	[SerializeField]
	BetGroupTaiXiu betGroupTaiXiu;

	[SerializeField]
	RankGroupTaiXiu rankGroupTaiXiu;

	[SerializeField]
	HistoryGroupTaiXiu historyGroupTaiXiu;
	[SerializeField]
	Sprite[] sprite_tai_xiu;
	[SerializeField]
	Animator anim;
	[SerializeField]
	Transform batTf;

	[SerializeField]
	TimeCountDown timeCountDown;

	[SerializeField]
	Image[] img_dices;

	[SerializeField]
	GameObject obj_can_cua;

	#endregion

	#region Avariable
	const int COUNT_HIS = 24;
	public bool isPlaying = false;
	long money_tai = 0, money_xiu = 0;
	long old_money_tai = 0, old_money_xiu = 0;
	public long money_me_tai = 0, money_me_xiu = 0;

	long money_refund = 0;
	string cua_refund = "";

	public long moneyKhaDung = 0;
	#endregion

	void Awake() {
		instance = this;
	}

	void Start() {
		InvokeRepeating("UpdateNew", 0.02f, 0.02f);
		//this.gameObject.SetActive(false);
	}
	void UpdateNew() {
		if (isPlaying) {
			if (timeCountDown.timeCurrent <= 0.5f) {
				CanCua();
			}
		}
	}

	#region Other Method
	public void HideEffect() {
		effect_select_tai.SetActive(false);
		effect_select_xiu.SetActive(false);
		effect_win_tai.SetActive(false);
		effect_win_xiu.SetActive(false);
	}


	void CanCua() {
		obj_can_cua.SetActive(true);
		batTf.gameObject.SetActive(true);
		betGroupTaiXiu.OnHide();
		isPlaying = false;
		HideEffect();
		if (TaiXiuTouchMoveControl.instance != null)
			TaiXiuTouchMoveControl.instance.SetState(0, "", true, false);
	}

	#endregion

	#region Click

	[SkipRename]
	public void OnClickTai() {
		if (money_me_xiu > 0) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("tai_xiu_chi_dat_cua"));
			return;
		}
		if (isPlaying) {
			betGroupTaiXiu.OnShow(true);
			effect_select_tai.SetActive(true);
			effect_select_xiu.SetActive(false);
		}
	}

	[SkipRename]
	public void OnClickXiu() {
		if (money_me_tai > 0) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("tai_xiu_chi_dat_cua"));
			return;
		}
		if (isPlaying) {
			betGroupTaiXiu.OnShow(false);
			effect_select_tai.SetActive(false);
			effect_select_xiu.SetActive(true);
		}
	}

	[SkipRename]
	public void OnClickHistory() {
		//PopupAndLoadingScript.instance.ShowLoading();
		////Controller.OnHandleUIEvent("SendHistoryRequest", new object[] { });
	}

	[SkipRename]
	public void OnClickRank() {
		//RankRequest(0);
	}

[SkipRename]
	public void RankRequest(int type) {
		//PopupAndLoadingScript.instance.ShowLoading();
		////Controller.OnHandleUIEvent("SendRankRequest", new object[] { type });
	}

	[SkipRename]
	public void OnClickHelp() {

	}

	#endregion

	#region Request

	public void SendBetCuoc(long bet, int txState) {
		SendData.onCuocTaiXiu((byte)txState, bet);
	}

	#endregion

	#region Animation Xoc Dia, Mo bat
	void OnDisable() {
		StopCoroutine(IESetAnimationNormal());
		anim.Play("normal");
		batTf.gameObject.SetActive(true);
		for (int i = 0; i < img_dices.Length; i++) {
			img_dices[i].transform.parent.gameObject.SetActive(false);
		}
	}

	void OnEnable() {
		StopCoroutine(IESetAnimationNormal());
		//anim.Play("normal");
		//batTf.gameObject.SetActive(true);
		//for (int i = 0; i < img_dices.Length; i++) {
		//	img_dices[i].transform.parent.gameObject.SetActive(false);
		//}
		if (!isStop)
			SetAnimationNormal();
		else {
			StartCoroutine(ShowDices());
		}
	}

	void StartXocDiaAnim() {
		SetAnimationUpBat(() => {
			batTf.gameObject.SetActive(false);
			anim.Play("Xoc");

			if (isActiveAndEnabled) {
				StartCoroutine(IESetAnimationNormal());
			} else {
				SetAnimationNormal();
			}
		});
	}

	IEnumerator IESetAnimationNormal() {
		float time = anim.GetCurrentAnimatorStateInfo(0).length + 1;
		yield return new WaitForSeconds(time);
		SetAnimationNormal();
	}

	void MoBat() {
		batTf.gameObject.SetActive(true);
		anim.Play("normal");
		float px = batTf.transform.localPosition.x;
		batTf.localPosition = new Vector3(px, 0, 0);
		batTf.DOLocalMoveY(600, .4f).OnComplete(() => {
			batTf.gameObject.SetActive(false);
		});
	}
	//[SkipRename]
	void SetAnimationNormal() {
		batTf.gameObject.SetActive(true);
		anim.Play("normal");
		for (int i = 0; i < img_dices.Length; i++) {
			img_dices[i].transform.parent.gameObject.SetActive(false);
		}
		if (TaiXiuTouchMoveControl.instance != null)
			TaiXiuTouchMoveControl.instance.SetTX("");
	}

	//[SkipRename]
	void SetAnimationUpBat(UnityAction callback) {
		SetAnimationNormal();
		float px = batTf.transform.localPosition.x;
		batTf.localPosition = new Vector3(px, 600, 0);
		batTf.gameObject.SetActive(true);
		batTf.DOLocalMoveY(0, .4f).OnComplete(delegate {
			if (callback != null) {
				callback.Invoke();
			}
		});
	}

	#endregion

	#region Receive

	#region HighLowRealtime

	//[SkipRename]
	//[HandleUpdateEvent(EventType = HandlerType.EVN_UPDATEUI_HANDLER)]
	private void HighLowRealtime(params object[] param) {
		try {
			long H = (long)param[0];
			long L = (long)param[1];
			long NH = (long)param[2];
			long NL = (long)param[3];
			int time = (int)param[4];
			List<int> strH = (List<int>)param[5];//lich su
			old_money_tai = money_tai;
			old_money_xiu = money_xiu;
			money_tai = H;
			money_xiu = L;
			//txt_sum_money_tai.text = MoneyHelper.FormatMoneyNormal(H);
			//txt_sum_money_xiu.text = MoneyHelper.FormatMoneyNormal(L);
			EffectRunMoney(txt_sum_money_tai, old_money_tai, money_tai);
			EffectRunMoney(txt_sum_money_xiu, old_money_xiu, money_xiu);

			txt_sum_player_tai.text = "(" + NH + ")";
			txt_sum_player_xiu.text = "(" + NL + ")";

			timeCountDown.SetTime(time);
			if (TaiXiuTouchMoveControl.instance != null) {
				TaiXiuTouchMoveControl.instance.SetTime(time);
				TaiXiuTouchMoveControl.instance.SetTX("");
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	#endregion

	#region HighLow || onjoinTaiXiu

	//[SkipRename]
	internal void HighLow(Message message) {
		/*try {
			long H = (long)param[0];
			long L = (long)param[1];
			long NH = (long)param[2];
			long NL = (long)param[3];
			long UH = (long)param[4];
			long UL = (long)param[5];
			int time = (int)param[6];
			List<int> strH = (List<int>)param[7];//lich su

			long MB = (long)param[8];
			if (time > 2)
				SetAnimationNormal();
			moneyKhaDung = MB;
			txt_sum_money_tai.text = MoneyHelper.FormatMoneyNormal(H);
			txt_sum_money_xiu.text = MoneyHelper.FormatMoneyNormal(L);
			txt_sum_player_tai.text = "(" + NH + ")";
			txt_sum_player_xiu.text = "(" + NL + ")";
			txt_me_money_tai.text = MoneyHelper.FormatMoneyNormal(UH);
			txt_me_money_xiu.text = MoneyHelper.FormatMoneyNormal(UL);
			txt_me_money_current.text = MoneyHelper.FormatMoneyNormal(MB);

			money_tai = H;
			money_xiu = L;

			timeCountDown.SetTime(time);
			if (TaiXiuTouchMoveControl.instance != null)
				TaiXiuTouchMoveControl.instance.SetState(time, "", false, true);
			isPlaying = true;
			//if (tf_parent_his.childCount <= 0) {
			if (Item_His_TX == null) {
				LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_HIS_TX, (objPre) => {
					Item_His_TX = objPre;
					Item_His_TX.SetActive(false);
					for (int i = 0; i < strH.Count; i++) {
						UpdateHistory(strH[i] > 10);
					}
				});
			} else {
				for (int i = 0; i < strH.Count; i++) {
					UpdateHistory(strH[i] > 10);
				}
			}
			//}
		} catch (Exception e) {
			Debug.LogException(e);
		}
		*/
		try {
			isPlaying = message.reader().ReadBoolean();// isplaying
			int time = message.reader().ReadInt();
			if (time > 0) {
				if (isPlaying) {
					timeCountDown.SetTime(time);
					if (TaiXiuTouchMoveControl.instance != null)
						TaiXiuTouchMoveControl.instance.SetState(time, "", false, true);
				} else {
					//groupSoDiem.setVisible(true);
					//groupSoDiem.sodiem.setVisible(false);
					//groupSoDiem.countDown.setCountDown(time);
				}
			} else {
				timeCountDown.SetTime(0);
				if (TaiXiuTouchMoveControl.instance != null)
					TaiXiuTouchMoveControl.instance.SetState(time, "", false, false);
				//countDown.setCountDown(0);
				//groupSoDiem.setVisible(false);
				//groupSoDiem.sodiem.setVisible(false);
				//groupSoDiem.countDown.setCountDown(0);
			}

			txt_sum_money_tai.text = MoneyHelper.FormatMoneyNormal(message.reader().ReadLong());
			txt_sum_money_xiu.text = MoneyHelper.FormatMoneyNormal(message.reader().ReadLong());
			txt_me_money_tai.text = MoneyHelper.FormatMoneyNormal(message.reader().ReadLong());
			txt_me_money_xiu.text = MoneyHelper.FormatMoneyNormal(message.reader().ReadLong());

			//mainGame.mainScreen.dialogWaitting.onHide();
			int size = message.reader().ReadInt();
			//lichSuTaiXiu.reset_lichSu();
			//			lichSuTaiXiu.index = -1;
			int valueTX = 0;
			int phien_ss = 1;
			for (int i = 0; i < size; i++) {
				valueTX = message.reader().ReadByte();
				phien_ss = message.reader().ReadInt();
				//lichSuTaiXiu.update(valueTX, phien_ss);
				UpdateHistory(valueTX > 10);
			}
			int number_tai = message.reader().ReadInt();
			int number_xiu = message.reader().ReadInt();
			txt_sum_player_tai.text = number_tai + "";
			txt_sum_player_xiu.text = number_tai + "";

			int phien = message.reader().ReadInt();
			txt_phien.text = "Phiên: " + phien;
		} catch (Exception e) {
			Debug.LogError(e);
		}
	}

	#endregion

	#region CMD_TIME_START_TAIXIU || OnTimeStartTaiXiu || OnAutoStartTaiXiu
	internal void OnTimeStartTaiXiu(Message message) {
		int time = message.reader().ReadByte();
		timeCountDown.SetTime(time);
		SetAnimationNormal();
	}
	public void OnAutoStartTaiXiu(Message message) {
		try {
			int time = message.reader().ReadByte();
			timeCountDown.SetTime(time);
			int phien = message.reader().ReadInt();

			isPlaying = true;
			if (TaiXiuTouchMoveControl.instance != null) {
				TaiXiuTouchMoveControl.instance.SetTX("");
				TaiXiuTouchMoveControl.instance.SetState(time, "", false, true);
				TaiXiuTouchMoveControl.instance.SetMoneyWin(0);
			}

			txt_sum_money_tai.text = "0";
			txt_sum_money_xiu.text = "0";
			txt_sum_player_tai.text = "(0)";
			txt_sum_player_xiu.text = "(0)";
			txt_me_money_tai.text = "0";
			txt_me_money_xiu.text = "0";

			money_tai = 0;
			money_xiu = 0;

			money_me_tai = 0;
			money_me_xiu = 0;
			money_refund = 0;

			HideEffect();
			StartXocDiaAnim();
			isStop = false;
			txt_phien.text = "Phiên: " + phien;
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
	#endregion

	#region HighLowStop || onGameoverTaixiu
	bool isStop = false;
	//[SkipRename]
	internal void HighLowStop(Message message) {
		try {
			//long S = (long)param[0];
			//int[] R = (int[])param[1];
			message.reader().ReadInt();
			int[] R = new int[3];
			R[0] = message.reader().ReadInt();
			R[1] = message.reader().ReadInt();
			R[2] = message.reader().ReadInt();
			int taihayxiu = message.reader().ReadInt();
			int phien_nb = message.reader().ReadInt();
			txt_phien.text = "Phiên: " + phien_nb;
			MoBat();

			//timeCountDown.SetTime(S);
			//int sum = 0;
			for (int i = 0; i < img_dices.Length; i++) {
				img_dices[i].transform.parent.gameObject.SetActive(true);
				LoadAssetBundle.LoadSprite(img_dices[i], BundleName.DICE, R[i] + "");
				//sum += R[i];
			}

			betGroupTaiXiu.OnHide();
			if (taihayxiu != 1) {
				effect_win_tai.SetActive(false);
				effect_win_xiu.SetActive(true);
			} else {
				effect_win_tai.SetActive(true);
				effect_win_xiu.SetActive(false);
			}
			UpdateHistory(taihayxiu == 1);

			isPlaying = false;
			if (TaiXiuTouchMoveControl.instance != null)
				TaiXiuTouchMoveControl.instance.SetFinish(taihayxiu == 1 ? ClientConfig.Language.GetText("tai_xiu_tai") : ClientConfig.Language.GetText("tai_xiu_xiu"));

			obj_can_cua.SetActive(false);
			if (money_refund > 0) {
				if (TaiXiuTouchMoveControl.instance != null) {
					TaiXiuTouchMoveControl.instance.SetMessageTaiXiu(money_refund, cua_refund);
				}
				money_refund = 0;
			}

			isStop = true;
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	#endregion

	#region HighLowWait

	//[SkipRename]
	//[HandleUpdateEvent(EventType = HandlerType.EVN_UPDATEUI_HANDLER)]
	private void HighLowWait(params object[] param) {
		try {
			int T = (int)param[0];
			List<int> strH = (List<int>)param[1];
			long MB = (long)param[2];
			moneyKhaDung = MB;

			timeCountDown.SetTime(T);
			txt_me_money_current.text = MoneyHelper.FormatMoneyNormal(MB);
			isPlaying = false;
			if (TaiXiuTouchMoveControl.instance != null)
				TaiXiuTouchMoveControl.instance.SetState(T, "", false, false);
			if (!isStop)
				SetAnimationNormal();
			else {
				StartCoroutine(ShowDices());
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	IEnumerator ShowDices() {
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < img_dices.Length; i++) {
			img_dices[i].transform.parent.gameObject.SetActive(true);
		}
		batTf.gameObject.SetActive(false);
		anim.Play("normal");
	}

	#endregion

	#region BetHighLow || OnCuocTaiXiu

	//[SkipRename]
	internal void BetHighLow(Message message) {
		try {
			old_money_tai = money_tai;
			old_money_xiu = money_xiu;

			long old_money_me_tai = money_me_tai;
			long old_money_me_xiu = money_me_xiu;
			string nick = message.reader().ReadUTF();
			long money = message.reader().ReadLong();
			long my_money = message.reader().ReadLong();
			sbyte cua = message.reader().ReadByte();
			int number_people = message.reader().ReadInt();
			Debug.LogError("nick: " + nick
						  + "\nmoney: " + money
						  + "\nmy_money: " + my_money
						  + "\ncua: " + cua
						  + "\nnumber_people: " + number_people);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				if (cua == 1) {
					money_me_tai = my_money;
					EffectRunMoney(txt_me_money_tai, old_money_me_tai, money_me_tai);
				} else {
					money_me_xiu = my_money;
					EffectRunMoney(txt_me_money_xiu, old_money_me_xiu, money_me_xiu);
				}
			}
			if (cua == 1) {
				money_tai = money;
				EffectRunMoney(txt_sum_money_tai, old_money_tai, money_tai);
				txt_sum_player_tai.text = "(" + number_people + ")";
			} else {
				money_xiu = money;
				EffectRunMoney(txt_sum_money_xiu, old_money_xiu, money_xiu);
				txt_sum_player_xiu.text = "(" + number_people + ")";
			}

			//EffectRunMoney(txt_me_money_current, moneyKhaDung, MB);
			//moneyKhaDung = MB;
			//ClientConfig.UserInfo.CASH_FREE = AG;
			UpdateMoney();
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	#endregion

	#region HighLowWin

	//[SkipRename]
	//[HandleUpdateEvent(EventType = HandlerType.EVN_UPDATEUI_HANDLER)]
	private void HighLowWin(params object[] param) {
		try {
			long W = (long)param[0];
			string N = (string)param[1];
			long AG = (long)param[2];
			long MB = (long)param[3];

			txt_me_money_current.text = MoneyHelper.FormatMoneyNormal(MB);
			moneyKhaDung = MB;
			ClientConfig.UserInfo.CASH_FREE = AG;
			if (W != 0) {
				if (TaiXiuTouchMoveControl.instance != null) {
					TaiXiuTouchMoveControl.instance.SetMoneyWinTaiXiu(W);
					TaiXiuTouchMoveControl.instance.SetMoneyWin(W);
				}
			}
			UpdateMoney();
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	#endregion

	#region Rank

	//[SkipRename]
	//[HandleUpdateEvent(EventType = HandlerType.EVN_UPDATEUI_HANDLER)]
	private void HighLowRank(params object[] param) {
		//List<DataItemRankTaiXiu> list = (List<DataItemRankTaiXiu>)param[0];
		//rankGroupTaiXiu.Init(list);
	}

	#endregion

	#region History

	//[SkipRename]
	//[HandleUpdateEvent(EventType = HandlerType.EVN_UPDATEUI_HANDLER)]
	private void HighLowHistory(params object[] param) {
		//List<DataItemHistoryTaiXiu> list = (List<DataItemHistoryTaiXiu>)param[0];
		//historyGroupTaiXiu.Init(list);
		//PopupAndLoadingScript.instance.HideLoading();
	}

	#endregion

	#region Info Tai Xiu
	internal void OnInfoTaiXiu(Message message) {
		try {
			int sizee = message.reader().ReadShort();
			for (int i = 0; i < sizee; i++) {
				string time = message.reader().ReadUTF();
				sbyte cua_dat = message.reader().ReadByte();
				sbyte ketqua = message.reader().ReadByte();
				long moneyCuoc = message.reader().ReadLong();
				long moneyTralai = message.reader().ReadLong();
				long moneyAn = message.reader().ReadLong();

			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
	#endregion
	#region Lich Su Phien Tai Xiu
	public void OnInfoLSTheoPhienTaiXiu(Message message) {
		try {
			int[] kq = new int[3];
			for (int i = 0; i < 3; i++) {
				kq[i] = message.reader().ReadByte();
			}
			if (kq[0] <= 0) {
				return;
			}
			string date = message.reader().ReadUTF();
			long tongDatTai = message.reader().ReadLong();
			long tongHoanTai = message.reader().ReadLong();
			long tongDatXiu = message.reader().ReadLong();
			long tongHoanXiu = message.reader().ReadLong();
			int sizee_Tai = message.reader().ReadInt();

			for (int i = 0; i < sizee_Tai; i++) {
				string time = message.reader().ReadUTF();
				string nameTai = message.reader().ReadUTF();
				long muccuoc = message.reader().ReadLong();
				long tralai = message.reader().ReadLong();
			}
			int sizee_Xiu = message.reader().ReadInt();
			for (int i = 0; i < sizee_Xiu; i++) {
				string time = message.reader().ReadUTF();
				string nameXiu = message.reader().ReadUTF();
				long muccuoc = message.reader().ReadLong();
				long tralai = message.reader().ReadLong();
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
	#endregion

	#endregion

	#region Update History

	GameObject Item_His_TX;
	List<Image> listHis = new List<Image>();

	void UpdateHistory(bool isTai) {
		if (Item_His_TX == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_HIS_TX, (objPre) => {
				Item_His_TX = objPre;
				Item_His_TX.SetActive(false);
				CreateHis(isTai);
			});
		} else {
			CreateHis(isTai);
		}
	}

	void CreateHis(bool isTai) {
		if (listHis.Count < COUNT_HIS) {
			GameObject obj = Instantiate(Item_His_TX);
			obj.transform.SetParent(tf_parent_his);
			obj.transform.localScale = Vector3.one;
			obj.SetActive(true);
			Image img = obj.GetComponent<Image>();
			//LoadAssetBundle.LoadSprite(img
			//	, BundleName.UI
			//	, isTai ? UIName.UI_TAI_XIU_TAI : UIName.UI_TAI_XIU_XIU);
			img.sprite = isTai ? sprite_tai_xiu[0] : sprite_tai_xiu[1];
			listHis.Add(img);
			for (int i = 0; i < listHis.Count - 1; i++) {
				listHis[i].transform.DOKill();
				listHis[i].transform.localScale = Vector3.one;
			}
			listHis[listHis.Count - 1].transform.DOKill();
			listHis[listHis.Count - 1].transform.DOScale(1.2f, 0.4f).SetLoops(-1, LoopType.Yoyo);// = Vector3.one;
		} else {
			for (int i = 0; i < listHis.Count - 1; i++) {
				listHis[i].sprite = listHis[i + 1].sprite;
				listHis[i].transform.DOKill();
				listHis[i].transform.localScale = Vector3.one;
			}

			listHis[listHis.Count - 1].sprite = isTai ? sprite_tai_xiu[0] : sprite_tai_xiu[1];
			listHis[listHis.Count - 1].transform.DOKill();
			listHis[listHis.Count - 1].transform.localScale = Vector3.one;
			listHis[listHis.Count - 1].transform.DOScale(1.2f, 0.4f).SetLoops(-1, LoopType.Yoyo);
		}
	}

	#endregion

	public void UpdateMoney() {

	}

	public void OnHide() {
		//SendData.onExitTaiXiu();
		gameObject.SetActive(false);
	}
	public bool IsShow() {
		return gameObject.activeSelf;
	}

	void EffectRunMoney(Text txt_target, long oldMoney, long newMoney) {
		if (oldMoney == newMoney) {
			return;
		}
		if (isActiveAndEnabled) {
			StartCoroutine(RunEffectText(txt_target, oldMoney, newMoney));
		} else {
			txt_target.text = MoneyHelper.FormatMoneyNormal(newMoney);
		}
	}

	IEnumerator RunEffectText(Text txt_target, long oldMoney, long newMoney) {
		yield return new WaitForEndOfFrame();
		long increMoney = (newMoney - oldMoney) / 20;
		for (int i = 0; i < 20; i++) {
			oldMoney += increMoney;
			if (increMoney > 0) {
				if (oldMoney >= newMoney) {
					oldMoney = newMoney;
				}
			} else {
				if (oldMoney <= newMoney) {
					oldMoney = newMoney;
				}
			}
			txt_target.text = MoneyHelper.FormatMoneyNormal(oldMoney);
			yield return new WaitForSeconds(0.001f);
			if (i >= 19) {
				txt_target.text = MoneyHelper.FormatMoneyNormal(newMoney);
			}
		}
		//txt_target.text = MoneyHelper.FormatMoneyNormal(newMoney);
	}
}
