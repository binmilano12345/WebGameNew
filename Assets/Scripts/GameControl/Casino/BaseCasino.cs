using AppConfig;
using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;
using DG.Tweening;
using System.Xml.Linq;
using System.Linq;

public abstract class BaseCasino : MonoBehaviour
{
	protected int MaxUser = 4;
	protected List<BasePlayer> ListPlayer = new List<BasePlayer> ();
	internal BasePlayer playerMe;
	[SerializeField]
	Transform[] tf_invite;
	[SerializeField]
	Transform tf_parent_player;
	[SerializeField]
	Text txt_id_table, txt_bet_table, txt_game_name;

	internal bool isPlaying = false;

	internal void Start ()
	{
		for (int i = 1; i < tf_invite.Length; i++) {
			tf_invite [i].GetComponent<UIButton> ()._onClick.AddListener (OnClickInvite);
		}
		UnloadSceneGame ();
	}

	public void UnloadSceneGame ()
	{
		StartCoroutine (WaitUnloadSceneGame ());
	}

	IEnumerator WaitUnloadSceneGame ()
	{
		yield return new WaitForEndOfFrame ();
		GameControl.instance.UnloadScene (SceneName.SCENE_ROOM);
		GameControl.instance.UnloadScene (SceneName.SCENE_LOBBY);
		//		GameControl.instance.UnloadScene (SceneName.SCENE_MAIN);
		GameControl.instance.UnloadSubScene ();
		PopupAndLoadingScript.instance.OnHideAll ();
	}

	void OnClickInvite ()
	{
		PopupAndLoadingScript.instance.ShowLoading ();
		SendData.onGetListInvite ();
	}

	#region Xu li trong game

	internal void OnChat (string nick, string msg)
	{
		BasePlayer pl = GetPlayerWithName (nick);
		if (pl != null) {
			pl.SetChat (msg);
		}
	}

