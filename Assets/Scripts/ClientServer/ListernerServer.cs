using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppConfig;
using DataBase;
using System.Linq;
using UnityEngine.SceneManagement;

public class ListernerServer : IChatListener
{
	//GameControl gameControl;

	public void initConnect ()
	{
		NetworkUtil.GI ().registerHandler (ProcessHandler.getInstance ());
		ProcessHandler.setListenner (this);
		PHandler.setListenner (this);
		TLMNHandler.setListenner (this);
		XiToHandler.setListenner (this);
	}

	public ListernerServer (/*GameControl gameControl*/)
	{
		//this.gameControl = gameControl;
		initConnect ();
	}

	public void onDisConnect ()
	{
		PopupAndLoadingScript.instance.HideLoading ();
		PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("reconect_disconnect"), delegate {
			NetworkUtil.GI ().close ();
			LoadAssetBundle.LoadScene (SceneName.SCENE_MAIN, SceneName.SCENE_MAIN);
		});
		Debug.Log ("Mất kết nối!");
	}

	public void OnLogin (Message message)
	{
		sbyte b = message.reader ().ReadByte ();
		#region Dang nhap Thanh cong
		if (b == 1) {
			try {
				LinkFixed.LinkForum = message.reader ().ReadUTF ();
				GameConfig.SMS_CHANGE_PASS_SYNTAX = message.reader ().ReadUTF ();
				GameConfig.SMS_CHANGE_PASS_NUMBER = message.reader ().ReadUTF ();
				GameConfig.IsShowDoiThuong = message.reader ().ReadByte ();
			} catch (Exception e) {
				Debug.LogException (e);
			}
			#region Lay thong tin khac
			SendData.onSendSmsSyntax ();
			SendData.onGetTopGame ((int)GameID.TLMN);
			SendData.onSendSms9029 (GameConfig.TELCO_CODE);
			#endregion
			LoadAssetBundle.LoadScene (SceneName.SCENE_LOBBY, SceneName.SCENE_LOBBY);
		}
        #endregion       
        #region Dang nhap That bai
        else {
			string str = message.reader ().ReadUTF ();
			switch (b) {
			case 0:
				PopupAndLoadingScript.instance.messageSytem.OnShow (str);
				break;
			case 2://Nhap so dt
				break;
			}
		}
		#endregion

		PopupAndLoadingScript.instance.HideLoading ();
	}

	public void	OnGetPass (Message message)
	{
		PopupAndLoadingScript.instance.HideLoading ();
		string sms = message.reader ().ReadUTF ();
		string ds = message.reader ().ReadUTF ();
		PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("popup_lay_mk")
			, delegate {
			GameControl.instance.SendSMS (ds, sms);
		});
	}

	public void OnGetPhoneCSKH (Message message)
	{
		try {
			string a = message.reader ().ReadUTF ();
			GameConfig.IS_LOGIN_FB_AVARIABLE = message.reader ().ReadBoolean ();
			string[] b = a.Split (',');
			GameConfig.HOT_LINE = b [0];
			if (b.Length > 1) {
				GameConfig.MAIL_HELPER = b [1];
			}
			if (b.Length > 2) {
				GameConfig.FANPAGE = b [2];
			}

			GameConfig.IP2 = message.reader ().ReadUTF ();
			GameConfig.CONTENT_DAILY = message.reader ().ReadUTF ();
			GameConfig.PHONE_NUMBER_DAILY = message.reader ().ReadUTF ();

			if (MainControl.instance != null) {
				MainControl.instance.SetHotline ();
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnRegister (Message message)
	{
		int status = message.reader ().ReadByte ();
		PopupAndLoadingScript.instance.HideLoading ();
		if (status == 0) {
			PopupAndLoadingScript.instance.messageSytem.OnShow (message.reader ().ReadUTF ());
		} else {
			PopupAndLoadingScript.instance.toast.showToast (ClientConfig.Language.GetText ("popup_dang_ky_thanh_cong"));
			if (RegisterControl.instance != null)
				RegisterControl.instance.OnHide ();
			LoadAssetBundle.LoadScene (SceneName.SUB_LOGIN, SceneName.SUB_LOGIN, () => {
				LoginControl.instance.Init ();
			});
		}
	}

	public void OnMessageServer (string message)
	{
		//string msg = message.reader().ReadUTF();
		PopupAndLoadingScript.instance.messageSytem.OnShow (message);
		PopupAndLoadingScript.instance.HideLoading ();
	}

	public void OnPopupNotify (Message message)
	{//clam
		GameControl.instance.ListNoti.Clear ();
		int size = message.reader ().ReadInt ();
		if (size != 0) {
			for (int i = 0; i < size; i++) {
				ItemNotiData item = new ItemNotiData ();
				item.Id = message.reader ().ReadInt ();
				item.Title = message.reader ().ReadUTF ();
				item.Content = message.reader ().ReadUTF ();
				GameControl.instance.ListNoti.Add (item);
			}
		}
	}

	public void OnProfile (Message msg)
	{
		ClientConfig.UserInfo.UNAME = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.USER_ID = (int)msg.reader ().ReadLong ();
		ClientConfig.UserInfo.CASH_FREE = msg.reader ().ReadLong ();
		ClientConfig.UserInfo.NAME = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.DISPLAY_NAME = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.LINK_AVATAR = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.AVATAR_ID = msg.reader ().ReadInt ();
		ClientConfig.UserInfo.EXP = msg.reader ().ReadLong ();
		ClientConfig.UserInfo.SCORE_VIP = msg.reader ().ReadLong ();
		ClientConfig.UserInfo.TOTAL_MONEY_CHARGING = msg.reader ()
               .ReadLong ();
		ClientConfig.UserInfo.TOTAL_TIME_PLAY = msg.reader ().ReadLong ();

		ClientConfig.UserInfo.SO_LAN_THANG = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.SO_LAN_THUA = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.SO_TIEN_MAX = msg.reader ().ReadLong ();
		ClientConfig.UserInfo.SO_LAN_GIAO_DICH = msg.reader ().ReadInt ();

		String email_sdt = msg.reader ().ReadUTF ();
		ClientConfig.UserInfo.SEX = msg.reader ().ReadByte ();
		ClientConfig.UserInfo.LEVEL = msg.reader ().ReadByte ();
		ClientConfig.UserInfo.PHONE = msg.reader ().ReadUTF ();

		string[] s = Regex.Split (email_sdt, "\\*");

		ClientConfig.UserInfo.LOGIN_END = s [0];

		if (ClientConfig.UserInfo.EXP < 10) {
			ClientConfig.UserInfo.LEVEL_SCORE = 0;
		} else if (ClientConfig.UserInfo.EXP >= 30
		           && ClientConfig.UserInfo.EXP < 100) {
			ClientConfig.UserInfo.LEVEL_SCORE = 1;
		} else if (ClientConfig.UserInfo.EXP >= 100
		           && ClientConfig.UserInfo.EXP < 300) {
			ClientConfig.UserInfo.LEVEL_SCORE = 2;
		} else if (ClientConfig.UserInfo.EXP >= 300
		           && ClientConfig.UserInfo.EXP < 10000) {
			ClientConfig.UserInfo.LEVEL_SCORE = 3;
		} else if (ClientConfig.UserInfo.EXP >= 10000
		           && ClientConfig.UserInfo.EXP < 100000) {
			ClientConfig.UserInfo.LEVEL_SCORE = 4;
		} else if (ClientConfig.UserInfo.EXP > 100000) {
			ClientConfig.UserInfo.LEVEL_SCORE = 5;
		}
		string[] listthang = ClientConfig.UserInfo.SO_LAN_THANG.Split (',');
		int thang = 0;
		for (int i = 0; i < listthang.Length; i++) {
			thang += int.Parse (listthang [i].Split ('-') [1]);
		}
		ClientConfig.UserInfo.TONG_VAN_THANG = thang;
		String[] listthua = ClientConfig.UserInfo.SO_LAN_THUA.Split (',');
		int thua = 0;
		for (int i = 0; i < listthua.Length; i++) {
			thua += int.Parse (listthua [i].Split ('-') [1]);
		}
		ClientConfig.UserInfo.TONG_VAN_THUA = thua;

	}

	public void OnChangeName (Message message)
	{
		try {
			sbyte type = message.reader ().ReadByte ();
			string info = message.reader ().ReadUTF ();
			if (type == 1) {
				ClientConfig.UserInfo.DISPLAY_NAME = message.reader ().ReadUTF ();
				if (LobbyControl.instance != null) {
					LobbyControl.instance.SetDisplayName ();
				}
				if (RoomControl.instance != null) {
					RoomControl.instance.SetDisplayName ();
				}
			}
			PopupAndLoadingScript.instance.messageSytem.OnShow (info);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnChangePass (Message message)
	{
		try {
//			sbyte type = message.reader().ReadByte();
//			string info = message.reader().ReadUTF();
//			if (type == 1) {
//				ClientConfig.UserInfo.DISPLAY_NAME = message.reader().ReadUTF();
//				if(LobbyControl.instance != null){
//					LobbyControl.instance.SetDisplayName ();
//				}
//				if(RoomControl.instance != null){
//					RoomControl.instance.SetDisplayName ();
//				}
//			}
//			PopupAndLoadingScript.instance.messageSytem.OnShow (info);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnChangeAvatar (Message message)
	{
		try {
			sbyte type = message.reader ().ReadByte ();
			string info = message.reader ().ReadUTF ();
			if (LobbyControl.instance != null) {
				LobbyControl.instance.SetAvatar ();
			}
			if (RoomControl.instance != null) {
				RoomControl.instance.SetAvatar ();
			}
			PopupAndLoadingScript.instance.messageSytem.OnShow (info);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnRateScratchCard (Message message)
	{
		try {
			int size = message.reader ().ReadInt ();
			for (int i = 0; i < size; i++) {
				int menhgia = message.reader ().ReadInt ();
				int xu = message.reader ().ReadInt ();
			}
			GameConfig.SMS_10 = message.reader ().ReadInt ();
			GameConfig.SMS_15 = message.reader ().ReadInt ();

			//mainGame.mainScreen.dialogNapTien
			//        .initTygia(BaseInfo.gI().list_tygia);
			//mainGame.mainScreen.dialogNapTien.initSMS();
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnListBetMoney (Message message)
	{
		try {
			//ListBetMoney.gI().listBetVip.clear();
			int size = message.reader ().ReadShort ();
			for (int i = 0; i < size; i++) {
				//BetMoney betMoney = new BetMoney();
				long a = message.reader ().ReadLong ();
				//betMoney.setListBet(a);
				//ListBetMoney.gI().listBetVip.add(betMoney);
			}
			//mainGame.mainScreen.room.groupChonMucCuoc.initMuccuoc(ListBetMoney
			//        .gI().listBetVip);
			//mainGame.mainScreen.taiXiuMinigame.initMuccuoc(message.reader()
			//        .readUTF());
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnListProduct (Message message)
	{
		//mainGame.listProductIDs.clear();
		try {
			int size = message.reader ().ReadInt ();
			for (int i = 0; i < size; i++) {
				//ProductID productID = new ProductID();
				string productid = message.reader ().ReadUTF ();
				int price = message.reader ().ReadInt ();
				int xu = message.reader ().ReadInt ();
				//mainGame.listProductIDs.add(productID);
			}
			//mainGame.mainScreen.dialogNapTien.createIAP(mainGame,
			//        mainGame.listProductIDs);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnTop (Message message)
	{
		try {
			GameControl.instance.ListRank.Clear ();
			int size = message.reader ().ReadByte ();
			for (int i = 0; i < size; i++) {
				ItemRankData item = new ItemRankData ();
				item.Rank = i + 1;
				item.Name = message.reader ().ReadUTF ();
				item.Avata_Id = message.reader ().ReadInt ();
				if (item.Avata_Id < 0 || item.Avata_Id > GameConfig.NUM_AVATA) {
					item.Avata_Id = 0;
				}
				item.Money = message.reader ().ReadLong ();
				GameControl.instance.ListRank.Add (item);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnInboxMessage (Message message)
	{
		try {
			//ArrayList<MessInfo> homThu = new ArrayList<MessInfo>();
			int total = message.reader ().ReadByte ();
			for (int i = 0; i < total; i++) {
				//MessInfo info = new MessInfo();

				int id = message.reader ().ReadInt ();
				string guitu = message.reader ().ReadUTF ();
				string guiLuc = message.reader ().ReadUTF ();
				string noiDung = message.reader ().ReadUTF ();
				sbyte isread = message.reader ().ReadByte ();
				//homThu.add(info);

			}
			//BaseInfo.gI().allMess.clear();
			//BaseInfo.gI().allMess.addAll(homThu);
			//GroupHomThu.initTinHethong = true;
			//mainGame.mainScreen.dialogWaitting.onHide();

		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnGetAlertLink (Message message)
	{
		try {
			string content = message.reader ().ReadUTF ();
			if (!content.Equals ("")) {
				GameConfig.TXT_NOTI = content;
				//PopupAndLoadingScript.instance.alert.SetAlert(content);
				if (LobbyControl.instance != null) {
					LobbyControl.instance.SetNoti ();
				}
			}
			if (GameConfig.IsShowDoiThuong == 0) {
				//mainGame.mainScreen.menu.textNoti
				//        .setText("Chào Mừng Bạn đã đến với Game Bài "
				//                + Res.tengame + " Online!");
			} else {
				//mainGame.mainScreen.menu.textNoti.setText(Res.TXT_Noti);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnInfoSMS (Message message)
	{
		try {
			int len = message.reader ().ReadByte ();
			GameConfig.IsCharging = len;
			//mainGame.myPref.putCharging(BaseInfo.gI().isCharging);
			for (int i = 0; i < 2; i++) {
				message.reader ().ReadUTF ();// name
				string syntax = message.reader ().ReadUTF ();
				short port = message.reader ().ReadShort ();
				int type = (port % 1000) / 100;
				if (type == 6) {
					GameConfig.Syntax10 = syntax;
					GameConfig.Port10 = port + "";
				} else if (type == 7) {
					GameConfig.Syntax15 = syntax;
					GameConfig.Port15 = port + "";
				} else {
					if (i == 0) {
						GameConfig.Syntax10 = syntax;
						GameConfig.Port10 = port + "";
					} else {
						GameConfig.Syntax15 = syntax;
						GameConfig.Port15 = port + "";
					}
				}
			}
			//        if (GameConfig.IsCharging != 10) {
			//            mainGame.mainScreen.menu.event.setVisible(true);
			//} else {
			//mainGame.mainScreen.menu.event.setVisible(false);
			//}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnSMS9029 (Message message)
	{
		try {
			//mainGame.listSms9029.clear();
			sbyte size = message.reader ().ReadByte ();
			for (int i = 0; i < size; i++) {
				//SMS9029 sms = new SMS9029();
				string name = message.reader ().ReadUTF ();
				string syntax = message.reader ().ReadUTF ();
				short port = message.reader ().ReadShort ();
				long money = message.reader ().ReadLong ();
				//mainGame.listSms9029.add(sms);
			}
			//Collections.sort(mainGame.listSms9029, sort_sms9029);
			//mainGame.mainScreen.dialogNapTien.initSMS9029(mainGame.listSms9029);
		} catch (Exception e) {
			Debug.LogException (e);
		}

	}

	public void OnInvite (Message message)
	{
		sbyte confirm = message.reader ().ReadByte ();
		if (confirm == 0) {
			string str = message.reader ().ReadUTF ();
		} else {
			string nickinvite = message.reader ().ReadUTF ();
			string displayNameInvite = message.reader ().ReadUTF ();
			int gameid = message.reader ().ReadByte ();
			short tblid = message.reader ().ReadShort ();
			long money = message.reader ().ReadLong ();
			long minMoney = message.reader ().ReadLong ();
			long maxMoney = message.reader ().ReadLong ();

			if (SettingConfig.IsInvite == 1) {
				PopupAndLoadingScript.instance.messageSytem.OnShowCancelAll (string.Format (ClientConfig.Language.GetText ("popup_invite_play"), string.IsNullOrEmpty (displayNameInvite) ? nickinvite : displayNameInvite, GameConfig.GameName [gameid], money), delegate {
					SendData.onAcceptInviteFriend ((byte)gameid, tblid, -1);
				});
			}
		}
	}

	public void OnListInvite (Message message)
	{
		try {
			short total = message.reader ().ReadShort ();
			if (total <= 0) {
				PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText (""));
				return;
			}
			List<ItemInviteData> list = new List<ItemInviteData> ();
			for (int i = 0; i < total; i++) {
				ItemInviteData ittt = new ItemInviteData ();
				ittt.Name = message.reader ().ReadUTF ();
				ittt.Dispayname = message.reader ().ReadUTF ();
				ittt.Money = message.reader ().ReadLong ();
				list.Add (ittt);
			}
			LoadAssetBundle.LoadScene (SceneName.SUB_INVITE, SceneName.SUB_INVITE, () => {
				DialogInvite.instance.Init (list);
			});
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnJoinGame (Message message)
	{
		//BaseInfo.gI().sort_giam_dan_muccuoc = false;
		//BaseInfo.gI().type_sort = 2;
		//BaseInfo.gI().sort_giam_dan_bancuoc = false;
		//RoomStage.money_chonmc = 0;
		//mainGame.mainScreen.room.groupChonMucCuoc.onHide();
		//if (mainGame.mainScreen.room.rowChonMuccuoc != null)
		//    mainGame.mainScreen.room.rowChonMuccuoc.setChon(false);
		//mainGame.mainScreen.room.groupChonMucCuoc.img_chon.setVisible(true);
		//mainGame.mainScreen.room.rowChonMuccuoc = mainGame.mainScreen.room.groupChonMucCuoc.rowAll;
		//mainGame.mainScreen.room.rowChonMuccuoc.setChon(true);
		//mainGame.mainScreen.room.groupChonMucCuoc.btnChonmc
		//        .setText("Tất cả MC");
		OnGameID (message);
		SendData.onJoinRoom (ClientConfig.UserInfo.UNAME, 0);

	}

	public void OnJoinRoom (Message message)
	{
		short status = message.reader ().ReadShort ();
		if (status == -1) {//that bai
			message.reader ().ReadUTF ();
		} else {
			OnListTable (status, message);
		}
	}

	public void OnGameID (Message message)
	{
		try {
			sbyte gameID = message.reader ().ReadByte ();
			GameConfig.CurrentGameID = gameID;
			if (gameID == -99) {
				gameID = message.reader ().ReadByte ();
				GameConfig.CurrentGameID = gameID;
				message.reader ().ReadUTF ();// ten room
			}
		} catch (Exception e) {
		}

		//switch (mainGame.gameID) {
		//    case GameID.PHOM:
		//        Card.setCardType(0);
		//        ProcessHandler.setSecondHandler(PHandler.getInstance());
		//        break;
		//    case GameID.TLMN:
		//        Card.setCardType(1);
		//        ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
		//        break;
		//    case GameID.TLMNSL:
		//        Card.setCardType(1);
		//        ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
		//        break;
		//    case GameID.BACAY:
		//        Card.setCardType(0);
		//        ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
		//        break;
		//    case GameID.XITO:
		//        Card.setCardType(0);
		//        ProcessHandler.setSecondHandler(XiToHandler.getInstance());
		//        break;
		//    case GameID.LIENG:
		//        Card.setCardType(0);
		//        ProcessHandler.setSecondHandler(XiToHandler.getInstance());
		//        break;
		//    case GameID.POKER:
		//        Card.setCardType(1);
		//        ProcessHandler.setSecondHandler(XiToHandler.getInstance());
		//        break;
		//    case GameID.MAUBINH:
		//        Card.setCardType(1);
		//        break;
		//    case GameID.SAM:
		//        Card.setCardType(1);
		//        ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
		//        break;
		//    default:
		//        break;
		//}
	}

	public void OnListTable (int totalTB, Message message)
	{
		List<ItemTableData> listTable = new List<ItemTableData> ();
		for (int i = 0; i < totalTB; i++) {
			try {
				ItemTableData ctb = new ItemTableData ();
				ctb.TableName = "Bàn " + i;
				ctb.Id = (message.reader ().ReadShort ());
				ctb.Status = (message.reader ().ReadByte ());
				ctb.NUser = (message.reader ().ReadByte ());
				ctb.IsLock = message.reader ().ReadByte ();
				ctb.Money = message.reader ().ReadLong ();
				ctb.NeedMoney = message.reader ().ReadLong ();
				ctb.MaxMoney = message.reader ().ReadLong ();
				ctb.MaxUser = (message.reader ().ReadByte ());
				//if (roomstage.anbanfull && ctb.nuser == ctb.maxuser) {//sua
				//    continue;
				//}
				listTable.Add (ctb);
			} catch (Exception ex) {
				Debug.LogException (ex);
			}
		}

		listTable.OrderBy (r => r.Money);
		LoadAssetBundle.LoadScene (SceneName.SCENE_ROOM, SceneName.SCENE_ROOM, () => {
			RoomControl.instance.CreateTable (listTable);
		});
	}

	public void OnUpdateRoom (Message message)
	{
		short totalTB = message.reader ().ReadShort ();
		List<ItemTableData> listTable = new List<ItemTableData> ();
		for (int i = 0; i < totalTB; i++) {
			try {
				ItemTableData ctb = new ItemTableData ();
				ctb.TableName = "Bàn " + i;
				ctb.Id = (message.reader ().ReadShort ());
				ctb.Status = (message.reader ().ReadByte ());
				ctb.NUser = (message.reader ().ReadByte ());
				ctb.IsLock = message.reader ().ReadByte ();
				ctb.Money = message.reader ().ReadLong ();
				ctb.NeedMoney = message.reader ().ReadLong ();
				ctb.MaxMoney = message.reader ().ReadLong ();
				ctb.MaxUser = (message.reader ().ReadByte ());

				listTable.Add (ctb);
			} catch (Exception ex) {
				Debug.LogException (ex);
			}
		}

		//listTable.OrderBy(r => r.Money);
		if (!SceneManager.GetSceneByName (SceneName.SCENE_ROOM).isLoaded) {
			LoadAssetBundle.LoadScene (SceneName.SCENE_ROOM, SceneName.SCENE_ROOM, () => {
				RoomControl.instance.CreateTable (listTable);
			});
		} else {
			if (RoomControl.instance != null)
				RoomControl.instance.UpdateListTable (listTable);
		}
	}

	public void OnChat (Message message)
	{
		string nick = message.reader ().ReadUTF ();
		string msg = message.reader ().ReadUTF ();
		bool outttt = message.reader ().ReadBoolean ();
		GameControl.instance.CurrentCasino.OnChat (nick, msg);
	}

	public void OnJoinTablePlay (Message message)
	{
		// check = SerializerHelper.readInt(message);
		sbyte status = message.reader ().ReadByte ();
		Debug.LogError ("======== " + status);
		if (status == 1) {
			sbyte numPlayer = message.reader ().ReadByte ();
			GameControl.instance.SetCasino (numPlayer == 9 ? 1 : 0, () => {
				GameControl.instance.CurrentCasino.OnJoinTablePlaySuccess (message);
			});
		} else {
			if (status == -1) {
				string a = message.reader ().ReadUTF ();

				OnMessageServer (a);
			} else if (status == 0) {
				message.reader ().ReadInt ();
				OnMessageServer (message.reader ().ReadUTF ());
			}
		}
	}

	public void OnUserExitTable (Message message)
	{
		int idTb = message.reader ().ReadShort ();
		string master = message.reader ().ReadUTF ();
		string nick = message.reader ().ReadUTF ();
		GameControl.instance.CurrentCasino.OnUserExitTable (nick, master);
	}

	public void OnUserJoinTable (Message message)
	{
		GameControl.instance.CurrentCasino.OnUserJoinTable (message);
	}

	public void OnUpdateMoneyTbl (Message message)
	{
		GameControl.instance.CurrentCasino.OnUpdateMoneyTbl (message);
	}

	#region Bat dau van

	public void InfoCardPlayerInTbl (Message message)
	{
		GameControl.instance.CurrentCasino.InfoCardPlayerInTbl (message);
	}

	public void OnReady (Message message)
	{
		try {
			message.reader ().ReadShort ();// tbid
			int totalReady = message.reader ().ReadByte ();
			for (int i = 0; i < totalReady; i++) {
				string nick = message.reader ().ReadUTF ();
				bool ready = message.reader ().ReadBoolean ();

				GameControl.instance.CurrentCasino.OnReady (nick, ready);
			}
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public void OnStartFail (string info)
	{
		PopupAndLoadingScript.instance.messageSytem.OnShow (info);
		GameControl.instance.CurrentCasino.OnStartFail ();
	}

	public void OnStartSuccess (Message message)
	{
		try {
			//int[] cardHand = new int[1];
			int size = message.reader ().ReadInt ();
			byte[] c = new byte[size];
			message.reader ().Read (c, 0, size);
			int[] cardHand = new int[c.Length];
			for (int i = 0; i < c.Length; i++) {
				cardHand [i] = c [i];
			}

			int size1 = message.reader ().ReadByte ();
			string[] playingName = new string[size1];
			for (int i = 0; i < size1; i++) {
				playingName [i] = message.reader ().ReadUTF ();
			}

			GameControl.instance.CurrentCasino.StartTableOk (cardHand, message, playingName);
			//mainGame.mainScreen.curentCasino.isStart = true;
			//mainGame.mainScreen.curentCasino.isPlaying = true;

		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public void OnStartForView (Message message)
	{
		try {
			int size = message.reader ().ReadByte ();
			string[] playingName = new string[size];
			for (int i = 0; i < size; i++) {
				playingName [i] = message.reader ().ReadUTF ();
			}
			GameControl.instance.CurrentCasino.OnStartForView (playingName, message);
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public void OnSetNewMaster (string nick)
	{
		GameControl.instance.CurrentCasino.SetMaster (nick);
	}

	public void OnSetTurn (Message message)
	{
		string nick = message.reader ().ReadUTF ();
		GameControl.instance.CurrentCasino.SetTurn (nick, message);
	}

	#endregion

	#region TLMN

	public void OnNickSkip (string nick, string turnName)
	{
		GameControl.instance.CurrentCasino.OnNickSkip (nick, turnName);
	}

	public void OnNickSkip (string nick, Message msg)
	{
		GameControl.instance.CurrentCasino.OnNickSkip (nick, msg);
	}

	public void OnFinishGame (Message message)
	{
		GameControl.instance.CurrentCasino.OnFinishGame (message);
	}

	public void OnAllCardPlayerFinish (Message message)
	{
		try {
			string nick = message.reader ().ReadUTF ();
			int size = message.reader ().ReadInt ();
			byte[] c = new byte[size];
			message.reader ().Read (c, 0, size);
			int[] card = new int[c.Length];
			for (int i = 0; i < c.Length; i++) {
				card [i] = c [i];
			}
			GameControl.instance.CurrentCasino.AllCardFinish (nick, card);
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public void OnFinishTurnTLMN (Message message)
	{
		GameControl.instance.CurrentCasino.OnFinishTurn ();
	}

	#endregion

	#region Sam

	public void OnBaoSam (Message message)
	{
		try {
			sbyte type = message.reader ().ReadByte ();
			if (type == 0) {
				sbyte time = message.reader ().ReadByte ();
				((SamControl)GameControl.instance.CurrentCasino).OnHoiBaoSam (time);
				//mainGame.mainScreen.curentCasino.onHoiBaoXam(time);
			} else if (type == 1) {
				string name = message.reader ().ReadUTF ();
				//mainGame.mainScreen.curentCasino.onNickBaoXam(name);
				((SamControl)GameControl.instance.CurrentCasino).OnNickBaoSam (name);
			}
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	#endregion

	#region Phom

	public void OnGetCardNocSuccess (Message message)
	{
		int card = message.reader ().ReadByte ();
		if (card != -1) {
			string nick = message.reader ().ReadUTF ();
			((PhomControl)GameControl.instance.CurrentCasino).OnGetCardNocSuccess (nick, card);
		}
	}

	public void OnEatCardSuccess (Message message)
	{
		int card = message.reader ().ReadByte ();
		if (card != -1) {
			string thangBiAn = message.reader ().ReadUTF ();
			string thangAn = message.reader ().ReadUTF ();
			((PhomControl)GameControl.instance.CurrentCasino).OnEatCardSuccess (thangBiAn, thangAn, card);
		}
	}

	public void OnBalanceCard (Message message)
	{
		int card = message.reader ().ReadByte ();
		string from = message.reader ().ReadUTF ();
		string to = message.reader ().ReadUTF ();

		((PhomControl)GameControl.instance.CurrentCasino).OnBalanceCard (from, to, card);
	}

	public void OnDropPhomSuccess (Message message)
	{
		int card = message.reader ().ReadByte ();
		if (card != 0) {
			string nn = message.reader ().ReadUTF ();
			int size = message.reader ().ReadInt ();
			sbyte[] arry = new sbyte[size];
			for (int i = 0; i < size; i++) {
				arry [i] = message.reader ().ReadByte ();
			}

			int[] cdp = new int[arry.Length];
			for (int i = 0; i < arry.Length; i++) {
				cdp [i] = arry [i];
			}
			((PhomControl)GameControl.instance.CurrentCasino).OnDropPhomSuccess (nn, cdp);
		}
	}

	public void OnAttachCard (Message message)
	{
		string fromplayer = message.reader ().ReadUTF ();
		string toplayer = message.reader ().ReadUTF ();
		int sizePhomGui = message.reader ().ReadInt ();
		int[] phomgui = new int[sizePhomGui];
		for (int i = 0; i < sizePhomGui; i++) {
			phomgui [i] = message.reader ().ReadByte ();
		}
		int sizeCardGui = message.reader ().ReadInt ();
		int[] cardgui = new int[sizeCardGui];
		for (int i = 0; i < sizeCardGui; i++) {
			cardgui [i] = message.reader ().ReadByte ();
		}
		((PhomControl)GameControl.instance.CurrentCasino).OnAttachCard (fromplayer, toplayer, phomgui, cardgui);
	}

	public void OnPhomha (Message message)
	{
		((PhomControl)GameControl.instance.CurrentCasino).OnPhomha (message);
	}

	#endregion

	#region Mau Binh

	public void OnRankMauBinh (Message message)
	{
		((MauBinhControl)GameControl.instance.CurrentCasino).OnRankMauBinh (message);
	}

	#endregion
}