using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataBase;
using System.Runtime.CompilerServices;


public enum TYPE_CARD {
    MAU_THAU = 0,
    DOI,
    THU,
    SAM_CO,
    SANH,
    THUNG,
    CU_LU,
    TU_QUY,
    THUNG_PHA_SANH
};
namespace AppConfig {
	public class GameConfig {
		public const string IP = "game.atmdoithuong.com";
		public static string IP2 = "";
        //public const string IP = "115.84.179.166";
        public const int PORT = 3966;
        //public const string IP = "gamebai.thanbai68.net";//so do
        //public const int PORT = 4322;


        public const string Version = "1.0.0";

        public static int NUM_AVATA = 36;
        public static string SMS_CHANGE_PASS_SYNTAX = "";
        public static string SMS_CHANGE_PASS_NUMBER = "";
        public static sbyte IsShowDoiThuong = 0;
        public static int TELCO_CODE = 1;
        public static int SMS_10 = 4000;
        public static int SMS_15 = 6000;
        public static int IsCharging = 0;// 0: disable, 1: enable, 10, enable inapp
        public static string Syntax10, Syntax15;
        public static string Port10 = "", Port15 = "";

        public static string TXT_NOTI = "";

        public const int NUM_GAME = 10;
        public static int[] IdGame = new int[] { GameID.XOCDIA, GameID.TLMN, GameID.TLMNSL, GameID.MAUBINH, GameID.LIENG, GameID.XITO, GameID.POKER, GameID.BACAY, GameID.PHOM, GameID.SAM };
        public static string[] GameName = new string[] { "PHỎM", "TIẾN LÊN MIỀN NAM", "XÌ TỐ", "MẬU BINH", "BA CÂY", "LIÊNG", "SÂM", "CHƯƠNG", "POKER", "XÓC ĐĨA", "TÀI XỈU", "TIẾN LÊN MIỀN NAM SOLO" };
		public static int CurrentGameID;

		public static string HOT_LINE = "";
		public static string MAIL_HELPER = "";
		public static string FANPAGE = "";
		public static string CONTENT_DAILY = "";
		public static string PHONE_NUMBER_DAILY = "";
		public static bool IS_LOGIN_FB_AVARIABLE = true;

		public const string MONEY_UNIT_VIP = "GOLD";
    }

    public class LinkFixed {
        public static string LinkForum = "";
    }

    public class GameID {
        public const int PHOM = 0;
        public const int TLMN = 1;
        public const int XITO = 2;
        public const int MAUBINH = 3;
        public const int POKER = 8;
        public const int SAM = 6;
        public const int CHUONG = 7;
        public const int BACAY = 4;
        public const int LIENG = 5;
        public const int RESET = -1;
        public const int XOCDIA = 9;
        public const int TAIXIU = 10;
        public const int TLMNSL = 11;
        public const int VONGQUAY = 12;
    }

    public class SettingConfig {
        private static int isSound;//0- tat, 1 - bat
        public static int IsSound {
            get { return isSound; }
            set {
                isSound = value;
                PlayerPrefs.SetInt("isSound", isSound);
                PlayerPrefs.Save();
            }
        }
        private static int isAutoReady;//0- tat, 1 - bat
        public static int IsAutoReady {
            get { return isAutoReady; }
            set {
                isAutoReady = value;
                PlayerPrefs.SetInt("isAutoReady", isAutoReady);
                PlayerPrefs.Save();
            }
        }
        private static int isInvite;//0- tat, 1 - bat
        public static int IsInvite {
            get { return isInvite; }
            set {
                isInvite = value;
                PlayerPrefs.SetInt("isInvite", isInvite);
                PlayerPrefs.Save();
            }
        }
        public static void InitSetting() {
            SettingConfig.IsSound = PlayerPrefs.GetInt("isSound", 1);
            SettingConfig.IsAutoReady = PlayerPrefs.GetInt("isAutoReady", 1);
            SettingConfig.IsInvite = PlayerPrefs.GetInt("isInvite", 1);
        }
    }

    public class BundleName {
		public const string PREFAPS = "prefabs";
		public const string UI = "ui";
		public const string CARDS = "cards";
        public const string ICON_GAME = "icon_game";
        public const string AVATAS = "avatas";
        public const string CHIP = "chip";
        public const string DICE = "dice";
        public const string EMOTIONS = "emotions";
    }

    public class UIName {
        public static string[] UI_GAME = new string[] { "ic_xocdia", "ic_TLMNdemla", "ic_TLMNsolo", "ic_maubinh", "ic_lieng", "ic_xito", "ic_poker", "ic_3cay", "ic_phom", "ic_sam" };
        public static string UI_XOC_DIA_ORANGE = "orange";
        public static string UI_XOC_DIA_WHITE = "white";
        public static string[] UI_ANI_WIN = new string[] { "rank_u", "rank_win", "rank_thangtrang", "rank_mom", "rank_cong", "rank_lung" };
    }

