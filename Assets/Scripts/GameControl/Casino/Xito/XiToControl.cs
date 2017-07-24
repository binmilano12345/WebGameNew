using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Us.Mobile.Utilites;
using AppConfig;
using Beebyte.Obfuscator;

public class XiToControl : BaseCasino {
	public static XiToControl instance;
	[SerializeField]
	UIButton btn_bo, btn_xembai, btn_theo, btn_to;
	[SerializeField]
	UIButton btn_ruttien;
	[SerializeField]
	Text txt_bo, txt_xem, txt_theo, txt_to;

	[SerializeField]
	GameObject txt_chon_bai;

	long MoneyCuoc = 0;
	long MinToMoney = 0, MaxToMoney = 0;
	long tongMoney = 0;
	LiengPlayer plMe;

	[SerializeField]
	ChipControl SumChipControl;
	[SerializeField]
	GroupTo groupMoneyTo;
	[SerializeField]
	TimeCountDown timeCountDown;

	[SerializeField]
	Transform dealerPos;
	void Awake() {
		instance = this;
		SumChipControl.IsChipSum = true;
	}

	void SetActiveButton(bool is_bo = true, bool is_xem = true, bool is_theo = true, bool is_to = true) {
		btn_bo.gameObject.SetActive(is_bo);
		btn_xembai.gameObject.SetActive(is_xem);
		btn_theo.gameObject.SetActive(is_theo);
		btn_to.gameObject.SetActive(is_to);
	}

	void SetEnableButton(bool is_bo = true, bool is_xem = true, bool is_theo = true, bool is_to = true) {
		btn_bo.enabled = is_bo;
		btn_xembai.enabled = is_xem;
		btn_theo.enabled = is_theo;
		btn_to.enabled = is_to;

		txt_bo.color = is_bo ? Color.white : Color.gray;
		txt_xem.color = is_xem ? Color.white : Color.gray;
		txt_theo.color = is_theo ? Color.white : Color.gray;
		txt_to.color = is_to ? Color.white : Color.gray;
	}

	internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
		base.StartTableOk(cardHand, msg, nickPlay);
		for (int i = 0; i < ListPlayer.Count; i++) {
			((LiengPlayer)ListPlayer[i]).MoneyChip = 0;
		}
		groupMoneyTo.OnHide();
		MinToMoney = GameConfig.BetMoney * 2;
		MaxToMoney = ClientConfig.UserInfo.CASH_FREE - MinToMoney;

		SumChipControl.MoneyChip = 0;
		tongMoney = 0;

