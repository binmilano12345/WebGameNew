
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AppConfig;
using System;
using DataBase;
using System.Linq;
using Us.Mobile.Utilites;
using UnityEngine.Events;
using Beebyte.Obfuscator;

public class XocDiaControl : BaseCasino {
	public static XocDiaControl instance;

	void Awake() {
		instance = this;
	}

	#region UI

	[SerializeField]
	TimeCountDown timeCountDown;

	[Header("DAT CUOC")]
	/// <summary>
	/// 0-chan, 1-le, 2-4do, 3-4trang, 4-3do, 5-3trang
	/// </summary>
	[SerializeField]
	Button[] btn_cua_cuoc;
	/// <summary>
	/// 0-chan, 1-le, 2-4do, 3-4trang, 4-3do, 5-3trang
	/// </summary>
	[SerializeField]
	GameObject[] win_effect;
	/// <summary>
	/// 0-chan, 1-le, 2-4do, 3-4trang, 4-3do, 5-3trang
	/// </summary>
	[SerializeField]
	Text[] txt_sum_money;
	/// <summary>
	/// 0-chan, 1-le, 2-4do, 3-4trang, 4-3do, 5-3trang
	/// </summary>
	[SerializeField]
	Text[] txt_me_money;
	[SerializeField]
	UIButton btn_dat_x2, btn_dat_lai, btn_huy_cuoc, btn_lam_cai, btn_huy_cai;
	[SerializeField]
	Transform img_girl;
	[SerializeField]
	GameObject obj_pre_chip;
	//load prefab chip

	[SerializeField]
	Image[] img_dices;

	[Header("BET")]
	[SerializeField]
	Button[] btn_bet_money;
	[SerializeField]
	GameObject[] img_effect_bet;
	[SerializeField]
	Text[] txt_bet_money;
	[Header("HISTORY")]
	[SerializeField]
	Image[] chanleImgs;
	[SerializeField]
	Animator anim;
	[SerializeField]
	Transform batTf;
	//[SerializeField]
	//Image img_dia;
	[SerializeField]
	Text chanTextHis;
	[SerializeField]
	Text leTextHis;

	#endregion

	#region Variable

	int numbChanHis = 0;
	int numbLeHis = 0;
	bool IsDatCuoc = false;
	long CurrentBetMoney = 0;
	long[] SelectBetMoney = new long[4];
	long[] sum_money = new long[6];
	long[] sum_me_money = new long[6];

	List<GameObject> ListChipCua0 = new List<GameObject>();
	List<GameObject> ListChipCua1 = new List<GameObject>();
	List<GameObject> ListChipCua2 = new List<GameObject>();
	List<GameObject> ListChipCua3 = new List<GameObject>();
	List<GameObject> ListChipCua4 = new List<GameObject>();
	List<GameObject> ListChipCua5 = new List<GameObject>();

	#endregion

	new void Start() {
		base.Start();
		int i = 0;
		for (i = 0; i < btn_bet_money.Length; i++) {
			btn_bet_money[i].name = i + "";
			GameObject obj = btn_bet_money[i].gameObject;
			btn_bet_money[i].onClick.AddListener(delegate {
				OnChangeBet(obj);
			});
		}
		i = 0;
		for (i = 0; i < btn_cua_cuoc.Length; i++) {
			btn_cua_cuoc[i].name = i + "";

			GameObject obj = btn_cua_cuoc[i].gameObject;
			btn_cua_cuoc[i].onClick.AddListener(delegate {
				OnClickDatCuoc(obj);
			});
		}
	}

	#region CLICK

