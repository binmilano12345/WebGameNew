using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AppConfig;

public class XocDiaControl : BaseCasino
{
	#region UI

	[SerializeField]
	TimeCountDown timeCountDown;

	[Header ("DAT CUOC")]
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

	[Header ("BET")]
	[SerializeField]
	Toggle[] tg_bet_money;
	[SerializeField]
	Text[] txt_bet_money;
	[Header ("HISTORY")]
	[SerializeField]
	Image[] chanleImgs;
	[SerializeField]
	Animator animXocDia;
	[SerializeField]
	Image img_bat;
	[SerializeField]
	Text chanTextHis;
	[SerializeField]
	Text leTextHis;

	#endregion

	#region Variable

	int result = 0;
	int numbChanHis = 0;
	int numbLeHis = 0;
	bool IsDatCuoc = false;
	long CurrentBetMoney = 0;
	long[] SelectBetMoney = new long[4];
	#endregion

	void Start ()
	{
		base.Start ();
		int i = 0;
		for (i = 0; i < tg_bet_money.Length; i++) {
			tg_bet_money [i].name = i + "";
			tg_bet_money [i].onValueChanged.AddListener (delegate {
				OnChangeBet (tg_bet_money [i].gameObject);
			});
		}
		for (i = 0; i < btn_cua_cuoc.Length; i++) {
			btn_cua_cuoc [i].name = i + "";
			btn_cua_cuoc [i].onClick.AddListener (delegate {
				OnClickDatCuoc (btn_cua_cuoc [i].gameObject);
			});
		}
	}

	#region CLICK

	void OnChangeBet (GameObject obj)
	{
		Debug.LogError ("==========>  " + obj.name);
		int index = int.Parse (obj.name);
		if (index >= tg_bet_money.Length - 1)
			CurrentBetMoney = ClientConfig.UserInfo.CASH_FREE;
		else
			CurrentBetMoney = SelectBetMoney[index];
	}

	void OnClickDatCuoc (GameObject obj)
	{
		Debug.LogError ("OnClickDatCuoc==========>  " + obj.name);
		int cua = int.Parse (obj.name);

		if (IsDatCuoc) {
			SendData.onsendXocDiaDatCuoc ((byte)cua, CurrentBetMoney); 
		}
	}

	public void OnClickShowHistory ()
	{
		if (!isRunShow)
			return;
		isRunShow = false;
		if (isShow) {
			tf_parent_ls_item.DOLocalMoveY (VT_Y, 0.6f).OnComplete (() => {
				isRunShow = true;
			});
		} else {
			Vector3 vt = tf_parent_ls_item.localPosition;
			VT_Y = vt.y;
			int cout = tf_parent_ls_item.childCount;
			vt.y += (cout - 1) * 40;
			tf_parent_ls_item.DOLocalMoveY (vt.y, 0.6f).OnComplete (() => {
				isRunShow = true;
			});
		}
		if (listItemHis.Count > 0) {
			listItemHis [listItemHis.Count - 1].SetArrowUp (isShow);
		}
		isShow = !isShow;
	}

	#endregion

	#region Handle Lich Su Van Choi

	[SerializeField]
	Transform tf_parent_ls_image, tf_parent_ls_item;
	List<Image> listImage = new List<Image> ();
	List<ItemHistoryXocDia> listItemHis = new List<ItemHistoryXocDia> ();

	bool isShow = false;
	float VT_Y = -140;
	bool isRunShow = true;

	void UpdateHistory ()
	{
		int index = 0;
		switch (result) {
		case 1://chan
			index = 2;
			break;
		case 2:
			index = 3; //le
			break;
		case 3:
			index = 0; //4 vang
			break;
		case 4://3 trang 1 vang
			index = 3;
			break;
		case 5:
			index = 1; //1 trang 3 vang
			break;
		case 6:
			index = 4; // 4 trang
			break;
		}
		#region Update His Image
		if (listImage.Count < 24) {
			//Debug.LogError("Ket qua: " + result);//sai
			LoadAssetBundle.LoadPrefab (BundleName.PREFAPS, PrefabsName.PRE_IMAGE_LICH_SU_XOC_DIA, (objPre) => {
				Image obj = objPre.GetComponent<Image> ();
				obj.transform.SetParent (tf_parent_ls_image);
				obj.transform.localScale = Vector3.one;
				LoadAssetBundle.LoadSprite (obj, BundleName.UI, (index % 2 != 0 ? UIName.UI_XD_RED : UIName.UI_XD_WHITE));
				listImage.Add (obj);
				if (index % 2 != 0) {
					numbLeHis++;
					leTextHis.text = "<color = yellow>" + numbLeHis + "</color>\nLẻ";
				} else {
					numbChanHis++;
					chanTextHis.text = "<color = yellow>" + numbChanHis + "</color>\nChẵn";
				}
			});
		} else {
			for (int i = 0; i < listImage.Count; i++) {
				Image objImg = listImage [i];
				if (i < listImage.Count - 1) {
					objImg.sprite = listImage [i + 1].sprite;
				} else {
					LoadAssetBundle.LoadSprite (objImg, BundleName.UI, (index % 2 != 0 ? UIName.UI_XD_RED : UIName.UI_XD_WHITE));
					if (index % 2 != 0) {
						numbLeHis++;
						leTextHis.text = "Lẻ \n" + numbLeHis;
					} else {
						numbChanHis++;
						chanTextHis.text = "Chẵn \n" + numbChanHis;
					}
				}
			}
		}
		#endregion

		#region Update item His
		if (listItemHis.Count < 9) {
			LoadAssetBundle.LoadPrefab (BundleName.PREFAPS, PrefabsName.PRE_ITEM_LICH_SU_XOC_DIA, (objPre) => {
				for (int i = 0; i < listItemHis.Count; i++) {
					listItemHis [i].SetShowArrow (false);
				}
				ItemHistoryXocDia objItem = objPre.GetComponent<ItemHistoryXocDia> ();
				objItem.transform.SetParent (tf_parent_ls_item);
				objItem.transform.SetAsFirstSibling ();
				objItem.transform.localScale = Vector3.one;

				objItem.SetInfo (result);
				objItem.SetShowArrow (true);
				listItemHis.Add (objItem);
			});
		} else {
			for (int i = 0; i < listItemHis.Count; i++) {
				ItemHistoryXocDia objHIs = listItemHis [i];
				if (i < listItemHis.Count - 1) {
					objHIs.SetInfo (listItemHis [i + 1].result);
				} else {
					objHIs.SetInfo (result);
				}
			}
		}
		#endregion
	}

	#endregion
}
