using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AppConfig;

public class TypeCardMauBinh {
    public static TYPE_CARD GetTypeCardMauBinh(int[] arrayCard) {
        arrayCard = SortArrCard(arrayCard);
        TYPE_CARD typeCard = TYPE_CARD.MAU_THAU;
        typeCard = CheckGroup(arrayCard);
        if (typeCard == TYPE_CARD.MAU_THAU) {
            typeCard = CheckThungSanh(arrayCard);
        }
        //Debug.LogError(typeCard);
        return typeCard;
    }
    /// <summary>
    ///kiểm tra mảng truyền vào là đôi, thú, sám cô, cù lũ, tứ quý, hay mậu thầu
    /// </summary>
    static TYPE_CARD CheckGroup(int[] arrayCard) {
        //TYPE_CARD typeCard = TYPE_CARD.MAU_THAU;
        List<int[]> list = GetGroupIDByValueInArray(arrayCard, TYPE_CARD.MAU_THAU);

        int dem3 = 0, dem2 = 0;
        for (int i = 0; i < list.Count; i++) {
            if (list[i].Length == 4) {
                //tu quy
                return TYPE_CARD.TU_QUY;
            } else if (list[i].Length == 3) {
                dem3++;
            } else if (list[i].Length == 2) {
                dem2++;
            }
        }

        if (dem3 == 1) {
            if (dem2 == 1)
                return TYPE_CARD.CU_LU;
            else
                return TYPE_CARD.SAM_CO;
        } else if (dem2 == 2) {
            return TYPE_CARD.THU;
        } else if (dem2 == 1) {
            return TYPE_CARD.DOI;
        }

        return TYPE_CARD.MAU_THAU;
    }
    /// <summary>
    ///kiểm tra mảng truyền vào là sảnh, thùng, thùng phá sảnh, hay mậu thầu
    /// </summary>
    static TYPE_CARD CheckThungSanh(int[] arrayCard) {
        if (arrayCard.Length == 3) {
            return TYPE_CARD.MAU_THAU;
        }

        int temp = arrayCard[0];
        int typeC = GetType(arrayCard[0]);
        int demsanh = 0, demthung = 0;

        for (int i = 1; i < arrayCard.Length; i++) {
            if (GetValue(temp) == GetValue(arrayCard[i]) - 1) {
                demsanh++;
                temp = arrayCard[i];
            }
            if (typeC == GetType(arrayCard[i])) {
                demthung++;
            }
        }
        //1-2-3-4-5
        if (ConstanceValue(arrayCard, 14) && ConstanceValue(arrayCard, 2) && ConstanceValue(arrayCard, 3) && ConstanceValue(arrayCard, 4) && ConstanceValue(arrayCard, 5)) {
            if (demthung == 4) {
                return TYPE_CARD.THUNG_PHA_SANH;
            } else {
                return TYPE_CARD.SANH;
            }
        }

        if (demsanh == 4) {
            if (demthung == 4) {
                return TYPE_CARD.THUNG_PHA_SANH;
            } else {
                return TYPE_CARD.SANH;
            }
        } else if (demthung == 4) {
            return TYPE_CARD.THUNG;
        }
        return TYPE_CARD.MAU_THAU;
    }
    /// <summary>
    /// Kiem tra xem arr1 > arr2 ? true : false
    /// </summary>
    public static bool IsBigger2Array(int[] arr1, int[] arr2) {
        TYPE_CARD typeCard1 = GetTypeCardMauBinh(arr1);
        TYPE_CARD typeCard2 = GetTypeCardMauBinh(arr2);

        if (typeCard1 > typeCard2) {
            return true;
        }
        if (typeCard1 < typeCard2) {
            return false;
        }
        switch (typeCard1) {
            case TYPE_CARD.MAU_THAU:
            case TYPE_CARD.THUNG:
            case TYPE_CARD.SANH:
            case TYPE_CARD.THUNG_PHA_SANH:
                return IsBig2_MauThau_Thung_Sanh(arr1, arr2);
            case TYPE_CARD.DOI:
            case TYPE_CARD.SAM_CO:
            case TYPE_CARD.CU_LU:
            case TYPE_CARD.TU_QUY:
                return IsBig_Doi_Thu_SamCo_CuLu_TuQuy(arr1, arr2, typeCard1);
            case TYPE_CARD.THU:
                return IsBig_Doi_Thu_SamCo_CuLu_TuQuy(arr1, arr2, typeCard1);
        }

        return false;
    }
    static bool IsBig2_MauThau_Thung_Sanh(int[] array1, int[] array2) {
        array1 = SortArrCard(array1);
        array2 = SortArrCard(array2);

        int max1 = GetValue(array1[array1.Length - 1]) * 10 + GetType(array1[array1.Length - 1]);
        int max2 = GetValue(array2[array2.Length - 1]) * 10 + GetType(array2[array2.Length - 1]);

        if (max1 > max2)
            return true;
        return false;
    }
    static bool IsBig_Doi_Thu_SamCo_CuLu_TuQuy(int[] array1, int[] array2, TYPE_CARD type) {
        List<int[]> list1 = GetGroupIDByValueInArray(array1, type);
        List<int[]> list2 = GetGroupIDByValueInArray(array2, type);

        int[] arr = list1[list1.Count - 1];
        int max1 = GetValue(arr[arr.Length - 1]) * 10 + GetType(arr[arr.Length - 1]);
        int[] arr2 = list2[list2.Count - 1];
        int max2 = GetValue(arr2[arr2.Length - 1]) * 10 + GetType(arr2[arr2.Length - 1]);

        if (max1 > max2)
            return true;
        return false;
    }
    /// <summary>
    /// Lấy các nhóm id theo giá trị, rồi sắp xếp theo độ dài mảng
    /// </summary>
    static List<int[]> GetGroupIDByValueInArray(int[] arr, TYPE_CARD type) {
        var result = arr.GroupBy(x => GetValue(x));
        List<int[]> list = new List<int[]>();
        foreach (var item in result) {
            List<int> temp = new List<int>();
            foreach (var item2 in item) {
                temp.Add(item2);
            }
            if (temp.Count > 1) {
                temp.Sort();
                list.Add(temp.ToArray());
            }
        }
        if (type == TYPE_CARD.THU) {
            list.Sort(delegate (int[] arr1, int[] arr2) {
                return GetValue(arr1[0]).CompareTo(GetValue(arr2[0]));
            });
        } else {
            list.OrderBy(r => r.Length).ThenBy(r1 => r1.Length);
        }
        return list;
    }
    static bool ConstanceValue(int[] arrC, int value) {
        int id = value - 1;
        for (int i = 0; i < 4; i++) {
            if (arrC.Contains(id + i * 13)) {
                return true;
            }
        }

        return false;
    }

