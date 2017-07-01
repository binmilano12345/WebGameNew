using UnityEngine;
using System.Collections;
using System;
using AppConfig;

public class LiengHandler : MessageHandler {
    private static IChatListener listenner;
    private static LiengHandler instance;

    public static LiengHandler getInstance() {
        if (instance == null) {
            instance = new LiengHandler();
        }
        return instance;
    }

    public static void setListenner(ListernerServer listener) {
        listenner = listener;
    }

    protected override void serviceMessage(Message message, int messageId) {
        try {
            switch (messageId) {
                case CMDClient.CMD_FIRE_CARD:
                    break;
                case CMDClient.CMD_PASS:
                    break;
                default:
                    break;
            }
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public override void onConnectionFail() {
    }

    public override void onDisconnected() {
    }

    public override void onConnectOk() {
    }
}
