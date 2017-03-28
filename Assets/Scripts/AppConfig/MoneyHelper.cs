using System.Collections;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AppConfig;

namespace Us.Mobile.Utilites {
    public class MoneyHelper {
        //public static readonly string TAG = "MONEYHELPER";
        public static readonly string UNIT_CASH = "Xu";
        private static readonly string PATTERN_US = "{0:#,###,###.##}";
        private static readonly string PATTERN_VN = "{0:#.###.###,##}";
        private static readonly string PATTERN_FORMAT = "#,###.##";


        //https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx
        private static readonly string CULTURE_VN = "vi-VN";
        private static readonly string CULTURE_US = "en-US";

        private static Dictionary<string, string[]> MONEY_CHARACTER = new Dictionary<string, string[]>() {
                                                            { ClientConfig.Language.VN, new string[] { "", " K", " M", " B" } },
                                                            { ClientConfig.Language.EN, new string[] { "", " K", " M", " B" } } };
        private static Dictionary<string, string[]> MONEY_CHARACTER_POKER = new Dictionary<string, string[]>() {
        { ClientConfig.Language.VN, new string[] { "", "K", "M", "B" } },
        { ClientConfig.Language.EN, new string[] { "", "K", "M", "B" } }};


        /// <summary>
        /// convert number to pattern #,###
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatNumberAbsolute(long number) {
            //return String.Format(PATTERN_US, cash) + " " + UNIT_CASH;
            CultureInfo cul = CultureInfo.GetCultureInfo(GetCulture(ClientConfig.Language.LANG));   // try with "en-US"
            string a = number.ToString(PATTERN_FORMAT, cul.NumberFormat);
            return a == null || a.Length == 0 ? "0" : a;
        }

        /// <summary>
        /// convert cash to pattern #,### Xu
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatAbsoluteWithoutUnit(long cash) {
            return FormatNumberAbsolute(cash);
        }

        /// <summary>
        /// convert cash to pattern #,### Xu
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatAbsolute(long cash) {
            return FormatNumberAbsolute(cash) + " " + UNIT_CASH;
        }

        /// <summary>
        /// convert cash to pattern #,### Bạc
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatAbsoluteSilver(long cash) {
            return FormatNumberAbsolute(cash) + " " + ClientConfig.Language.GetText("silver");
        }

        /// <summary>
        /// convert cash to pattern #,### Vàng
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatAbsoluteGold(long cash) {
            return FormatNumberAbsolute(cash) + " " + ClientConfig.Language.GetText("gold");
        }

        /// <summary>
        /// conver cash to pattern #,###.##
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        private static string FormatFloat(float cash) {
            //do khi lam tron 0.005 se bi lam tron len 0.01 nen kiem tra truoc
            if ((Convert.ToInt64(cash * 1000) % 10) >= 5)
                cash = cash - 0.005f;

            //return String.Format(PATTERN_US, cash);
            CultureInfo cul = CultureInfo.GetCultureInfo(GetCulture(ClientConfig.Language.LANG));   // try with "en-US"
            string a = cash.ToString(PATTERN_FORMAT, cul.NumberFormat);
            return a == null || a.Length == 0 ? "0" : a;
        }

        public static string FormatRelativelyWithoutUnitForCenter(long cash) {
            if (cash < 1000)
                return cash + "";
            if (cash < 1000000)
                return FormatFloat((float)cash / 1000) + MONEY_CHARACTER_POKER[ClientConfig.Language.LANG][1];
            if (cash < 1000000000)
                return FormatFloat((float)(cash / 1000) / 1000) + MONEY_CHARACTER_POKER[ClientConfig.Language.LANG][2];
            return FormatFloat((float)(cash / 1000000) / 1000) + MONEY_CHARACTER_POKER[ClientConfig.Language.LANG][3];
        }

        /// <summary>
        /// convert cash to pattern #,### [N, Tr, Ty...] Xu 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatRelatively(long cash) {
            return FormatRelativelyWithoutUnit(cash) + " " + UNIT_CASH;
        }