    public class PrefabsName {
        public const string PRE_MESSAGE_SYTEM = "PanelMessageSytem";
        public const string PRE_TOAST = "Toast";
        public const string PRE_ALERT = "Alert";
        public const string PRE_LOAD = "PanelLoad";

        public const string PRE_PLAYER_TLMN = "TLMNPlayer";
        public const string PRE_PLAYER_SAM = "SamPlayer";
        public const string PRE_PLAYER_PHOM = "PhomPlayer";

        public const string PRE_ITEM_INVITE = "ItemInvite";

        public const string PRE_ITEM_TABLE = "Item_Table";
        public const string PRE_ITEM_GAME = "ItemGame";
        public const string PRE_ITEM_RANK = "ItemRank";
        public const string PRE_ITEM_NOTI = "ItemNoti";
        public const string PRE_CARD = "Card";
        public const string PRE_CHAT_TEXT = "ButtonChatText";
        public const string PRE_CHAT_SMILE = "ButtonEmotion";

        public const string PRE_CHIP = "Chip";
        public const string PRE_TEXT_LICH_SU_MINI_DICE = "Text_LichSu_MiniDice";
        public const string PRE_ITEM_LICH_SU_MINI_DICE = "Img_Lich_Su";
        public const string PRE_IMAGE_LICH_SU_XOC_DIA = "Image_LichSu_XocDia";
        public const string PRE_ITEM_LICH_SU_XOC_DIA = "Item_Lich_Su_XD";
        public const string PRE_ITEM_NHAN_GOLD = "ItemReceiveGold";
        public const string PRE_ITEM_AVATA = "ItemAvata";
        public const string PRE_ITEM_DOI_THUONG = "ItemDoiThuong";
        public const string PRE_ITEM_VALUE_GOLD = "ItemValueGold";//Chuyen gold
        public const string PRE_ITEM_SAFE_GOLD = "ItemSafeGold";//ket bac
        public const string PRE_ITEM_MAIL = "ItemMail";
    }

    public class SceneName {
        public const string SCENE_MAIN = "main";
        public const string SCENE_LOBBY = "lobby";
        public const string SCENE_ROOM = "room";//chon muc cuoc va phong choi

        public const string SUB_LOGIN = "sub_login";
        public const string SUB_REGISTER = "sub_register";
        public const string SUB_SETTING = "sub_setting";
        public const string SUB_RANK = "sub_rank";
        public const string SUB_NOTI = "sub_noti";
        public const string SUB_CHAT = "sub_chat";
		public const string SUB_HELP = "sub_help";
		public const string SUB_INVITE = "sub_invite";
		public const string SUB_INFO_PLAYER = "sub_info_player";
		public const string SUB_CHANGE_NAME = "sub_change_name";
		public const string SUB_CHANGE_PASS = "sub_change_pass";
		public const string SUB_CHANGE_AVATAR = "sub_change_avatar";

        public const string GAME_TLMN = "game_tlmn";
        public const string GAME_TLMN_SOLO = "game_tlmnsolo";
        public const string GAME_SAM = "game_sam";
		public const string GAME_PHOM = "game_phom";
		public const string GAME_MAUBINH = "game_maubinh";
    }