	internal virtual void OnJoinTablePlaySuccess (Message message)
	{
		short idTable = message.reader ().ReadShort ();
		long betMoney = message.reader ().ReadLong ();
		long needMoney = message.reader ().ReadLong ();
		long maxMoney = message.reader ().ReadLong ();

		txt_id_table.text = "Bàn " + idTable;
		txt_bet_table.text = "Mức cược " + "<color=yellow>" + betMoney + "</color>";
		txt_game_name.text = GameConfig.GameName [GameConfig.CurrentGameID];

		try {
			int luatPhom = message.reader ().ReadByte ();
			SetLuatChoi (luatPhom);
			string master = message.reader ().ReadUTF ();
			int len = message.reader ().ReadByte ();
			GameControl.instance.TimerTurnInGame = message.reader ().ReadInt ();
			isPlaying = message.reader ().ReadBoolean ();
			for (int i = 0; i < len; i++) {
				PlayerData pl = new PlayerData ();
				pl.Name = message.reader ().ReadUTF ();
				pl.DisplaName = message.reader ().ReadUTF ();
				pl.Avata_Link = message.reader ().ReadUTF ();
				pl.Avata_Id = message.reader ().ReadInt ();
				pl.SitOnSever = message.reader ().ReadByte ();
				pl.Money = message.reader ().ReadLong ();
				pl.IsReady = message.reader ().ReadBoolean ();
				pl.FolowMoney = message.reader ().ReadLong ();
				pl.IsMaster = pl.Name.Equals (master);
				if (isPlaying) {
					pl.IsReady = false;
				}
				GameObject objPlayer = Instantiate (GameControl.instance.objPlayer);
				objPlayer.transform.SetParent (tf_parent_player);
				BasePlayer plUI = objPlayer.GetComponent<BasePlayer> ();
				plUI.SetInfo (pl.Name, pl.Money, pl.IsMaster, pl.IsReady, pl.Avata_Id);
				if (pl.Name.Equals (ClientConfig.UserInfo.UNAME)) {
					playerMe = plUI;
				}
				objPlayer.SetActive (false);
				var match = ListPlayer.FirstOrDefault(item => item.NamePlayer== pl.Name);
				//ListPlayer.Any (item=>item.NamePlayer == pl.Name

				if (match == null) {
					ListPlayer.Add (plUI);
				} else {
					Destroy (plUI);
				}
			}

			OnJoinTableSuccess (master);
			SortSitPlayer ();
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal virtual void OnFireCardFail ()
	{
	}

	internal virtual void OnFinishTurn ()
	{
	}

	internal virtual void OnInfome (Message message)
	{
	}

	internal virtual void OnFinishGame (Message message)
	{
		isPlaying = false;
		int total = message.reader ().ReadByte ();
		for (int i = 0; i < total; i++) {
			string nick = message.reader ().ReadUTF ();
			int rank = message.reader ().ReadByte ();
			if (rank != 1 && rank != 5) {
			}
			long money = message.reader ().ReadLong ();
			string dau = "";
			if (rank == 1 || rank == 5) {
				dau = "+";
			}
			BasePlayer pl = GetPlayerWithName (nick);
			if (pl != null) {
				pl.SetEffect (dau + MoneyHelper.FormatMoneyNormal (money));
				pl.SetRank (rank);
				pl.IsReady = false;
			}
		}
	}

	//protected String[] luatchoi = new String[] { "TÁI GỬI", "KHÔNG TÁI GỬI" };
	internal virtual void SetLuatChoi (int rule)
	{

		//if (screen.game.gameID == GameID.PHOM)
		//    luatChoi.setText(luatchoi[readByte]);
	}

	internal virtual void OnJoinTableSuccess (string master)
	{

	}

	internal virtual void OnJoinTableSuccess (Message message)
	{

	}

	public void OnJoinView (Message message)
	{
		try {
			//    BaseInfo.gI().isView = true;
			//    resetData();
			//    for (int i = 0; i < players.length; i++) {
			//        players[i].setExit();
			//    }

			int rule = message.reader ().ReadByte ();
			SetLuatChoi (rule);
			string master = message.reader ().ReadUTF ();
			//    masterID = master;
			int len = message.reader ().ReadByte ();
			GameControl.instance.TimerTurnInGame = message.reader ().ReadInt ();
			isPlaying = message.reader ().ReadBoolean ();
			for (int i = 0; i < len; i++) {
				PlayerData pl = new PlayerData ();
				pl.Name = message.reader ().ReadUTF ();
				pl.DisplaName = message.reader ().ReadUTF ();
				pl.Avata_Link = message.reader ().ReadUTF ();
				pl.Avata_Id = message.reader ().ReadInt ();
				pl.SitOnSever = message.reader ().ReadByte ();
				pl.Money = message.reader ().ReadLong ();
				pl.IsReady = message.reader ().ReadBoolean ();
				pl.FolowMoney = message.reader ().ReadLong ();
				pl.IsMaster = pl.Name.Equals (master);
				if (isPlaying) {
					pl.IsReady = false;
				}

				GameObject objPlayer = Instantiate (GameControl.instance.objPlayer);
				objPlayer.transform.SetParent (tf_parent_player);
				BasePlayer plUI = objPlayer.GetComponent<BasePlayer> ();
				plUI.SetInfo (pl.Name, pl.Money, pl.IsMaster, pl.IsReady, pl.Avata_Id);
				if (pl.Name.Equals (ClientConfig.UserInfo.UNAME)) {
					playerMe = plUI;
				}
				objPlayer.SetActive (false);
				var match = ListPlayer.FirstOrDefault(item => item.NamePlayer== pl.Name);
				//ListPlayer.Any (item=>item.NamePlayer == pl.Name

				if (match == null) {
					ListPlayer.Add (plUI);
				} else {
					Destroy (plUI);
				}
				OnJoinTableSuccess (master);
				//    screen.dialogWaitting.onShow();
				//    if (len < BaseInfo.gI().numberPlayer)
				//        SendData.onJoinTable(BaseInfo.gI().mainInfo.nick, BaseInfo.gI().idTable, "", -1);

				//} catch (IOException ex) {
				//    ex.printStackTrace();
				//}
				//try {
				//    setTableName("Phòng ");
			}
			SortSitPlayer ();
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal void OnUserExitTable (string nick, string master)
	{
		if (nick.Equals (ClientConfig.UserInfo.UNAME)) {
			//LoadAssetBundle.LoadScene(SceneName.SCENE_ROOM, SceneName.SCENE_ROOM, () => {
			GameControl.instance.CurrentCasino = null;
			//});
		} else {
			BasePlayer pl = GetPlayerWithName (nick);
			if (pl != null) {
				tf_invite [pl.SitOnClient].gameObject.SetActive (true);
				Destroy (pl.gameObject);
				ListPlayer.Remove (pl);
			}
			BasePlayer plMaster = GetPlayerWithName (master);
			if (plMaster != null) {
				plMaster.SetShowReady (false);
				plMaster.IsMaster = true;
				plMaster.SetShowMaster (true);
			}
		}
		SortSitPlayer (true);
	}

	internal void OnUserJoinTable (Message message)
	{
		int tbid = message.reader ().ReadShort ();

		PlayerData pl = new PlayerData ();
		pl.Name = message.reader ().ReadUTF ();
		pl.DisplaName = message.reader ().ReadUTF ();
		pl.Avata_Link = message.reader ().ReadUTF ();
		pl.Avata_Id = message.reader ().ReadInt ();
		pl.SitOnSever = message.reader ().ReadByte ();
		pl.Money = message.reader ().ReadLong ();
		pl.FolowMoney = message.reader ().ReadLong ();
		pl.IsMaster = false;
		pl.IsReady = false;

		GameObject objPlayer = Instantiate (GameControl.instance.objPlayer);
		objPlayer.transform.SetParent (tf_parent_player);
		BasePlayer plUI = objPlayer.GetComponent<BasePlayer> ();
		plUI.SetInfo (pl.Name, pl.Money, pl.IsMaster, pl.IsReady, pl.Avata_Id);
//		if (pl.Name.Equals (ClientConfig.UserInfo.UNAME)) {
//			playerMe = plUI;
//			indexMe = i;
//		}
		objPlayer.SetActive (false);
		var match = ListPlayer.FirstOrDefault(item => item.NamePlayer== pl.Name);
		//ListPlayer.Any (item=>item.NamePlayer == pl.Name

		if (match == null) {
			ListPlayer.Add (plUI);
		} else {
			Destroy (plUI);
		}
		SortSitPlayer (true);
	}

	internal void InfoCardPlayerInTbl (Message message)
	{
		try {
			string turnName = message.reader ().ReadUTF ();
			int time = message.reader ().ReadInt ();
			sbyte numP = message.reader ().ReadByte ();
			InfoCardPlayerInTbl (message, turnName, time, numP);
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	internal virtual void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP)
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			ListPlayer [i].IsPlaying = false;
		}
	}

	internal virtual void OnReady (string nick, bool ready)
	{
		BasePlayer pl = GetPlayerWithName (nick);
		if (pl != null) {
			pl.IsReady = ready;
			pl.SetShowReady (ready);
		}
	}

	internal virtual void StartTableOk (int[] cardHand, Message msg, string[] nickPlay)
	{
		GameControl.instance.TimerTurnInGame = 20;
		for (int i = 0; i < nickPlay.Length; i++) {
			BasePlayer pl = GetPlayerWithName (nickPlay [i]);
			if (pl != null) {
				pl.IsReady = false;
				pl.SetShowReady (false);
				pl.IsPlaying = true;
			}
		}
	}

	internal virtual void OnStartForView (string[] playingName, Message msg)
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			ListPlayer [i].SetShowReady (false);
			ListPlayer [i].IsPlaying = false;
		}
		for (int i = 0; i < playingName.Length; i++) {
			BasePlayer pl = GetPlayerWithName (playingName [i]);
			if (pl != null) {
				pl.IsPlaying = (true);
			}
			//if (playingName[i].equals(BaseInfo.gI().mainInfo.nick)) {
			//    MainInfo.setPlayingUser(false);
			//}
		}
	}

	internal virtual void OnStartFail ()
	{
	}

	internal virtual void SetTurn (string nick, Message message)
	{
		if (string.IsNullOrEmpty (nick)) {
			return;
		}
		for (int i = 0; i < ListPlayer.Count; i++) {
			ListPlayer [i].SetTurn (0);
		}
		BasePlayer plTurn = GetPlayerWithName (nick);
		if (plTurn != null) {
			plTurn.SetTurn (GameControl.instance.TimerTurnInGame);
		}
	}

	internal virtual void OnFireCard (string nick, string turnName, int[] card)
	{
		BasePlayer plFire = GetPlayerWithName (nick);
		if (plFire != null) {
			plFire.SetTurn (0);
		}
		BasePlayer plTurn = GetPlayerWithName (nick);
		if (plTurn != null) {
			plTurn.SetTurn (GameControl.instance.TimerTurnInGame);
		}
		SetTurn (turnName, null);
	}

	internal virtual void OnNickSkip (string nick, string turnname2)
	{
		BasePlayer plTurn = GetPlayerWithName (nick);
		if (plTurn != null) {
			plTurn.SetEffect (ClientConfig.Language.GetText ("ingame_leave"));
			plTurn.SetTurn (0);
		}
	}

	internal void OnNickSkip (string nick, Message msg)
	{
		BasePlayer plTurn = GetPlayerWithName (nick);
		if (plTurn != null) {
			plTurn.SetEffect (ClientConfig.Language.GetText ("ingame_leave"));
			plTurn.SetTurn (0);
			//BaseInfo.gI().media_countdown.pause();
		}
	}

	internal virtual void SetMaster (string nick)
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			ListPlayer [i].IsMaster = false;
			ListPlayer [i].SetShowMaster (false);
		}
		BasePlayer plMaster = GetPlayerWithName (nick);
		if (plMaster != null) {
			plMaster.IsMaster = true;
			plMaster.IsReady = true;
			plMaster.SetShowMaster (true);
			playerMe.SetShowReady (false);
		}
	}

	internal void OnUpdateMoneyTbl (Message message)
	{
		try {
			int size = message.reader ().ReadByte ();
			string _name = "";
			long money = 0;
			long folowMoney = 0;
			for (int i = 0; i < size; i++) {
				 _name = message.reader ().ReadUTF ();
				 money = message.reader ().ReadLong ();
				 folowMoney = message.reader ().ReadLong ();
				bool isGetMoney = message.reader ().ReadBoolean ();
				BasePlayer pl = GetPlayerWithName (_name);
				if(pl!= null){
				pl.SetMoney (folowMoney);
				if (!isGetMoney) {
					pl.SetEffect (MoneyHelper.FormatMoneyNormal (folowMoney));
				}
				if (name.Equals (ClientConfig.UserInfo.UNAME)) {
					ClientConfig.UserInfo.CASH_FREE = money;
					pl.SetMoney (money);
				}
				}
			}
		} catch (Exception ex) {
			Debug.LogError (ex);
		}

	}

	internal virtual void AllCardFinish (string nick, int[] card)
	{

	}

	#endregion

	#region Xep cho ngoi

	void SortSitPlayer (bool issEffect = false)
	{
		int indexMe = 0;
		int j = 1;
		int i = 0;
		for (i = 0; i < ListPlayer.Count; i++) {
			if (ListPlayer [i].NamePlayer.Equals (ClientConfig.UserInfo.UNAME)) {
				indexMe = i;
				playerMe = ListPlayer [i];
				ListPlayer [i].SitOnClient = 0;
				break;
			}
		}
		if (playerMe != null) {
			playerMe.transform.localPosition = tf_invite [0].localPosition;
			playerMe.SitOnClient = 0;
			playerMe.gameObject.SetActive (true);
			playerMe.name = ClientConfig.UserInfo.UNAME;
		}
		for (i = indexMe + 1; i < ListPlayer.Count; i++) {
			if (j >= tf_invite.Length)
				break;
			ListPlayer [i].transform.localPosition = tf_invite [j].localPosition;
			tf_invite [j].gameObject.SetActive (false);
			ListPlayer [i].gameObject.SetActive (true);
			ListPlayer [i].SitOnClient = j;
			ListPlayer [i].name = ListPlayer [i].NamePlayer;
			j++;
		}
		j = tf_invite.Length - 1;
		for (i = indexMe - 1; i >= 0; i--) {
			if (j <= 0)
				break;
			ListPlayer [i].transform.localPosition = tf_invite [j].localPosition;
			tf_invite [j].gameObject.SetActive (false);
			ListPlayer [i].gameObject.SetActive (true);
			ListPlayer [i].SitOnClient = j;
			ListPlayer [i].name = ListPlayer [i].NamePlayer;
			j--;
		}
		switch (GameConfig.CurrentGameID) {
		case GameID.TLMN:
		case GameID.TLMNSL:
			InitPlayerTLMN ();
			break;
		case GameID.SAM:
			InitPlayerSam ();
			break;
		case GameID.PHOM:
			InitInfoPlayer_PHOM ();
			break;
		}
	}
	#region Init Player TLMN
	void InitPlayerTLMN ()
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			TLMNPlayer pl = (TLMNPlayer)ListPlayer [i];
			pl.CardHand.CardCount = 13;
			switch (pl.SitOnClient) {
			case 0:
				pl.CardHand.isSmall = false;
				pl.CardHand.isTouched = true;
				pl.CardHand.align_Anchor = Align_Anchor.CENTER;
				pl.CardHand.MaxWidth = 800;
				pl.SetPositionChatLeft (true);
				pl.CardHand.Init ();
				pl.CardHand.SetInputChooseCard ();
				break;
			case 1:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.RIGHT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (false);
				pl.SetPositionChatAction (Align_Anchor.RIGHT);
				pl.CardHand.Init ();
				break;
			case 2:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.LEFT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (true);
				pl.SetPositionChatAction (Align_Anchor.BOT);
				pl.CardHand.Init ();
				break;
			case 3:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.LEFT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (true);
				pl.SetPositionChatAction (Align_Anchor.LEFT);
				pl.CardHand.Init ();
				break;
			}
		}
	}
	#endregion
	#region Init Player Sam
	void InitPlayerSam ()
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			SamPlayer pl = (SamPlayer)ListPlayer [i];
			pl.CardHand.CardCount = 10;
			switch (pl.SitOnClient) {
			case 0:
				pl.CardHand.isSmall = false;
				pl.CardHand.isTouched = true;
				pl.CardHand.align_Anchor = Align_Anchor.CENTER;
				pl.CardHand.MaxWidth = 800;
				pl.SetPositionChatLeft (true);
				pl.CardHand.Init ();
				pl.CardHand.SetInputChooseCard ();
				break;
			case 1:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.RIGHT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (false);
				pl.SetPositionChatAction (Align_Anchor.RIGHT);
				pl.CardHand.Init ();
				break;
			case 2:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.LEFT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (true);
				pl.SetPositionChatAction (Align_Anchor.BOT);
				pl.CardHand.Init ();
				break;
			case 3:
				pl.CardHand.isSmall = true;
				pl.CardHand.isTouched = false;
				pl.CardHand.align_Anchor = Align_Anchor.LEFT;
				pl.CardHand.MaxWidth = 500;
				pl.SetPositionChatLeft (true);
				pl.SetPositionChatAction (Align_Anchor.LEFT);
				pl.CardHand.Init ();
				break;
			}
		}
	}
	#endregion
	#region Init Player Phom
	public void InitInfoPlayer_PHOM ()
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			PhomPlayer pl = (PhomPlayer)ListPlayer [i];

			switch (pl.SitOnClient) {
			case 0:
				pl.cardTaLaManager.Init (true);
				pl.cardTaLaManager.SetDragDropCard ();
				pl.cardTaLaManager.SetPositionArryCard (Align_Anchor.CENTER, Align_Anchor.LEFT, Align_Anchor.CENTER, pl.SitOnClient);
				pl.SetPositionChatLeft (true);
				break;
			case 1:
				pl.cardTaLaManager.Init (false);
				pl.SetPositionChatLeft (false);
				pl.cardTaLaManager.SetPositionArryCard (Align_Anchor.RIGHT, Align_Anchor.RIGHT, Align_Anchor.RIGHT, pl.SitOnClient);
				pl.SetPositionChatAction (Align_Anchor.RIGHT);
				break;
			case 2:
				pl.cardTaLaManager.Init (false);
				pl.cardTaLaManager.SetPositionArryCard (Align_Anchor.LEFT, Align_Anchor.RIGHT, Align_Anchor.CENTER, pl.SitOnClient);
				pl.SetPositionChatLeft (false);
				pl.SetPositionChatAction (Align_Anchor.BOT);
				break;
			case 3:
				pl.cardTaLaManager.Init (false);
				pl.cardTaLaManager.SetPositionArryCard (Align_Anchor.LEFT, Align_Anchor.LEFT, Align_Anchor.LEFT, pl.SitOnClient);
				pl.SetPositionChatLeft (true);
				pl.SetPositionChatAction (Align_Anchor.LEFT);
				break;
			}
		}
	}
	#endregion
	#endregion

	internal BasePlayer GetPlayerWithName (string nick)
	{
		for (int i = 0; i < ListPlayer.Count; i++) {
			if (ListPlayer [i].NamePlayer.Equals (nick)) {
				return ListPlayer [i];
			}
		}
		return null;
	}

	#region Button Click

	public void OnClickBack ()
	{
		PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("popup_quittable"), () => {
			SendData.onOutTable ();
			PopupAndLoadingScript.instance.ShowLoading ();
		});
	}

	public void OnClickChat ()
	{
		LoadAssetBundle.LoadScene (SceneName.SUB_CHAT, SceneName.SUB_CHAT);
	}

	public void OnClickSetting ()
	{
		LoadAssetBundle.LoadScene (SceneName.SUB_SETTING, SceneName.SUB_SETTING);
	}

	#endregion


	public void DemoNguoiVao ()
	{
		PlayerData pl = new PlayerData ();
		pl.Name = "ten";
		pl.DisplaName = "Ten dep";
		pl.Avata_Link = "";
		pl.Avata_Id = UnityEngine.Random.Range (0, 30);
		pl.SitOnSever = 1;
		pl.Money = 9999;
		pl.IsReady = true;
		pl.FolowMoney = 9999;

		GameObject objPlayer = Instantiate (GameControl.instance.objPlayer);
		objPlayer.transform.SetParent (tf_parent_player);
		BasePlayer plUI = objPlayer.GetComponent<BasePlayer> ();
		plUI.SetInfo (pl.Name, pl.Money, pl.IsMaster, pl.IsReady, pl.Avata_Id);
		objPlayer.SetActive (false);
		if (!ListPlayer.Contains (plUI)) {
			ListPlayer.Add (plUI);
		} else {
			Destroy (plUI);
		}
		SortSitPlayer ();
	}
}
