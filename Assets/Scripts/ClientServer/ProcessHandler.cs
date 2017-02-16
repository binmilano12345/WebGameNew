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
                        string a = message.reader().ReadUTF();
                        break;
                    case CMDClient.CMD_SERVER_MESSAGE:
                        listenner.OnMessageServer(message);
                        break;
                    case CMDClient.CMD_LOGIN:
                        listenner.OnLogin(message);
                        break;
                    case CMDClient.CMD_POPUP_NOTIFY:
                        listenner.OnPopupNotify(message);
                        break;
                    case CMDClient.CMD_PROFILE:
                        listenner.OnProfile(message);
                        break;
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
                        //card = message.reader().readShort();
                        //if (card == -1) {
                        //    listenner.onJoinRoomFail(message.reader().readUTF());
                        //} else {
                        //    listenner.onListTable(card, message);
                        //}

                        listenner.OnJoinRoom(message);
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