    public class CMDClient {
        //public const sbyte PROVIDER_ID = 82;
        public const sbyte PROVIDER_ID = 1;
        public const sbyte CMD_SESSION_ID = -27;
        public const sbyte CMD_LOGIN = 0;
        public const sbyte CMD_LIST_ROOM = 1;
        public const sbyte CMD_LIST_TABLE = 2;
        public const sbyte CMD_JOIN_TABLE = 3;
        public const sbyte CMD_FIRE_CARD = 4;
        public const sbyte CMD_GET_CARD = 5;
        public const sbyte CMD_EAT_CARD = 6;
        public const sbyte CMD_DROP_PHOM = 7;
        public const sbyte CMD_JOIN_GAME = 8;
        public const sbyte CMD_GET_FREE_MONEY = 9;
        public const sbyte CMD_SHOP_AVATAR = 10;
        public const sbyte CMD_BUY_AVATAR = 11;
        public const sbyte CMD_PROFILE = 12;
        public const sbyte CMD_LOGIN_FIRST = 13;
        public const sbyte CMD_TOP_PLAYER = 14;
        public const sbyte CMD_START_GAME = 15;
        public const sbyte CMD_EXIT_TABLE = 16;
        public const sbyte CMD_EXIT_ROOM = 17;
        public const sbyte CMD_UPDATE_CURRENT_TABLE = 18;
        public const sbyte CMD_USER_JOIN_TABLE = 19;
        public const sbyte CMD_JOIN_ROOM = 20;
        public const sbyte CMD_EXIT_GAME = 21;
        public const sbyte CMD_SET_TURN = 22;
        public const sbyte CMD_INFOMATION = 23;
        public const sbyte CMD_CHAT_MSG = 24;
        public const sbyte CMD_READY = 25;
        public const sbyte CMD_LIST_USER_IN_ROOM = 26;
        public const sbyte CMD_VESION = 27;
        public const sbyte CMD_MOM = 28;
        public const sbyte CMD_BALANCE = 29;
        public const sbyte CMD_U = 30;
        public const sbyte CMD_GUI_CARD = 31;
        public const sbyte CMD_ALLCARD_FINISH = 32;
        public const sbyte CMD_GAMEOVER = 33;
        public const sbyte CMD_INVITE_FRIEND = 34;
        public const sbyte CMD_ANSWER_INVITE_FRIEND = 35;
        public const sbyte CMD_RESPONSE_INVITE = 36;
        public const sbyte CMD_TOP_RICH = 37;
        public const sbyte CMD_REGISTER = 38;
        public const sbyte CMD_GET_CAPCHA = 39;
        public const sbyte CMD_VIEW = 40;
        public const sbyte CMD_KICK = 41;
        public const sbyte CMD_FRIEND_LIST = 42;
        public const sbyte CMD_UPDATE_WAITTING_ROOM = 43;
        public const sbyte CMD_UPDATE_ROOM = 44;
        public const sbyte CMD_UPDATE_PROFILE = 45;
        public const sbyte CMD_PING_PONG = 46;
        public const sbyte CMD_KILL_PIG = 47;
        public const sbyte CMD_PASS = 48;
        public const sbyte CMD_GET_INBOX_MESSAGE = 51;
        public const sbyte CMD_SEND_MESSAGE = 52;
        public const sbyte CMD_DEL_MESSAGE = 53;
        public const sbyte CMD_READ_MESSAGE = 54;
        public const sbyte CMD_UNREAD_MESSAGE = 55;
        public const sbyte CMD_GET_SMS_STRUCTURE = 56;
        public const sbyte CMD_ID_GAME = 57;
        public const sbyte CMD_FINISH = 58;
        public const sbyte CMD_CUOC = 59;
        public const sbyte CMD_THEO = 60;
        public const sbyte CMD_HA_PHOM_TAY = 62;
        public const sbyte CMD_USE_ITEM = 63;
        public const sbyte CMD_ACTIVE = 64;
        public const sbyte CMD_REQUEST_GET_ALL_AVATAR = 65;
        public const sbyte CMD_SET_PASSWORD = 66;
        public const sbyte CMD_SET_NEW_MASTER = 67;
        public const sbyte CMD_CHANGERULETBL = 68;
        public const sbyte CMD_UPDATEMONEY_PLAYER_INTBL = 69;
        public const sbyte CMD_RANK = 70;
        public const sbyte CMD_FINISHTURNTLMN = 71;
        public const sbyte CMD_SERVER_MESSAGE = 101;
        public const sbyte CMD_GOP_Y = 103;
        public const sbyte CMD_VIEW_INFO_FRIEND = 104;
        public const sbyte CMD_SET_MONEY = 105;
        public const sbyte CMD_CHAT = 106;
        public const sbyte CMD_SMS = 107;
        public const sbyte PAYCARD = 108;
        public const sbyte CMD_UPDATE_MONEY = 109;
        public const sbyte CMD_UPDATE_VERSION = 110;
        public const sbyte CMD_GET_PASS = 111;
        public const sbyte CMD_ADD_FRIEND_CHAT = 113;
        public const sbyte CMD_LIST_INVITE = 114;
        public const sbyte CMD_SEND_PROVIDER = -1;
        public const sbyte MATCH_TURN = -2;
        public const sbyte INTRODUCE_FRIEND = -118;
        public const sbyte CMD_NHAN_MONEY_QUEST = -115;
        public const sbyte CMD_UPDATE_QUEST = -114;
        public const sbyte CMD_QUESTINFO = -113;
        public const sbyte CMD_AUTOJOINTABLE = -112;
        public const sbyte CMD_TBLID = -111;
        public const sbyte CMD_INFOPOCKERTABLE = -109;
        public const sbyte CMD_ADDCARDTABLE_POCKER = -106;
        public const sbyte CMD_GETPHONECSKH = -104;
        public const sbyte CMD_ALERT_LINK = -103;
        public const sbyte CMD_INFO_WINPLAYER = -101;
        public const sbyte CMD_INFOPLAYER_TBL = -100;
        public const sbyte CMD_INFO_GIFT = -99;
        public const sbyte CMD_SEND_GIFT = -98;
        public const sbyte CMD_GET_MONEY = -97;
        public const sbyte CMD_TIME_AUTOSTART = -96;
        public const sbyte CMD_START_FLIP = -95;
        public const sbyte CMD_FLIP_CARD = -94;
        public const sbyte CMD_UPDATE_VERSION_NEW = -93;
        public const sbyte CMD_REGISTER_GCM = -92;
        public const sbyte CMD_FINAL_MAUBINH = -91;
        public const sbyte CMD_CALMB_RANKS = -90;
        public const sbyte CMD_WINMAUBINH = -89;
        public const sbyte CMD_INFO_ME = -88;
        public const sbyte CMD_BEGINRISE_3CAY = -87;
        public const sbyte CMD_FLIP_3CAY = -86;
        public const sbyte CMD_CUOC_3CAY = -85;
        public const sbyte CMD_ADD_MONEY = -84;
        public const sbyte CMD_FOR_VIEW = -83;
        public const sbyte CMD_EXIT_VIEW = -82;
        public const sbyte CMD_LOGIN_NEW = -81;
        public const sbyte CMD_CHANGE_NAME = -80;
        public const sbyte CMD_UPDATE_AVATA = -78;
        public const sbyte CMD_CREATE_TABLE = -77;
        public const sbyte CMD_CONFIRM_TRANFER = -76;
        public const sbyte CMD_LSGD = -75;
        public const sbyte CMD_RECEIVE_FREE_MONEY = -74;
        public const sbyte CMD_HISTORY_TRANFER = -73;
        public const sbyte CMD_TRANFER_MONEY = -72;
        public const sbyte CMD_TOP = -71;
        public const sbyte CMD_RATE_SCRATCH_CARD = -70;
        public const sbyte CMD_LIST_EVENT = -69;
        public const sbyte CMD_CHANGE_BETMONEY = -68;
        public const sbyte CMD_CHIP_TO_XU = -67;
        public const sbyte CMD_SEND_REGISTER_ID = -66;
        public const sbyte CMD_JOIN_TABLE_PLAY = -65;
        public const sbyte CMD_SEND_INAPP = -64;
        public const sbyte CMD_HIDE_NAPTIEN = -63;
        public const sbyte CMD_LIST_BET_MONEY = -62;
        public const sbyte CMD_POPUP_NOTIFY = -61;
        public const sbyte CMD_PHOM_HA = -60;
        public const sbyte CMD_LIST_PRODUCT = -59;
        public const sbyte CMD_INFO_GIFT2 = -58;
        public const sbyte CMD_RQ_GETGIFT2 = -57;
        public const sbyte CMD_BAO_SAM = -56;
        public const sbyte CMD_CHOINGAY = -55;
        public const sbyte CMD_CARD_XEP_MB = -54;
        public const sbyte CMD_SMS_9029 = -53;
        public const sbyte CMD_MA_DU_THUONG = -52;
        public const sbyte CMD_BEGIN_XOCDIA = -102;
        public const sbyte CMD_UPDATE_CUA = -123;
        public const sbyte CMD_MO_BAT = -122;
        public const sbyte CMD_CHUCNANG_HUYCUA = -121;
        public const sbyte CMD_HISTORY_XD = -120;
        public const sbyte CMD_DATLAI = -119;
        public const sbyte CMD_GAPDOI = -124;
        public const sbyte CMD_HUYCUOC = -117;
        public const sbyte CMD_LAMCAI = -116;
        public const sbyte CMD_ARR_BET_XD = -110;
        public const sbyte CMD_XOCDIA_DATCUOC = -108;
        public const sbyte CMD_BEGIN_XOCDIA_DUNGCUOC = -107;
        public const sbyte CMD_BEGIN_XOCDIA_CUOC = -105;
        public const sbyte AMINXOCDIA = -126;
        public const sbyte CMD_CUOC_TAIXIU = -47;
        public const sbyte CMD_GAMEOVER_TAIXIU = -46;
        public const sbyte CMD_JOIN_TAIXIU = -45;
        public const sbyte CMD_INFO_TAIXIU = -44;
        public const sbyte CMD_TIME_START_TAIXIU = -43;
        public const sbyte CMD_AUTO_START_TAIXIU = -42;
        public const sbyte CMD_EXIT_TAIXIU = -41;
        public const sbyte CMD_UPDATE_MONEY_TAIXIU = -40;
        public const sbyte CMD_XEM_LS_THEO_PHIEN = -39;
        public const sbyte CMD_ADMIN_TAIXIU = -36;
        public const sbyte CMD_REGISTER_FB = -38;
        public const sbyte CONFIRM_FACEBOOK = -37;
    }
}