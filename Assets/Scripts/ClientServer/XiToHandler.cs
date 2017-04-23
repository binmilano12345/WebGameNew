using UnityEngine;
using System.Collections;
using System;

public class XiToHandler : MessageHandler {
    public static XiToHandler getInstance() {
        if (instance == null) {
            instance = new XiToHandler();
        }
        return instance;
    }

    private static XiToHandler instance = null;
    private static IChatListener listenner;
    public static void setListenner(ListernerServer listener) {
        listenner = listener;
    }

    protected override void serviceMessage(Message message, int messageId) {
        try {
            switch (messageId) {
                default:
                    Debug.Log("Khong vao cau lenh naooooooooooooo ");
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