	void OnChangeBet(GameObject obj) {
		Debug.LogError("==========>  " + obj.name);
		int index = int.Parse(obj.name);
		for (int i = 0; i < img_effect_bet.Length; i++) {
			if (i != index) {
				img_effect_bet[i].SetActive(false);
			}
		}
		//		if (tg_bet_money [index].isOn) {

		img_effect_bet[index].SetActive(true);
		if (index >= btn_bet_money.Length - 1)
			CurrentBetMoney = ClientConfig.UserInfo.CASH_FREE;
		else
			CurrentBetMoney = SelectBetMoney[index];
		//		}

		Debug.LogError("==========>CurrentBetMoney  " + CurrentBetMoney);
	}

[SkipRename]
	void OnClickDatCuoc(GameObject obj) {
		//		Debug.LogError ("OnClickDatCuoc==========>  " + obj.name);
		int cua = int.Parse(obj.name);

		if (IsDatCuoc) {
			SendData.onsendXocDiaDatCuoc((byte)cua, CurrentBetMoney);
		}
	}

[SkipRename]
	public void OnClickDatX2() {
		SendData.onsendGapDoi();
	}

[SkipRename]
	public void OnClickDatLai() {
		SendData.onsendDatLai();
	}

[SkipRename]
	public void OnClickHuyCuoc() {
		SendData.onsendHuyCuoc();
	}

[SkipRename]
	public void OnClickLamCai() {
		SendData.onsendLamCai();
	}

[SkipRename]
	public void OnClickHuyCai() {
	}

[SkipRename]
	public void OnClickShowHistory() {
		if (!isRunShow)
			return;
		isRunShow = false;
		if (isShow) {
			tf_parent_ls_item.DOLocalMoveY(VT_Y, 0.6f).OnComplete(() => {
				isRunShow = true;
			});
		} else {
			Vector3 vt = tf_parent_ls_item.localPosition;
			VT_Y = vt.y;
			int cout = tf_parent_ls_item.childCount;
			vt.y += (cout - 1) * 40;
			tf_parent_ls_item.DOLocalMoveY(vt.y, 0.6f).OnComplete(() => {
				isRunShow = true;
			});
		}
		if (listItemHis.Count > 0) {
			listItemHis[listItemHis.Count - 1].SetArrowUp(isShow);
		}
		isShow = !isShow;
	}

	#endregion

	#region Handle Lich Su Van Choi

	[SerializeField]
	Transform tf_parent_ls_image, tf_parent_ls_item;
	List<Image> listImage = new List<Image>();
	List<ItemHistoryXocDia> listItemHis = new List<ItemHistoryXocDia>();

	bool isShow = false;
	float VT_Y = -140;
	bool isRunShow = true;
	[SerializeField]
	GameObject objPreImgHis, objPreItemHis;
	void UpdateHistory(int resultRed) {
		#region Update His Image
		Debug.LogError("-------------->   " + listImage.Count);
		if (listImage.Count < 32) {
			//LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_IMAGE_LICH_SU_XOC_DIA, (objPre) => {
			//objPreImgHis = objPre;
			Image obj = Instantiate(objPreImgHis).GetComponent<Image>();
			obj.transform.SetParent(tf_parent_ls_image);
			obj.transform.localScale = Vector3.one;
			LoadAssetBundle.LoadSprite(obj, BundleName.UI, (resultRed % 2 != 0 ? UIName.UI_XD_RED : UIName.UI_XD_WHITE));
			listImage.Add(obj);
			if (resultRed % 2 != 0) {
				numbLeHis++;
				leTextHis.text = "<color=red>" + numbLeHis + "</color>\nLẻ";
			} else {
				numbChanHis++;
				chanTextHis.text = "<color=yellow>" + numbChanHis + "</color>\nChẵn";
			}
			//});
		} else {
			for (int i = 0; i < listImage.Count - 1; i++) {
				listImage[i].sprite = listImage[i + 1].sprite;
			}
			LoadAssetBundle.LoadSprite(listImage[listImage.Count - 1], BundleName.UI, (resultRed % 2 != 0 ? UIName.UI_XD_RED : UIName.UI_XD_WHITE));
			if (resultRed % 2 != 0) {
				numbLeHis++;
				leTextHis.text = "<color=red>" + numbLeHis + "</color>\nLẻ";
			} else {
				numbChanHis++;
				chanTextHis.text = "<color=yellow>" + numbChanHis + "</color>\nChẵn";
			}
		}
		#endregion

		#region Update item His
		if (listItemHis.Count < 9) {
			//LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_LICH_SU_XOC_DIA, (objPre) => {
			for (int i = 0; i < listItemHis.Count; i++) {
				listItemHis[i].SetShowArrow(false);
			}
			ItemHistoryXocDia objItem = Instantiate(objPreItemHis).GetComponent<ItemHistoryXocDia>();
			objItem.transform.SetParent(tf_parent_ls_item);
			objItem.transform.SetAsFirstSibling();
			objItem.transform.localScale = Vector3.one;

			objItem.SetInfo(resultRed);
			objItem.SetShowArrow(true);
			listItemHis.Add(objItem);
			//});
		} else {
			for (int i = 0; i < listItemHis.Count; i++) {
				ItemHistoryXocDia objHIs = listItemHis[i];
				if (i < listItemHis.Count - 1) {
					objHIs.SetInfo(listItemHis[i + 1].result);
				} else {
					objHIs.SetInfo(resultRed);
				}
			}
		}
		#endregion
	}

