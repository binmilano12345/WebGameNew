using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AppConfig;

public class SendData {
    public static void onSendSmsSyntax() {
        Message msg = new Message(CMDClient.CMD_SMS);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onAutoJoinTable() {
        try {
            Message msg = new Message(CMDClient.CMD_AUTOJOINTABLE);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onGetListInvite() {
        Message msg = new Message(CMDClient.CMD_LIST_INVITE);
        try {
            msg.writer().WriteShort((short)1);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetPhoneCSKH() {
        try {
            Message msg = new Message(CMDClient.CMD_GETPHONECSKH);
            msg.writer().WriteByte((byte)CMDClient.PROVIDER_ID);
            NetworkUtil.GI().connect(msg);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onGetPass(String nick) {
        Message msg = new Message(CMDClient.CMD_GET_PASS);
        try {
            msg.writer().WriteByte((byte)CMDClient.PROVIDER_ID);
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static Message onGetMessagePass(String nick) {
        Message msg = new Message(CMDClient.CMD_GET_PASS);
        try {
            msg.writer().WriteByte((byte)CMDClient.PROVIDER_ID);
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        return msg;
    }

    public static void onGopY(String st) {
        Message msg = new Message(CMDClient.CMD_GOP_Y);
        try {
            msg.writer().WriteUTF(st);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onViewInfoFriend(String nick) {
        Message msg = new Message(CMDClient.CMD_VIEW_INFO_FRIEND);
        try {
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSetMoneyTable(long money) {
        Message msg = new Message(CMDClient.CMD_SET_MONEY);
        try {
            msg.writer().WriteLong(money);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onDelItem(int idItem) {
        Message msg = new Message(CMDClient.CMD_USE_ITEM);
        try {
            msg.writer().WriteByte((byte)0);
            msg.writer().WriteShort((short)idItem);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendVersionImage(byte vs) {
        Message msg = new Message(CMDClient.CMD_REQUEST_GET_ALL_AVATAR);
        try {
            msg.writer().WriteByte(vs);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onUseItem(int idItem) {
        Message msg = new Message(CMDClient.CMD_USE_ITEM);
        try {
            msg.writer().WriteByte((byte)1);
            msg.writer().WriteShort((short)idItem);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetStore() {
        Message msg = new Message(CMDClient.CMD_USE_ITEM);
        try {
            msg.writer().WriteByte((byte)2);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendSkipTurn() {
        Message msg = new Message(CMDClient.CMD_PASS);
        try {
            msg.writer().WriteShort((short)1);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onJoinWaittingRoom(String nick) {
        Message msg = new Message(CMDClient.CMD_LIST_ROOM);
        try {
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onJoinRoom(String nick, int rid) {
        Message msg = new Message(CMDClient.CMD_LIST_TABLE);
        try {
            msg.writer().WriteByte((byte)rid);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onJoinTable(String nick, int tbid, String pass, long folowMoney) {
        Message msg = new Message(CMDClient.CMD_JOIN_TABLE);
        try {
            msg.writer().WriteShort((short)tbid);
            // msg.writer().WriteUTF(nick);
            msg.writer().WriteUTF(pass);
            msg.writer().WriteLong(folowMoney);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onJoinTableForView(int tbid, String pass) {
        Message msg = new Message(CMDClient.CMD_FOR_VIEW);
        try {
            msg.writer().WriteShort((short)tbid);
            msg.writer().WriteUTF(pass);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onOutTable() {
        Message msg = new Message(CMDClient.CMD_EXIT_TABLE);
        try {
            msg.writer().WriteShort(0);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(Integer.parseInt(tbid));

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onStartGame() {
        Message msg = new Message(CMDClient.CMD_START_GAME);
        try {
            msg.writer().WriteShort((short)1);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(tbid);

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendArrayPhom(int currentTableID, int[][] array) {
        Message msg = new Message(CMDClient.CMD_HA_PHOM_TAY);
        try {
            msg.writer().WriteShort((short)currentTableID);
            msg.writer().WriteByte((byte)array.Length);
            for (int i = 0; i < array.Length; i++) {
                byte[] card = new byte[array[i].Length];
                msg.writer().WriteInt(array[i].Length);
                for (int j = 0; j < card.Length; j++) {
                    card[j] = (byte)array[i][j];
                }
                // SerializerHelper.writeArrayInt(array[i]);
                msg.writer().Write(card, 0, card.Length);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onFireCard(int card) {
        Message msg = new Message(CMDClient.CMD_FIRE_CARD);
        try {
            msg.writer().WriteShort(1);
            msg.writer().WriteByte((byte)card);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onFireCardTL(int[] card) {
        Message msg = new Message(CMDClient.CMD_FIRE_CARD);
        try {
            msg.writer().WriteShort(1);
            // SerializerHelper.WriteInt(card);
            byte[] data = new byte[card.Length];
            for (int i = 0; i < data.Length; i++) {
                data[i] = (byte)card[i];
            }
            // SerializerHelper.writeArrayInt(card);
            msg.writer().WriteInt(data.Length);
            msg.writer().Write(data, 0, data.Length);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(Integer.parseInt(tbid));

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetCardNoc() {
        Message msg = new Message(CMDClient.CMD_GET_CARD);
        try {
            msg.writer().WriteShort(1);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(tbid);

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetCardFromPlayer() {
        Message msg = new Message(CMDClient.CMD_EAT_CARD);
        try {
            msg.writer().WriteShort(1);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(tbid);

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onLogOut() {
        Message msg = new Message(CMDClient.CMD_EXIT_GAME);
        try {
            msg.writer().WriteUTF(ClientConfig.UserInfo.UNAME);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onUpdateProfile(String email, String sdt) {
        Message msg = new Message(CMDClient.CMD_UPDATE_PROFILE);
        try {
            msg.writer().WriteUTF(email);
            msg.writer().WriteUTF(sdt);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onReady(int ready) {
        Message msg = new Message(CMDClient.CMD_READY);
        try {
            msg.writer().WriteShort(1);
            // SerializerHelper.WriteInt(ready);
            msg.writer().WriteByte((byte)ready);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(Integer.parseInt(tbid));

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onHaPhom(int[][] array) {
        try {
            Message msg = new Message(CMDClient.CMD_DROP_PHOM);
            if (array == null) {
                msg.writer().WriteShort(0);
            } else {
                msg.writer().WriteShort(1);
                msg.writer().WriteByte((byte)array.Length);
                for (int i = 0; i < array.Length; i++) {
                    byte[] card = new byte[array[i].Length];
                    msg.writer().WriteInt(array[i].Length);
                    for (int j = 0; j < card.Length; j++) {
                        card[j] = (byte)array[i][j];
                    }
                    msg.writer().Write(card, 0, card.Length);
                }
            }
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onToiBanSpecial(long money_toiban) {
        Message msg = new Message(CMDClient.CMD_SHOP_AVATAR);
        try {
            msg.writer().WriteLong(money_toiban);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onBuyAvatar(short idAvatar) {
        Message msg = new Message(CMDClient.CMD_BUY_AVATAR);
        try {
            msg.writer().WriteShort(idAvatar);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetProfile() {
        Message msg = new Message(CMDClient.CMD_PROFILE);
        try {
            msg.writer().WriteUTF(ClientConfig.UserInfo.UNAME);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetTop10Rich() {
        Message msg = new Message(CMDClient.CMD_TOP_RICH);
        // msg.writer().WriteUTF(Login.userName);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetTop10(int idgame) {
        Message msg = new Message(CMDClient.CMD_TOP_PLAYER);
        try {
            msg.writer().WriteByte((byte)idgame);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onInviteFriend(String nick_accept) {
        Message msg = new Message(CMDClient.CMD_INVITE_FRIEND);
        try {
            msg.writer().WriteUTF(nick_accept);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onAcceptInviteFriend(byte gameid, short tblid, long folowMoney) {
        Message msg = new Message(CMDClient.CMD_ANSWER_INVITE_FRIEND);
        try {
            msg.writer().WriteByte(gameid);
            msg.writer().WriteShort(tblid);
            msg.writer().WriteLong(folowMoney);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onKick(String nick) {
        Message msg = new Message(CMDClient.CMD_KICK);
        try {
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onUpdateWaitting() {
        Message msg = new Message(CMDClient.CMD_UPDATE_WAITTING_ROOM);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onUpdateRoom() {
        Message msg = new Message(CMDClient.CMD_UPDATE_ROOM);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onViewTable(int id) {
        Message msg = new Message(CMDClient.CMD_VIEW);
        try {
            msg.writer().WriteByte((byte)id);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSetTblPass(String pass) {
        Message msg = new Message(CMDClient.CMD_SET_PASSWORD);
        try {
            msg.writer().WriteShort(0);
            msg.writer().WriteUTF(pass);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendMsgChat(String stmsg) {
        Message msg = new Message(CMDClient.CMD_CHAT_MSG);
        try {
            msg.writer().WriteShort(1);
            msg.writer().WriteUTF(stmsg);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendPingPong() {
        Message msg = new Message(CMDClient.CMD_PING_PONG);
        try {
            msg.writer().WriteByte((byte)1);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendMsgChat(String toNick, String msgSend) {
        Message msg = new Message(CMDClient.CMD_CHAT);
        try {
            msg.writer().WriteUTF(toNick);
            msg.writer().WriteUTF(msgSend);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);

    }

    public static void onGetFriendList(String nick) {
        Message msg = new Message(CMDClient.CMD_FRIEND_LIST);
        try {
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetInboxMessage() {
        Message msg = new Message(CMDClient.CMD_GET_INBOX_MESSAGE);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendMessageToUser(String sender, String nick, String content) {
        Message msg = new Message(CMDClient.CMD_SEND_MESSAGE);
        try {
            msg.writer().WriteUTF(sender);
            msg.writer().WriteUTF(nick);
            msg.writer().WriteUTF(content);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onDelMessage(int id) {
        Message msg = new Message(CMDClient.CMD_DEL_MESSAGE);
        try {
            msg.writer().WriteInt(id);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onReadMessage(int id) {
        Message msg = new Message(CMDClient.CMD_READ_MESSAGE);
        try {
            msg.writer().WriteInt(id);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        // System.out.println("send get noi dung msg");
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendGameID(byte id) {
        Message msg = new Message(CMDClient.CMD_JOIN_GAME);
        try {
            msg.writer().WriteByte(id);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendMoneyCuoc(int tbID, long money) {
        Message msg = new Message(CMDClient.CMD_CUOC);
        try {
            msg.writer().WriteShort((short)tbID);
            msg.writer().WriteLong(money);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onAccepFollow() {
        Message msg = new Message(CMDClient.CMD_THEO);
        try {
            msg.writer().WriteShort(1);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onChangeRuleTbl() {
        Message msg = new Message(CMDClient.CMD_CHANGERULETBL);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onSendSmsChangePass(String old, String newp, String renewp) {

    }

    public static void onAddFriendChat(String nick) {
        Message msg = new Message(CMDClient.CMD_ADD_FRIEND_CHAT);
        try {
            msg.writer().WriteUTF(nick);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void sendKey() {
        NetworkUtil.GI().sendMessage(new Message((sbyte)-27));
    }

    public static void onCuocXT(int type, long money) {
        Message msg = new Message(CMDClient.CMD_CUOC);
        try {
            msg.writer().WriteInt(type);
            msg.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onGetInfoTable() {
        try {
            Message msg = new Message(CMDClient.CMD_INFOPLAYER_TBL);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onGetInfoGift() {
        try {
            Message msg = new Message(CMDClient.CMD_INFO_GIFT2);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onSendGift(int id, long gia) {
        try {
            Message msg = new Message(CMDClient.CMD_RQ_GETGIFT2);
            msg.writer().WriteInt(id);
            msg.writer().WriteLong(gia);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onSendGetMoney(long money) {
        try {
            Message msg = new Message(CMDClient.CMD_GET_MONEY);
            msg.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onFlipCard(byte index) {
        try {
            Message msg = new Message(CMDClient.CMD_FLIP_CARD);
            msg.writer().WriteByte(index);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onUpdateVersionNew(byte providerID) {
        try {
            Message msg = new Message(CMDClient.CMD_UPDATE_VERSION_NEW);
            msg.writer().WriteByte(providerID);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static Message onGetMessageUpdateVersionNew(byte providerID) {
        Message msg = null;
        try {
            msg = new Message(CMDClient.CMD_UPDATE_VERSION_NEW);
            msg.writer().WriteByte(providerID);
            // NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
        return msg;
    }

    public static void onIntroduceFriend(byte providerID) {
        try {
            Message msg = new Message(CMDClient.INTRODUCE_FRIEND);
            msg.writer().WriteByte(providerID);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static Message onGetMessageIntroduceFriend(byte providerID) {
        Message msg = null;
        try {
            msg = new Message(CMDClient.INTRODUCE_FRIEND);
            msg.writer().WriteByte(providerID);
        } catch (Exception e) {
        }
        return msg;
    }

    public static void onSendGCM(String regID) {
        try {
            Message msg = new Message(CMDClient.CMD_REGISTER_GCM);
            msg.writer().WriteUTF(regID);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onFinalMauBinh(int[] card) {
        Message msg = new Message(CMDClient.CMD_FINAL_MAUBINH);
        try {
            // msg.writer().WriteShort(1);
            // SerializerHelper.WriteInt(card);
            byte[] data = new byte[card.Length];
            for (int i = 0; i < data.Length; i++) {
                data[i] = (byte)card[i];
            }
            // SerializerHelper.writeArrayInt(card);
            msg.writer().WriteInt(data.Length);
            msg.writer().Write(data, 0, data.Length);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        // SerializerHelper.WriteInt(Integer.parseInt(tbid));

        NetworkUtil.GI().sendMessage(msg);

    }

    public static void onSendCuocBC(long money) {
        try {
            Message msg = new Message(CMDClient.CMD_CUOC);
            msg.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onFlip3Cay() {
        try {
            Message msg = new Message(CMDClient.CMD_FLIP_3CAY);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onAddMoney(long money) {
        try {
            Message msg = new Message(CMDClient.CMD_ADD_MONEY);
            msg.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
        }
    }

    public static void onOutView() {
        Message msg = new Message(CMDClient.CMD_EXIT_VIEW);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onChangeName(String newName) {
        Message msg = new Message(CMDClient.CMD_CHANGE_NAME);
        try {
            msg.writer().WriteUTF(newName);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    /**
	 * 
	 * @param username
	 * @param pass
	 * @param type
	 *            : 1-facebook 2-choingay 3-gmail 4-login normal
	 * @param imei
	 * @param link_avatar
	 * @param tudangky
	 *            : 1 la tu dang ky, 0
	 * @param displayName
	 * @param accessToken
	 * @param regPhone
	 */
    public static void doLogin(String username, String pass, sbyte type, String imei, String link_avatar, sbyte tudangky,
            String displayName, String accessToken, String regPhone, bool isMayao) {
        //BaseInfo.gI().isUpdate_Avatar = false;//sua
        Message msg = new Message(CMDClient.CMD_LOGIN_NEW);
        try {
            msg.writer().WriteByte(type);
            msg.writer().WriteUTF(username);
            msg.writer().WriteUTF(pass);
            msg.writer().WriteUTF(GameConfig.Version);
            msg.writer().WriteByte(CMDClient.PROVIDER_ID);
            msg.writer().WriteUTF(imei);
            msg.writer().WriteUTF(link_avatar);
            msg.writer().WriteByte(tudangky);
            msg.writer().WriteUTF(displayName);
            if (accessToken != null) {
                msg.writer().WriteUTF(accessToken);
            } else {
                msg.writer().WriteUTF("");
            }
            msg.writer().WriteUTF(regPhone);
            msg.writer().WriteBoolean(isMayao);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().connect(msg);//sua

        //NetworkUtil.GI().sendMessage(msg);
        ClientConfig.UserInfo.UNAME = username;
        ClientConfig.UserInfo.PASSWORD = pass;
    }

    public static void onUpdateAvata(int idAvata) {
        Message msg = new Message(CMDClient.CMD_UPDATE_AVATA);
        try {
            msg.writer().WriteInt(idAvata);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onRegister(String username, String pass, String imei, bool isMayao) {
        Message msg = new Message(CMDClient.CMD_REGISTER);
        try {
            msg.writer().WriteInt((int)CMDClient.PROVIDER_ID);
            msg.writer().WriteUTF(username);
            msg.writer().WriteUTF(pass);
            msg.writer().WriteUTF(imei);
            msg.writer().WriteBoolean(isMayao);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        //NetworkUtil.GI().connect(msg);//sua

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onCreateTable(int gameid, int roomid, long money, int maxplayer, int choinhanh, String password) {
        Message msg = new Message(CMDClient.CMD_CREATE_TABLE);
        try {
            //GameConfig.NUM_PLAYER = maxplayer;
            msg.writer().WriteInt(gameid);
            msg.writer().WriteInt(roomid);
            msg.writer().WriteLong(money);
            msg.writer().WriteInt(maxplayer);
            msg.writer().WriteInt(choinhanh);
            msg.writer().WriteUTF(password);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onTranferMoney(long userID, long xu) {
        Message msg = new Message(CMDClient.CMD_TRANFER_MONEY);
        try {
            msg.writer().WriteLong(userID);
            msg.writer().WriteLong(xu);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onChipToXu(long chip) {
        Message msg = new Message(CMDClient.CMD_CHIP_TO_XU);
        try {
            msg.writer().WriteLong(chip);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onChangeBetMoney(long money) {
        Message msg = new Message(CMDClient.CMD_CHANGE_BETMONEY);
        try {
            msg.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(msg);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void send_register_ID(bool landau, String regID) {
        Message msg = new Message(CMDClient.CMD_SEND_REGISTER_ID);
        try {
            msg.writer().WriteBoolean(landau);
            msg.writer().WriteUTF(regID);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        //NetworkUtil.GI().connect(msg);//sua

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onJoinTablePlay(int tbid, long folowMoney) {
        Message msg = new Message(CMDClient.CMD_JOIN_TABLE_PLAY);
        try {
            msg.writer().WriteShort((short)tbid);
            msg.writer().WriteLong(folowMoney);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void send_data_inapp(String data, String signature) {
        Message msg = new Message(CMDClient.CMD_SEND_INAPP);
        try {
            msg.writer().WriteUTF(data);
            msg.writer().WriteUTF(signature);
        } catch (Exception e) {
            Debug.LogException(e);
        }

        //NetworkUtil.GI().connect(msg);//sua

        NetworkUtil.GI().sendMessage(msg);
    }

    public static void baoxam(int type) {
        try {
            Message message = new Message(CMDClient.CMD_BAO_SAM);
            message.writer().WriteByte(type);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    /**
	 * 
	 * @param telco
	 *            1-viettel, 2-vina, 3-mobi
	 */
    public static void onSendSms9029(int telco) {
        Message msg = new Message(CMDClient.CMD_SMS_9029);
        try {
            msg.writer().WriteInt(telco);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onGetTopGame(int gameid) {
        Message msg = new Message(CMDClient.CMD_TOP);
        try {
            msg.writer().WriteByte(gameid);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onLoginfirst(String sdt, String name, byte gioitinh, String giftcode, int idAvata) {
        Message msg = new Message(CMDClient.CMD_LOGIN_FIRST);
        try {
            msg.writer().WriteUTF(sdt);
            msg.writer().WriteUTF(name);
            msg.writer().WriteByte(gioitinh);
            msg.writer().WriteUTF(giftcode);
            msg.writer().WriteInt(idAvata);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onNhanmoneyquest(byte id) {
        Message msg = new Message(CMDClient.CMD_NHAN_MONEY_QUEST);
        try {
            msg.writer().WriteByte(id);
        } catch (Exception e) {
            Debug.LogException(e);
        }
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onChoingay() {
        Message msg = new Message(CMDClient.CMD_CHOINGAY);
        NetworkUtil.GI().sendMessage(msg);
    }

    public static void onsendXocDiaDatCuoc(byte cua, long money) {
        try {
            Message message = new Message(CMDClient.CMD_XOCDIA_DATCUOC);
            message.writer().WriteByte(cua);
            message.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onsendGapDoi() {
        try {
            Message message = new Message(CMDClient.CMD_GAPDOI);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onsendDatLai() {
        try {
            Message message = new Message(CMDClient.CMD_DATLAI);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onsendHuyCuoc() {
        try {
            Message message = new Message(CMDClient.CMD_HUYCUOC);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onsendLamCai() {
        try {
            Message message = new Message(CMDClient.CMD_LAMCAI);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onsendChucNangCai(byte type) {
        try {
            Message message = new Message(CMDClient.CMD_CHUCNANG_HUYCUA);
            message.writer().WriteByte(type);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onjoinTaiXiu(int loaiphong) {
        try {
            Message message = new Message(CMDClient.CMD_JOIN_TAIXIU);
            message.writer().WriteInt(loaiphong);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onExitTaiXiu() {
        try {
            Message message = new Message(CMDClient.CMD_EXIT_TAIXIU);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    // cua 1-tai,2-xiu
    public static void onCuocTaiXiu(byte cua, long money) {
        try {
            Message message = new Message(CMDClient.CMD_CUOC_TAIXIU);
            message.writer().WriteByte(cua);
            message.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onXemLSTaiXiu(int phien) {
        try {
            Message message = new Message(CMDClient.CMD_XEM_LS_THEO_PHIEN);
            message.writer().WriteInt(phien);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onSendTKPhu(String userName, String pass, String sdt) {
        try {
            Message message = new Message(CMDClient.CMD_REGISTER_FB);
            message.writer().WriteUTF(userName);
            message.writer().WriteUTF(pass);
            message.writer().WriteUTF(sdt);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onLSGD() {
        try {
            Message message = new Message(CMDClient.CMD_LSGD);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onConfirmTranfer(long id, long money) {
        try {
            Message message = new Message(CMDClient.CMD_CONFIRM_TRANFER);
            message.writer().WriteLong(id);
            message.writer().WriteLong(money);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onHistoryTranfer() {
        try {
            Message message = new Message(CMDClient.CMD_HISTORY_TRANFER);
            NetworkUtil.GI().sendMessage(message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public static void onAdminTaiXiu(int tx) {
    }
}

