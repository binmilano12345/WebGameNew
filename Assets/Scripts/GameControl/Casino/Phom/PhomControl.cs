using AppConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PhomControl : BaseCasino, IHasChanged
{
	public static PhomControl instace;
	[SerializeField]
	GameObject objBatDau, objSanSang, objDanh, objBoc, objAn, objHaPhom, objXepBai;

	[SerializeField]
	Text txt_num_card_boc;

	int totalCardNoc = 0;
	List<int> ListIdCardAn = new List<int> ();
	int cardDanhTruocDo = 0;

	[SerializeField]
	Text txt_luat_choi;
	void Awake ()
	{
		instace = this;
	}
	// Use this for initialization
	public new void Start ()
	{
		base.Start ();
		OnRule (0);
	}

	/// <summary>
	/// 1-isBatDau, 2-isSanSang, 3-isDanh, 4-isBocBai, 5-isAnBai, 6-isHaBai, 7-isXepBai
	/// </summary>
	internal void SetActiveButton (bool isBatDau, bool isSanSang, bool isDanh, bool isBoc, bool isAn, bool isHaPhom, bool isXepBai)
	{
		objBatDau.SetActive (isBatDau);
		objSanSang.SetActive (isSanSang);
		objDanh.SetActive (isDanh);
		objBoc.SetActive (isBoc);
		objAn.SetActive (isAn);
		objHaPhom.SetActive (isHaPhom);
		objXepBai.SetActive (isXepBai);
	}

	#region Click

	public void OnClickBatDau ()
	{
//        bool isAllReady = true;
//        for (int i = 0; i < ListPlayer.Count; i++) {
//            if (!ListPlayer[i].IsReady) {
//                isAllReady = false;
//                break;
//            }
//        }
//
//        if (isAllReady) {
		SendData.onStartGame ();
//        } else {
//            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("popup_con_ng_san_sang"));
//        }
	}

	public void OnClickSanSang ()
	{
		SendData.onReady (1);
	}

	public void OnClickDanh ()
	{
		int[] cards = ((PhomPlayer)playerMe).cardTaLaManager.GetCardChooseInHand ();
		if (cards.Length <= 0) {
			PopupAndLoadingScript.instance.toast.showToast ("Chưa chọn bài");
			return;
		} else if (cards.Length > 1) {
			PopupAndLoadingScript.instance.toast.showToast ("Bạn chỉ được đánh mỗi lần 1 lá bài");
			return;
		}
		for (int i = 0; i < cards.Length; i++) {
			if (ListIdCardAn.Contains (cards [i])) {
				PopupAndLoadingScript.instance.toast.showToast ("Bạn không được đánh con đã ăn!");
				return;
			}
		}

		SendData.onFireCard (cards [0]);//sua
	}

	public void OnClickBoc ()
	{
		SendData.onGetCardNoc ();
	}

	public void OnClickHaPhom ()
	{
		//int[][] phom = RTL.checkPhom(arr, eatArr);
		//if (phom == null) {
		SendData.onHaPhom (null);//tu dong ha
		//} else {
		//    SendData.onHaPhom(phom);//ha thu cong
		//    btn_ha_phom.setVisible(false);
		//}
	}

	public void OnClickAnBai ()
	{
		SendData.onGetCardFromPlayer ();
	}

	public void OnClickRule ()
	{
		SendData.onChangeRuleTbl ();
	}

	public void OnClickSortCard ()
	{
		((PhomPlayer)playerMe).cardTaLaManager.SortCard (ListIdCardAn);
	}

	public void OnClickChangeRule ()
	{
		SendData.onChangeRuleTbl();
	}

	#endregion

	internal override void OnJoinTableSuccess (string master)
	{
		base.OnJoinTableSuccess (master);
		if (master.Equals (ClientConfig.UserInfo.UNAME)) {
			SetActiveButton (true, false, false, false, false, false, false);
		} else {
			SetActiveButton (false, true, false, false, false, false, false);
		}
	}

	internal override void SetMaster (string nick)
	{
		base.SetMaster (nick);
		if (nick.Equals (ClientConfig.UserInfo.UNAME)) {
			SetActiveButton (true, false, false, false, false, false, false);
		} else {
			SetActiveButton (false, true, false, false, false, false, false);
		}
	}

	internal override void StartTableOk (int[] cardHand, Message msg, string[] nickPlay)
	{
		base.StartTableOk (cardHand, msg, nickPlay);
		totalCardNoc = nickPlay.Length * 4 - 1;
		SetNumCardLoc (totalCardNoc);
		cardDanhTruocDo = 0;
		ListIdCardAn.Clear ();
		StopCoroutine (WaitFinish ());
		for (int i = 0; i < nickPlay.Length; i++) {
			PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nickPlay [i]);
			if (pl != null) {
				pl.cardTaLaManager.SetChiaBai (cardHand, pl.SitOnClient == 0);
			}
		}
	}

	internal override void SetTurn (string nick, Message message)
	{
		base.SetTurn (nick, message);
		try {
			PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nick);
			if (nick.Equals (ClientConfig.UserInfo.UNAME) || string.IsNullOrEmpty (nick)) {
				bool bocbai = false, anbai = false, haphom = false, danhbai = false;
				anbai = false;
				bocbai = true;
				int[] cardMe = pl.cardTaLaManager.GetCardIdCardHand ();
				if (cardMe.Length < 10) {
					bocbai = true;
				} else {
					bocbai = false;
				}
				if (pl.cardTaLaManager.NumCardFire () >= 4) {
					haphom = true;
					danhbai = false;
					anbai = false;
					bocbai = false;
				} else {
					int[] cardPhom = AutoChooseCardTaLa.GetPhomAnDuoc (cardMe, cardDanhTruocDo, ListIdCardAn.ToArray ());

					if (cardPhom != null) {
						anbai = true;
					}
				}
				SetActiveButton (false, false, danhbai, bocbai, anbai, haphom, false);//sua check de hien nut an
			} else {
				SetActiveButton (false, false, false, false, false, false, true);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnFireCard (string nick, string turnName, int[] card)
	{
		base.OnFireCard (nick, turnName, card);
		PhomPlayer plFire = (PhomPlayer)GetPlayerWithName (nick);
		cardDanhTruocDo = card [0];
		if (plFire != null) {
			plFire.SetTurn (0, false);
			plFire.cardTaLaManager.OnFireCard (card [0], nick.Equals (ClientConfig.UserInfo.UNAME));

			if (nick.Equals (ClientConfig.UserInfo.UNAME)) {
				if (plFire.cardTaLaManager.NumCardFire () >= 3) {
					int[] arrr = AutoChooseCardTaLa.GetPhomTrenTayOneArray (plFire.cardTaLaManager.GetCardIdCardHand ());
					if (arrr.Length <= 0) {
						SetActiveButton (false, false, false, false, false, false, true);
					} else
						SetActiveButton (false, false, false, false, false, true, false);
				} else {
					SetActiveButton (false, false, false, false, false, false, true);
				}
			}
		}

//		PhomPlayer plTurn = (PhomPlayer)GetPlayerWithName (turnName);
		bool issMe = turnName.Equals (ClientConfig.UserInfo.UNAME);
		if (playerMe != null) {
			if (issMe) {
				int[] temp = ((PhomPlayer)playerMe).cardTaLaManager.GetCardIdCardHand ();
				int[] arr = AutoChooseCardTaLa.GetPhomAnDuoc (temp, cardDanhTruocDo, ListIdCardAn.ToArray ());
//				for (int i = 0; i < temp.Length; i++) {
//					Debug.LogError ("Tren tay:  " + temp[i] + " = " + AutoChooseCardTaLa.GetValue (temp[i]));
//				}
//				for (int i = 0; i < arr.Length; i++) {
//					Debug.LogError ("Phom an duoc:  " + arr[i] + " = " + AutoChooseCardTaLa.GetValue (arr[i]));
//				}
				if (arr != null && arr.Length > 0) {
					((PhomPlayer)playerMe).cardTaLaManager.ArrayCardHand.SetChooseCard (arr);
					SetActiveButton (false, false, false, true, true, false, true);
				}
			}
		}
	}
	//Boc bai
	internal void OnGetCardNocSuccess (string nick, int card)
	{
		totalCardNoc--;
		SetNumCardLoc (totalCardNoc);
		PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nick);
		bool issMe = nick.Equals (ClientConfig.UserInfo.UNAME);
		if (pl != null) {
			pl.cardTaLaManager.BocBai (card, issMe, () => {
				if (issMe) {
//					if (pl.cardTaLaManager.NumCardFire () >= 3) {
//						int[] arrr = AutoChooseCardTaLa.GetPhomTrenTayOneArray (pl.cardTaLaManager.GetCardIdCardHand ());
//						if (arrr.Length <= 0) {
//							SetActiveButton (false, false, true, false, false, false, false);
//						} else
//							SetActiveButton (false, false, false, false, false, true, false);
//					} else {
//						SetActiveButton (false, false, true, false, false, false, true);
//					}

					SetActiveButton (false, false, true, false, false, false, true);
				}
			});
		}
	}

	internal void OnEatCardSuccess (string thangAn, string thangBiAn, int card)
	{
//		Debug.LogError (ClientConfig.UserInfo.UNAME + "   Thang An:  " + thangAn);
//		Debug.LogError ("Thang Bi An:  " + thangBiAn);
		Card cardAn = null;
		PhomPlayer plThangBiAn = (PhomPlayer)GetPlayerWithName (thangBiAn);
		PhomPlayer plThangAn = (PhomPlayer)GetPlayerWithName (thangAn);

//		for (int i = 0; i < ListPlayer.Count; i++) {
//			cardAn = ((PhomPlayer)ListPlayer [i]).cardTaLaManager.ArrayCardFire.GetCardbyIDCard (card);
//			if (cardAn != null) {
//				break;
//			}
//		}

        cardAn = plThangBiAn.cardTaLaManager.ArrayCardFire.GetCardbyIDCard(card);
		if (cardAn != null) {
			bool isTao = thangAn.ToUpper ().Equals (ClientConfig.UserInfo.UNAME.ToUpper ());
			if (plThangAn != null) {
				plThangAn.cardTaLaManager.SetEatCard (card, isTao, cardAn, () => {
					if (isTao) {
						plThangAn.cardTaLaManager.ArrayCardHand.ResetCard ();
						ListIdCardAn.Add (card);
						if (plThangAn.cardTaLaManager.NumCardFire () >= 3) {
							SetActiveButton (false, false, false, false, false, true, false);
						} else
							SetActiveButton (false, false, true, false, false, false, true);
					}
				});
			}
		}
	}
	//chuyen bai
	internal void OnBalanceCard (string tenThangGuiBai, string guiDenThangNay, int card)
	{
		try {
			PhomPlayer plTuThangNay = (PhomPlayer)GetPlayerWithName (tenThangGuiBai);
			PhomPlayer plDenThangNay = (PhomPlayer)GetPlayerWithName (guiDenThangNay);
			if (plTuThangNay != null && plDenThangNay != null) {
				plDenThangNay.cardTaLaManager.ChuyenBai (card, plTuThangNay, () => {
					if (tenThangGuiBai.Equals (ClientConfig.UserInfo.UNAME) && plTuThangNay.IsTurn) {
						if (plTuThangNay.cardTaLaManager.NumCardFire () >= 3) {
							SetActiveButton (false, false, false, false, false, true, false);
						} else
							SetActiveButton (false, false, true, false, false, false, true);
					}
				});
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnFinishGame (Message message)
	{
		base.OnFinishGame (message);

		//sua de tinh diem
		//for (int i = 0; i < 4; i++) {
		//    if (players[i].getName().length() > 0) {
		//        players[i].setInfo(false, false, true, players[i].diem);
		//    }
		//    cardDrop[i].removeAllCard();
		//}
		StartCoroutine (WaitFinish ());
	}

	IEnumerator WaitFinish ()
	{
		yield return new WaitForSeconds (3);
		if (playerMe.IsMaster)
			SetActiveButton (true, false, false, false, false, false, false);
		else
			SetActiveButton (false, true, false, false, false, false, false);
		for (int i = 0; i < ListPlayer.Count; i++) {
			PhomPlayer pl = (PhomPlayer)ListPlayer [i];
			pl.cardTaLaManager.SetActiveCardHand ();
		}
	}

	internal void OnDropPhomSuccess (string nick, int[] arrayPhom)
	{
		PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nick);
		if (pl != null) {
			bool isMe = nick.Equals (ClientConfig.UserInfo.UNAME);
			List<int[]> phommm = AutoChooseCardTaLa.GetPhomTrenTayMultiArray (arrayPhom);
			for (int i = 0; i < phommm.Count; i++) {
				pl.cardTaLaManager.HaBai (phommm [i], isMe, ListIdCardAn);
			}

			//if (isMe) {
			//    int[] p = AutoChooseCardTaLa.GetPhomTrenTayOneArray(pl.cardTaLaManager.GetCardIdCardHand());
			//    if (p.Length <= 0)
			//        SetActiveButton(false, false, true, false, false, false, false, true);
			//    else
			//        SetActiveButton(false, false, true, false, false, true, false, true);
			//}
		}
	}
	//Gui bai
	internal void OnAttachCard (string tenThangGuiBai, string guiDenThangNay, int[] phomgui, int[] card)
	{
		try {
			PhomPlayer plTuThangNay = (PhomPlayer)GetPlayerWithName (tenThangGuiBai);
			PhomPlayer plDenThangNay = (PhomPlayer)GetPlayerWithName (guiDenThangNay);
			if (plTuThangNay != null && plDenThangNay != null)
				plDenThangNay.cardTaLaManager.GuiBai (card, plTuThangNay, tenThangGuiBai.Equals (ClientConfig.UserInfo.UNAME));
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP)
	{
		base.InfoCardPlayerInTbl (message, turnName, time, numP);
		try {
			int numCardNoc = 0;
			for (int i = 0; i < numP; i++) {
				string nameP = message.reader ().ReadUTF ();
				PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nameP);
				if (pl != null) {
					pl.IsPlaying = (true);
					int[] temp = new int[9];
					for (int j = 0; j < temp.Length; j++) {
						temp [j] = 52;
					}
					pl.cardTaLaManager.SetChiaBai (temp, false);

					int sizeCardAn = message.reader ().ReadInt ();
					for (int j = 0; j < sizeCardAn; j++) {
						pl.cardTaLaManager.SetCardAn (message.reader ().ReadInt (), j);
					}
					int sizeRub = message.reader ().ReadInt ();
					int[] cardDanh = new int[sizeRub];
					for (int j = 0; j < sizeRub; j++) {
						cardDanh [j] = message.reader ().ReadInt ();
					}
					pl.cardTaLaManager.ArrayCardFire.SetActiveCardWithArrID (cardDanh);

					numCardNoc += sizeRub;

					int sizePhom = message.reader ().ReadByte ();
					if (sizePhom > 0) {
						int[] phom = new int[sizePhom];
						for (int j = 0; j < sizePhom; j++) {
							phom [j] = message.reader ().ReadByte ();
						}
						List<int[]> listP = AutoChooseCardTaLa.GetPhomTrenTayMultiArray(phom);
						for (int k = 0; k < listP.Count; k++) {
							pl.cardTaLaManager.HaBai(listP[k], false);
						}
					}
				}
			}
			SetNumCardLoc (numP * 4 - numCardNoc);

			GameControl.instance.TimerTurnInGame = time;
			//SetTurn(turnName, null);
			BasePlayer plTurn = GetPlayerWithName (turnName);
			if (plTurn != null) {
				plTurn.SetTurn (time);
			}
			if (turnName.Equals (ClientConfig.UserInfo.UNAME)) {
				SetActiveButton (false, false, true, true, false, false, false);//kiem tra an dc hay ko
			} else {
				SetActiveButton (false, false, false, false, false, false, false);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void AllCardFinish (string nick, int[] card)
	{
		base.AllCardFinish (nick, card);
		PhomPlayer pl = (PhomPlayer)GetPlayerWithName (nick);
		if (pl != null) {
			pl.cardTaLaManager.SetCardKhiHetGame (card);
		}
	}

	internal override void OnInfome (Message message)
	{
		base.OnInfome (message);
		try {
			bool bocbai = false, anbai = false, haphom = false, danhbai = false;
			GameControl.instance.TimerTurnInGame = 20;
			playerMe.IsPlaying = (true);
			int sizeCardHand = message.reader ().ReadByte ();
			int[] cardHand = new int[sizeCardHand];
			for (int i = 0; i < sizeCardHand; i++) {
				cardHand [i] = message.reader ().ReadByte ();
			}

			((PhomPlayer)playerMe).cardTaLaManager.ArrayCardHand.SetActiveCardWithArrID (cardHand);

			int sizeCardFire = message.reader ().ReadByte ();
			int[] cardFire = new int[sizeCardFire];
			for (int i = 0; i < sizeCardFire; i++) {
				cardFire [i] = message.reader ().ReadByte ();
			}
			((PhomPlayer)playerMe).cardTaLaManager.ArrayCardFire.SetActiveCardWithArrID (cardFire);

			int sizeCardPhom = message.reader ().ReadByte ();
			if (sizeCardPhom > 0) {
				int[] cardPhom = new int[sizeCardPhom];
				for (int i = 0; i < sizeCardFire; i++) {
					cardPhom [i] = message.reader ().ReadByte ();
				}
				List<int[]> listP = AutoChooseCardTaLa.GetPhomTrenTayMultiArray(cardPhom);
				for (int k = 0; k < listP.Count; k++) {
					((PhomPlayer)playerMe).cardTaLaManager.HaBai (listP[k], true, ListIdCardAn);	
				}
			}

			string turnName = message.reader ().ReadUTF ();
			int turnTime = message.reader ().ReadInt ();

			PhomPlayer plTurn = (PhomPlayer)GetPlayerWithName (turnName);
			if (plTurn != null) {
				plTurn.SetTurn (turnTime);
			}

			if (turnName.Equals (ClientConfig.UserInfo.UNAME)) {
				if (plTurn.cardTaLaManager.NumCardFire () >= 4) {
					haphom = true;
				} else {
					int[] cardMe = plTurn.cardTaLaManager.GetCardIdCardHand ();
					if (cardMe.Length < 10) {
						bocbai = true;
					} else {
						danhbai = true;
					}
					int[] cardPhom = AutoChooseCardTaLa.GetPhomAnDuoc (cardMe, cardDanhTruocDo, ListIdCardAn.ToArray ());

					if (cardPhom != null) {
						anbai = true;
						bocbai = true;
					}
				}
				//    SetActiveButton(false, false, true, sizeCardFire > 0);
				//} else {
				//    SetActiveButton(false, false, false, false);
			}

			SetActiveButton (false, false, danhbai, bocbai, anbai, haphom, false);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnStartForView (string[] playingName, Message msg)
	{
		base.OnStartForView (playingName, msg);
		SetActiveButton (false, false, false, false, false, false, false);
		for (int i = 0; i < ListPlayer.Count; i++) {
			PhomPlayer pl = (PhomPlayer)ListPlayer [i];
			if (pl.IsPlaying) {
				pl.cardTaLaManager.ArrayCardHand.SetCardWithId52 ();
			}
		}
	}

	internal void OnPhomha (Message message)
	{
		try {
			int len = message.reader ().ReadInt ();
			for (int i = 0; i < len; i++) {
				int len2 = message.reader ().ReadInt ();
				int[] phom = new int[len2];
				for (int j = 0; j < len2; j++) {
					phom [j] = message.reader ().ReadInt ();
				}
				List<int[]> listP = AutoChooseCardTaLa.GetPhomTrenTayMultiArray(phom);
				for (int k = 0; k < listP.Count; k++) {
					((PhomPlayer)playerMe).cardTaLaManager.HaBai (listP[k], true, ListIdCardAn);
				}
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}


	 string[] luatchoi = new string[] { "TÁI GỬI", "KHÔNG TÁI GỬI" };
	internal void OnRule (int luat){
		txt_luat_choi.text = luatchoi [luat];
	}
	internal override void OnStartFail ()
	{
		SetActiveButton (true, false, false, false, false, false, false);
	}

	internal override void OnJoinTableSuccess (Message message)
	{
		base.OnJoinTableSuccess (message);
		if (IsPlaying)
			SetActiveButton (false, false, false, false, false, false, false);
	}

	internal override void OnReady (string nick, bool ready)
	{
		base.OnReady (nick, ready);
		if (nick.Equals (ClientConfig.UserInfo.UNAME) && !playerMe.IsMaster) {
			if (ready)
				SetActiveButton (false, false, false, false, false, false, false);
			else
				SetActiveButton (false, true, false, false, false, false, false);
		}
	}

	internal override void OnJoinTablePlaySuccess (Message message)
	{
		base.OnJoinTablePlaySuccess (message);
		if (IsPlaying)
			SetActiveButton (false, false, false, false, false, false, false);
	}

	internal override void OnFireCardFail ()
	{
		base.OnFireCardFail ();
		SetActiveButton (false, false, true, false, false, false, false);
		PopupAndLoadingScript.instance.toast.showToast (ClientConfig.Language.GetText ("popup_danh_bai_loi"));
	}

	public void SetNumCardLoc (int NumCard)
	{
		if (NumCard <= 0) {
			objBoc.SetActive (false);
			txt_num_card_boc.transform.parent.gameObject.SetActive (false);
		} else {
			txt_num_card_boc.transform.parent.gameObject.SetActive (true);
			totalCardNoc = NumCard;
			txt_num_card_boc.text = "" + NumCard;
		}
	}


	#region IHasChanged

	[SerializeField]
	Card card_drag_drop;
	//tha
	public void HasDrop ()
	{
	}

	public void HasDrop (Card idDrag, Card idDrop)
	{
		card_drag_drop.SetVisible (false);
		List<Card> list = ((PhomPlayer)playerMe).cardTaLaManager.ArrayCardHand.listCardHand;
		int count = list.Count (c => c.isBatHayChua);
		int indexDrag = list.FindIndex (c => c.ID == idDrag.ID);
		int indexDrop = list.FindIndex (c => c.ID == idDrop.ID);

		Card temp = new Card ();
		temp = idDrag;
		if (indexDrag < indexDrop) {
			if (indexDrag >= count - 1) {
				list.Remove (idDrag);
				list.Add (temp);
			} else {
				list.Insert (indexDrop + 1, temp);
				list.Remove (idDrag);
			}
		} else {
			list.Remove (idDrag);
			list.Insert (indexDrop, temp);
		}
		((PhomPlayer)playerMe).cardTaLaManager.ArrayCardHand.SortCardActive (true, 0.2f);
	}

	//keo
	public void HasBeginDrag (int id)
	{
		card_drag_drop.SetVisible (true);
		card_drag_drop.SetCardWithId (id);
	}
	//dang keo
	public void HasDrag (Vector3 vtPos)
	{
		//Debug.LogError(vtPos);
		card_drag_drop.transform.position = vtPos;
	}
	//ko keo nua
	public void HasEndDrag ()
	{
		card_drag_drop.SetVisible (false);
	}

	#endregion

	public void Demo ()
	{
		int[] ccccH = new int[] { 1, 2, 4, 6, 7, 8, 33, 44, 34 };
		int[] ccccF = new int[] { 11, 12, 13, 14 };
		int[] ccccP1 = new int[] { 1, 2, 4};
		int[] ccccP2 = new int[] { 6, 7, 8 };
		int[] ccccP3 = new int[] { 33, 44, 34 };
		int[] ccccPAn = new int[] { 23, 02, 17 };

		((PhomPlayer)playerMe).cardTaLaManager.SetChiaBai (ccccH, true);
		((PhomPlayer)playerMe).cardTaLaManager.ArrayCardFire.SetActiveCardWithArrID (ccccF);
		((PhomPlayer)playerMe).cardTaLaManager.SetCardPhom (ccccP1, ccccP2, ccccP3, ccccPAn, true);

		for (int i = 1; i < ListPlayer.Count; i++) {
			PhomPlayer pl = (PhomPlayer)ListPlayer [i];
			if (pl != null) {
				pl.cardTaLaManager.SetChiaBai (ccccH, false, () => {
					pl.cardTaLaManager.ArrayCardFire.SetActiveCardWithArrID (ccccF);
					pl.cardTaLaManager.SetCardPhom (ccccP1, ccccP2, ccccP3, ccccPAn, false);
				});
			}
		}
	}
}
