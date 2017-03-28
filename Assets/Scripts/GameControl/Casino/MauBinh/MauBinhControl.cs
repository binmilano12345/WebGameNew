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
	TimeCountDown time;

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
	}

	public void OnClickXepBai ()
	{
		((MauBinhPlayer)playerMe).cardMauBinh.XepBai ();
	}

	public void OnClickXepLai ()
	{
		((MauBinhPlayer)playerMe).cardMauBinh.SetXepLai (true);
	}

	#endregion

	internal override void OnJoinTableSuccess (string master)
	{
		base.OnJoinTableSuccess (master);
		if (master.Equals (ClientConfig.UserInfo.UNAME)) {
			SetActiveButton (true, false, false, false);
		} else {
			SetActiveButton (false, true, false, false);
		}
	}

	internal override void StartTableOk (int[] cardHand, Message msg, string[] nickPlay)
	{
		base.StartTableOk (cardHand, msg, nickPlay);
		for (int i = 0; i < nickPlay.Length; i++) {
			MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (nickPlay [i]);
			if (pl != null) {
				if (pl.SitOnClient == 0) {
					pl.cardMauBinh.SetCard (cardHand, true, () => {
						SetActiveButton (false, false, false, false);
						time.SetTime (60);
					});
				} else {
					pl.cardMauBinh.SetCard (cardHand, false);
				}
			}
		}
		time.SetTime (60);
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

	void OnLung(string namePlayer, long moneyEarn){
		MauBinhPlayer pl = (MauBinhPlayer)GetPlayerWithName (namePlayer);
		if (pl != null) {
			if (moneyEarn < 0) {
				pl.SetLung (moneyEarn);
			}
			pl.SetEffect (MoneyHelper.FormatMoneyNormal (moneyEarn));
		}
	}

	internal void OnRankMauBinh(Message message){
	}
	internal override void OnStartFail ()
	{
		SetActiveButton (true, false, false, false);
	}

	internal override void OnNickSkip (string nick, string turnname)
	{
		base.OnNickSkip (nick, turnname);
		SetTurn (turnname, null);
		if (nick.Equals (ClientConfig.UserInfo.UNAME)) {
			((TLMNPlayer)playerMe).CardHand.ResetCard ();
		}
	}

	internal override void OnFinishTurn ()
	{
		base.OnFinishTurn ();
	}

	internal override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP)
	{
		base.InfoCardPlayerInTbl (message, turnName, time, numP);
		try {
			for (int i = 0; i < numP; i++) {
				string name = message.reader ().ReadUTF ();
				sbyte numCard = message.reader ().ReadByte ();
				TLMNPlayer pl = (TLMNPlayer)GetPlayerWithName (name);
				if (pl != null) {
					pl.IsPlaying = (true);
					int[] temp = new int[numCard];
					for (int j = 0; j < temp.Length; j++) {
						temp [j] = 52;
					}
					pl.CardHand.SetCardWithId53 ();
					pl.CardHand.SetActiveCardHand (true);
					pl.SetNumCard (numCard);
				}
			}
			GameControl.instance.TimerTurnInGame = time;
			BasePlayer plTurn = GetPlayerWithName (turnName);
			if (plTurn != null) {
				plTurn.SetTurn (time);
			}
			if (turnName.Equals (ClientConfig.UserInfo.UNAME)) {
				SetActiveButton (false, false, true, true);
			} else {
				SetActiveButton (false, false, false, false);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnInfome (Message message)
	{
		base.OnInfome (message);
		try {
			GameControl.instance.TimerTurnInGame = 20;
			playerMe.IsPlaying = (true);
			int sizeCardHand = message.reader ().ReadByte ();
			int[] cardHand = new int[sizeCardHand];
			for (int i = 0; i < sizeCardHand; i++) {
				cardHand [i] = message.reader ().ReadByte ();
			}
			((TLMNPlayer)playerMe).CardHand.SetCardWithArrID (cardHand);
			((TLMNPlayer)playerMe).CardHand.SetActiveCardHand (true);

			int sizeCardFire = message.reader ().ReadByte ();
			if (sizeCardFire > 0) {
				int[] cardFire = new int[sizeCardFire];
				for (int i = 0; i < sizeCardFire; i++) {
					cardFire [i] = message.reader ().ReadByte ();
				}
			}
			string turnName = message.reader ().ReadUTF ();
			int turnTime = message.reader ().ReadInt ();
			BasePlayer plTurn = GetPlayerWithName (turnName);
			if (plTurn != null) {
				plTurn.SetTurn (turnTime);
			}

			if (turnName.Equals (ClientConfig.UserInfo.UNAME)) {
				SetActiveButton (false, false, true, sizeCardFire > 0);
			} else {
				SetActiveButton (false, false, false, false);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal override void OnJoinTableSuccess (Message message)
	{
		base.OnJoinTableSuccess (message);
		if (isPlaying)
			SetActiveButton (false, false, false, false);
	}

	internal override void OnReady (string nick, bool ready)
	{
		base.OnReady (nick, ready);
		if (nick.Equals (ClientConfig.UserInfo.UNAME) && !playerMe.IsMaster) {
			if (ready)
				SetActiveButton (false, false, false, false);
			else
				SetActiveButton (false, true, false, false);
		}
	}

	internal override void OnJoinTablePlaySuccess (Message message)
	{
		base.OnJoinTablePlaySuccess (message);
		if (isPlaying)
			SetActiveButton (false, false, false, false);
	}

	internal override void OnFireCardFail ()
	{
		base.OnFireCardFail ();
		SetActiveButton (false, false, true, true);
		PopupAndLoadingScript.instance.toast.showToast (ClientConfig.Language.GetText ("popup_danh_bai_loi"));
	}

	internal override void SetMaster (string nick)
	{
		base.SetMaster (nick);
		if (nick.Equals (ClientConfig.UserInfo.UNAME)) {
			SetActiveButton (true, false, false, false);
		} else {
			SetActiveButton (false, true, false, false);
		}
	}

	internal override void OnStartForView (string[] playingName, Message msg)
	{
		base.OnStartForView (playingName, msg);
		SetActiveButton (false, false, false, false);
		for (int i = 0; i < ListPlayer.Count; i++) {
			if (ListPlayer [i].IsPlaying) {
				((TLMNPlayer)ListPlayer [i]).CardHand.SetActiveCardHand (true);
			}
		}
	}

	internal override void OnFinishGame (Message message)
	{
		base.OnFinishGame (message);
		if (playerMe.IsMaster)
			SetActiveButton (true, false, false, false);
		else
			SetActiveButton (false, true, false, false);
	}

	internal override void AllCardFinish (string nick, int[] card)
	{
		base.AllCardFinish (nick, card);
		TLMNPlayer pl = (TLMNPlayer)GetPlayerWithName (nick);
		if (pl != null) {
			pl.CardHand.SetCardKhiKetThucGame (AutoChooseCard.SortArrCard (card));
		}
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
			//txt_typecards[1].text = GameConfig.STR_THANG_TRANG[thangtrang];
			return;
		}

		int type1 = (int)TypeCardMauBinh.GetTypeCardMauBinh (chi1);
		int type2 = (int)TypeCardMauBinh.GetTypeCardMauBinh (chi2);
		int type3 = (int)TypeCardMauBinh.GetTypeCardMauBinh (chi3);

		SetLoaiBaiCuaChi (0, type1);
		SetLoaiBaiCuaChi (1, type2);
		SetLoaiBaiCuaChi (2, type3);

		if (TypeCardMauBinh.IsLung (arr)) {
			for (int i = 0; i < txt_typecards.Length; i++) {
				txt_typecards [i].gameObject.SetActive (false);
			}
			txt_typecards [1].gameObject.SetActive (true);
			txt_typecards [1].text = "Binh Lủng";
		}
	}

	public void SetScoreMauBinh (int score, int bonus, int chi, bool isHide = false)
	{
		if (isHide) {
			for (int i = 0; i < txt_typecards.Length; i++) {
				txt_typecards [i].gameObject.SetActive (false);
			}
		} else {
			txt_typecards [chi].color = Color.white;
			txt_typecards [chi].gameObject.SetActive (true);
			txt_typecards [chi].text = (score > 0 ? "<color=yellow>+" + score + "</color>" : "" + score) + (bonus > 0 ? " <color=yellow>(+" + bonus + ")</color>" : "");
			txt_typecards [chi].transform.DOScale (1.2f, 0.2f).OnComplete (delegate {
				txt_typecards [chi].transform.DOScale (1f, 0.1f);
			});
		}
	}

	void SetLoaiBaiCuaChi (int chi, int type)
	{
		txt_typecards [chi].gameObject.SetActive (true);
		//txt_typecards[chi].text = (chi + 1) + ". " + GameConfig.STR_TYPE_CARD[type];

	}
	//tha
	public void HasDrop ()
	{
		if (playerMe != null) {
			//SetLoaiBai(playerMe.cardMauBinh.GetArrCardID());
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
}