	internal void OnXocDia_LichSu(Message message) {
		try {
			string a = message.reader().ReadUTF();
			Debug.LogError("Lich su:   " + a);
			if (a.Length <= 0)
				return;
			string[] chuoi = a.Split(',');
			for (int i = 0; i < chuoi.Length - 1; i++) {
				int r = UnityEngine.Random.Range(0, 3);
				if (int.Parse(chuoi[i]) == 0) {//chan
											   //UpdateHistory(4 - r * 2);
											   //					numbChanHis++;
				} else {//le
					if (r == 0)
						r = 1;
					//UpdateHistory(5 - r * 2);
				}
			}

		} catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	#endregion


	#region Animation Xoc Dia, Mo bat
	void StartXocDiaAnim() {
		SetAnimationUpBat(() => {
			batTf.gameObject.SetActive(false);
			anim.Play("Xoc");
			StartCoroutine(IESetAnimationNormal());
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
			img_dices[i].gameObject.SetActive(false);
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


	private void SetActiveButton(bool isGapDoi, bool isDatLai, bool isHuyCuoc, bool isLamCai) {
		btn_dat_x2.gameObject.SetActive(isGapDoi);
		btn_dat_lai.gameObject.SetActive(isDatLai);
		btn_huy_cuoc.gameObject.SetActive(isHuyCuoc);
		btn_lam_cai.gameObject.SetActive(isLamCai);
		btn_huy_cai.gameObject.SetActive(!isLamCai);
	}

	private void SetEnableButton(bool isGapDoi, bool isDatLai, bool isHuyCuoc, bool isLamCai, bool isHuyCai) {
		btn_dat_x2.enabled = isGapDoi;
		btn_dat_lai.enabled = isDatLai;
		btn_huy_cuoc.enabled = isHuyCuoc;
		btn_lam_cai.enabled = isLamCai;
		btn_huy_cai.enabled = isHuyCai;
	}

	internal override void SetMaster(String nick) {
		OnJoinTableSuccess(nick);
	}

	internal override void OnTimeAuToStart(int time) {
		PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("xd_cho_van_moi"), 5);
		timeCountDown.SetTime(time);

		SetAnimationNormal();
		SetEnableButton(false, false, false, true, false);
	}

	internal void OnBeGinXocDia(int time) {
		PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("xd_nha_cai_xoc"), 5);

		for (int i = 0; i < img_dices.Length; i++) {
			img_dices[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < txt_sum_money.Length; i++) {
			sum_money[i] = 0;
			sum_me_money[i] = 0;
			txt_sum_money[i].text = MoneyHelper.FormatMoneyNormal(sum_money[i]);
			txt_me_money[i].text = MoneyHelper.FormatMoneyNormal(sum_me_money[i]);
		}
		for (int i = 0; i < win_effect.Length; i++) {
			win_effect[i].SetActive(false);
		}

		timeCountDown.SetTime(0);
		StartXocDiaAnim();
	}

	internal void OnXocDia_DatCuoc(Message message) {
		string nick = message.reader().ReadUTF();
		sbyte cua = message.reader().ReadByte();
		long money = message.reader().ReadLong();
		int typeCHIP = message.reader().ReadByte();

		XocDiaPlayer pl = (XocDiaPlayer)GetPlayerWithName(nick);
		if (pl != null) {
			//			pl.ActionChipDatCuoc (cua, btn_cua_cuoc [cua].transform.position, obj_pre_chip);
			pl.ActionChipDatCuoc(cua, GenPostionRandomInCua(cua), obj_pre_chip, money);
		}
		sum_money[cua] += money;
		txt_sum_money[cua].text = MoneyHelper.FormatMoneyNormal(sum_money[cua]);
		if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
			sum_me_money[cua] += money;
			txt_me_money[cua].text = MoneyHelper.FormatMoneyNormal(sum_me_money[cua]);
		}
	}

	internal void OnXocDia_DatX2(Message message) {
		string nick = message.reader().ReadUTF();
		sbyte socua = message.reader().ReadByte();
		XocDiaPlayer pl = (XocDiaPlayer)GetPlayerWithName(nick);
		for (int i = 0; i < socua; i++) {
			sbyte cua = message.reader().ReadByte();
			if (pl != null) {
				//				pl.ActionChipDatX2 (cua, btn_cua_cuoc [cua].transform.position);
				pl.ActionChipDatX2(cua, GenPostionRandomInCua(cua));
			}

			sum_money[cua] *= 2;
			txt_sum_money[cua].text = MoneyHelper.FormatMoneyNormal(sum_money[cua]);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				sum_me_money[cua] *= 2;
				txt_me_money[cua].text = MoneyHelper.FormatMoneyNormal(sum_me_money[cua]);
			}
		}
	}

	internal void OnXocDia_DatLai(Message message) {
		string nick = message.reader().ReadUTF();
		sbyte socua = message.reader().ReadByte();
		XocDiaPlayer pl = (XocDiaPlayer)GetPlayerWithName(nick);
		for (int i = 0; i < socua; i++) {
			sbyte cua = message.reader().ReadByte();
			sbyte a = message.reader().ReadByte();
			if (a == 1) {
				sbyte soloaichip = message.reader().ReadByte();
				for (int j = 0; j < soloaichip; j++) {
					sbyte loaichip = message.reader().ReadByte();
					int sochip = message.reader().ReadInt();
					for (int k = 0; k < sochip; k++) {
						//						pl.ActionChipDatCuoc (cua, btn_cua_cuoc [cua].transform.position, obj_pre_chip);
						pl.ActionChipDatCuoc(cua, GenPostionRandomInCua(cua), obj_pre_chip, sochip);
					}
				}
			}
		}
	}

	internal void OnBeGinXocDia_TG_DatCuoc(int time) {
		PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("xd_bd_dat_cuoc"), 5);
		timeCountDown.SetTime(time);
		SetEnableButton(true, true, true, false, false);
		IsDatCuoc = true;

		ListChipCua0.Clear();
		ListChipCua1.Clear();
		ListChipCua2.Clear();
		ListChipCua3.Clear();
		ListChipCua4.Clear();
		ListChipCua5.Clear();
		chipThua.Clear();
	}