    public static int IsThangTrang(int[] arr) {
        #region Rong cuon
        int typeC = GetType(arr[0]);
        int demthung = 0;
        for (int i = 1; i < arr.Length; i++) {
            if (typeC == GetType(arr[i])) {
                demthung++;
            }
        }
        //1-2-3-4-5-6-7-8-9-10-J-Q-K
        int demsanh = 0;
        for (int i = 0; i < 13; i++) {
            if (ConstanceValue(arr, i + 3)) {
                demsanh++;
            }
        }

        if (demsanh == 13) {
            if (demthung == 12) {
                return 5;//rong cuon
            } else
                return 4;//sanh rong
        } else {
            if (demthung == 12) {
                return 3;//Dong Hoa
            }
        }


        #endregion
        #region 6 DOI
        List<int[]> list = GetGroupIDByValueInArray(arr, TYPE_CARD.MAU_THAU);
        if (list.Count == 6) {
            return 2;//luc phe bon
        }
        #endregion
        #region 3 Sanh, 3 Thung
        int[] chi1 = new int[3];
        int[] chi2 = new int[5];
        int[] chi3 = new int[5];
        for (int i = 0; i < arr.Length; i++) {
            if (i < 3) {
                chi1[i] = arr[i];
            } else if (i < 8) {
                chi2[i - 3] = arr[i];
            } else {
                chi3[i - 8] = arr[i];
            }
        }
        chi1 = SortArrCard(chi1);
        chi2 = SortArrCard(chi2);
        chi3 = SortArrCard(chi3);

        TYPE_CARD type1 = CheckThungSanh3(chi1);
        TYPE_CARD type2 = CheckThungSanh(chi2);
        TYPE_CARD type3 = CheckThungSanh(chi3);

        if (type1 == TYPE_CARD.THUNG && type2 == TYPE_CARD.THUNG && type3 == TYPE_CARD.THUNG) {
            return 1;//3 thung
        }
        if (type1 == TYPE_CARD.SANH && type2 == TYPE_CARD.SANH && type3 == TYPE_CARD.SANH) {
            return 0;//3 sanh
        }
        #endregion
        return -1;
    }
    public static bool IsLung(int[] arr) {
        int[] chi1 = new int[3];
        int[] chi2 = new int[5];
        int[] chi3 = new int[5];
        for (int i = 0; i < arr.Length; i++) {
            if (i < 3) {
                chi1[i] = arr[i];
            } else if (i < 8) {
                chi2[i - 3] = arr[i];
            } else {
                chi3[i - 8] = arr[i];
            }
        }
        if (IsBigger2Array(chi3, chi2)
            && IsBigger2Array(chi2, chi1)
            && IsBigger2Array(chi3, chi1)) {
            return false;
        }
        return true;
    }
    static TYPE_CARD CheckThungSanh3(int[] arrayCard) {
        int temp = arrayCard[0];
        int typeC = GetType(arrayCard[0]);
        int demsanh = 0, demthung = 0;

        for (int i = 1; i < arrayCard.Length; i++) {
            if (GetValue(temp) == GetValue(arrayCard[i]) - 1) {
                demsanh++;
                temp = arrayCard[i];
            }
            if (typeC == GetType(arrayCard[i])) {
                demthung++;
            }
        }
        //1-2-3
        if (ConstanceValue(arrayCard, 14) && ConstanceValue(arrayCard, 2) && ConstanceValue(arrayCard, 3)) {
            if (demthung == 4) {
                return TYPE_CARD.THUNG;
            } else {
                return TYPE_CARD.SANH;
            }
        }
        if (demsanh == 2) {
            if (demthung == 2) {
                return TYPE_CARD.THUNG;
            } else {
                return TYPE_CARD.SANH;
            }
        } else if (demthung == 2) {
            return TYPE_CARD.THUNG;
        }
        return TYPE_CARD.MAU_THAU;
    }

