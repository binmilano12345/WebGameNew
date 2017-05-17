using UnityEngine;
using System.Collections;
using System;
using AppConfig;

public class XocDiaHandler : MessageHandler
{
	private static IChatListener listenner;
	private static XocDiaHandler instance;

	public XocDiaHandler ()
	{
	}

	public static XocDiaHandler getInstance ()
	{
		if (instance == null) {
			instance = new XocDiaHandler ();
		}
		return instance;
	}

	public static void setListenner (ListernerServer listener)
	{
		listenner = listener;
	}

	protected override void serviceMessage (Message message, int messageId)
	{
		try {
			switch (messageId) {
			case CMDClient.CMD_BEGIN_XOCDIA:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnBeGinXocDia (message.reader ().ReadByte ());
				break;
			case CMDClient.CMD_BEGIN_XOCDIA_CUOC:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnBeGinXocDia_TG_DatCuoc (message.reader ().ReadByte ());
				break;
			case CMDClient.CMD_MO_BAT:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnBeGinXocDia_MoBat (message.reader ().ReadByte ());
				break;
			case CMDClient.CMD_XOCDIA_DATCUOC:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_DatCuoc (message);
				break;
			case CMDClient.CMD_ARR_BET_XD:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnNhanCacMucCuocXD (message);
				break;
			case CMDClient.CMD_UPDATE_CUA:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDiaUpdateCua (message);
				break;
			case CMDClient.CMD_HUYCUOC:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDiaHuyCuoc (message);
				break;
			case CMDClient.CMD_HISTORY_XD:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_LichSu (message);
				break;
			case CMDClient.CMD_CHUCNANG_HUYCUA:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_HuyCua_LamCai (message);
				break;
			case CMDClient.CMD_BEGIN_XOCDIA_DUNGCUOC:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_TG_DungCuoc (message);
				break;
			case CMDClient.CMD_GAPDOI:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_DatX2 (message);
				break;
			case CMDClient.CMD_DATLAI:
				if (GameControl.instance.CurrentCasino == null) {
					GameControl.instance.ListCMDID.Add (messageId);
					GameControl.instance.ListMsg.Add (message);
				} else
					listenner.OnXocDia_DatLai (message);
				break;
			default:
				break;
			}
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public override void onConnectionFail ()
	{
	}

	public override void onDisconnected ()
	{
	}

	public override void onConnectOk ()
	{
	}
}