	internal void OnXocDia_TG_DungCuoc(Message message) {
		SetEnableButton(true, true, true, false, false);
		try {
			int time = message.reader().ReadByte();
			PopupAndLoadingScript.instance.toast.showToast(ClientConfig.Language.GetText("xd_nha_cai_dung_cuoc"), 5);
			timeCountDown.SetTime(time);
			IsDatCuoc = false;
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal void OnBeGinXocDia_MoBat(int quando) {
		timeCountDown.SetTime(0);
		MoBat();
		SetEnableButton(false, false, false, false, false);
		Debug.LogError("so quan do:   " + quando);
		for (int i = 0; i < img_dices.Length; i++) {
			img_dices[i].gameObject.SetActive(true);
			LoadAssetBundle.LoadSprite(img_dices[i], BundleName.UI, i < quando ? UIName.UI_XD_RED : UIName.UI_XD_WHITE);
		}
		UpdateHistory(quando);
	}

	#region Finish

	internal override void OnFinishGame(Message message) {
		try {
			//			dangchoi = false;
			int cua1 = message.reader().ReadByte();
			int cua2 = message.reader().ReadByte();
			int size = message.reader().ReadByte();
			for (int i = 0; i < size; i++) {
				string _name = message.reader().ReadUTF();
				long moneyEarn = message.reader().ReadLong();
				XocDiaPlayer pl = (XocDiaPlayer)GetPlayerWithName(_name);
				if (pl != null) {
					if (moneyEarn > 0) {
						pl.SetRank(1);
					} else {//thua
						pl.SetRank(6);
					}
					pl.SetEffect((moneyEarn > 0 ? "+" : "") + MoneyHelper.FormatMoneyNormal(moneyEarn));
					pl.IsReady = false;
				}
			}
			Debug.LogError("Cua 1: " + cua1 + "  Cua 2: " + cua2);
			win_effect[cua1].SetActive(true);
			if (cua2 > 1 && cua2 < 6)
				win_effect[cua2].SetActive(true);

			StartCoroutine(MoveMoneyFinishGame(cua1, cua2));
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	public void AddChipToList(int cua, GameObject objChip) {
		switch (cua) {
			case 0:
				ListChipCua0.Add(objChip);
				break;
			case 1:
				ListChipCua1.Add(objChip);
				break;
			case 2:
				ListChipCua2.Add(objChip);
				break;
			case 3:
				ListChipCua3.Add(objChip);
				break;
			case 4:
				ListChipCua4.Add(objChip);
				break;
			case 5:
				ListChipCua5.Add(objChip);
				break;
		}
	}

	public void RemoveChipFromList(int cua, GameObject objChip) {
		switch (cua) {
			case 0:
				ListChipCua0.Remove(objChip);
				break;
			case 1:
				ListChipCua1.Remove(objChip);
				break;
			case 2:
				ListChipCua2.Remove(objChip);
				break;
			case 3:
				ListChipCua3.Remove(objChip);
				break;
			case 4:
				ListChipCua4.Remove(objChip);
				break;
			case 5:
				ListChipCua5.Remove(objChip);
				break;
		}
	}

	IEnumerator MoveMoneyFinishGame(int cua1, int cua2) {
		yield return new WaitForSeconds(0.5f);
		ActionLayTien(cua1, cua2);
		yield return new WaitForSeconds(0.5f);
		ActionTraTien(cua1, cua2);
		yield return new WaitForSeconds(0.5f);
		TraTienNguoiChoi(cua1, cua2);
	}

	void ActionLayTien(int cua1, int cua2) {
		if (cua1 == 0) {
			LayTienCuaThua(ListChipCua1);
		} else {
			LayTienCuaThua(ListChipCua0);
		}
		switch (cua2) {
			case 2:
				LayTienCuaThua(ListChipCua3);
				LayTienCuaThua(ListChipCua4);
				LayTienCuaThua(ListChipCua5);
				break;
			case 3:
				LayTienCuaThua(ListChipCua2);
				LayTienCuaThua(ListChipCua4);
				LayTienCuaThua(ListChipCua5);
				break;
			case 4:
				LayTienCuaThua(ListChipCua2);
				LayTienCuaThua(ListChipCua3);
				LayTienCuaThua(ListChipCua5);
				break;
			case 5:
				LayTienCuaThua(ListChipCua2);
				LayTienCuaThua(ListChipCua3);
				LayTienCuaThua(ListChipCua4);
				break;
			default:
				LayTienCuaThua(ListChipCua2);
				LayTienCuaThua(ListChipCua3);
				LayTienCuaThua(ListChipCua4);
				LayTienCuaThua(ListChipCua5);
				break;
		}
	}

	void ActionTraTien(int cua1, int cua2) {
		Debug.LogError(cua1 + " =-=-=Tra tien-=-=-= " + cua2);
		if (cua1 == 0) {
			for (int i = 0; i < chipThua.Count; i++) {
				GameObject obj = chipThua[i];
				if (i < ListChipCua0.Count) {
					//					obj.transform.DOMove (btn_cua_cuoc [cua1].transform.position, 0.1f);
					obj.transform.DOMove(GenPostionRandomInCua(cua1), 0.1f);
					ListChipCua0.Add(obj);
					chipThua.Remove(obj);
					i--;
				}
			}
		} else {
			for (int i = 0; i < chipThua.Count; i++) {
				GameObject obj = chipThua[i];
				if (i < ListChipCua1.Count) {
					//					obj.transform.DOMove (btn_cua_cuoc [cua1].transform.position, 0.1f);
					obj.transform.DOMove(GenPostionRandomInCua(cua2), 0.1f);
					ListChipCua1.Add(obj);
					chipThua.Remove(obj);
					i--;
				}
			}
		}
		switch (cua2) {
			case 2:
				for (int i = 0; i < chipThua.Count; i++) {
					GameObject obj = chipThua[i];
					//				obj.transform.DOMove (btn_cua_cuoc [cua2].transform.position, 0.1f);

					obj.transform.DOMove(GenPostionRandomInCua(cua2), 0.1f);
					ListChipCua2.Add(obj);
				}
				break;
			case 3:
				for (int i = 0; i < chipThua.Count; i++) {
					GameObject obj = chipThua[i];
					//				obj.transform.DOMove (btn_cua_cuoc [cua2].transform.position, 0.1f);
					obj.transform.DOMove(GenPostionRandomInCua(cua2), 0.1f);
					ListChipCua3.Add(obj);
				}
				break;
			case 4:
				for (int i = 0; i < chipThua.Count; i++) {
					GameObject obj = chipThua[i];
					//				obj.transform.DOMove (btn_cua_cuoc [cua2].transform.position, 0.1f);
					obj.transform.DOMove(GenPostionRandomInCua(cua2), 0.1f);
					ListChipCua4.Add(obj);
				}
				break;
			case 5:
				for (int i = 0; i < chipThua.Count; i++) {
					GameObject obj = chipThua[i];
					//				obj.transform.DOMove (btn_cua_cuoc [cua2].transform.position, 0.1f);
					obj.transform.DOMove(GenPostionRandomInCua(cua2), 0.1f);
					ListChipCua5.Add(obj);
				}
				break;
		}

		chipThua.Clear();
	}

	List<GameObject> chipThua = new List<GameObject>();

	void LayTienCuaThua(List<GameObject> listCua) {
		//		Debug.LogError (listCua.Count + "    " + img_dia.transform.position);
		//Vector3 pos = anim.transform.position;
		//float x = img_dia.rectTransform.sizeDelta.x;
		//float y = img_dia.rectTransform.sizeDelta.y;
		//pos.x -= x / 4;
		chipThua.Clear();
		for (int i = 0; i < listCua.Count; i++) {
			GameObject obj = listCua[i];
			obj.transform.DOMove(anim.transform.position, 0.1f).OnComplete(() => {
				//chipThua.Add (obj);
				if (i == listCua.Count - 1) {
					listCua.Clear();
				}
			});
			chipThua.Add(obj);
		}
		//co thang lam cai
		//		for (int i = 0; i < arr.size(); i++) {
		//			arr.get(i)
		//				.moveRemove(new Vector2(players[getPlayer(master)].getX(), players[getPlayer(master)].getY()));
		//		}
	}

	void TraTienNguoiChoi(int cua1, int cua2) {
		for (int i = 0; i < ListPlayer.Count; i++) {
			XocDiaPlayer player = (XocDiaPlayer)ListPlayer[i];
			player.ActionChipToPlayerWin(cua1, cua2);
			StartCoroutine(player.HideAllChip());
		}
	}

	#endregion

	internal void OnXocDiaUpdateCua(Message message) {
		try {
			for (int i = 0; i < sum_money.Length; i++) {
				sum_money[i] = 0;
				sum_me_money[i] = 0;
			}
			string nick = message.reader().ReadUTF();
			bool isMe = nick.Equals(ClientConfig.UserInfo.UNAME);
			for (int i = 0; i < 6; i++) {
				sum_money[i] = message.reader().ReadLong();
				sum_me_money[i] = message.reader().ReadLong();
				txt_sum_money[i].text = MoneyHelper.FormatMoneyNormal(sum_money[i]);
				if (isMe) {
					txt_me_money[i].text = MoneyHelper.FormatMoneyNormal(sum_me_money[i]);
				}
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal void OnXocDiaHuyCuoc(Message message) {
		try {
			string nick = message.reader().ReadUTF();
			long moneycua0 = message.reader().ReadLong();
			long moneycua1 = message.reader().ReadLong();
			long moneycua2 = message.reader().ReadLong();
			long moneycua3 = message.reader().ReadLong();
			long moneycua4 = message.reader().ReadLong();
			long moneycua5 = message.reader().ReadLong();
			XocDiaPlayer pl = (XocDiaPlayer)GetPlayerWithName(nick);
			if (pl != null) {
				pl.ActionTraTienCuoc(moneycua0, moneycua1, moneycua2, moneycua3, moneycua4, moneycua5);
			}
		} catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	internal void OnNhanCacMucCuocXD(Message message) {
		try {
			for (int i = 0; i < 4; i++) {
				long mucc = message.reader().ReadLong();
				SelectBetMoney[i] = mucc;
				txt_bet_money[i].text = MoneyHelper.FormatRelativelyWithoutUnit(mucc);
			}
			img_effect_bet[0].SetActive(true);
			for (int i = 1; i < img_effect_bet.Length; i++) {
				img_effect_bet[i].SetActive(false);
			}
			CurrentBetMoney = SelectBetMoney[0];

		} catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	internal void OnXocDia_HuyCua_LamCai(Message message) {
		try {
			sbyte type = message.reader().ReadByte();
			switch (type) {
				case 2:
					// huy cua chan
					//				for (MoveMoney money : arrMoveMoneysCUA0) {
					//					money.tra_tien();
					//				}
					//				screen.toast.showToast("Nhà cái hủy của chẵn");
					//				arrMoveMoneysCUA0.clear();
					break;
				case 1:
					// huy cua le
					//				for (MoveMoney money : arrMoveMoneysCUA1) {
					//					money.tra_tien();
					//				}
					//				arrMoveMoneysCUA1.clear();
					//				screen.toast.showToast("Nhà cái hủy của lẻ");
					break;
			}
			//			btnHuyCuaChan.setDisabled2(true);
			//			btnHuyCuaLe.setDisabled2(true);
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	Vector3 GenPostionRandomInCua(int cua) {
		Vector3 pos = btn_cua_cuoc[cua].transform.position;

		Image img = btn_cua_cuoc[cua].image;
		float x = img.rectTransform.sizeDelta.x;
		float y = img.rectTransform.sizeDelta.y;
		pos.x += UnityEngine.Random.Range(-x / 3, x / 3);
		pos.y += UnityEngine.Random.Range(-y / 3.5f, y / 3.5f);

		return pos;
	}
}