		//Debug.LogError("Card Hand   " + cardHand.Length);
		int[] ccc = new int[2];
		int ii = 0;
		for (int i = 0; i < cardHand.Length; i++) {
			//Debug.LogError("Card Hand  i " + cardHand[i]);
			if (cardHand[i] >= 0 && cardHand[i] < 52) {
				ccc[ii] = cardHand[i];
				ii++;
			}
		}
		list_my_card.Clear();
		list_my_card.AddRange(ccc);
		for (int i = 0; i < ListPlayer.Count; i++) {
			LiengPlayer player = (LiengPlayer)ListPlayer[i];
			if (player != null) {
				player.MoneyChip = GameConfig.BetMoney;
				player.MoveChip(GameConfig.BetMoney, SumChipControl.transform.position);
				if (player.SitOnClient == 0) {
					player.CardHand.SetAllDark(true);
					//player.SetTypeCard(PokerCard.getTypeOfCardsPoker(list_my_card.ToArray()));
					//player.SetTypeCard(
					TYPE_CARD type = TypeCardMauBinh.GetTypeCardMauBinh(list_my_card.ToArray());
					int type2 = PokerCard.getTypeOfCardsPoker(list_my_card.ToArray());

					//Debug.LogError("11111111111Type card:  " + type);
					//Debug.LogError("22222222222Type card:  " + type2);

					player.CardHand.ChiaBaiTienLen(ccc, true);
					player.CardHand.SetTouchCardHand(true);
				} else
					player.CardHand.ChiaBaiTienLen(new int[] { 52, 52 }, true);
			}
			tongMoney += GameConfig.BetMoney;
		}
		SumChipControl.OnShow();
		SumChipControl.MoneyChip = tongMoney;
	}

	internal override void OnStartForView(string[] nickPlay, Message msg) {
		base.OnStartForView(nickPlay, msg);
		tongMoney = 0;
		for (int i = 0; i < nickPlay.Length; i++) {
			LiengPlayer player = (LiengPlayer)GetPlayerWithName(nickPlay[i]);
			if (player != null) {
				player.MoneyChip = GameConfig.BetMoney;
				tongMoney += GameConfig.BetMoney;
				if (player.SitOnClient != 0)
					player.CardHand.ChiaBaiTienLen(new int[] { 52, 52 }, true);
			}
		}

		SumChipControl.OnShow();
		SumChipControl.MoneyChip = tongMoney;
		SetActiveButton(false, false, false, false);
	}

	internal override void OnJoinView(Message message) {
		// TODO Auto-generated method stub
		base.OnJoinView(message);
		plMe = (LiengPlayer)playerMe;

		SetActiveButton(false, false, false, false);
	}
	internal override void OnJoinTableSuccess(string master) {
		plMe = (LiengPlayer)playerMe;
		SetActiveButton(false, false, false, false);
	}

	internal override void SetTurn(string nick, Message message) {
		base.SetTurn(nick, message);
		try {
			MoneyCuoc = message.reader().ReadLong();
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
                baseSetTurn();
			} else {
				hideThanhTo();
				SetActiveButton(false, false, false, false);
			}
		} catch (Exception e) {
			// TODO: handle exception
			Debug.LogException(e);
		}
	}

	private void hideThanhTo() {
		groupMoneyTo.OnHide();
	}

	internal override void OnFinishGame(Message message) {
		try {
			IsPlaying = false;
			isStart = false;

			int total = message.reader().ReadByte();
			for (int i = 0; i < total; i++) {
				string nick = message.reader().ReadUTF();
				int rank = message.reader().ReadByte();
				long mn = message.reader().ReadLong();
				LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
				if (pl != null) {
					pl.SetRank(rank);
					pl.IsReady = false;
					pl.SetShowReady(false);
					pl.MoneyChip = 0;
					pl.SetDiemLieng(true, pl.CardHand.GetArrayIDCard());
				}
			}
			SetActiveButton(false, false, false, false);
			OnJoinTableSuccess(masterID);
			for (int j = 0; j < ListPlayer.Count; j++) {
				ListPlayer[j].SetShowReady(false);
				ListPlayer[j].SetTurn(0, false);
			}

			tongMoney = 0;
			SumChipControl.MoneyChip = tongMoney;
		} catch (Exception ex) {
			Debug.LogException(ex);

		}
	}

	internal override void OnNickSkip(string nick, Message msg) {
		try {
			string nick_turn = msg.reader().ReadUTF();
			LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
			if (pl != null) {
				pl.SetEffect("Bỏ");
				pl.SetTurn(0, false);
				pl.SetRank(4);
				pl.CardHand.SetAllDark(true);
				pl.IsPlaying = false;
			}

			pl.MoveChip(pl.MoneyChip, SumChipControl.transform.position);
			pl.MoneyChip = 0;

			SetTurn(nick_turn, msg);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton();
			}else if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton();
				baseSetTurn();
			} else {
				hideThanhTo();
				SetActiveButton(false, false, false, false);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal override void OnNickCuoc(Message message) {
		try {
			long moneyInPot = message.reader().ReadLong();
			MoneyCuoc = message.reader().ReadLong();
			long moneyBoRa = message.reader().ReadLong();
			Debug.LogError("So tien to: " + MoneyCuoc);
			Debug.LogError("So tien bo ra: " + moneyBoRa);
			string nick = message.reader().ReadUTF();
			string nick_turn = message.reader().ReadUTF();
			if (GameConfig.BetMoney * 2 >= MoneyCuoc) {
				MinToMoney = GameConfig.BetMoney * 2;
			} else {
				MinToMoney = MoneyCuoc;
			}
			LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
			if (pl != null) {
				pl.SetEffect("Tố");
				pl.MoveChip(moneyBoRa + pl.MoneyChip, SumChipControl.transform.position);
				pl.MoneyChip += moneyBoRa;
				//gameControl.sound.startToAudio();
				tongMoney += MoneyCuoc;
				SumChipControl.MoneyChip = tongMoney;
			}

			SetTurn(nick_turn, message);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				hideThanhTo();

				SetActiveButton(false, false, false, false);
			}else if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
                SetActiveButton();
				baseSetTurn();
			} else {
				hideThanhTo();
				SetActiveButton(false, false, false, false);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal override void OnNickTheo(Message message) {
		try {
			MoneyCuoc = message.reader().ReadLong();
			string nick = message.reader().ReadUTF();
			string nick_turn = message.reader().ReadUTF();

			LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
			if (MoneyCuoc == 0) {
				pl.SetEffect("Xem");
			} else {
				pl.SetEffect("Theo");

				pl.MoveChip(MoneyCuoc + pl.MoneyChip, SumChipControl.transform.position);
				pl.MoneyChip += MoneyCuoc;
				tongMoney += MoneyCuoc;
				SumChipControl.MoneyChip = tongMoney;
			}
			SetTurn(nick_turn, message);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton(false, false, false, false);
			}else if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton();
                baseSetTurn();
			} else {
				hideThanhTo();
				SetActiveButton(false, false, false, false);
			}
		} catch (Exception e) {
			// TODO: handle exception
			Debug.LogException(e);
		}
	}


