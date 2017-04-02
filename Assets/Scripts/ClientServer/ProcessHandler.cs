using UnityEngine;
using System.Collections;
using System;
using AppConfig;

public class ProcessHandler : MessageHandler {
    protected override void serviceMessage(Message message, int messageId) {
		try {
            DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
                switch (messageId) {
                    case CMDClient.CMD_GETPHONECSKH:
					listenner.OnGetPhoneCSKH (message);
                        break;
                    case CMDClient.CMD_SERVER_MESSAGE:
                        listenner.OnMessageServer(message.reader().ReadUTF());
                        break;
                    case CMDClient.CMD_LOGIN:
                        listenner.OnLogin(message);
                        break;
				case CMDClient.CMD_GET_PASS:
					listenner.OnGetPass(message);
					break;
                    case CMDClient.CMD_REGISTER:
                        listenner.OnRegister(message);
                        break;
                    case CMDClient.CMD_POPUP_NOTIFY:
                        listenner.OnPopupNotify(message);
                        break;
                    case CMDClient.CMD_PROFILE:
                        listenner.OnProfile(message);
                        break;
				case CMDClient.CMD_CHANGE_NAME:
					listenner.OnChangeName(message);
					break;
				case CMDClient.CMD_UPDATE_AVATA:
					listenner.OnChangeAvatar(message);
					break;		
				case CMDClient.CMD_GET_FREE_MONEY:
					listenner.OnMoneyFree(message.reader().ReadLong());
					break;
//				case CMDClient.PA:
//					listenner.OnChangeAvatar(message);
//					break;
                    case CMDClient.CMD_RATE_SCRATCH_CARD:
                        listenner.OnRateScratchCard(message);
                        break;
                    case CMDClient.CMD_LIST_BET_MONEY:
                        listenner.OnListBetMoney(message);
                        break;
                    case CMDClient.CMD_LIST_PRODUCT:
                        listenner.OnListProduct(message);
                        break;
                    case CMDClient.CMD_TOP:
                        listenner.OnTop(message);
                        break;
                    case CMDClient.CMD_GET_INBOX_MESSAGE:
                        listenner.OnInboxMessage(message);
                        break;
                    case CMDClient.CMD_ALERT_LINK:
                        listenner.OnGetAlertLink(message);
                        break;
                    case CMDClient.CMD_SMS:
                        listenner.OnInfoSMS(message);
                        break;
                    case CMDClient.CMD_HIDE_NAPTIEN:
                        int isHidexuchip = message.reader().ReadInt();
                        break;
                    case CMDClient.CMD_SMS_9029:
                        listenner.OnSMS9029(message);
                        break;
                    case CMDClient.CMD_JOIN_GAME:
                        listenner.OnJoinGame(message);
                        break;
                    case CMDClient.CMD_LIST_TABLE:
                        listenner.OnJoinRoom(message);
                        break;
                    case CMDClient.CMD_UPDATE_ROOM:
                        listenner.OnUpdateRoom(message);
                        break;
				case CMDClient.CMD_ID_GAME:
					listenner.OnGameID(message);
					break;
                    case CMDClient.CMD_JOIN_TABLE_PLAY:
                        listenner.OnJoinTablePlay(message);
                        break;
				case CMDClient.CMD_USER_JOIN_TABLE:
					if (GameControl.instance.CurrentCasino == null) {
						GameControl.instance.ListCMDID.Add(messageId);
						GameControl.instance.ListMsg.Add(message);
					} else
						listenner.OnUserJoinTable(message);
							break;
					break;
				case CMDClient.CMD_UPDATEMONEY_PLAYER_INTBL:	
					if (GameControl.instance.CurrentCasino == null) {
						GameControl.instance.ListCMDID.Add(messageId);
						GameControl.instance.ListMsg.Add(message);
				} else
					listenner.OnUpdateMoneyTbl(message);
					break;
                    case CMDClient.CMD_EXIT_TABLE:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnUserExitTable(message);
                        break;
                    case CMDClient.CMD_INFOPLAYER_TBL:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.InfoCardPlayerInTbl(message);
                        break;
                    case CMDClient.CMD_READY:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnReady(message);
                        break;
                    case CMDClient.CMD_CHAT_MSG:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnChat(message);
                        break;
                    case CMDClient.CMD_START_GAME:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else {
                            int by = message.reader().ReadByte();
                            if (by == 0) {
                                string info = message.reader().ReadUTF();
                                listenner.OnStartFail(info);
                            } else if (by == 1) {
                                listenner.OnStartSuccess(message);
                            } else if (by == 2) {
                                listenner.OnStartForView(message);
                            }
                        }
                        break;
                    case CMDClient.CMD_SET_NEW_MASTER:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnSetNewMaster(message.reader().ReadUTF());
                        break;
                    case CMDClient.CMD_GAMEOVER:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnFinishGame(message);
                        break;
                    case CMDClient.CMD_ALLCARD_FINISH:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnAllCardPlayerFinish(message);
                        break;
                    case CMDClient.CMD_FINISHTURNTLMN:
                        if (GameControl.instance.CurrentCasino == null) {
                            GameControl.instance.ListCMDID.Add(messageId);
                            GameControl.instance.ListMsg.Add(message);
                        } else
                            listenner.OnFinishTurnTLMN(message);
                        break;
                    case CMDClient.CMD_SET_TURN:
                        listenner.OnSetTurn(message);
                        break;
                    case CMDClient.CMD_LIST_INVITE:
                        listenner.OnListInvite(message);
                        break;
                    case CMDClient.CMD_INVITE_FRIEND:// nhan loi moi tu a vao tbid
                        listenner.OnInvite(message);
                        break;
                    case CMDClient.CMD_BAO_SAM:
                        listenner.OnBaoSam(message);
                        break;
				case CMDClient.CMD_CALMB_RANKS:
					listenner.OnRankMauBinh(message);
					break;
                    default:
                        if (secondHandler != null) {
                            secondHandler.processMessage(message);
                        }
                        break;
                }
            });
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public override void onConnectionFail() {
        throw new System.NotImplementedException();
    }

    public override void onDisconnected() {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            listenner.onDisConnect();
        });

    }

    public override void onConnectOk() {
        Debug.Log("Connect OK...");
    }

    private static ProcessHandler instance;
    int send = 0;
    static int step;
    private static IChatListener listenner;

    public ProcessHandler() {

    }

    public static ProcessHandler getInstance() {
        if (instance == null) {
            instance = new ProcessHandler();
        }

        return instance;
    }

    public static void setListenner(ListernerServer listener) {
        listenner = listener;
    }

    public static void setSecondHandler(MessageHandler handler) {
        secondHandler = null;
        secondHandler = handler;
    }

    private static MessageHandler secondHandler;
}
