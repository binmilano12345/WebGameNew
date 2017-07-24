using UnityEngine;
using System.Collections;
using System;
using AppConfig;

public class PHandler : MessageHandler {
    private static IChatListener listenner;
    private static PHandler instance;

    public PHandler() {

    }
    public static PHandler getInstance() {
        if (instance == null)
            instance = new PHandler();
        return instance;
    }

    public static void setListenner(ListernerServer listener) {
        listenner = listener;
    }

    protected override void serviceMessage(Message message, int messageId) {
        try {
            switch (messageId) {
                case CMDClient.CMD_FIRE_CARD: 
                    int card = message.reader().ReadByte();
                    if (card == -1) {
                        GameControl.instance.CurrentCasino.OnFireCardFail();
                    } else {
                        string nick = message.reader().ReadUTF();
                        string turnName = message.reader().ReadUTF();
                        GameControl.instance.CurrentCasino.OnFireCard(nick, turnName, new int[] { card });
                    }
                    break;
			case CMDClient.CMD_MOM://khong dung
				string from = message.reader().ReadUTF();
//				listenner.onInfoMom(from);
				break;
                case CMDClient.CMD_EAT_CARD:
                        listenner.OnEatCardSuccess(message);
                    break;
                case CMDClient.CMD_BALANCE:
                        listenner.OnBalanceCard(message);
                    break;
                case CMDClient.CMD_DROP_PHOM:
				//vi du co phom laf 444, 555
				//se gui theo thu tu: 444, 555
                        listenner.OnDropPhomSuccess(message);
                    break;
                case CMDClient.CMD_GUI_CARD:
                        listenner.OnAttachCard(message);
                    break;
			case CMDClient.CMD_CHANGERULETBL:
				listenner.OnChangeRuleTbl(message);
				break;
//                case CMDClient.CMD_U:
                //    // SerializerHelper.readInt(message);
                //    message.reader().readByte();
                //    listenner.onInfoU(message.reader().readUTF());
                //    break;
                case CMDClient.CMD_PHOM_HA:
                        listenner.OnPhomha(message);
                    break;
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
