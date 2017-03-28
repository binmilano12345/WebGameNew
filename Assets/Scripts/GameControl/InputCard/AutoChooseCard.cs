using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public enum TypeValueCard {
    None,
    Le,
    Doi,
    Sanh,
    Xam,
    BaDoiThong,
    TuQuy,
    BonDoiThong
}
public class AutoChooseCard {
    static List<int> cardResult = new List<int>();
    public static List<int> CardTrenBan = new List<int>();
    //public static TypeValueCard typeValueCard;
    public static int[] ChooseCard(int[] cardHand) {
        try {
            if (CardTrenBan.Count <= 0) { CardTrenBan.Clear(); return null; }
            //List<int> listTempTamThoi = new List<int>();

            cardResult.Clear();

            //listTempTamThoi.Clear();
            //listTempTamThoi.AddRange(cardHandInput);
            //listTempTamThoi.Sort();
            //int[] cardHand = listTempTamThoi.ToArray();
            Array.Sort(cardHand);
            //Array.Sort(cardTrenBanInput);

            //CardTrenBan.Clear();
            //CardTrenBan.AddRange(cardTrenBanInput);
            CardTrenBan.Sort();

            TypeValueCard typeValueCard = TypeValueCard.None;
            typeValueCard = GetTypeValueCard(CardTrenBan.ToArray());
            //            Debug.LogError(typeValueCard);
            switch (typeValueCard) {
                case TypeValueCard.Le:
                    List<int> l1 = BatLe(CardTrenBan.ToArray(), cardHand);
                    if (l1 != null)
                        cardResult.AddRange(l1);
                    break;
                case TypeValueCard.Doi:
                    List<int> l2 = BatDoi(CardTrenBan.ToArray(), cardHand);
                    if (l2 != null)
                        cardResult.AddRange(l2);
                    break;
                case TypeValueCard.Sanh:
                    List<int> l3 = BatSanh(CardTrenBan.ToArray(), cardHand);
                    if (l3 != null)
                        cardResult.AddRange(l3);
                    break;
                case TypeValueCard.Xam:
                    List<int> l4 = BatXam(CardTrenBan.ToArray(), cardHand);
                    if (l4 != null)
                        cardResult.AddRange(l4);
                    break;
                case TypeValueCard.BaDoiThong:
                    List<int> l5 = BatBaDoiThong(CardTrenBan.ToArray(), cardHand);
                    if (l5 != null)
                        cardResult.AddRange(l5);
                    break;
                case TypeValueCard.TuQuy:
                    List<int> l6 = BatTuQuy(CardTrenBan.ToArray(), cardHand);
                    if (l6 != null)
                        cardResult.AddRange(l6);
                    break;
                case TypeValueCard.BonDoiThong:
                    List<int> l7 = BatBonDoiThong(CardTrenBan.ToArray(), cardHand);
                    if (l7 != null)
                        cardResult.AddRange(l7);
                    break;
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
        //CardTrenBan.Clear();
        return cardResult.ToArray();
    }
    //TypeValueCard typeVaCurrent;
    static void AutoNhacCard(int idCardChoose, Card[] cardHandInput) {
        try {
            for (int i = 0; i < cardHandInput.Length; i++) {
                Card card = cardHandInput[i];
                if (card.IsChoose && card.ID == idCardChoose && card.isBatHayChua) {
                    card.IsChoose = false;
                    return;
                }
            }

            for (int i = 0; i < cardHandInput.Length; i++) {
                Card card = cardHandInput[i];
                if (card.IsChoose && GetValue(card.ID) == GetValue(idCardChoose) && card.isBatHayChua) {
                    GetCardByID(idCardChoose, cardHandInput).IsChoose = true;
                    return;
                }
            }

            List<Card> CardSang = new List<Card>();
            for (int i = 0; i < cardHandInput.Length; i++) {
                if (!cardHandInput[i].isDark && cardHandInput[i].isBatHayChua) {
                    CardSang.Add(cardHandInput[i]);
                }
            }

            TypeValueCard typeValueCard = TypeValueCard.None;
            if (CardTrenBan.Count > 0) {
                typeValueCard = GetTypeValueCard(CardTrenBan.ToArray());
            }
            Debug.LogError("Kieu bai: " + typeValueCard);
            switch (typeValueCard) {
                case TypeValueCard.Le:
                    if (GetValue(CardTrenBan[0]) == 15) {
                        if (GetValue(idCardChoose) != 15) {
                            bool isNhacChat2 = NhacTuQuy(idCardChoose, CardSang.ToArray());
                            if (!isNhacChat2) {
                                isNhacChat2 = NhacBonDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                                //Debug.LogError("Nhac bon doi thong " + isNhacChat2);
                                if (!isNhacChat2) {
                                    NhacBaDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                                }
                            }
                        } else {
                            NhacLe(idCardChoose, CardSang.ToArray());
                        }
                    } else {
                        NhacLe(idCardChoose, CardSang.ToArray());
                    }
                    break;
                case TypeValueCard.Doi:
                    if (GetValue(CardTrenBan[0]) == 15) {
                        bool isDoi = false;
                        if (GetValue(idCardChoose) == 15) {
                            isDoi = NhacDoi(idCardChoose, CardSang.ToArray());
                        }
                        if (!isDoi) {
                            if (!NhacTuQuy(idCardChoose, CardSang.ToArray())) {
                                NhacBonDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                            }
                        }
                    } else {
                        NhacDoi(idCardChoose, CardSang.ToArray());
                    }
                    break;
                case TypeValueCard.Sanh:
                    NhacSanh(idCardChoose, CardSang.ToArray(), CardTrenBan.Count);
                    break;
                case TypeValueCard.Xam:
                    NhacXam(idCardChoose, CardSang.ToArray());
                    break;
                case TypeValueCard.BaDoiThong:
                    if (!NhacTuQuy(idCardChoose, CardSang.ToArray())) {
                        NhacBaDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                    }
                    break;
                case TypeValueCard.TuQuy:
                    if (!NhacTuQuy(idCardChoose, CardSang.ToArray())) {
                        NhacBonDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                    }
                    break;
                case TypeValueCard.BonDoiThong:
                    NhacBonDoiThongKhongChat(idCardChoose, CardSang.ToArray());
                    break;
                default:
                    AutoNhac(idCardChoose, CardSang.ToArray());

                    break;
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    #region Tu dong khi khong phai chat
    static void AutoNhac(int idCardChoose, Card[] cardHandInput) {
        try {
            bool isNhac = NhacTuQuy(idCardChoose, cardHandInput);
            //Debug.LogError("Nhac Tu Quy: " + isNhac);
            if (!isNhac) {
                isNhac = NhacBonDoiThongKhongChat(idCardChoose, cardHandInput);
                //Debug.LogError("Nhac Bon Doi Thuong: " + isNhac);
                if (!isNhac) {
                    isNhac = NhacBaDoiThongKhongChat(idCardChoose, cardHandInput);
                    //Debug.LogError("Nhac Ba Doi Thuong: " + isNhac);
                    if (!isNhac) {
                        isNhac = NhacXam(idCardChoose, cardHandInput);
                        //Debug.LogError("Nhac Xam: " + isNhac);
                        if (!isNhac) {
                            isNhac = NhacDoi(idCardChoose, cardHandInput);
                            //Debug.LogError("Nhac Doi: " + isNhac);
                            if (!isNhac) {
                                isNhac = NhacSanhKhongChat(idCardChoose, cardHandInput);
                                //Debug.LogError("Nhac Sanh: " + isNhac);
                                if (!isNhac) {
                                    NhacLe(idCardChoose, cardHandInput);
                                }
                            }
                        }
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    static void ResetArrCard(Card[] cardHandInput) {
        try {
            for (int i = 0; i < cardHandInput.Length; i++) {
                cardHandInput[i].IsChoose = false;
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    #endregion
    #region Goi y nhung quan bai co the chat duoc
    #region Lay Gia tri Bai
    public static int GetValue(int id) {
        int vl = Card.cardPaint[id] % 13 + 1;
        if (vl == 1) vl = 14;
        if (vl == 2) vl = 15;
        return vl;
    }
    /// <summary>
    ///Co - 3, ro - 2, tep - 1, bich - 0
    /// </summary>
    public static int GetType(int id) {
        //return (id - 1) / 13 + 1;
        return Card.cardPaint[id] / 13;
    }
    #endregion
    #region Lay kieu bai tren ban
    public static TypeValueCard GetTypeValueCard(int[] cardTrenBan) {
        if (cardTrenBan.Length == 1) {
            return TypeValueCard.Le;
        } else if (cardTrenBan.Length == 2) {
            return TypeValueCard.Doi;
        } else if (cardTrenBan.Length == 3) {
            return IsType3Con(cardTrenBan);
        } else if (cardTrenBan.Length == 4) {
            return IsType4Con(cardTrenBan);
        } else if (cardTrenBan.Length == 5 || cardTrenBan.Length == 7 || cardTrenBan.Length == 9 || cardTrenBan.Length == 11) {
            return TypeValueCard.Sanh;
        } else if (cardTrenBan.Length == 6) {
            return IsType6Con(cardTrenBan);
        } else if (cardTrenBan.Length == 8) {
            return IsType8Con(cardTrenBan);
        } else if (cardTrenBan.Length == 10) {
            return IsType10Con(cardTrenBan);
        } else if (cardTrenBan.Length == 12) {
            return IsType12Con(cardTrenBan);
        } else return TypeValueCard.None;
    }
    static TypeValueCard IsType3Con(int[] cardTrenBan) {
        if (GetValue(cardTrenBan[0]) == GetValue(cardTrenBan[1]))
            return TypeValueCard.Xam;

        return TypeValueCard.Sanh;
    }
    static TypeValueCard IsType4Con(int[] cardTrenBan) {
        if (GetValue(cardTrenBan[0]) == GetValue(cardTrenBan[1]) && GetValue(cardTrenBan[1]) == GetValue(cardTrenBan[2])) {
            //if (GetValue(cardTrenBan[0]) == 15) {
            //    return TypeValueCard.None;
            //}
            return TypeValueCard.TuQuy;
        }
        return TypeValueCard.Sanh;
    }
    static TypeValueCard IsType6Con(int[] cardTrenBan) {
        int[] ccc = new int[cardTrenBan.Length];
        for (int i = 0; i < cardTrenBan.Length; i++) {
            ccc[i] = GetValue(cardTrenBan[i]);
        }

        List<int> list = ccc.ToList();
        list.Sort();
        if (GetValue(list[0]) == GetValue(list[1]) && GetValue(list[2]) == GetValue(list[3]) && (GetValue(list[0]) + 1) == GetValue(list[2]))
            return TypeValueCard.BaDoiThong;

        return TypeValueCard.Sanh;
    }
    static TypeValueCard IsType8Con(int[] cardTrenBan) {
        if (IsType6Con(cardTrenBan) == TypeValueCard.BaDoiThong)
            return TypeValueCard.BonDoiThong;
        return TypeValueCard.Sanh;
    }
    static TypeValueCard IsType10Con(int[] cardTrenBan) {
        if (IsType8Con(cardTrenBan) == TypeValueCard.BonDoiThong)
            return TypeValueCard.None;
        return TypeValueCard.Sanh;
    }
    static TypeValueCard IsType12Con(int[] cardTrenBan) {
        if (IsType10Con(cardTrenBan) == TypeValueCard.None)
            return TypeValueCard.None;

        List<int> list = cardTrenBan.ToList();
        list.Sort();
        int count = 0;
        count += cardTrenBan.TakeWhile(n => n == list[0]).ToArray().Length;
        count += cardTrenBan.TakeWhile(n => n == list[2]).ToArray().Length;
        count += cardTrenBan.TakeWhile(n => n == list[4]).ToArray().Length;
        count += cardTrenBan.TakeWhile(n => n == list[6]).ToArray().Length;
        count += cardTrenBan.TakeWhile(n => n == list[8]).ToArray().Length;
        count += cardTrenBan.TakeWhile(n => n == list[10]).ToArray().Length;
        if (count == 6) {
            return TypeValueCard.None;
        }

        return TypeValueCard.Sanh;
    }
    #endregion
    #region Get Group
    static List<int[]> GetGroupInCardHand(int[] cardH2) {
        int[] cardH = (int[])cardH2.Clone();
        List<int[]> list = new List<int[]>();
        for (int i = 0; i < cardH.Length; i++) {
            if (cardH[i] != 0) {
                int[] dm = GetGroup(cardH, cardH[i], i);
                if (dm != null) {
                    list.Add(dm);
                }
            }
        }
        return list;
    }

    static int[] GetGroup(int[] cardH, int value, int index) {
        List<int> l = new List<int>();
        for (int i = index; i < cardH.Length; i++) {
            if (GetValue(value) == GetValue(cardH[i]) && cardH[i] != 0) {
                l.Add(cardH[i]);
                cardH[i] = 0;
            }
        }
        l.Sort();
        if (l.Count > 1) {
            return l.ToArray();
        }
        return null;
    }
    #endregion
    #region Bat Le
    static List<int> BatLe(int[] cardTrenBan, int[] cardHand) {
        int giatri = GetValue(cardTrenBan[0]);
        int chat = GetType(cardTrenBan[0]);
        int temp = giatri * 10 + chat;

        List<int> listResult = new List<int>();
        for (int i = 0; i < cardHand.Length; i++) {
            int giatri2 = GetValue(cardHand[i]);
            int chat2 = GetType(cardHand[i]);
            if (giatri2 * 10 + chat2 > temp) {
                listResult.Add(cardHand[i]);
            }
        }
        if (giatri == 15) {//Neu la 2
                           //            Debug.LogError("Co thang danh 2 " + chat);
            List<int> l3 = LayRaDoiThong(cardHand, 3);
            if (l3 != null) {
                for (int i = 0; i < l3.Count; i++) {
                    if (!listResult.Contains(l3[i])) {
                        listResult.Add(l3[i]);
                    }
                }
            }

            List<int> ltu = LayRaTuQuy(cardHand);
            if (ltu != null) {
                for (int i = 0; i < ltu.Count; i++) {
                    if (!listResult.Contains(ltu[i])) {
                        listResult.Add(ltu[i]);
                    }
                }
            }
        }
        return listResult;
    }
    #endregion
    #region Bat Doi
    static List<int> BatDoi(int[] cardTrenBan, int[] cardHand) {
        List<int> listChon = new List<int>();
        int giatri = GetValue(cardTrenBan[cardTrenBan.Length - 1]);
        int chat = GetType(cardTrenBan[cardTrenBan.Length - 1]);
        int diemtrenban = giatri * 10 + chat;
        List<int[]> listGroup = GetGroupInCardHand(cardHand);
        for (int i = 0; i < listGroup.Count; i++) {
            int[] demo = listGroup[i];
            int giatri2 = GetValue(demo[demo.Length - 1]);
            int chat2 = GetType(demo[demo.Length - 1]);
            int diemtrentay = giatri2 * 10 + chat2;
            if (diemtrentay > diemtrenban) {
                for (int j = 0; j < demo.Length; j++) {
                    listChon.Add(demo[j]);
                }
            }
        }
        if (giatri == 15) {//Neu la doi 2
            List<int> l4 = AutoChooseCard.LayRaDoiThong(cardHand, 4);
            List<int> ltu = AutoChooseCard.LayRaTuQuy(cardHand);
            if (l4 != null)
                for (int i = 0; i < l4.Count; i++) {
                    if (!listChon.Contains(l4[i])) {
                        listChon.Add(l4[i]);
                    }
                }
            if (ltu != null)
                for (int i = 0; i < ltu.Count; i++) {
                    if (!listChon.Contains(ltu[i])) {
                        listChon.Add(ltu[i]);
                    }
                }
        }
        return listChon;
    }
    #endregion
    #region Bat Ba Con
    static List<int> BatXam(int[] cardTrenBan, int[] cardHand) {
        List<int> listChon = new List<int>();
        int giatri = GetValue(cardTrenBan[cardTrenBan.Length - 1]);
        int chat = GetType(cardTrenBan[cardTrenBan.Length - 1]);
        int diemtrenban = giatri * 10 + chat;

        List<int[]> list = GetGroupInCardHand(cardHand);
        for (int i = 0; i < list.Count; i++) {
            int[] demo = list[i];
            if (demo.Length >= 3) {
                int giatri2 = GetValue(demo[demo.Length - 1]);
                int chat2 = GetType(demo[demo.Length - 1]);
                if (giatri2 * 10 + chat2 > diemtrenban) {
                    for (int j = 0; j < demo.Length; j++) {
                        listChon.Add(demo[j]);
                    }
                }
            }
        }
        return listChon;
    }
    #endregion
    #region Bat Tu Quy
    static List<int> BatTuQuy(int[] cardTrenBan, int[] cardHand) {
        List<int> listChon = new List<int>();
        int giatri = GetValue(cardTrenBan[cardTrenBan.Length - 1]);
        int chat = GetType(cardTrenBan[cardTrenBan.Length - 1]);
        int diemtrenban = giatri * 10 + chat;

        List<int[]> list = GetGroupInCardHand(cardHand);

        for (int i = 0; i < list.Count; i++) {
            int[] demo = list[i];
            if (demo.Length >= 4) {
                int giatri2 = GetValue(demo[demo.Length - 1]);
                int chat2 = GetType(demo[demo.Length - 1]);
                if (giatri2 * 10 + chat2 > diemtrenban) {
                    for (int j = 0; j < demo.Length; j++) {
                        listChon.Add(demo[j]);
                    }
                }
            }
        }
        return listChon;
    }
    #endregion
    #region Bat Ba Doi Thong
    static List<int> BatBaDoiThong(int[] cardTrenBan, int[] cardHand) {
        List<int> listResult = new List<int>();
        int diemtrenban = GetValue(cardTrenBan[cardTrenBan.Length - 1]) * 10 + GetType(cardTrenBan[cardTrenBan.Length - 1]);

        List<int> l3Thong = LayRaDoiThong(cardHand, 3);
        if (l3Thong != null) {
            if (GetValue(l3Thong[l3Thong.Count - 1]) * 10 + GetType(l3Thong[l3Thong.Count - 1]) > diemtrenban || l3Thong.Count >= 8) {
                for (int i = 0; i < l3Thong.Count; i++) {
                    if (!listResult.Contains(l3Thong[i])) {
                        listResult.Add(l3Thong[i]);
                    }
                }
            }
        }

        List<int> ltu = LayRaTuQuy(cardHand);
        if (ltu != null) {
            for (int i = 0; i < ltu.Count; i++) {
                if (!listResult.Contains(ltu[i])) {
                    listResult.Add(ltu[i]);
                }
            }
        }
        return listResult;
    }
    #endregion
    #region Bat Bon Doi Thong
    static List<int> BatBonDoiThong(int[] cardTrenBan, int[] cardHand) {
        List<int> listResult = new List<int>();
        int diemtrenban = GetValue(cardTrenBan[cardTrenBan.Length - 1]) * 10 + GetType(cardTrenBan[cardTrenBan.Length - 1]);

        List<int> l4Thong = LayRaDoiThong(cardHand, 4);
        if (l4Thong != null) {
            if (GetValue(l4Thong[l4Thong.Count - 1]) * 10 + GetType(l4Thong[l4Thong.Count - 1]) > diemtrenban) {
                for (int i = 0; i < l4Thong.Count; i++) {
                    if (!listResult.Contains(l4Thong[i])) {
                        listResult.Add(l4Thong[i]);
                    }
                }
            }
        }
        return listResult;
    }
    #endregion
    #region Bat Sanh
    static List<int[]> LayMangCacSanhTrenTay(int[] cardTT) {
        int[] cardTrenTay = SortArrCard(cardTT);
        //Lay mang gia tri roi sap xep tang dan
        List<int> lSortValue = new List<int>();
        for (int i = 0; i < cardTrenTay.Length; i++) {
            int vl = GetValue(cardTrenTay[i]);
            if (!lSortValue.Contains(vl) && vl != 15) {
                lSortValue.Add(vl);
            }
        }
        lSortValue.Sort();
        //End====Lay mang gia tri roi sap xep tang dan
        //Lay danh sach cac mang sanh
        #region Lay mang gia tri sanh
        List<int[]> listArr = new List<int[]>();//gia tri cac sanh
        int indexLength = 0;
        while (indexLength < lSortValue.Count) {
            int[] dm = GetSanhInArrValue(lSortValue.ToArray(), indexLength);
            indexLength += dm.Length;
            listArr.Add(dm);
        }
        #endregion
        #region Lay mang ID trong card Hand theo mang gia tri sanh
        List<int[]> listId = new List<int[]>();
        for (int i = 0; i < listArr.Count; i++) {
            List<int> listtamThoi = new List<int>();
            for (int k = 0; k < listArr[i].Length; k++) {
                listtamThoi.AddRange(GetIDByValue(cardTrenTay, listArr[i][k]));
            }
            if (listtamThoi.Count > 0)
                listId.Add(listtamThoi.ToArray());
        }
        #endregion
        //End=====Lay danh sach cac mang sanh
        return listId;
    }
    static List<int> BatSanh(int[] cardTB, int[] cardTT) {
        //Sap xep cac mang truyen vao theo thu tu tang dan gia tri
        int[] cardTrenBan = SortArrCard(cardTB);
        int[] cardTrenTay = SortArrCard(cardTT);
        //End

        int DoDaiSanhTrenBan = cardTrenBan.Length;
        int diemtrenbanMax = GetValue(cardTrenBan[cardTrenBan.Length - 1]) * 10 + GetType(cardTrenBan[cardTrenBan.Length - 1]);
        int giatritrenbanMin = GetValue(cardTrenBan[0]);
        //Lay mang gia tri roi sap xep tang dan
        List<int> lSortValue = new List<int>();
        for (int i = 0; i < cardTrenTay.Length; i++) {
            int vl = GetValue(cardTrenTay[i]);
            if (!lSortValue.Contains(vl) && vl != 15) {
                lSortValue.Add(vl);
            }
        }
        lSortValue.Sort();
        //End====Lay mang gia tri roi sap xep tang dan
        //Lay danh sach cac mang sanh
        #region Lay mang gia tri sanh
        List<int[]> listArr = new List<int[]>();//gia tri cac sanh
        int indexLength = 0;
        while (indexLength < lSortValue.Count) {
            int[] dm = GetSanhInArrValue(lSortValue.ToArray(), indexLength);
            indexLength += dm.Length;
            if (dm.Length >= DoDaiSanhTrenBan) {
                int d = dm[dm.Length - 1] * 10 + dm[dm.Length - 1];
                if (d > diemtrenbanMax)
                    listArr.Add(dm);
            }
        }
        #endregion
        #region Lay mang ID trong card Hand theo mang gia tri sanh
        List<int[]> listId = new List<int[]>();
        for (int i = 0; i < listArr.Count; i++) {
            List<int> listtamThoi = new List<int>();
            for (int k = 0; k < listArr[i].Length; k++) {
                listtamThoi.AddRange(GetIDByValue(cardTrenTay, listArr[i][k]));
            }
            if (listtamThoi.Count > 0)
                listId.Add(listtamThoi.ToArray());
        }
        #endregion
        //End=====Lay danh sach cac mang sanh
        #region Lay danh sach card hand dat chuan
        List<int> lResult = new List<int>();
        for (int i = 0; i < listId.Count; i++) {
            int[] mangTt = LaySanhDatChuan(cardTrenBan, SortArrCard(listId[i]));
            //DanhSachSanhGoiY.Add(mangTt);
            for (int k = 0; k < mangTt.Length; k++) {
                if (!lResult.Contains(mangTt[k])) {
                    lResult.Add(mangTt[k]);
                }
            }
        }
        #endregion
        return lResult;
    }
    /// <summary>
    /// tra ve mot mang sanh theo gia tri
    /// </summary>
    static int[] GetSanhInArrValue(int[] arr, int index) {
        List<int> l = new List<int>();
        int temp = arr[index];
        l.Add(arr[index]);
        for (int i = index + 1; i < arr.Length; i++) {
            if (temp == arr[i] - 1) {
                l.Add(arr[i]);
                temp = arr[i];
            } else {
                break;
            }
        }
        l.Sort();
        return l.ToArray();
    }
    static int[] GetIDByValue(int[] cardHand, int value) {//Lay cac id trang card hand boi ID
        List<int> arrr = new List<int>();
        for (int i = 0; i < cardHand.Length; i++) {
            if (value == GetValue(cardHand[i])) {
                arrr.Add(cardHand[i]);
            }
        }
        return arrr.ToArray();
    }
    static int[] LaySanhDatChuan(int[] sanhtrenban, int[] sanhTT) {
        List<int> lTemp = new List<int>();
        int diemtrenbanMax = GetValue(sanhtrenban[sanhtrenban.Length - 1]) * 10 + GetType(sanhtrenban[sanhtrenban.Length - 1]);
        int giatritrenbanMin = GetValue(sanhtrenban[0]);
        int thangCuoiCungCuaMangTT = sanhTT[sanhTT.Length - 1];
        lTemp.AddRange(SortArrCard(sanhTT).ToList());
        for (int k = 0; k < sanhTT.Length; k++) {
            if (GetValue(thangCuoiCungCuaMangTT) == GetValue(sanhTT[k])) {
                int diemtrentaytamthoi = GetValue(sanhTT[k]) * 10 + GetType(sanhTT[k]);
                if (diemtrentaytamthoi < diemtrenbanMax) {
                    lTemp.Remove(sanhTT[k]);
                }
            }
            if (GetValue(sanhTT[k]) < giatritrenbanMin) {
                lTemp.Remove(sanhTT[k]);
            }
        }
        List<int> lResult = new List<int>();
        int indexForced = 0;
        for (int i = lTemp.Count - 1; i >= 0; i--) {
            if (GetValue(lTemp[i]) * 10 + GetType(lTemp[i]) > diemtrenbanMax) {
                indexForced = i;
                lResult.Add(lTemp[i]);
            } else {
                int[] dm = LaySanhChuanTheoDoDai(indexForced, lTemp, sanhtrenban.Length);
                for (int j = 0; j < dm.Length; j++) {
                    if (!lResult.Contains(dm[j])) {
                        lResult.Add(dm[j]);
                    }
                }
                break;
            }
        }

        return SortArrCard(lResult.ToArray());
    }
    static int[] LaySanhChuanTheoDoDai(int index, List<int> sanhTT, int lengthSanhTB) {
        List<int> lResult = new List<int>();
        int temp = sanhTT[index];
        //lResult.Add(temp);
        int count = 1;
        for (int i = index - 1; i >= 0; i--) {
            if (GetValue(temp) == GetValue(sanhTT[i])) {
                if (!lResult.Contains(sanhTT[i])) {
                    lResult.Add(sanhTT[i]);
                }
            } else if (GetValue(temp) == GetValue(sanhTT[i]) + 1) {
                temp = sanhTT[i];
                count++;
                if (!lResult.Contains(sanhTT[i])) {
                    lResult.Add(sanhTT[i]);
                }
            }
            if (count >= lengthSanhTB) break;
        }
        return lResult.ToArray();
    }
    #endregion
    #region Lay Ra Doi Thong
    public static List<int> LayRaDoiThong(int[] cardHand, int SoThong) {
        List<int> listResult = new List<int>();
        int coutThong = 1;
        try {
            //lay cac nhom roi Sap xep lai
            List<int[]> list = GetGroupInCardHand(SortArrCard(cardHand));
            //        for (int i = 0; i < list.Count; i++) {
            //            Debug.LogError("Trc khi sap xep: " + GetValue(list[i][0]));
            //        }
            //list.Sort(delegate (int[] arr1, int[] arr2) {
            //    return arr1[0].CompareTo(arr2[0]);
            //});
            list.OrderBy(r => r[0]).ThenBy(r2 => r2[0]);

            //        Debug.LogError("Lay nhom " + list.Count);
            //        for (int i = 0; i < list.Count; i++) {
            //            Debug.LogError("Sau khi sap xep: " + GetValue(list[i][0]));
            //        }
            if (list.Count < SoThong) return null;

            listResult.Clear();
            for (int i = 1; i < list.Count; i++) {
                if (coutThong >= SoThong) {
                    break;
                }
                int valueCuaThangTruoc = GetValue(list[i - 1][0]);
                int valueCuaThangSau = GetValue(list[i][0]);
                //Debug.LogError("valueCuaThangTruoc:   " + valueCuaThangTruoc);
                //Debug.LogError("valueCuaThangSau:   " + valueCuaThangSau);
                if ((valueCuaThangTruoc + 1) == valueCuaThangSau) {
                    for (int j = 0; j < list[i - 1].Length; j++) {
                        if (!listResult.Contains(list[i - 1][j])) {
                            listResult.Add(list[i - 1][j]);
                            //Debug.LogError("Add thang bo me dau tien nay vao " + list[i - 1][j]);
                        }
                    }

                    if (valueCuaThangSau != 15) {
                        coutThong++;
                        for (int j = 0; j < list[i].Length; j++) {
                            if (!listResult.Contains(list[i][j])) {
                                listResult.Add(list[i][j]);
                                //Debug.LogError("Add thang bo me tu thu hai nay vao " + list[i][j]);
                            }
                        }
                    }
                } else {
                    if (coutThong < SoThong) {
                        coutThong = 1;
                        listResult.Clear();
                        for (int j = 0; j < list[i].Length; j++) {
                            if (!listResult.Contains(list[i][j])) {
                                listResult.Add(list[i][j]);
                            }
                        }
                    } else {
                        break;
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
        //        Debug.LogError("Lay ra so thang: " + listResult.Count);
        if (listResult.Count >= SoThong * 2 && coutThong >= SoThong)
            return listResult;
        return null;
    }
    #endregion
    #region Lay Ra Tu Quy
    static List<int> LayRaTuQuy(int[] cardHand) {
        List<int> listChon = new List<int>();
        List<int[]> list = GetGroupInCardHand(cardHand);

        for (int i = 0; i < list.Count; i++) {
            int[] demo = list[i];
            if (demo.Length >= 4) {
                for (int j = 0; j < demo.Length; j++) {
                    listChon.Add(demo[j]);
                }
            }
        }
        return listChon;
    }
    #endregion
    #endregion Goi y nhung quan bai co the chat duoc
    #region Sap xep mang theo gia tri
    /// <summary>
    /// Sap xep mang tang dan
    /// </summary>
    public static int[] SortArrCard(int[] arrCard) {
        Dictionary<int, int> dicTemp = new Dictionary<int, int>();
        for (int i = 0; i < arrCard.Length; i++) {
            int value = GetValue(arrCard[i]);
            if (!dicTemp.ContainsKey(arrCard[i]))
                dicTemp.Add(arrCard[i], value);
        }
        List<int> listResult = new List<int>();
        foreach (var item in dicTemp.OrderBy(r => r.Value)) {
            listResult.Add(item.Key);
        }
        return listResult.ToArray();
    }
    #endregion
    #region Lay nhung quan bai khi duoc chon
    public static void ClickCard(Card card, Card[] cardHandInput) {
        AutoNhacCard(card.ID, cardHandInput);
    }
    #region Nhac Le
    static void NhacLe(int id, Card[] cardHandInput) {
        ResetArrCard(cardHandInput);
        for (int i = 0; i < cardHandInput.Length; i++) {
            Card card = cardHandInput[i];
            card.IsChoose = false;
            if (card.ID == id) {
                card.IsChoose = true;
            }
            //}
        }
    }
    #endregion
    #region Nhac Doi
    static bool NhacDoi(int id, Card[] cardHandInput) {
        ResetArrCard(cardHandInput);
        bool KieuGICungChon = false;
        int coutDoi = 0;
        for (int i = 0; i < cardHandInput.Length; i++) {
            Card card = cardHandInput[i];
            card.IsChoose = false;
            if (GetValue(card.ID) == GetValue(id)) {
                if (card.ID == id) {
                    card.IsChoose = true;
                    coutDoi++;
                } else if (!KieuGICungChon) {
                    card.IsChoose = true;
                    coutDoi++;
                    KieuGICungChon = true;
                }
            }

            if (coutDoi == 2) break;
        }
        if (coutDoi == 2) return true;
        return false;
    }
    #endregion
    #region Nhac Xam
    static bool NhacXam(int id, Card[] cardHandInput) {
        ResetArrCard(cardHandInput);
        int countXam = 0;
        int KieuGICungChon = -1;
        List<Card> cardValue = new List<Card>();
        for (int i = 0; i < cardHandInput.Length; i++) {
            Card card = cardHandInput[i];
            card.IsChoose = false;
            if (GetValue(card.ID) == GetValue(id)) {
                if (card.ID == id) KieuGICungChon = i;
                cardValue.Add(card);
            }
        }
        if (cardValue.Count == 3) {
            for (int i = 0; i < cardValue.Count; i++) {
                Card card = cardValue[i];
                card.IsChoose = true;
                countXam++;
            }
        } else {
            if (KieuGICungChon < 2) {
                for (int i = 0; i < cardValue.Count - 1; i++) {
                    Card card = cardValue[i];
                    card.IsChoose = true;
                    countXam++;
                }
            } else {
                for (int i = 1; i < cardValue.Count; i++) {
                    Card card = cardValue[i];
                    card.IsChoose = true;
                    countXam++;
                }
            }
        }

        if (countXam == 3) return true;
        return false;
    }
    #endregion
    #region Nhac Tu Quy
    static bool NhacTuQuy(int id, Card[] cardHandInput) {
        ResetArrCard(cardHandInput);
        int cout = 0;
        for (int i = 0; i < cardHandInput.Length; i++) {
            Card card = cardHandInput[i];
            card.IsChoose = false;
            if (!card.isDark) {
                if (card.ID == id) {
                    card.IsChoose = true;
                    cout++;
                }
                if (GetValue(card.ID) == GetValue(id) && card.ID != id) {
                    card.IsChoose = true;
                    cout++;
                }
            }
        }
        if (cout == 4) return true;
        return false;

    }
    #endregion
    #region Nhac Sanh
    //static List<int[]> DanhSachSanhGoiY = new List<int[]>();
    static void NhacSanh(int id, Card[] cardHandInput, int lengthSanh) {
        try {
            ResetArrCard(cardHandInput);
            List<int> llll = new List<int>();
            for (int i = 0; i < cardHandInput.Length; i++) {
                if (cardHandInput[i].isBatHayChua)
                    llll.Add(cardHandInput[i].ID);
            }
            List<int[]> ListSanhGoiY = LayMangCacSanhTrenTay(llll.ToArray());

            if (ListSanhGoiY.Count <= 0) return;

            List<int> MangNayNe = new List<int>();
            for (int i = 0; i < ListSanhGoiY.Count; i++) {
                if (ListSanhGoiY[i].Contains(id)) {
                    MangNayNe.AddRange(ListSanhGoiY[i]);
                    break;
                }
            }
            int count = 0;
            int indexI = 0;
            for (int i = 0; i < MangNayNe.Count; i++) {
                Card card = GetCardByID(MangNayNe[i], cardHandInput);
                if (card != null) {
                    //card.IsChoose = false;
                    if (card.ID == id) {
                        card.IsChoose = true;
                        count++;
                        indexI = i;
                        //Debug.LogError("Nhac thang duoc chon " + GetValue(MangNayNe[i]) + " Count " + count);
                        break;
                    };
                }
            }

            if (MangNayNe.Count <= 2) return;
            List<Card> CardNhac = new List<Card>();
            int temp = id;
            for (int i = indexI + 1; i < MangNayNe.Count; i++) {
                if (count >= lengthSanh) break;
                if (GetValue(temp) == GetValue(MangNayNe[i]) - 1) {
                    Card card = GetCardByID(MangNayNe[i], cardHandInput);
                    if (card != null) {
                        //card.IsChoose = true;
                        CardNhac.Add(card);

                        temp = MangNayNe[i];
                        count++;
                        //Debug.LogError("Nhac thang ben phai " + GetValue(MangNayNe[i]) + " Count " + count);
                    }
                }
            }
            if (count < lengthSanh) {
                temp = id;
                for (int i = indexI - 1; i >= 0; i--) {
                    if (count >= lengthSanh) break;
                    if (GetValue(temp) == GetValue(MangNayNe[i]) + 1) {
                        Card card = GetCardByID(MangNayNe[i], cardHandInput);
                        if (card != null) {
                            //card.IsChoose = true;
                            CardNhac.Add(card);
                            temp = MangNayNe[i];
                            count++;
                            //Debug.LogError("Nhac thang ben trai " + GetValue(MangNayNe[i]) + " Count " + count);
                        }
                    }
                }
            }
            if (count >= lengthSanh) {
                for (int i = 0; i < CardNhac.Count; i++) {
                    CardNhac[i].IsChoose = true;
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public static bool NhacSanhKhongChat(int id, Card[] cardHandInput) {
        try {
            ResetArrCard(cardHandInput);
            List<int> llll = new List<int>();
            for (int i = 0; i < cardHandInput.Length; i++) {
                if (cardHandInput[i].isBatHayChua)
                    llll.Add(cardHandInput[i].ID);
            }

            List<int[]> ListSanhGoiY = LayMangCacSanhTrenTay(llll.ToArray());
            List<int> MangNayNe = new List<int>();
            for (int i = 0; i < ListSanhGoiY.Count; i++) {
                if (ListSanhGoiY[i].Contains(id)) {
                    MangNayNe.AddRange(ListSanhGoiY[i]);
                    break;
                }
            }
            //int count = 0;
            int indexI = 0;
            for (int i = 0; i < MangNayNe.Count; i++) {
                Card card = GetCardByID(MangNayNe[i], cardHandInput);
                if (card != null) {
                    //card.IsChoose = false;
                    if (card.ID == id) {
                        card.IsChoose = true;
                        indexI = i;
                        break;
                    }
                }
            }

            if (MangNayNe.Count <= 2) return false;

            List<Card> CardNhac = new List<Card>();
            int temp = id;
            for (int i = indexI + 1; i < MangNayNe.Count; i++) {
                if (GetValue(temp) == GetValue(MangNayNe[i]) - 1) {
                    Card card = GetCardByID(MangNayNe[i], cardHandInput);
                    //card.IsChoose = true;
                    CardNhac.Add(card);
                    temp = MangNayNe[i];
                }
            }
            temp = id;

            for (int i = indexI - 1; i >= 0; i--) {
                if (GetValue(temp) == GetValue(MangNayNe[i]) + 1) {
                    Card card = GetCardByID(MangNayNe[i], cardHandInput);
                    //card.IsChoose = true;
                    CardNhac.Add(card);
                    temp = MangNayNe[i];
                }
            }
            if (CardNhac.Count >= 2) {
                for (int i = 0; i < CardNhac.Count; i++) {
                    CardNhac[i].IsChoose = true;
                }
                return true;
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
        return false;
    }
    static Card GetCardByID(int id, Card[] ArrCard) {
        for (int i = 0; i < ArrCard.Length; i++) {
            if (id == ArrCard[i].ID)
                return ArrCard[i];
        }

        return null;
    }
    #endregion
    #region Nhac ba doi thong
    static bool NhacBaDoiThong(int id, Card[] cardHand) {
        List<Card> ListThong = cardHand.ToList();
        for (int i = 0; i < ListThong.Count; i++) {
            if (GetValue(ListThong[i].ID) == 15) {
                ListThong.RemoveAt(i);
            }
        }

        if (ListThong.Count < 6) return false;

        ListThong.OrderBy(r => GetValue(r.ID)).ThenBy(r2 => GetValue(r2.ID));

        int currentValue = GetValue(cardHand[0].ID);
        //		Debug.LogError ("currentValue " + currentValue);
        List<int> listDoi = new List<int>();
        listDoi.Add(cardHand[0].ID);
        int count = 0;

        //List<int> lResult = new List<int>();
        for (int i = 1; i < ListThong.Count; i++) {
            if (currentValue != GetValue(ListThong[i].ID)) {
                currentValue = GetValue(ListThong[i].ID);
                count = 0;
                if (!listDoi.Contains(ListThong[i].ID))
                    listDoi.Add(ListThong[i].ID);
            } else {
                if (count == 0) {
                    if (!listDoi.Contains(ListThong[i].ID))
                        listDoi.Add(ListThong[i].ID);
                }
                count++;
            }
        }

        if (listDoi.Count < 6) return false;
        for (int i = 0; i < cardHand.Length; i++) {
            cardHand[i].IsChoose = false;
        }

        if (listDoi.Contains(id)) {
            if (listDoi.Count == 6) {
                for (int i = 0; i < listDoi.Count; i++) {
                    Card card = GetCardByID(listDoi[i], cardHand);
                    card.IsChoose = true;
                }
            }
            if (listDoi.Count == 8) {
                Card card = GetCardByID(listDoi[listDoi.Count - 1], cardHand);
                if (GetValue(card.ID) == GetValue(id)) {
                    for (int i = listDoi.Count - 1; i >= 2; i--) {
                        Card c = GetCardByID(listDoi[i], cardHand);
                        c.IsChoose = true;
                    }
                } else {
                    for (int i = 0; i < listDoi.Count - 2; i++) {
                        Card c = GetCardByID(listDoi[i], cardHand);
                        c.IsChoose = true;
                    }
                }
            }

        } else {
            if (listDoi.Count == 6) {
                Card c = GetCardByID(id, cardHand);
                c.IsChoose = true;
                int cout = 0;
                for (int i = 0; i < listDoi.Count; i++) {
                    Card card = GetCardByID(listDoi[i], cardHand);
                    if (GetValue(card.ID) == GetValue(id)) {
                        if (cout == 0) {
                            card.IsChoose = true;
                        }
                        cout++;
                    } else {
                        card.IsChoose = true;
                    }
                }
            }
            if (listDoi.Count == 8) {
                Card c = GetCardByID(id, cardHand);
                c.IsChoose = true;
                int cout = 0;
                Card card = GetCardByID(listDoi[listDoi.Count - 1], cardHand);
                if (GetValue(card.ID) == GetValue(id)) {
                    for (int i = listDoi.Count - 2; i >= 2; i--) {
                        Card cc = GetCardByID(listDoi[i], cardHand);
                        cc.IsChoose = true;
                    }
                } else {
                    for (int i = 0; i < listDoi.Count - 2; i++) {
                        Card cc = GetCardByID(listDoi[i], cardHand);
                        if (GetValue(cc.ID) == GetValue(id)) {
                            if (cout == 0) {
                                cc.IsChoose = true;
                            }
                            cout++;
                        } else {
                            cc.IsChoose = true;
                        }
                    }
                }
            }
        }

        return true;
    }
    static bool NhacBaDoiThongKhongChat(int id, Card[] cardHandInput) {
        List<int> llll = new List<int>();
        for (int i = 0; i < cardHandInput.Length; i++) {
            if (cardHandInput[i].isBatHayChua)
                llll.Add(cardHandInput[i].ID);
        }
        List<int> l3 = LayRaDoiThong(llll.ToArray(), 3);
        if (l3 == null) return false;
        if (!l3.Contains(id)) return false;
        List<Card> list = new List<Card>();
        for (int i = 0; i < l3.Count; i++) {
            list.Add(GetCardByID(l3[i], cardHandInput));
        }

        return NhacBaDoiThong(id, list.ToArray());
    }
    #endregion
    #region Nhac Bon Doi Thong
    static bool NhacBonDoiThong(int id, Card[] cardHand) {
        ResetArrCard(cardHand);
        List<Card> ListThong = cardHand.ToList();
        for (int i = 0; i < ListThong.Count; i++) {
            if (GetValue(ListThong[i].ID) == 15) {
                ListThong.RemoveAt(i);
            }
        }

        if (ListThong.Count < 8) return false;

        ListThong.OrderBy(r => GetValue(r.ID)).ThenBy(r2 => GetValue(r2.ID));
        int count = 0;

        int currentValue = GetValue(ListThong[0].ID);
        List<int> listDoi = new List<int>();
        listDoi.Add(cardHand[0].ID);
        count = 0;

        for (int i = 1; i < ListThong.Count; i++) {
            if (currentValue == GetValue(ListThong[i].ID)) {
                if (count == 0) {
                    if (!listDoi.Contains(cardHand[i].ID))
                        listDoi.Add(cardHand[i].ID);
                }
                count++;
            } else {
                currentValue = GetValue(cardHand[i].ID);
                count = 0;
                if (!listDoi.Contains(cardHand[i].ID))
                    listDoi.Add(cardHand[i].ID);
            }
        }
        if (listDoi.Count < 8) return false;

        if (listDoi.Contains(id)) {
            for (int i = 0; i < listDoi.Count; i++) {
                Card c = GetCardByID(listDoi[i], cardHand);
                c.IsChoose = true;
            }
        } else {
            Card c = GetCardByID(id, cardHand);
            c.IsChoose = true;
            bool isChonRoi = false;
            for (int i = 0; i < listDoi.Count; i++) {
                Card cc = GetCardByID(listDoi[i], cardHand);
                if (GetValue(id) == GetValue(listDoi[i])) {
                    if (!isChonRoi) {
                        cc.IsChoose = true;
                    }
                    isChonRoi = true;
                } else {
                    cc.IsChoose = true;
                }
            }
        }
        return true;
    }
    static bool NhacBonDoiThongKhongChat(int id, Card[] cardHandInput) {
        List<int> llll = new List<int>();
        for (int i = 0; i < cardHandInput.Length; i++) {
            if (cardHandInput[i].isBatHayChua)
                llll.Add(cardHandInput[i].ID);
        }
        List<int> l3 = LayRaDoiThong(llll.ToArray(), 4);
        if (l3 == null) return false;
        if (!l3.Contains(id)) return false;
        List<Card> list = new List<Card>();
        for (int i = 0; i < l3.Count; i++) {
            list.Add(GetCardByID(l3[i], cardHandInput));
        }

        return NhacBonDoiThong(id, list.ToArray());
    }
    #endregion
    #endregion

    #region Lay Ra 5 Doi Thong
    public static bool LayRaNamDoiThong(int[] cardHand) {
        List<int> l5Thong = LayRaDoiThong(cardHand, 5);
        if (l5Thong != null)
            return true;
        return false;
    }
    #endregion

    #region Lay Ra 6 Doi Thong
    public static bool LayRaSauDoiThong(int[] cardHand) {
        List<int[]> list6Doi = GetGroupInCardHand(cardHand);
        if (list6Doi.Count == 6)
            return true;
        return false;
    }
    #endregion

    #region Lay Ra Tu Quy 2
    public static bool LayRaTuQuyHai(int[] carhHand) {
        List<int> listTuQuy = LayRaTuQuy(carhHand);
        if (listTuQuy.Count > 0) {
            if (GetValue(listTuQuy[0]) == 15)
                return true;
        }
        return false;
    }
    #endregion
}