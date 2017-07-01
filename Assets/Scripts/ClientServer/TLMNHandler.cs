using UnityEngine;
using System.Collections;
using System;
using AppConfig;

public class TLMNHandler : MessageHandler {
    private static IChatListener listenner;
    private static TLMNHandler instance;

    public static TLMNHandler getInstance() {
        if (instance == null) {
            instance = new TLMNHandler();
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
                    if (GameControl.instance.CurrentCasino == null) {
                        GameControl.instance.ListCMDID.Add(messageId);
                        GameControl.instance.ListMsg.Add(message);
                    } else {
                        int status = message.reader().ReadInt();
                        if (status == -1) {
                            GameControl.instance.CurrentCasino.OnFireCardFail();
                        } else {
                            string nick = message.reader().ReadUTF();
                            int size = message.reader().ReadInt();
                            byte[] cardfire = new byte[size];
                            message.reader().Read(cardfire, 0, size);
                            int[] data = new int[cardfire.Length];
                            for (int i = 0; i < data.Length; i++) {
                                data[i] = cardfire[i];
                            }
                            string turnName = message.reader().ReadUTF();
                            GameControl.instance.CurrentCasino.OnFireCard(nick, turnName, data);
                        }
                    }
                    break;
                case CMDClient.CMD_PASS:// bo luot
                    if (GameControl.instance.CurrentCasino == null) {
                        GameControl.instance.ListCMDID.Add(messageId);
                        GameControl.instance.ListMsg.Add(message);
                    } else
                        listenner.OnNickSkip(message.reader().ReadUTF(), message.reader().ReadUTF());
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
