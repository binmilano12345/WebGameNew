using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Us.Mobile.Utilites;
using AppConfig;

public class PokerControl : BaseCasino {
	public static PokerControl instance;
	[SerializeField]
	UIButton btn_bo, btn_xembai, btn_theo, btn_to;
	[SerializeField]
	UIButton btn_ruttien;
	[SerializeField]
	Text txt_bo, txt_xem, txt_theo, txt_to;
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
	CardTableManager cardTableManager;

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
	}

	internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
		base.StartTableOk(cardHand, msg, nickPlay);
		for (int i = 0; i < ListPlayer.Count; i++) {
			((LiengPlayer)ListPlayer[i]).MoneyChip = 0;
		}
		groupMoneyTo.OnHide();
		//MinToMoney = GameConfig.BetMoney * 2;
		//MaxToMoney = ClientConfig.UserInfo.CASH_FREE - MinToMoney;

		SumChipControl.MoneyChip = 0;
		tongMoney = 0;
		cardTableManager.XoaHetCMNBaiTrenBan();
		for (int i = 0; i < ListPlayer.Count; i++) {
			LiengPlayer player = (LiengPlayer)ListPlayer[i];
			if (player != null) {
				//player.MoneyChip = GameConfig.BetMoney;
				//player.MoveChip(GameConfig.BetMoney, SumChipControl.transform.position);
				if (player.SitOnClient == 0) {
					player.CardHand.SetAllDark(false);
				}
				player.CardHand.ChiaBaiPoker(cardHand, player.SitOnClient == 0, dealerPos, i);
			}
			//tongMoney += GameConfig.BetMoney;
		}
		//SumChipControl.OnShow();
		//SumChipControl.MoneyChip = tongMoney;
	}
	internal override void OnStartForView(string[] nickPlay, Message msg) {
		base.OnStartForView(nickPlay, msg);
		tongMoney = 0;
		for (int i = 0; i < nickPlay.Length; i++) {
			LiengPlayer player = (LiengPlayer)GetPlayerWithName(nickPlay[i]);
			if (player != null) {
				//player.MoneyChip = GameConfig.BetMoney;
				//tongMoney += GameConfig.BetMoney;
				if (player.SitOnClient != 0)
					player.CardHand.ChiaBaiPoker(new int[] { 52, 52 }, false, dealerPos, i);
			}
		}

		//SumChipControl.OnShow();
		//SumChipControl.MoneyChip = tongMoney;
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
		for (int i = 0; i < ListPlayer.Count; i++) {
			((LiengPlayer)ListPlayer[i]).SetDiemLieng(false, null);
		}

		SetActiveButton(false, false, false, false);
	}

	internal override void SetTurn(string nick, Message message) {
		base.SetTurn(nick, message);
		try {
			long moneyCc = message.reader().ReadLong();
			Debug.LogError("-=-=-====SetTurn:  " + moneyCc);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				SendData.onAccepFollow();

				SetActiveButton();
				SetEnableButton(true, true, false, true);
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
			}
			if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
				if (MoneyCuoc <= plMe.MoneyFollow) {
					SetActiveButton();
					SetEnableButton(true, false, true, true);
				} else {
					MoneyCuoc = plMe.MoneyFollow;
					SetActiveButton();
					SetEnableButton(true, false, true, false);
				}

				txt_theo.text = "Theo " + MoneyHelper.FormatMoneyNormal(MoneyCuoc);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal void OnNickCuoc(Message message) {
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
			}
			if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
				if (MoneyCuoc <= plMe.MoneyFollow) {
					SetActiveButton();
					SetEnableButton(true, false, true, true);
				} else {
					MoneyCuoc = plMe.MoneyFollow;
					SetActiveButton();
					SetEnableButton(true, false, true, false);
				}

				txt_theo.text = "Theo " + MoneyHelper.FormatMoneyNormal(MoneyCuoc);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal void OnNickTheo(Message message) {
		try {
			long money = message.reader().ReadLong();
			string nick = message.reader().ReadUTF();
			string nick_turn = message.reader().ReadUTF();

			LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
			if (money == 0) {
				pl.SetEffect("Xem");
			} else {
				pl.SetEffect("Theo");

				pl.MoveChip(money + pl.MoneyChip, SumChipControl.transform.position);
				pl.MoneyChip += money;
				tongMoney += money;
				SumChipControl.MoneyChip = tongMoney;
			}
			SetTurn(nick_turn, message);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton(false, false, false, false);
			}
			if (nick_turn.Equals(ClientConfig.UserInfo.UNAME)) {
				if (MoneyCuoc <= plMe.MoneyFollow) {
					SetActiveButton();
					SetEnableButton(true, false, true, true);
				} else {
					MoneyCuoc = plMe.MoneyFollow;
					SetActiveButton();
					SetEnableButton(true, false, true, false);
				}

				txt_theo.text = "Theo " + MoneyHelper.FormatMoneyNormal(MoneyCuoc);
			}
		} catch (Exception e) {
			// TODO: handle exception
			Debug.LogException(e);
		}


	}
	public void clickButtonBo() {
		SendData.onSendSkipTurn();
	}
	public void clickButtonXem() {
		SendData.onAccepFollow();
	}
	public void clickButtonTheo() {
		SendData.onAccepFollow();
	}
	public void clickButtonTo() {
		groupMoneyTo.OnShow(MinToMoney, MaxToMoney);
	}

	public void OnTo(long moneyTo) {
		SendData.onCuocXT(-99, moneyTo);
	}

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
					pl.CardHand.SetBaiKhiKetNoiLaiGamePoker(new int[] { 52, 52 }, true);
					if (isSkip == 0) {
						pl.CardHand.SetAllDark(true);
					}
					//tongMoney += chips;
				}
			}


			int size = message.reader().ReadInt();
			int[] cards = new int[size];
			for (int i = 0; i < size; i++) {
				cards[i] = message.reader().ReadByte();
			}
			cardTableManager.SinhCardKhiKetNoiLai(cards);
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

			//SumChipControl.MoneyChip = tongMoney;
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

	internal void OnInfo(Message msg) {
	try {
		string nicksb = msg.reader().ReadUTF();
		string nickbb = msg.reader().ReadUTF();
		long moneyInPot = msg.reader().ReadLong();
		//if (players[0].isPlaying()) {
		//	players[0].setLoaiBai(PokerCard.getTypeOfCardsPoker(PokerCard.add2ArrayCard(
		//			players[0].cardHand.getArrCardAll(), cardTable.getArrCardAll())));
		//}
		//players[getPlayer(nickbb)].setMoneyChip(moneyInPot * 2 / 3);
		//players[getPlayer(nicksb)].setMoneyChip(moneyInPot / 3);
		//tongMoney = 0;
		//for (int i = 0; i < nUsers; i++) {
		//	if (players[i].isPlaying()) {
		//		tongMoney = tongMoney + players[i].getMoneyChip();

		//	}
		//}
		//chip_tong.setMoneyChip(tongMoney);
		//chip_tong.setVisible(true);
	} catch (Exception ex) {
			Debug.LogException(ex);
	}	}