    #region Lay Gia tri Bai
    public static int GetValue(int id) {
        int vl = (id - 1) % 13 + 2;
        //if (vl == 14) vl = 2;
        return vl;
    }
    /// <summary>
    ///Co - 4, ro - 3, tep - 2, bich - 1
    /// </summary>
    public static int GetType(int id) {
        return (id - 1) / 13 + 1;
    }
    #endregion
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
    /// <summary>
    ///Sap xep mang giam dan theo nhom, gia tri
    ///</summary>
    public static int[] SortDescendingArrCard(int[] arrCard) {
        List<int> listResult = new List<int>();

        var result = arrCard.GroupBy(x => GetValue(x));

        List<int[]> list = new List<int[]>();
        foreach (var item in result) {
            list.Add(item.ToArray());
        }

        var rr = list.OrderByDescending(a => a.Length).ThenByDescending(b => GetValue(b[0]));

        foreach (var item in rr) {
            listResult.AddRange(item.ToList());
        }

        return listResult.ToArray();
    }
    /// <summary>
    /// Sap xep mang tang dan theo nhom, gia tri
    /// </summary>
    public static int[] SortAscendingArrCard(int[] arrCard) {
        List<int> listResult = new List<int>();

        var result = arrCard.GroupBy(x => GetValue(x));
        List<int[]> list = new List<int[]>();
        foreach (var item in result) {
            list.Add(item.ToArray());
        }

        var rr = list.OrderBy(a => a.Length).ThenBy(b => GetValue(b[0]));
        foreach (var item in rr) {
            listResult.AddRange(item.ToList());
        }

        return listResult.ToArray();
    }
    #endregion

