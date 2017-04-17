using AppConfig;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class MauBinhControl : BaseCasino, IHasChanged
{
	public static MauBinhControl instace;
	[SerializeField]
	GameObject objSoBai, objDoiChi, objXepLai, objXepBai;
	[SerializeField]
	TimeCountDown timeCountDown;

	void Awake ()
	{
		instace = this;
	}
	// Use this for initialization
	public new void Start ()
	{
		base.Start ();
	}

	internal void SetActiveButton (bool isSoBai, bool isDoiChi, bool isXepLai, bool isXepBai)
	{
		objSoBai.SetActive (isSoBai);
		objDoiChi.SetActive (isDoiChi);
		objXepLai.SetActive (isXepLai);
		objXepBai.SetActive (isXepBai);
	}

	#region Click

	public void OnClickSoBai ()
	{
//        ((MauBinhPlayer)playerMe).cardMauBinh.SetSoBai(true);
		int[] cardFinal = ((MauBinhPlayer)playerMe).cardMauBinh.GetArrCardID ();
		SendData.onFinalMauBinh (cardFinal);
	}

	public void OnClickDoiChi ()
	{
		((MauBinhPlayer)playerMe).cardMauBinh.DoiChi ();

		int[] cardFinal = ((MauBinhPlayer)playerMe).cardMauBinh.GetArrCardID ();
		SetLoaiBai (cardFinal);
	}

	public void OnClickXepBai ()
	{
		((MauBinhPlayer)playerMe).cardMauBinh.XepBai ();
		int[] cardFinal = ((MauBinhPlayer)playerMe).cardMauBinh.GetArrCardID ();
		SetLoaiBai (cardFinal);
	}

	public void OnClickXepLai ()
	{
		((MauBinhPlayer)playerMe).cardMauBinh.SetXepLai (true);
		SetActiveButton (true, true, false, true);
	}

	#endregion

	internal override void OnJoinTableSuccess (string master)
	{
		base.OnJoinTableSuccess (master);
//		if (master.Equals (ClientConfig.UserInfo.UNAME)) {
		SetActiveButton (false, false, false, false);
//		} else {
//			SetActiveButton (false, true, false, false);
//		}
	}

	internal override void StartTableOk (int[] cardHand, Message msg, string[] nickPlay)
	{
		base.StartTableOk (cardHand, msg, nickPlay);
		int	turntimeMB = 60;
		try {
			turntimeMB = msg.reader ().ReadByte ();
		} catch (Exception e) {
			Debug.LogException (e);
		}
		for (int i = 0; i < nickPlay.Length; i++) {
			MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nickPlay [i]);
			if (pl != null) {
				pl.SetDisableAction ();
				if (pl.SitOnClient == 0) {
					pl.cardMauBinh.SetCard (cardHand, true, () => {
						SetActiveButton (true, true, false, true);
						SetLoaiBai (cardHand);
					});
				} else {
					pl.cardMauBinh.SetCard (cardHand, false);
				}
			}
		}
		timeCountDown.SetTime (turntimeMB);
		SetLoaiBai (cardHand);
	}

	void OnSapBaChi (string namePlayer, long moneyEarn)
	{
		MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (namePlayer);
		if (pl != null) {
			if (moneyEarn < 0) {
				pl.SetSap3Chi (moneyEarn);
			}
			pl.SetEffect (MoneyHelper.FormatMoneyNormal (moneyEarn));
		}
	}

	void OnLung (string namePlayer, long moneyEarn)
	{
		MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (namePlayer);
		if (pl != null) {
			if (moneyEarn < 0) {
				pl.SetLung (moneyEarn);
			}
			pl.SetEffect (MoneyHelper.FormatMoneyNormal (moneyEarn));
		}
	}

	internal void OnRankMauBinh (Message message)
	{
		SetActiveButton (false, false, false, false);
		timeCountDown.SetTime (0);

		int chi = message.reader ().ReadByte ();
		int size, i;
		if (chi != 4 && chi != 5) {
			size = message.reader ().ReadByte ();
			for (i = 0; i < size; i++) {
				string namePlayer = message.reader ().ReadUTF ();
				int typeCard = message.reader ().ReadByte ();
				long moneyEarn = message.reader ().ReadLong ();
				int size2 = message.reader ().ReadByte ();
				int[] cardChi = new int[size2];
				for (int k = 0; k < size2; k++) {
					cardChi [k] = message.reader ().ReadByte ();
				}
				MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (namePlayer);
				if (pl != null) {
					int iCard = 0;
					if (chi == 1) {
						iCard = 2;
					} else if (chi == 2) {
						iCard = 1;
					} else if (chi == 3) {
						iCard = 0;
					}
					pl.cardMauBinh.ShowChi (cardChi, iCard, typeCard, pl.SitOnClient == 0);
					pl.SetEffect (moneyEarn + "");
				}
//				onRankMauBinh(chi, namePlayer, typeCard, moneyEarn, cardChi);
			}
		} else if (chi == 4) {
			size = message.reader ().ReadByte ();
			for (i = 0; i < size; i++) {
				string namePlayer = message.reader ().ReadUTF ();
				long moneyEarn = message.reader ().ReadLong ();
				OnSapBaChi (namePlayer, moneyEarn);
			}
		} else if (chi == 5) {
			size = message.reader ().ReadByte ();
			for (i = 0; i < size; i++) {
				string namePlayer = message.reader ().ReadUTF ();
				long moneyEarn = message.reader ().ReadLong ();
				OnLung (namePlayer, moneyEarn);
			}
		}
	}

	internal override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP)
	{
		base.InfoCardPlayerInTbl (message, turnName, time, numP);
		try {
			for (int i = 0; i < numP; i++) {
				string nameP = message.reader ().ReadUTF ();
				bool isDangXep = message.reader ().ReadBoolean ();
				Debug.LogError ("Ten " + nameP);

				MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nameP);
				if (pl != null) {
					pl.IsPlaying = true;
					pl.SetXepXong (!isDangXep);
					int[] temp = new int[13];
					for (int j = 0; j < temp.Length; j++) {
						temp [j] = 52;
					}

					pl.cardMauBinh.SetCard (temp, false);
				}
			}
			GameControl.instance.TimerTurnInGame = time;
			timeCountDown.SetTime (time);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnInfome (Message message)
	{
		base.OnInfome (message);
		try {
			playerMe.IsPlaying = (true);
			bool isDangXep = message.reader ().ReadBoolean ();
			sbyte len = message.reader ().ReadByte ();
			sbyte[] c = new sbyte[len];
			for (int i = 0; i < len; i++) {
				c [i] = message.reader ().ReadByte ();
			}

			int[] cardHand = new int[c.Length];
			for (int i = 0; i < c.Length; i++) {
				cardHand [i] = c [i];
			}

//			((MauBinhPlayer)playerMe).SetXepXong(!isDangXep);

			((MauBinhPlayer)playerMe).cardMauBinh.SetCard (cardHand, true);
			SetActiveButton (true, true, false, true);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnFinishGame (Message message)
	{
		SetActiveButton (false, false, false, false);
		for (int i = 0; i < ListPlayer.Count; i++) {
			MauBinhPlayer pl = (MauBinhPlayer)ListPlayer [i];
			if (pl != null) {
				pl.SetDisableAction ();
				pl.cardMauBinh.SetActiveCard ();
			}
		}
	}

	internal override void OnJoinTableSuccess (Message message)
	{
		base.OnJoinTableSuccess (message);
		if (isPlaying)
			SetActiveButton (false, false, false, false);
	}

	internal override void OnJoinTablePlaySuccess (Message message)
	{
		base.OnJoinTablePlaySuccess (message);
		if (isPlaying)
			SetActiveButton (false, false, false, false);
	}

	internal override void OnStartForView (string[] playingName, Message msg)
	{
		base.OnStartForView (playingName, msg);
		SetActiveButton (false, false, false, false);
		int[] cardHand = new int[13];
		for (int i = 0; i < cardHand.Length; i++) {
			cardHand [i] = 52;
		}
		for (int i = 0; i < ListPlayer.Count; i++) {
			if (ListPlayer [i].IsPlaying) {
				MauBinhPlayer pl = (MauBinhPlayer)ListPlayer [i];
				if (pl != null) {
					pl.cardMauBinh.SetCard (cardHand, false);
				}
			}
		}

		timeCountDown.SetTime (60);
	}

	internal override void AllCardFinish (string nick, int[] card)
	{
		base.AllCardFinish (nick, card);
		try {
			Debug.LogError(nick + "   " + card.Length);
			if (card.Length > 0) {
				int[] cardChi1 = new int[] { card [0], card [1], card [2], card [3], card [4] };
				int[] cardChi2 = new int[] { card [5], card [6], card [7], card [8], card [9] };
				int[] cardChi3 = new int[] { card [10], card [11], card [12] };
				MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nick);
				if (pl != null) {
					pl.cardMauBinh.SetCardKetThuc (true, cardChi1, cardChi2, cardChi3, pl.SitOnClient == 0);
				}
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
		SetActiveButton (false, false, false, false);
	}

	internal override void OnTimeAuToStart (int time)
	{
		base.OnTimeAuToStart (time);
		timeCountDown.SetTime (time);
	}

	public void OnFinalMauBinh (Message message)
	{
		string nameP = message.reader ().ReadUTF ();
		MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nameP);
		if (pl != null) {
			pl.cardMauBinh.SetSoBai (pl.SitOnClient == 0);
			pl.SetXepXong (true);
		}
		if (nameP.Equals (ClientConfig.UserInfo.UNAME)) {
			SetActiveButton (false, false, true, false);
		}
	}

	internal void OnWinMauBinh (Message message)
	{
		string nameP = message.reader ().ReadUTF ();
		int typeC = message.reader ().ReadByte ();

		Debug.LogError (nameP + " -=Thang trang=- " + typeC);
		MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nameP);
		if (pl != null) {
			pl.SetMauBinh (typeC);
		}
		SetActiveButton (false, false, false, false);
	}

	#region IHasChanged

	void SetLoaiBai (int[] arr)
	{
		int[] chi1 = new int[5];
		int[] chi2 = new int[5];
		int[] chi3 = new int[3];
		for (int i = 0; i < arr.Length; i++) {
			if (i < 5) {
				chi1 [i] = arr [i];
			} else if (i < 10) {
				chi2 [i - 5] = arr [i];
			} else {
				chi3 [i - 10] = arr [i];
			}
		}


		int thangtrang = TypeCardMauBinh.IsThangTrang (arr);
		if (thangtrang != -1) {
			for (int i = 0; i < txt_typecards.Length; i++) {
				txt_typecards [i].gameObject.SetActive (false);
			}
			txt_typecards [1].gameObject.SetActive (true);
			txt_typecards [1].text = GameConfig.STR_THANG_TRANG [thangtrang];
			return;
		}

		TYPE_CARD type1 = TypeCardMauBinh.GetTypeCardMauBinh (chi1);
		TYPE_CARD type2 = TypeCardMauBinh.GetTypeCardMauBinh (chi2);
		TYPE_CARD type3 = TypeCardMauBinh.GetTypeCardMauBinh (chi3);

		string str = "";
		for (int i = 0; i < chi1.Length; i++) {
			str += TypeCardMauBinh.GetValue (chi1 [i]);
		}
		Debug.LogError (type1 + "\n" + str);
		str = "";
		for (int i = 0; i < chi2.Length; i++) {
			str += TypeCardMauBinh.GetValue (chi2 [i]);
		}
		Debug.LogError (type2 + "\n" + str);

		str = "";
		for (int i = 0; i < chi3.Length; i++) {
			str += TypeCardMauBinh.GetValue (chi3 [i]);
		}
		Debug.LogError (type3 + "\n" + str);

		SetLoaiBaiCuaChi (2, (int)type1);
		SetLoaiBaiCuaChi (1, (int)type2);
		SetLoaiBaiCuaChi (0, (int)type3);
	
		if (TypeCardMauBinh.IsLung (chi1, chi2, chi3)) {
			for (int i = 0; i < txt_typecards.Length; i++) {
				txt_typecards [i].gameObject.SetActive (false);
			}
			txt_typecards [1].gameObject.SetActive (true);
			txt_typecards [1].text = "Binh Lủng";
		}
	}

	//	public void SetScoreMauBinh (int score, int bonus, int chi, bool isHide = false)
	//	{
	//		if (isHide) {
	//			for (int i = 0; i < txt_typecards.Length; i++) {
	//				txt_typecards [i].gameObject.SetActive (false);
	//			}
	//		} else {
	//			txt_typecards [chi].color = Color.white;
	//			txt_typecards [chi].gameObject.SetActive (true);
	//			txt_typecards [chi].text = (score > 0 ? "<color=yellow>+" + score + "</color>" : "" + score) + (bonus > 0 ? " <color=yellow>(+" + bonus + ")</color>" : "");
	//			txt_typecards [chi].transform.DOScale (1.2f, 0.2f).OnComplete (delegate {
	//				txt_typecards [chi].transform.DOScale (1f, 0.1f);
	//			});
	//		}
	//	}
	//
	void SetLoaiBaiCuaChi (int chi, int type)
	{
		txt_typecards [chi].gameObject.SetActive (true);
		txt_typecards [chi].text = (chi + 1) + ". " + GameConfig.STR_TYPE_CARD [type];

	}
	//tha
	public void HasDrop ()
	{
		if (playerMe != null) {
			SetLoaiBai (((MauBinhPlayer)playerMe).cardMauBinh.GetArrCardID ());
		}

		card_show_mb.SetVisible (false);
	}

	public void HasDrop (Card idDrag, Card idDrop)
	{
	}

	[SerializeField]
	Card card_show_mb;
	[SerializeField]
	Text[] txt_typecards;
	//keo
	public void HasBeginDrag (int id)
	{
		card_show_mb.SetVisible (true);
		card_show_mb.SetCardWithId (id);
	}
	//dang keo
	public void HasDrag (Vector3 vtPos)
	{
		//Debug.LogError(vtPos);
		card_show_mb.transform.position = vtPos;
	}
	//ko keo nua
	public void HasEndDrag ()
	{
		card_show_mb.SetVisible (false);
	}

	#endregion

	public void Demo ()
	{
		int[] arrr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
		((MauBinhPlayer)playerMe).cardMauBinh.SetCard (arrr, true);
		for (int i = 1; i < ListPlayer.Count; i++) {
			MauBinhPlayer pl = (MauBinhPlayer)ListPlayer [i];
			if (pl != null) {
				pl.cardMauBinh.SetCard (arrr, false);
			}
		}
	}
}