void baseSetTurn() {
	SetActiveButton();
	if (MoneyCuoc <= 0) {
		SetEnableButton(true, true, false, true);
	} else if (MoneyCuoc < plMe.MoneyFollow) {
		SetEnableButton(true, false, true, false);
		txt_theo.text = "Theo " + MoneyHelper.FormatMoneyNormal(MoneyCuoc);
	} else {
		SetEnableButton(true, false, true, false);
		txt_theo.text = "Theo " + MoneyHelper.FormatMoneyNormal(plMe.MoneyFollow);
	}
}

public void OnTo(long moneyTo) {
	SendData.onCuocXT(-99, moneyTo);
}

	#region CLICK
	[SkipRename]
	public void clickButtonBo() {
		SendData.onSendSkipTurn();
	}
[SkipRename]
	public void clickButtonXem() {
		SendData.onAccepFollow();
	}
[SkipRename]
	public void clickButtonTheo() {
		SendData.onAccepFollow();
	}

[SkipRename]
	public void clickButtonTo() {
		groupMoneyTo.OnShow(MinToMoney, MaxToMoney);
	}

[SkipRename]
	public void clickButtnRutTien() {
		if (ClientConfig.UserInfo.CASH_FREE < GameConfig.BetMoney * 10) {
			PopupAndLoadingScript.instance.messageSytem.OnShow("Không đủ tiền để rút, bạn có muốn nạp thêm?");

		} else {
			//Show rut tien
			Debug.LogError("Show rut tien");
			LoadAssetBundle.LoadScene(SceneName.SUB_RUT_TIEN, SceneName.SUB_RUT_TIEN, () => {
				//if (players[0].getFolowMoney() < BaseInfo.gI().currentMinMoney) {
				//	PanelRutTien.instance.show(BaseInfo.gI().currentMinMoney,
				//			BaseInfo.gI().currentMaxMoney, 2, 0, 0, 0, BaseInfo.gI().typetableLogin);
				//} else {
				//	PanelRutTien.instance.show(BaseInfo.gI().currentMinMoney,
				//		  BaseInfo.gI().currentMaxMoney, 3, 0, 0, 0, BaseInfo.gI().typetableLogin);

				//}
			});
		}
	}