        /// <summary>
        /// convert cash to pattern #,### [N, Tr, Ty...] Bạc 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatRelativelySilver(long cash) {
            return FormatRelativelyWithoutUnit(cash) + " " + ClientConfig.Language.GetText("silver"); ;
        }

        /// <summary>
        /// convert cash to pattern #,### [N, Tr, Ty...] Vàng 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatRelativelyGold(long cash) {
            return FormatRelativelyWithoutUnit(cash) + " " + ClientConfig.Language.GetText("gold"); ;
        }

        /// <summary>
        /// convert cash to pattern #,### [N, Tr, Ty...]
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public static string FormatRelativelyWithoutUnit(long cash) {
            if (cash < 1000)
                return cash + "";
            if (cash < 1000000)
                return FormatFloat((float)cash / 1000) + MONEY_CHARACTER[ClientConfig.Language.LANG][1];
            if (cash < 1000000000)
                return FormatFloat((float)(cash / 1000) / 1000) + MONEY_CHARACTER[ClientConfig.Language.LANG][2];
            return FormatFloat((float)(cash / 1000000) / 1000) + MONEY_CHARACTER[ClientConfig.Language.LANG][3];
        }

        public static long FormatRelatively(string cash) {
            string CASH_CHAN = "";
            string CASH_LE = "";

            cash = cash.Replace(UNIT_CASH, "");
            for (int i = 0; i < cash.Length; i++) {
                if (cash[i].ToString() == "," || cash[i].ToString() == ".") {
                    CASH_CHAN = cash.Substring(0, i);
                    CASH_LE = cash.Substring(i + 1);
                    CASH_LE = CASH_LE.Replace(" ", "");

                }
            }
            cash = cash.Replace(",", "");
            cash = cash.Replace(".", "");
            cash = cash.Replace(" ", "");
            if (cash.EndsWith("N")) {
                if (CASH_CHAN != "" && CASH_LE != "") {
                    double pow = double.Parse((CASH_LE.Length - 1).ToString());
                    long a = long.Parse(Math.Pow(10, pow).ToString());
                    return (long.Parse(CASH_CHAN) * 1000) + (long.Parse(CASH_LE.Substring(0, CASH_LE.Length - 1)) * (1000 / a));
                }
                return long.Parse(cash.Substring(0, cash.Length - 1)) * 1000;
            }
            if (cash.EndsWith("Tr")) {
                if (CASH_CHAN != "" && CASH_LE != "") {
                    double pow = double.Parse((CASH_LE.Length - 2).ToString());
                    long a = long.Parse(Math.Pow(10, pow).ToString());
                    return (long.Parse(CASH_CHAN) * 1000000) + (long.Parse(CASH_LE.Substring(0, CASH_LE.Length - 2)) * (1000000 / a));
                }
                return long.Parse(cash.Substring(0, cash.Length - 2)) * 1000000;
            }
            if (cash.EndsWith("Ty")) {
                if (CASH_CHAN != "" && CASH_LE != "") {
                    double pow = double.Parse((CASH_LE.Length - 2).ToString());
                    long a = long.Parse(Math.Pow(10, pow).ToString());
                    return (long.Parse(CASH_CHAN) * 1000000000) + (long.Parse(CASH_LE.Substring(0, CASH_LE.Length - 2)) * (1000000000 / a));
                }
                return long.Parse(cash.Substring(0, cash.Length - 2)) * 1000000000;
            }
            return long.Parse(cash);
        }
        /// <summary>
        /// format money to pattern #,###
        /// </summary>
        public static string FormatMoneyNormal(long m) {
            //if (m <= 0) return "0";
            return m.ToString("0,0");
        }
        private static string GetCulture(string lang) {
            if (lang.Equals(ClientConfig.Language.VN)) {
                return CULTURE_VN;
            }
            return CULTURE_US;
        }
    }
}
