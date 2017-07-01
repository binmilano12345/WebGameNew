using UnityEngine;
using System;
using AppConfig;

public class BaCayControl : BaseCasino {
	public static BaCayControl instance;
	[SerializeField]
	UIButton btn_bo, btn_to;
	[SerializeField]
	UIButton btn_ruttien;
	long MoneyCuoc = 0;
	long MinToMoney = 0, MaxToMoney = 0;
	long tongMoney = 0;
	LiengPlayer plMe;

	[SerializeField]
	ChipControl SumChipControl;
	[SerializeField]
	GroupTo groupMoneyTo;
	[SerializeField]
	TimeCountDownBaCay timeCountDown;

	[SerializeField]
	Transform dealerPos;
	void Awake() {
		instance = this;
		SumChipControl.IsChipSum = true;
	}

	void SetActiveButton(bool is_bo = true, bool is_to = true) {
		btn_bo.gameObject.SetActive(is_bo);
		btn_to.gameObject.SetActive(is_to);
	}

	public void clickBtnCuoc() {
		MinToMoney = GameConfig.BetMoney * 2;
		if (MaxToMoney > GameConfig.BetMoney * 10) {
			MaxToMoney = GameConfig.BetMoney * 10;
			if (MaxToMoney >= plMe.Money) {
				MaxToMoney = plMe.Money;
			}
		} else {
			MaxToMoney = plMe.Money;
		}

		showDatcuoc(MinToMoney, MaxToMoney);
	}

	private void showDatcuoc(long min, long max) {
		groupMoneyTo.OnShow(min, max);
	}
	public void clickBtnBoCuoc() {
		SendData.onSendCuocBC(0);
	}

	public void OnCuoc(long money) {
		SendData.onSendCuocBC(money);
	}