#endregion
	internal override void OnTimeAuToStart(int time) {
		base.OnTimeAuToStart(time);
		timeCountDown.SetTime(time);
	}

	internal override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
		base.InfoCardPlayerInTbl(message, turnName, time, numP);
		try {
			for (int i = 0; i < numP; i++) {
				string nick = message.reader().ReadUTF();
				sbyte isSkip = message.reader().ReadByte(); // = 0 Skip.
				long chips = message.reader().ReadLong();
				LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
				if (pl != null) {
					pl.MoneyChip = chips;
					pl.IsPlaying = true;
					pl.CardHand.SetBaiKhiKetNoiLai(new int[] { 52, 52 }, true);
					if (isSkip == 0) {
						pl.CardHand.SetAllDark(true);
					}
					tongMoney += chips;
				}
			}

			int size = message.reader().ReadInt();
			int[] cards = new int[size];
			for (int i = 0; i < size; i++) {
				cards[i] = message.reader().ReadByte();
			}

			sbyte len1 = message.reader().ReadByte();
			for (int i = 0; i < len1; i++) {
				long money = message.reader().ReadLong();
				sbyte len2 = message.reader().ReadByte();
				for (int j = 0; j < len2; j++) {
					string nameNN = message.reader().ReadUTF();
					//moneyInPot[i].addChip2(money / len2, name, false);
				}
				//moneyInPot[i].setmMoneyInPotNonModifier(money);
			}

			SumChipControl.MoneyChip = tongMoney;
			//gameControl.sound.startchiabaiAudio();
			SetTurn(turnName, message);
			if (turnName.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton();
				SetEnableButton();
			}
		} catch (Exception e) {
			// TODO: handle exception
			Debug.LogException(e);
		}
	}

	internal override void OnInfome(Message message) {
		base.OnInfome(message);
		try {
			isStart = true;
			plMe.IsPlaying = true;
			int sizeCardHand = message.reader().ReadByte();
			int[] cardHand = new int[sizeCardHand];
			for (int j = 0; j < sizeCardHand; j++) {
				cardHand[j] = message.reader().ReadByte();
			}
			plMe.CardHand.SetBaiKhiKetNoiLaiGamePoker(cardHand, true);

			bool upBai = message.reader().ReadBoolean();
			if (upBai) {
				plMe.CardHand.SetAllDark(true);
			}
			string turnvName = message.reader().ReadUTF();
			int turnvTime = message.reader().ReadInt();
			long money = message.reader().ReadLong();
			long moneyC = message.reader().ReadLong();
			long mIP = message.reader().ReadLong();
			plMe.MoneyChip = moneyC;
			tongMoney += moneyC;
			SetTurn(turnvName, message);
			if (turnvName.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton();
				SetEnableButton();
			}
			SumChipControl.MoneyChip = tongMoney;
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
	List<int> list_my_card = new List<int>();

	internal void StartFlip(Message message) {
		sbyte timeChonBai = message.reader().ReadByte();
		timeCountDown.SetTime(timeChonBai);
		txt_chon_bai.SetActive(true);
	}

	internal void OnCardFlip(Message message) {
		sbyte a = message.reader().ReadByte();
		Debug.LogError("aaaaaaaaaaaaa    " + a);
		if (a == 0) {
			int temp1 = plMe.CardHand.listCardHand[0].ID;

			plMe.CardHand.listCardHand[0].SetCardWithId(plMe.CardHand.listCardHand[1].ID);
			plMe.CardHand.listCardHand[1].SetCardWithId(temp1);
		}
		plMe.CardHand.listCardHand[0].SetDarkCard(true);
		plMe.CardHand.listCardHand[1].SetDarkCard(false);
		plMe.CardHand.SetTouchCardHand(false);
		txt_chon_bai.SetActive(false);
	}

	internal void OnGetCardNocSuccess(string nick, int card) {
		LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
		bool issMe = nick.Equals(ClientConfig.UserInfo.UNAME);
		if (issMe) {
			list_my_card.Add(card);
		}
		if (pl != null) {
			//pl.cardTaLaManager.BocBai(card, issMe);
			GetCardNoc(pl.CardHand, card, issMe);
		}
	}
	#region Boc them bai
	void GetCardNoc(ArrayCard ArrayCardHand, int idCard, bool isTao) {
		if (ArrayCardHand.listCardHand[1].ID == 52) {
			ArrayCardHand.listCardHand[1].SetCardWithId(idCard);
		} else {
			Card c = GetCardOnArrayCard(ArrayCardHand);
			//if (isTao) {
			c.SetCardWithId(idCard);
			c.transform.localPosition = ArrayCardHand.GetPositonCardActive();
			c.SetDarkCard(false);
			c.SetTouched(isTao);
			c.IsChoose = false;
			//} else
			//	c.SetCardWithId(53);
			//Vector3 vt = ArrayCardHand.POS_CENTER;//ArrayCardHand.transform.InverseTransformPoint(ArrayCardHand.vtPosCenter);
			Vector3 vt = ArrayCardHand.transform.InverseTransformPoint(dealerPos.position);
			StartCoroutine(c.MoveFrom(vt, 0.4f, 0, () => {
				//ArrayCardHand.ResetCard(true);
				Card ctemp = new Card();
				ctemp = c;
				ArrayCardHand.listCardHand.Remove(c);
				ArrayCardHand.listCardHand.Add(ctemp);
				ArrayCardHand.SortCardActive(true, 0.2f);
			}));
		}
		if (isTao) {
			TYPE_CARD type = TypeCardMauBinh.GetTypeCardMauBinh(list_my_card.ToArray());
			Debug.LogError("Kieu bai cua toi" + type);
		}
	}

	Card GetCardOnArrayCard(ArrayCard arr) {
		for (int i = 0; i < arr.listCardHand.Count; i++) {
			Card c = arr.listCardHand[i];
			if (!c.isBatHayChua) {
				return c;
			}
		}
		return arr.AddAndGetCardOnArray();
	}
	#endregion
}
