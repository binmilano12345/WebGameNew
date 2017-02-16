using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AppConfig {
    public class ClientConfig {
        public class UserInfo {
            private static string uname = "", name = "", disp_name = "";
            private static int user_id = 0;
            private static int save_pass = 1;
            private static string password = "";
            private static int avatar_id = 0;
            private static string markid = "";
            private static long cash_vip = 0, cash_free = 0;
            private static int tutorial_finished = 0;
            private static int level = 0;
            private static int level_score = 0;
            private static string level_name = "";
            private static string facebook_id = "";
            private static string link_avatar = "";
            private static int is_first_time_login_facebook = 0;
            private static string dis_id = "";
            private static bool isMaster = false;
            private static string CurrentGameId = "";

            private static long exp;
            private static long score_vip;
            private static long total_money_charging;
            private static long total_time_play;
            private static string so_lan_thang;
            private static string so_lan_thua;
            private static long so_tien_max;
            private static int so_lan_giao_dich;
            private static string email;
            private static string phone;
            private static sbyte sex;
            private static string login_end;
            private static int tong_van_thang;
            private static int tong_van_thua;

            public static int TONG_VAN_THUA {
                get { return tong_van_thua; }
                set { tong_van_thua = value; }
            }

            public static int TONG_VAN_THANG {
                get { return tong_van_thang; }
                set { tong_van_thang = value; }
            }


            public static string LOGIN_END {
                get { return login_end; }
                set { login_end = value; }
            }


            public static sbyte SEX {
                get { return sex; }
                set { sex = value; }
            }


            public static string PHONE {
                get { return phone; }
                set { phone = value; }
            }

            public static string EMAIL {
                get { return email; }
                set { email = value; }
            }


            public static int SO_LAN_GIAO_DICH {
                get { return so_lan_giao_dich; }
                set { so_lan_giao_dich = value; }
            }


            public static long SO_TIEN_MAX {
                get { return so_tien_max; }
                set { so_tien_max = value; }
            }


            public static void InitUserInfo() {
                //init user info
                UserInfo.uname = PlayerPrefs.GetString("uname", "");
                UserInfo.name = PlayerPrefs.GetString("name", "");
                UserInfo.disp_name = PlayerPrefs.GetString("disp_name", "");
                UserInfo.user_id = PlayerPrefs.GetInt("user_id", 0);
                UserInfo.save_pass = PlayerPrefs.GetInt("savepass", 1);
                UserInfo.password = PlayerPrefs.GetString("password", "");
                UserInfo.avatar_id = PlayerPrefs.GetInt("avatar_id", 0);
                UserInfo.markid = PlayerPrefs.GetString("markid", "");
                UserInfo.cash_vip = (long)PlayerPrefs.GetFloat("cash_vip", 0);
                UserInfo.cash_free = (long)PlayerPrefs.GetFloat("cash_free", 0);
                UserInfo.tutorial_finished = PlayerPrefs.GetInt("tutorial_finished", 0);
                UserInfo.level = PlayerPrefs.GetInt("level", 0);
                UserInfo.level_name = PlayerPrefs.GetString("level_name", "");
                UserInfo.facebook_id = PlayerPrefs.GetString("facebook_id", "");
                UserInfo.link_avatar = PlayerPrefs.GetString("link_avatar", "");
                UserInfo.is_first_time_login_facebook = PlayerPrefs.GetInt("is_first_time_login_facebook", 0);
                UserInfo.dis_id = PlayerPrefs.GetString("dis_id", "");
                UserInfo.CurrentGameId = PlayerPrefs.GetString("CurrentGameId", "");
                //UserInfo.CurrentGameIP = PlayerPrefs.GetString("CurrentGameIP", "");
            }

            #region USER INFO
            public static string UNAME {
                get {
                    return uname;
                }
                set {
                    uname = value;
                    PlayerPrefs.SetString("uname", uname);
                    PlayerPrefs.Save();
                }
            }
            public static string NAME {
                get {
                    return name;
                }
                set {
                    name = value;
                }
            }

            public static string DISPLAY_NAME {
                get {
                    return (disp_name == null || disp_name.Length == 0) ? uname : disp_name;
                }
                set {
                    disp_name = value;
                }
            }
            public static int USER_ID {
                get {
                    return user_id;
                }
                set {
                    user_id = value;
                }
            }

            public static int SAVE_PASS {
                get {
                    return save_pass;
                }
                set {
                    save_pass = value;
                    PlayerPrefs.SetInt("savepass", save_pass);
                    PlayerPrefs.Save();
                }
            }

            public static string PASSWORD {
                get {
                    return password;
                }
                set {
                    password = value;
                    PlayerPrefs.SetString("password", password);
                    PlayerPrefs.Save();
                }
            }

            public static string MARK_ID {
                get {
                    return markid;
                }
                private set {
                    markid = value;
                }
            }

            public static long CASH_VIP {
                get {
                    return cash_vip;
                }
                set {
                    cash_vip = value;
                }
            }

            public static long CASH_FREE {
                get {
                    return cash_free;
                }
                set {
                    cash_free = value;
                }
            }

            public static bool TUTORIAL_FINISHED {
                get { return tutorial_finished == 0 ? false : true; }
                set {
                    tutorial_finished = value == true ? 1 : 0;
                    PlayerPrefs.SetInt("tutorial_finished", tutorial_finished);
                    PlayerPrefs.Save();
                }
            }

            public static int LEVEL {
                get {
                    return level;
                }
                set {
                    level = value;
                }
            }
            public static int LEVEL_SCORE {
                get {
                    return level_score;
                }
                set {
                    level_score = value;
                }
            }

            public static string LEVEL_NAME {
                get {
                    return level_name;
                }
                set {
                    level_name = value;
                }
            }

            public static string FACEBOOK_ID {
                get {
                    return facebook_id;
                }
                set {
                    facebook_id = value;
                }
            }

            public static string LINK_AVATAR {
                get {
                    return link_avatar;
                }
                set {
                    link_avatar = value;
                }
            }

            public static int IS_FIRST_TIME_LOGIN_FACEBOOK {
                get {
                    return is_first_time_login_facebook;
                }
                set {
                    is_first_time_login_facebook = value;
                    PlayerPrefs.SetInt("is_first_time_login_facebook", is_first_time_login_facebook);
                    PlayerPrefs.Save();
                }
            }

            public static string DIS_ID {
                get {
                    return dis_id;
                }
                set {
                    dis_id = value;
                }
            }

            public static int AVATAR_ID {
                get {
                    return avatar_id;
                }
                set {
                    avatar_id = value;
                }
            }

            public static bool IS_MASTER {
                get {
                    return isMaster;
                }
                set {
                    isMaster = value;
                }
            }

            public static string CURRENT_GAME_ID {
                get {
                    return CurrentGameId;
                }
                set {
                    CurrentGameId = value;
                }
            }

            public static long EXP {
                get { return exp; }
                set { exp = value; }
            }
            public static long SCORE_VIP {
                get { return score_vip; }
                set { score_vip = value; }
            }
            public static long TOTAL_MONEY_CHARGING {
                get { return total_money_charging; }
                set { total_money_charging = value; }
            }
            public static long TOTAL_TIME_PLAY {
                get { return total_time_play; }
                set { total_time_play = value; }
            }
            public static string SO_LAN_THANG {
                get { return so_lan_thang; }
                set { so_lan_thang = value; }
            }
            public static string SO_LAN_THUA {
                get { return so_lan_thua; }
                set { so_lan_thua = value; }
            }


            /// <summary>
            /// Chay trong mainthread vi co thao tac luu vao cache (PlayerPrefs) (@see MonoBihaviourHelper.ExecuteIEnumerator)
            /// </summary>
            /// <param name="uname"></param>
            /// <param name="password"></param>
            /// <param name="session"></param>
            /// <param name="cash"></param>
			public static void SetUserInfo(string uname, string name, string disp_name, string password, string session, string markid, long cash_gold, long cash_silver, int level, string level_name, int avatar_id) {
                UNAME = uname;
                PASSWORD = password;
                SAVE_PASS = save_pass;
            }

            private static void CacheUserInfo(string uname, string password, int save_pass) {
                PlayerPrefs.SetString("uname", uname);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetInt("savepass", save_pass);
                PlayerPrefs.Save();
            }

            public static void ClearUserInfo() {
                //UNAME = ""; ko clear username, su dung de autologin hoac dien vao phan goi y login
                NAME = "";
                PASSWORD = "";
                SAVE_PASS = 1;
            }
            #endregion
        }
        public class HardWare {
            /// <summary>
            /// hardware
            /// </summary>
            private static string imei = "";
            private static string cellid = "";
            private static string mnc = "";
            private static string mcc = "";
            private static string lac = "";
            private static string platform = "";
            private static string device = "";
            private static string macaddress = "";

            public static void InitHardWare() {
                //init hardware info
                HardWare.IMEI = SystemInfo.deviceUniqueIdentifier;
                HardWare.CELLID = "000000";
                HardWare.MNC = "04";
                HardWare.MCC = "452";
                HardWare.LAC = "0";
                HardWare.PLATFORM = Application.platform.ToString();
                HardWare.DEVICE = SystemInfo.deviceName;
            }

            #region HARDWARE
            public static string IMEI {
                get {
                    return imei;
                }
                private set {
                    imei = value != null ? value : "";
                }
            }

            public static string CELLID {
                get {
                    return cellid;
                }
                set {
                    cellid = value != null ? value : "";
                }
            }

            public static string MNC {
                get {
                    return mnc;
                }
                set {
                    mnc = value != null ? value : "";
                }
            }

            public static string MCC {
                get {
                    return mcc;
                }
                set {
                    mcc = value != null ? value : "";
                }
            }

            public static string LAC {
                get {
                    return lac;
                }
                set {
                    lac = value != null ? value : "";
                }
            }

            public static string PLATFORM {
                get {
                    return platform;
                }
                private set {
                    platform = value != null ? value : "";
                    if (platform.ToLower().Contains("iphone"))
                        platform = "iphone";
                    if (platform.ToLower().Contains("android"))
                        platform = "android";
                }
            }

            public static string DEVICE {
                get {
                    return device;
                }
                private set {
                    device = value != null ? value : "";
                }
            }

            public static string MACADDRESS {
                get {
                    return macaddress;
                }
                set {
                    macaddress = value != null ? value : "";
                }
            }
            #endregion
        }
        public class Language {
            //http://stackoverflow.com/questions/3191664/list-of-all-locales-and-their-short-codes
            public static readonly string EN = "en";
            public static readonly string VN = "vn";
            private static string lang = "vn";

            private static JObject N;
            private static bool isInited = false;

            public static void InitLanguage() {
                if (!isInited) {
                    TextAsset all_lang_as_json_string = Resources.Load("res_langs") as TextAsset;
                    N = JObject.Parse(all_lang_as_json_string.text);

                    LANG = VN;
                    isInited = true;
                }
            }

            public static string LANG {
                get {
                    return lang;
                }
                set {
                    lang = value;
                }
            }

            public static string GetText(string key) {
                //Debug.LogError(lang + "   " + key);
                string value = N[lang][key].ToString();
                if (string.IsNullOrEmpty(value))
                    return key + "-" + lang;
                else
                    return value;
            }

            public static string GetText(string lang, string key) {
                return N[lang][key].ToString();
            }
        }

        public static string GetDeviceAccount() {
#if UNITY_ANDROID
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                // NGUIDebug.Log("GetDeviceAccount: class = " + activityClass);
                AndroidJavaObject Activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                //NGUIDebug.Log("GetDeviceAccount: activity = " + Activity);
                using (AndroidJavaClass androidAction = new AndroidJavaClass("com.library.ActivityAction")) {
                    // NGUIDebug.Log("GetDeviceAccount: action = " + androidAction);
                    return androidAction.CallStatic<string>("GetDeviceAccount", Activity);
                }
            }
#elif UNITY_IPHONE
			//return CallPlugin.getUUID();
			return SystemInfo.deviceUniqueIdentifier;
#else
            return SystemInfo.deviceUniqueIdentifier;
#endif
        }
        /// <summary>
        /// quit app
        /// </summary>
        public static void QuitApplication(bool forceQuit) {
#if UNITY_ANDROID
            if (forceQuit) {
                Application.Quit();
                return;
            }

            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                AndroidJavaObject Activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaClass activityAction = new AndroidJavaClass("com.library.ActivityAction")) {
                    activityAction.CallStatic("QuitApplication", Activity);
                }
            }
#else
            Application.Quit();
#endif
        }

        public static void InitClient() {
            UserInfo.InitUserInfo();
            HardWare.InitHardWare();
            Language.InitLanguage();
            SettingConfig.InitSetting();
        }
    }
}