	internal void OnBeginRiseBacay(Message message) {
		try {
			tongMoney = 0;
			LiengPlayer plMaster = (LiengPlayer)GetPlayerWithName(masterID);
			Debug.LogError("Thang chu ban:   " + masterID);
			if (plMaster != null) {
				plMaster.MoneyChip = 0;
			}
			//BaseInfo.gI().startTineCountAudio();
			int turntimeBC = message.reader().ReadByte();
			//groupTimerClock.setText("Đặt cược");
			//groupTimerClock.setRun(turntimeBC);
			timeCountDown.Settext("Đặt cược");
			timeCountDown.SetTime(turntimeBC);
			//for (int j = 0; j < nUsers; j++) {
			//	if (players[j].getName().length() > 0) {
			//		players[j].setInfo(true, false, false, 0);
			//	}
			//}
			//for (int i = 0; i < players.length; i++) {
			//	players[i].resetData();
			//}

				plMe.IsPlaying = false;
			if (turntimeBC == -1) {
				turntimeBC = message.reader().ReadByte();

				timeCountDown.Settext("Đặt cược");
				timeCountDown.SetTime(turntimeBC);
				//	players[0].setPlaying(false);
				//	if (players[0].getName().equals(BaseInfo.gI().mainInfo.nick)) {
				//		MainInfo.setPlayingUser(false);
				//	}
				//	showButtonCuoc(false);
				SetActiveButton(false, false);
			} else {
				//plMe.IsPlaying = false;
				//	players[0].setPlaying(true);
				//	if (players[0].getName().equals(BaseInfo.gI().mainInfo.nick)) {
				//		MainInfo.setPlayingUser(true);
				//	}
				//	timeReceiveTurnBC = System.currentTimeMillis();
				if (!plMe.IsMaster) {
					SetActiveButton();
				}
			}
			for (int i = 0; i < ListPlayer.Count; i++) {
				((LiengPlayer)ListPlayer[i]).CardHand.SetActiveCardHand();
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	internal void OnCuoc3Cay(Message message) {
		try {
			int a = message.reader().ReadByte();
			if (a == 1) {
				string nickCuoc = message.reader().ReadUTF();
				long moneyCuoc = message.reader().ReadLong();
				LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nickCuoc);
				if (pl != null) {
					Debug.LogError("Tien cuoc:    " + moneyCuoc);
					pl.MoneyChip = moneyCuoc;
					pl.MoveChip(moneyCuoc, SumChipControl.transform.position);
					if (pl.SitOnClient == 0) {
						SetActiveButton(false, false);
						timeCountDown.SetTime(0);
						hideThanhTo();
					}
				}
				tongMoney += moneyCuoc * 2;
				//cuoc = cuoc + 1;

				//if (cuoc % 2 == 0) {
				//	chip_tong.image_chip2.setVisible(true);
				//}

				SumChipControl.MoneyChip = tongMoney;
			} else {
				String mess = message.reader().ReadUTF();
				SetActiveButton(true);
			}
		} catch (Exception e) {
			//e.printStackTrace();
			Debug.LogException(e);
		}
	}

	internal override void StartTableOk(int[] cardHand, Message msg, string[] nickPlay) {
		base.StartTableOk(cardHand, msg, nickPlay);
		//for (int i = 0; i < ListPlayer.Count; i++) {
		//	((LiengPlayer)ListPlayer[i]).MoneyChip = 0;
		//}
		groupMoneyTo.OnHide();
		MinToMoney = GameConfig.BetMoney * 2;
		MaxToMoney = ClientConfig.UserInfo.CASH_FREE - MinToMoney;
		SetActiveButton(false, false);
		//SumChipControl.MoneyChip = 0;
		//tongMoney = 0;
		for (int i = 0; i < ListPlayer.Count; i++) {
			LiengPlayer player = (LiengPlayer)ListPlayer[i];
			if (player != null) {
				//player.MoneyChip = GameConfig.BetMoney;
				//player.MoveChip(GameConfig.BetMoney, SumChipControl.transform.position);
				//if (player.SitOnClient == 0) {
				player.CardHand.SetAllDark(false);
				//}
				player.CardHand.ChiaBaiPoker(cardHand, player.SitOnClient == 0, dealerPos, i, () => {
					if (player.SitOnClient == 0) {
						player.SetDiemBaCay(true, cardHand);
						Debug.LogError("========> vao xet diem "+ cardHand.Length);
					}
				});
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
					player.CardHand.ChiaBaiPoker(new int[] { 52, 52, 52 }, false, dealerPos, i);
			}
		}

		//SumChipControl.OnShow();
		//SumChipControl.MoneyChip = tongMoney;
		SetActiveButton(false, false);
	}
	internal override void OnJoinView(Message message) {
		// TODO Auto-generated method stub
		base.OnJoinView(message);
		plMe = (LiengPlayer)playerMe;

		SetActiveButton(false, false);
	}
	internal override void OnJoinTableSuccess(string master) {
		plMe = (LiengPlayer)playerMe;
		for (int i = 0; i < ListPlayer.Count; i++) {
			((LiengPlayer)ListPlayer[i]).SetDiemBaCay(false, null);
		}

		SetActiveButton(false, false);
	}

	internal override void SetTurn(string nick, Message message) {
		base.SetTurn(nick, message);
		try {
			long moneyCc = message.reader().ReadLong();
			Debug.LogError("-=-=-====SetTurn:  " + moneyCc);
			if (nick.Equals(ClientConfig.UserInfo.UNAME)) {
				//if (plMe.MoneyFollow == 0) {
				SendData.onAccepFollow();
				//	} else {
				//		baseSetturn(moneyCuoc);
				//	}
				//} else {
				//	if (plMe.IsPlaying) {
				//		showAllButton(true, true, true);
				//	} else {
				//		showAllButton(true, false, false);
				//	}
				//             enableAllButton(false);
				//	setMoneyCuoc(moneyCuoc);
				SetActiveButton();
			} else {
				hideThanhTo();
				SetActiveButton(false, false);
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
				int score = message.reader().ReadInt();
				LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
				if (pl != null) {
					pl.SetRank(rank);
					pl.IsReady = false;
					pl.SetShowReady(false);
					pl.MoneyChip = 0;
					if (score == 100) {
						pl.SetDiemBaCay("Sáp");
					} else if (score == 99) {
						pl.SetDiemBaCay("10 Át rô");
					} else {
						pl.SetDiemBaCay(score+"");
					}
				}
			}
			SetActiveButton(false, false);
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
		timeCountDown.Settext("Xin chờ");
	}


	internal override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
		base.InfoCardPlayerInTbl(message, turnName, time, numP);
		try {
			Debug.LogError("numP " + numP);
			for (int i = 0; i < numP; i++) {
				string nick = message.reader().ReadUTF();
				sbyte isSkip = message.reader().ReadByte(); // = 0 Skip.
				long chips = message.reader().ReadLong();
				LiengPlayer pl = (LiengPlayer)GetPlayerWithName(nick);
				if (pl != null) {
					pl.MoneyChip = chips;
					pl.IsPlaying = true;
					pl.CardHand.SetBaiKhiKetNoiLaiGamePoker(new int[] { 52, 52, 52 }, true);
					if (isSkip == 0) {
						pl.CardHand.SetAllDark(true);
					}
					tongMoney += chips;
				}
			}
			SumChipControl.MoneyChip = tongMoney;
			//gameControl.sound.startchiabaiAudio();
			SetTurn(turnName, message);
			if (turnName.Equals(ClientConfig.UserInfo.UNAME)) {
				SetActiveButton();
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
			}
			SumChipControl.MoneyChip = tongMoney;
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
}
