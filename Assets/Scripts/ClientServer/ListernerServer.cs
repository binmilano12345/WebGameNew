using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppConfig;
using DataBase;
using System.Linq;

public class ListernerServer : IChatListener {
    //GameControl gameControl;

    public void initConnect() {
        NetworkUtil.GI().registerHandler(ProcessHandler.getInstance());
        ProcessHandler.setListenner(this);
    }

    public ListernerServer(/*GameControl gameControl*/) {
        //this.gameControl = gameControl;
        initConnect();
    }

    public void onDisConnect() {
        //    gameControl.panelWaiting.onHide();
        PopupAndLoadingScript.instance.HideLoading();
        PopupAndLoadingScript.instance.messageSytem.OnShow("", delegate {
            NetworkUtil.GI().close();
            LoadAssetBundle.LoadScene(SceneName.SUB_LOGIN, SceneName.SUB_LOGIN);
        });
        //    gameControl.panelMessageSytem.onShowDCN("Mất kết nối!", delegate {
        //        gameControl.disableAllDialog();
        //        gameControl.setStage(gameControl.login);
        Debug.Log("Mất kết nối!");
        //        NetworkUtil.GI().close();
        //    });
    }

    public void OnLogin(Message message) {
        int b = message.reader().ReadByte();
        #region Dang nhap Thanh cong
        if (b == 1) {
            try {
                LinkFixed.LinkForum = message.reader().ReadUTF();
                GameConfig.SMS_CHANGE_PASS_SYNTAX = message.reader().ReadUTF();
                GameConfig.SMS_CHANGE_PASS_NUMBER = message.reader().ReadUTF();
                GameConfig.IsShowDoiThuong = message.reader().ReadByte();
            } catch (Exception e) {
                Debug.LogException(e);
            }
            #region Lay thong tin khac
            SendData.onSendSmsSyntax();
            SendData.onGetTopGame((int)GameID.TLMN);
            SendData.onSendSms9029(GameConfig.TELCO_CODE);
            #endregion
            LoadAssetBundle.LoadScene(SceneName.SCENE_LOBBY, SceneName.SCENE_LOBBY);
        }
        #endregion       
        #region Dang nhap That bai
        else {
            string str = message.reader().ReadUTF();
            switch (b) {
                case 0:
                    PopupAndLoadingScript.instance.messageSytem.OnShow(str);
                    break;
                case 2://Nhap so dt
                    break;
            }
        }
        #endregion

        PopupAndLoadingScript.instance.HideLoading();
    }
    public void OnMessageServer(string message) {
        //string msg = message.reader().ReadUTF();
        PopupAndLoadingScript.instance.messageSytem.OnShow(message);
        PopupAndLoadingScript.instance.HideLoading();
    }
    public void OnPopupNotify(Message message) {//clam
        int size = message.reader().ReadInt();
        if (size != 0) {
            for (int i = 0; i < size; i++) {
                int id = message.reader().ReadInt();
                string title = message.reader().ReadUTF();
                string content = message.reader().ReadUTF();
            }

        }
    }
    public void OnProfile(Message msg) {
        ClientConfig.UserInfo.UNAME = msg.reader().ReadUTF();
        ClientConfig.UserInfo.USER_ID = (int)msg.reader().ReadLong();
        ClientConfig.UserInfo.CASH_FREE = msg.reader().ReadLong();
        ClientConfig.UserInfo.NAME = msg.reader().ReadUTF();
        ClientConfig.UserInfo.DISPLAY_NAME = msg.reader().ReadUTF();
        ClientConfig.UserInfo.LINK_AVATAR = msg.reader().ReadUTF();
        ClientConfig.UserInfo.AVATAR_ID = msg.reader().ReadInt();
        ClientConfig.UserInfo.EXP = msg.reader().ReadLong();
        ClientConfig.UserInfo.SCORE_VIP = msg.reader().ReadLong();
        ClientConfig.UserInfo.TOTAL_MONEY_CHARGING = msg.reader()
               .ReadLong();
        ClientConfig.UserInfo.TOTAL_TIME_PLAY = msg.reader().ReadLong();

        ClientConfig.UserInfo.SO_LAN_THANG = msg.reader().ReadUTF();
        ClientConfig.UserInfo.SO_LAN_THUA = msg.reader().ReadUTF();
        ClientConfig.UserInfo.SO_TIEN_MAX = msg.reader().ReadLong();
        ClientConfig.UserInfo.SO_LAN_GIAO_DICH = msg.reader().ReadInt();

        String email_sdt = msg.reader().ReadUTF();
        ClientConfig.UserInfo.SEX = msg.reader().ReadByte();
        ClientConfig.UserInfo.LEVEL = msg.reader().ReadByte();
        ClientConfig.UserInfo.PHONE = msg.reader().ReadUTF();

        string[] s = Regex.Split(email_sdt, "\\*");

        ClientConfig.UserInfo.LOGIN_END = s[0];

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
        string[] listthang = ClientConfig.UserInfo.SO_LAN_THANG.Split(',');
        int thang = 0;
        for (int i = 0; i < listthang.Length; i++) {
            thang += int.Parse(listthang[i].Split('-')[1]);
        }
        ClientConfig.UserInfo.TONG_VAN_THANG = thang;
        String[] listthua = ClientConfig.UserInfo.SO_LAN_THUA.Split(',');
        int thua = 0;
        for (int i = 0; i < listthua.Length; i++) {
            thua += int.Parse(listthua[i].Split('-')[1]);
        }
        ClientConfig.UserInfo.TONG_VAN_THUA = thua;

    }
    public void OnRateScratchCard(Message message) {
        try {
            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                int menhgia = message.reader().ReadInt();
                int xu = message.reader().ReadInt();
            }
            GameConfig.SMS_10 = message.reader().ReadInt();
            GameConfig.SMS_15 = message.reader().ReadInt();

            //mainGame.mainScreen.dialogNapTien
            //        .initTygia(BaseInfo.gI().list_tygia);
            //mainGame.mainScreen.dialogNapTien.initSMS();
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void OnListBetMoney(Message message) {
        try {
            //ListBetMoney.gI().listBetVip.clear();
            int size = message.reader().ReadShort();
            for (int i = 0; i < size; i++) {
                //BetMoney betMoney = new BetMoney();
                long a = message.reader().ReadLong();
                //betMoney.setListBet(a);
                //ListBetMoney.gI().listBetVip.add(betMoney);
            }
            //mainGame.mainScreen.room.groupChonMucCuoc.initMuccuoc(ListBetMoney
            //        .gI().listBetVip);
            //mainGame.mainScreen.taiXiuMinigame.initMuccuoc(message.reader()
            //        .readUTF());
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void OnListProduct(Message message) {
        //mainGame.listProductIDs.clear();
        try {
            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                //ProductID productID = new ProductID();
                string productid = message.reader().ReadUTF();
                int price = message.reader().ReadInt();
                int xu = message.reader().ReadInt();
                //mainGame.listProductIDs.add(productID);
            }
            //mainGame.mainScreen.dialogNapTien.createIAP(mainGame,
            //        mainGame.listProductIDs);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void OnTop(Message message) {
        try {
            //mainGame.topGames.clear();
            int size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                //TopGame topGame = new TopGame();
                string displayname = message.reader().ReadUTF();
                int idAvata = message.reader().ReadInt();
                long money = message.reader().ReadLong();
                //topGame.id = i + 1;
                //mainGame.topGames.add(topGame);
            }
            //mainGame.mainScreen.menu.createTop(mainGame.topGames);

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void OnInboxMessage(Message message) {
        try {
            //ArrayList<MessInfo> homThu = new ArrayList<MessInfo>();
            int total = message.reader().ReadByte();
            for (int i = 0; i < total; i++) {
                //MessInfo info = new MessInfo();

                int id = message.reader().ReadInt();
                string guitu = message.reader().ReadUTF();
                string guiLuc = message.reader().ReadUTF();
                string noiDung = message.reader().ReadUTF();
                sbyte isread = message.reader().ReadByte();
                //homThu.add(info);

            }
            //BaseInfo.gI().allMess.clear();
            //BaseInfo.gI().allMess.addAll(homThu);
            //GroupHomThu.initTinHethong = true;
            //mainGame.mainScreen.dialogWaitting.onHide();

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void OnGetAlertLink(Message message) {
        try {
            string content = message.reader().ReadUTF();
            if (!content.Equals("")) {
                GameConfig.TXT_NOTI = content;
                //PopupAndLoadingScript.instance.alert.SetAlert(content);
                if (LobbyControl.instance != null) {
                    LobbyControl.instance.SetNoti();
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
            Debug.LogException(e);
        }
    }
    public void OnInfoSMS(Message message) {
        try {
            int len = message.reader().ReadByte();
            GameConfig.IsCharging = len;
            //mainGame.myPref.putCharging(BaseInfo.gI().isCharging);
            for (int i = 0; i < 2; i++) {
                message.reader().ReadUTF();// name
                string syntax = message.reader().ReadUTF();
                short port = message.reader().ReadShort();
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
            Debug.LogException(e);
        }
    }
    public void OnSMS9029(Message message) {
        try {
            //mainGame.listSms9029.clear();
            sbyte size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                //SMS9029 sms = new SMS9029();
                string name = message.reader().ReadUTF();
                string syntax = message.reader().ReadUTF();
                short port = message.reader().ReadShort();
                long money = message.reader().ReadLong();
                //mainGame.listSms9029.add(sms);
            }
            //Collections.sort(mainGame.listSms9029, sort_sms9029);
            //mainGame.mainScreen.dialogNapTien.initSMS9029(mainGame.listSms9029);
        } catch (Exception e) {
            Debug.LogException(e);
        }

    }

    public void OnJoinGame(Message message) {
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
        OnGameID(message);
        SendData.onJoinRoom(ClientConfig.UserInfo.UNAME, 0);

    }
    public void OnJoinRoom(Message message) {
        short status = message.reader().ReadShort();
        if (status == -1) {//that bai
            message.reader().ReadUTF();
        } else {
            OnListTable(status, message);
        }
    }
    public void OnGameID(Message message) {
        try {
            sbyte gameID = message.reader().ReadByte();
            GameConfig.CurrentGameID = (GameID)gameID;
            if (gameID == -99) {
                gameID = message.reader().ReadByte();
                GameConfig.CurrentGameID = (GameID)gameID;
                message.reader().ReadUTF();// ten room
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
    public void OnListTable(int totalTB, Message message) {
        List<ItemTableData> listTable = new List<ItemTableData>();
        for (int i = 0; i < totalTB; i++) {
            try {
                ItemTableData ctb = new ItemTableData();
                ctb.TableName = "Bàn " + i;
                ctb.Id = (message.reader().ReadShort());
                ctb.Status = (message.reader().ReadByte());
                ctb.NUser = (message.reader().ReadByte());
                ctb.IsLock = message.reader().ReadByte();
                ctb.Money = message.reader().ReadLong();
                ctb.NeedMoney = message.reader().ReadLong();
                ctb.MaxMoney = message.reader().ReadLong();
                ctb.MaxUser = (message.reader().ReadByte());
                //if (roomstage.anbanfull && ctb.nuser == ctb.maxuser) {//sua
                //    continue;
                //}
                listTable.Add(ctb);
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
        }

        listTable.OrderBy(r => r.Money);

        LoadAssetBundle.LoadScene(SceneName.SCENE_ROOM, SceneName.SCENE_ROOM, () => {
            RoomControl.instance.CreateTable(listTable);
        });
    }

    public void OnJoinTablePlay(Message message) {
        // check = SerializerHelper.readInt(message);
        sbyte status = message.reader().ReadByte();

        if (status == 1) {
            sbyte numPlayer = message.reader().ReadByte();
            //short idTable = message.reader().ReadShort();
            //long betMoney = message.reader().ReadLong();
            //long needMoney = message.reader().ReadLong();
            //long maxMoney = message.reader().ReadLong();
            GameControl.instance.SetCasino(numPlayer == 9 ? 1 : 0, () => {
                GameControl.instance.currentCasino.OnJoinTablePlaySuccess(message);
            });
        } else {
            if (status == -1) {
                String a = message.reader().ReadUTF();
                //onJoinTablePlayFail(a);
            } else if (status == 0) {
                message.reader().ReadInt();
                OnMessageServer(message.reader().ReadUTF());
            }
        }
    }
}