public void OnAddCardTbl(Message message) {
	try {
		//luot = luot + 1;
		//// byte type = message.reader().readByte();
		//int size = message.reader().readInt();
		//int[] card = new int[size];
		//for (int i = 0; i < size; i++) {
		//	card[i] = message.reader().readByte();
		//}
		//if (size >= 3) {
		//	flyMoney();
		//}

		//BaseInfo.gI().startchiabaiAudio();
		//if (size == 3) {
		//	cardTable.setArrCard(card, false, false, false);
		//	for (int i = 0; i < cardTable.getSize(); i++) {
		//		onMoCard(cardTable.getCardbyPos(i), cardTable.getCardbyPos(i).getId());
		//	}
		//} else {
		//	for (int i = cardTable.getSize(); i < size; i++) {
		//		cardTable.addCard(card[i]);
		//		onMoCard(cardTable.getCardbyPos(i), cardTable.getCardbyPos(i).getId());
		//	}

		//}

		//if (players[0].isPlaying()) {
		//	players[0].setLoaiBai(PokerCard.getTypeOfCardsPoker(PokerCard.add2ArrayCard(
		//			players[0].cardHand.getArrCardAll(), cardTable.getArrCardAll())));
		//}

	} catch (Exception ex) {
			Debug.LogException(ex);
	}
//	this.addAction(Actions.sequence(Actions.delay(1f), new Action() {
//			@Override
//			public boolean act(float arg0) {
//	chip_tong.image_chip1.setVisible(true);

//	if (luot % 2 == 0) {
//		chip_tong.image_chip2.setVisible(true);
//	}
//	return true;
//}
//		}));
	}
}