    public static int[] SortCardMauBinh(int[] arrayCard, ref int numCou) {
        int[] result = arrayCard;
        #region Sap xep, de chon chi 3
        switch (numCou) {
            case 1:
                result = GetTuQuy(arrayCard);
                if (result == null) {
                    numCou = 2;
                    result = GetArryCuLu(arrayCard);
                    if (result == null) {
                        numCou = 3;
                        result = GetArryThung(arrayCard);
                        if (result == null) {
                            numCou = 4;
                            result = GetArraySanh(arrayCard);
                            if (result == null) {
                                numCou = 5;
                                result = GetArrySamCo(arrayCard);
                                if (result == null) {
                                    numCou = 6;
                                    result = GetArryThu(arrayCard);
                                    if (result == null) {
                                        numCou = 7;
                                        result = GetArryDoi(arrayCard);
                                        if (result == null) {
                                            result = SortArrCard(arrayCard);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 2:
                result = GetArryCuLu(arrayCard);
                if (result == null) {
                    numCou = 3;
                    result = GetArryThung(arrayCard);
                    if (result == null) {
                        numCou = 4;
                        result = GetArraySanh(arrayCard);
                        if (result == null) {
                            numCou = 5;
                            result = GetArrySamCo(arrayCard);
                            if (result == null) {
                                numCou = 6;
                                result = GetArryThu(arrayCard);
                                if (result == null) {
                                    numCou = 7;
                                    result = GetArryDoi(arrayCard);
                                    if (result == null) {
                                        result = SortArrCard(arrayCard);
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 3:
                result = GetArryThung(arrayCard);
                if (result == null) {
                    numCou = 4;
                    result = GetArraySanh(arrayCard);
                    if (result == null) {
                        numCou = 5;
                        result = GetArrySamCo(arrayCard);
                        if (result == null) {
                            numCou = 6;
                            result = GetArryThu(arrayCard);
                            if (result == null) {
                                numCou = 7;
                                result = GetArryDoi(arrayCard);
                                if (result == null) {
                                    result = SortArrCard(arrayCard);
                                }
                            }
                        }
                    }
                }
                break;
            case 4:
                result = GetArraySanh(arrayCard);
                if (result == null) {
                    numCou = 5;
                    result = GetArrySamCo(arrayCard);
                    if (result == null) {
                        numCou = 6;
                        result = GetArryThu(arrayCard);
                        if (result == null) {
                            numCou = 7;
                            result = GetArryDoi(arrayCard);
                            if (result == null) {
                                result = SortArrCard(arrayCard);
                            }
                        }
                    }
                }
                break;
            case 5:
                result = GetArrySamCo(arrayCard);
                if (result == null) {
                    numCou = 6;
                    result = GetArryThu(arrayCard);
                    if (result == null) {
                        numCou = 7;
                        result = GetArryDoi(arrayCard);
                        if (result == null) {
                            result = SortArrCard(arrayCard);
                        }
                    }
                }
                break;
            case 6:
                result = GetArryThu(arrayCard);
                if (result == null) {
                    numCou = 7;
                    result = GetArryDoi(arrayCard);
                    if (result == null) {
                        result = SortArrCard(arrayCard);
                    }
                }
                break;
            case 7:
                result = GetArryDoi(arrayCard);
                if (result == null) {
                    result = SortArrCard(arrayCard);
                }
                break;
        }
        #endregion
        #region Sap xep chi 12
        List<int> listChi12 = new List<int>();
        List<int> list3 = new List<int>();
        for (int i = 0; i < result.Length; i++) {
            if (i < 5) {
                list3.Add(result[i]);
            } else {
                listChi12.Add(result[i]);
            }
        }

        List<int> list = new List<int>();
        list.AddRange(SortAscendingArrCard(listChi12.ToArray()));
        list.AddRange(list3);
        #endregion
        return list.ToArray();
    }

    static int[] GetTuQuy(int[] cards) {
        int[] list = SortDescendingArrCard(cards);
        if (GetValue(list[0]) == GetValue(list[1]) && GetValue(list[0]) == GetValue(list[2]) && GetValue(list[0]) == GetValue(list[3])) {
            return list;
        }
        return null;
    }
    static int[] GetArryCuLu(int[] cards) {
        int[] listResult = SortDescendingArrCard(cards);
        TYPE_CARD type = GetTypeCardMauBinh(new int[] { listResult[0], listResult[1], listResult[2], listResult[3], listResult[4] });
        if (type == TYPE_CARD.CU_LU) {
            return listResult;
        }
        return null;
    }
    static int[] GetArryThung(int[] cards) {
        var result = cards.GroupBy(x => GetType(x));//lay nhom cac bai cung chat

        List<int[]> list = new List<int[]>();//chua cac nhom cung chat >=5
        List<int> listTemp = new List<int>();//chua cac bai con lai
        foreach (var item in result) {
            List<int> temp = new List<int>();
            foreach (var item2 in item) {
                temp.Add(item2);
            }
            if (temp.Count >= 5)
                list.Add(temp.ToArray());
            else
                listTemp.AddRange(temp);
        }

        if (list.Count <= 0) return null;//ko co thung hop li

        var rr = list.OrderByDescending(a => GetValue(a[0]));//sap xep thung giam dan

        foreach (var item in rr) {
            foreach (var itt in item) {
                listTemp.Add(itt);
            }
        }
        List<int> listResult = new List<int>();
        for (int i = listTemp.Count - 1; i >= 0; i--) {
            listResult.Add(listTemp[i]);
        }
        return listResult.ToArray();
    }
    static int[] GetArraySanh(int[] arrCard) {
        var result = arrCard.GroupBy(x => GetValue(x));
        List<int> list = new List<int>();
        List<int> listTemp = new List<int>();
        foreach (var item in result) {
            List<int> temp = new List<int>();
            foreach (var item2 in item) {
                temp.Add(item2);
            }
            list.Add(temp[temp.Count - 1]);
            temp.RemoveAt(temp.Count - 1);
            listTemp.AddRange(temp);
        }

        var result2 = list.Distinct()
            .OrderBy(x => GetValue(x))
            .GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)).Distinct();

        List<int[]> listR = new List<int[]>();
        foreach (var item in result2) {
            List<int> temp = new List<int>();
            foreach (var it in item) {
                temp.Add(it);
            }
            if (temp.Count >= 5) {
                listR.Add(temp.ToArray());
            } else {
                listTemp.AddRange(temp);
            }
        }
        if (listR.Count <= 0) {
            return null;
        }

        var rr = listR.OrderBy(a => a.Length).ThenBy(b => GetValue(b[0]));

        foreach (var item in rr) {
            foreach (var itt in item) {
                listTemp.Add(itt);
            }
        }
        //lon nguoc lai
        list.Clear();
        for (int i = listTemp.Count - 1; i >= 0; i--) {
            list.Add(listTemp[i]);
        }
        return list.ToArray();
    }
    static int[] GetArrySamCo(int[] cards) {
        int[] listResult = SortDescendingArrCard(cards);
        TYPE_CARD type = GetTypeCardMauBinh(new int[] { listResult[0], listResult[1], listResult[2], listResult[3], listResult[4] });
        if (type == TYPE_CARD.SAM_CO) {
            return listResult;
        }
        return null;
    }
    static int[] GetArryThu(int[] cards) {
        int[] listResult = SortDescendingArrCard(cards);
        TYPE_CARD type = GetTypeCardMauBinh(new int[] { listResult[0], listResult[1], listResult[2], listResult[3], listResult[4] });
        if (type == TYPE_CARD.THU) {
            return listResult;
        }
        return null;
    }
    static int[] GetArryDoi(int[] cards) {
        int[] listResult = SortDescendingArrCard(cards);
        TYPE_CARD type = GetTypeCardMauBinh(new int[] { listResult[0], listResult[1], listResult[2], listResult[3], listResult[4] });
        if (type == TYPE_CARD.DOI) {
            return listResult;
        }
        return null;
    }
}

public static class LinqExtensions {
    public static IEnumerable<IEnumerable<T>> GroupAdjacentBy<T>(
        this IEnumerable<T> source, Func<T, T, bool> predicate) {
        using (var e = source.GetEnumerator()) {
            if (e.MoveNext()) {
                var list = new List<T> { e.Current };
                var pred = e.Current;
                while (e.MoveNext()) {
                    if (predicate(pred, e.Current)) {
                        list.Add(e.Current);
                    } else {
                        yield return list;
                        list = new List<T> { e.Current };
                    }
                    pred = e.Current;
                }
                yield return list;
            }
        }
    }
}
