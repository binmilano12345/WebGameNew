using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RTL {
    public static int scoreTrip;
    public static int scoreStraight;
    static string[] st = { "bi", "chuon", "ro", "co" };

    public static int[][] checkPhom(int[] card, int[] cardWin) {
        int[][] phom = getPhom3(card, cardWin);
        if (phom == null) {
            return null;
        }

        if (cardWin != null) {
            int len = 0;
            int ncw = 0;
            for (int i = 0; i < phom.Length; i++) {
                len += phom[i].Length;
                for (int j = 0; j < cardWin.Length; j++) {
                    if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                        ncw++;
                    }
                }
            }
            if (ncw < cardWin.Length || ncw == 0) {
                return null;
            }
            if (len < card.Length) {
                return null;
            }
            List<string> pos = new List<string>();
            for (int i = 0; i < phom.Length; i++) {
                int ndup = 0;
                pos.Clear();
                if (checkTrip(phom[i])) {
                    for (int j = 0; j < cardWin.Length; j++) {
                        if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                            ndup++;
                        }
                    }
                    if (ndup > 1) {
                        return null;
                    }
                } else {
                    if (phom[i].Length < 6) {
                        for (int j = 0; j < cardWin.Length; j++) {
                            if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                                ndup++;
                            }
                        }
                        if (ndup > 1) {
                            return null;
                        }
                    } else if (phom[i].Length == 6) {
                        for (int j = 0; j < phom[i].Length; j++) {
                            for (int k = 0; k < cardWin.Length; k++) {
                                if (phom[i][j] == cardWin[k]) {
                                    pos.Add(j + "");
                                    ndup++;
                                }
                            }
                        }
                        if (ndup == 3) {
                            return null;
                        }
                        if (ndup > 1) {
                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            if ((pos1 < 3 && pos2 < 3)
                                    || (pos1 >= 3 && pos2 >= 3)) {
                                return null;
                            }
                        }
                    } else if (phom[i].Length > 6) {
                        for (int j = 0; j < phom[i].Length; j++) {
                            for (int k = 0; k < cardWin.Length; k++) {
                                if (phom[i][j] == cardWin[k]) {
                                    pos.Add(j + "");
                                    ndup++;
                                }
                            }
                        }
                        if (ndup == 3) {
                            if (phom[i].Length < 9) {
                                return null;
                            }

                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            int pos3 = int.Parse(pos[2]);
                            if ((pos1 < 3 && pos2 < 3)
                                    || (pos1 >= 3 && pos1 < 6 && pos2 < 6 && pos2 >= 3)
                                    || (pos1 >= 6 && pos2 >= 6)) {
                                return null;
                            }
                            if ((pos1 < 3 && pos3 < 3)
                                    || (pos1 >= 3 && pos1 < 6 && pos3 < 6 && pos3 >= 3)
                                    || (pos1 >= 6 && pos3 >= 6)) {
                                return null;
                            }
                            if ((pos2 < 3 && pos3 < 3)
                                    || (pos3 >= 3 && pos3 < 6 && pos2 < 6 && pos2 >= 3)
                                    || (pos3 >= 6 && pos2 >= 6)) {
                                return null;
                            }
                        } else if (ndup > 1) {
                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            if ((pos1 < 3 && pos2 < 3)) {
                                return null;
                            }
                            if (phom[i].Length == 7) {
                                if ((pos1 >= 4 && pos2 >= 4)) {
                                    return null;
                                }
                            } else if (phom[i].Length == 8) {
                                if ((pos1 >= 5 && pos2 >= 5)) {
                                    return null;
                                }
                            } else if (phom[i].Length == 9) {
                                if ((pos1 >= 6 && pos2 >= 6)) {
                                    return null;
                                }
                            }

                            // if((pos1<3&&pos2<3)||(pos1>=3&&pos1<6&&pos2<6&&pos2
                            // >=3)||(pos1>=6&&pos2>=6))return false;
                        }
                    }
                }
            }
        }
        return phom;
    }

    /**
     * 
     * @param trip
     *            :
     * @param straight1
     *            : sáº£nh lá»›n
     * @param straight2
     *            : sáº£nh nhá»?
     * @param dup1
     *            :
     * @param dup2
     *            :
     * @return
     */
    public static int[][] compareTrip2Straight(int[] trip, int[] straight1,
            int[] straight2, int dup1, int dup2, int[] cardWin) {
        int[][] t = getStraight(splitArray(straight1, dup1)), result = null;
        if (t != null) {
            for (int i = 0; i < t.Length; i++) {
                result = setinfoArray(result, t[i]);
            }
            t = comparePhom(trip, straight2, dup2, cardWin, 1);
            for (int i = 0; i < t.Length; i++) {
                result = setinfoArray(result, t[i]);
            }
        } else {
            result = setinfoArray(result, straight1);
            t = comparePhom(splitArray(trip, dup1), straight2, dup2, cardWin, 1);
            for (int i = 0; i < t.Length; i++) {
                result = setinfoArray(result, t[i]);
            }
        }
        return result;
    }

    public static bool checkUKhan(int[] card) {
        if (isPhom(card)) {
            return false;
        }
        int len = card.Length;
        int[] temp = sort(card);// sort tu nho den lon
        for (int i = 0; i < len - 1; i++) {
            int[] inf = getCardInfo(temp[i]);
            int[] info = getCardInfo(temp[i + 1]);
            if ((info[1] == inf[1])
                    || ((abs(inf[1] - info[1]) <= 2) && (inf[0] == info[0]))) {
                return false;
            }
        }
        temp = sortType(card);// sort theo nuoc
        for (int i = 0; i < len - 1; i++) {
            int[] inf = getCardInfo(temp[i]);
            int[] info = getCardInfo(temp[i + 1]);
            if ((info[1] == inf[1])
                    || ((abs(inf[1] - info[1]) <= 2) && (inf[0] == info[0]))) {
                return false;
            }
        }
        return true;
    }

    public static bool isContain(int[] a, int v) {
        for (int i = 0; i < a.Length; i++) {
            if (a[i] == v) {
                return true;
            }
        }
        return false;
    }

    public static int[] subTwoArray(int[] a, int[] b) {
        if (a == null) {
            return new int[0];
        }
        if (b == null) {
            return a;
        }
        int[] remain = new int[a.Length - b.Length];
        int k = 0;
        for (int i = 0; i < a.Length; i++) {
            if (!isContain(b, a[i])) {
                remain[k++] = a[i];
            }
        }
        return remain;
    }

    /**
     * lay phom
     * 
     * @param card
     * @return
     */
    public static int[][] selectEqua(int[][] trip, int[][] straight, int[] cardWin, bool isOther) {
        int[][] result = null;
        int[] case1, case2;
        int lenStraight = straight.Length, lenTrip = trip.Length;
        if (lenStraight == 1) {
            case1 = checkDupStraightTrip(trip, straight[0]);
            if (case1[0] != -1) {
                return comparePhom(trip[0], straight[0], case1[0], cardWin, 1);
            } else {
                result = trip;
                result = setinfoArray(result, straight[0]);
            }
        } else if (lenStraight == 2) {
            int trip1 = -1, trip2 = -1, str1 = -1, str2 = -1, card1 = -1, card2 = -1, card3 = -1, card4 = -1;

            case1 = checkDupStraightTrip(trip, straight[0]);// so sanh 1 voi 2

            case2 = checkDupStraightTrip(trip, straight[1]);// // so sanh 2 voi
            // 2 trip
            if (cardWin != null) {
                trip1 = checkExistCardInPhom(trip[0], cardWin);
                trip2 = checkExistCardInPhom(trip[1], cardWin);
                str1 = checkExistCardInPhom(straight[0], cardWin);
                str2 = checkExistCardInPhom(straight[1], cardWin);

            }

            if (case1[0] == -1 && case1[1] != -1 && case2[0] != -1 && case2[1] != -1) {
                if (trip[1].Length > 3) {
                    result = setinfoArray(result, splitArray(trip[1], case1[1]));
                    result = setinfoArray(result, trip[0]);
                    result = setinfoArray(result, straight[0]);
                } else {
                    result = comparePhom(trip[1], straight[0], case1[1], cardWin, 1);
                    int[][] iys = comparePhom(trip[0], straight[1], case2[0], cardWin, 1);
                    if (isStraight(result[0])) {
                        for (int i = 0; i < iys.Length; i++) {
                            if (isPhom(iys[i])) {
                                result = setinfoArray(result, iys[i]);
                            }
                        }
                    } else {
                        result = setinfoArray(result, trip[0]);
                    }
                }
                return result;
            }
            if (case1[0] != -1 && case1[1] == -1 && case2[0] != -1 && case2[1] != -1) {
                if (trip[0].Length > 3) {
                    result = setinfoArray(result, splitArray(trip[0], case1[0]));
                    result = setinfoArray(result, trip[1]);
                    result = setinfoArray(result, straight[0]);
                } else {
                    result = comparePhom(trip[0], straight[0], case1[0], cardWin, 1);
                    int[][] iys = comparePhom(trip[1], straight[1], case2[1], cardWin, 1);
                    if (isStraight(result[0])) {
                        for (int i = 0; i < iys.Length; i++) {
                            if (isPhom(iys[i])) {
                                result = setinfoArray(result, iys[i]);
                            }
                        }
                    } else {
                        result = setinfoArray(result, trip[1]);
                    }
                }
                return result;
            }
            if (case1[0] != -1 && case1[1] != -1 && case2[0] == -1 && case2[1] != -1) {
                if (trip[1].Length > 3) {
                    result = setinfoArray(result, splitArray(trip[1], case2[1]));
                    result = setinfoArray(result, trip[0]);
                    result = setinfoArray(result, straight[1]);
                } else {
                    result = comparePhom(trip[1], straight[1], case2[1], cardWin, 1);
                    int[][] iys = comparePhom(trip[0], straight[0], case1[0], cardWin, 1);
                    if (isStraight(result[0])) {
                        for (int i = 0; i < iys.Length; i++) {
                            if (isPhom(iys[i])) {
                                result = setinfoArray(result, iys[i]);
                            }
                        }
                    } else {
                        result = setinfoArray(result, trip[0]);
                    }
                }
                return result;
            }
            if (case1[0] != -1 && case1[1] != -1 && case2[0] != -1 && case2[1] == -1) {
                if (trip[0].Length > 3) {
                    result = setinfoArray(result, splitArray(trip[0], case2[0]));
                    result = setinfoArray(result, trip[1]);
                    result = setinfoArray(result, straight[1]);
                } else {
                    result = comparePhom(trip[0], straight[1], case2[0], cardWin, 1);
                    int[][] iys = comparePhom(trip[1], straight[0], case1[1], cardWin, 1);
                    if (isStraight(result[0])) {
                        for (int i = 0; i < iys.Length; i++) {
                            if (isPhom(iys[i])) {
                                result = setinfoArray(result, iys[i]);
                            }
                        }
                    } else {
                        result = setinfoArray(result, trip[1]);
                    }
                }
                return result;
            }

            if (case1[0] != -1 && case1[1] != -1 && case2[0] != -1 && case2[1] != -1) {// 2s
                // trung
                // 2t
                int lenTrip1 = trip[0].Length, lenTrip2 = trip[1].Length, ndup1 = 0, ndup2 = 0;
                if (lenTrip1 == 4 && lenTrip2 == 4) {
                    if (cardWin != null) {
                        for (int i = 0; i < cardWin.Length; i++) {
                            if (checkExistCardInPhom(straight[0], cardWin[i]) != -1) {
                                ndup1++;
                            }
                            if (checkExistCardInPhom(straight[1], cardWin[i]) != -1) {
                                ndup2++;
                            }
                        }
                    }
                    if (ndup2 == 2) {
                        result = setinfoArray(result, splitArray(trip[0], case1[0]));
                        result = setinfoArray(result, splitArray(trip[1], case1[1]));
                        result = setinfoArray(result, straight[0]);
                        return result;
                    }
                    if (ndup1 == 2) {
                        result = setinfoArray(result, splitArray(trip[0], case2[0]));
                        result = setinfoArray(result, splitArray(trip[1], case2[1]));
                        result = setinfoArray(result, straight[1]);
                        return result;
                    }
                    if (ndup2 >= ndup1) {
                        result = setinfoArray(result, splitArray(trip[0], case2[0]));
                        result = setinfoArray(result, splitArray(trip[1], case2[1]));
                        result = setinfoArray(result, straight[1]);
                        return result;
                    } else {
                        result = setinfoArray(result, splitArray(trip[0], case1[0]));
                        result = setinfoArray(result, splitArray(trip[1], case1[1]));
                        result = setinfoArray(result, straight[0]);
                        return result;
                    }



                }

                if (trip1 == -1 && trip2 == -1) {// trip ko chua cardWin nao
                    int lent1 = trip[0].Length, lent2 = trip[1].Length;
                    int[][] tempSt;
                    if (lent1 < 4 && lent2 < 4) {
                        if ((straight[0].Length > 3 || straight[1].Length > 3)) {
                            int[][] temp11 = comparePhom(trip[0], straight[0], case1[0], cardWin, 1);
                            if (temp11 != null) {
                                int[] pusd = splitArray(straight[1], case1[1]);
                                if (isStraight(pusd)) {
                                    result = setinfoArray(temp11, pusd);
                                }
                            }
                            int[][] temp21 = comparePhom(trip[1], straight[1], case2[1], cardWin, 1);
                            if (temp21 != null) {
                                int[] pusd = splitArray(straight[0], case2[0]);
                                if (isStraight(pusd)) {
                                    result = setinfoArray(temp21, pusd);
                                }
                            }
                            return result;
                        } else {
                            return straight;
                        }
                    }
                    if (lent1 == 3 && lent2 == 4) {
                        tempSt = getStraight(splitArray(straight[0], case1[1]));// 0
                        // -
                        // trip1
                        // 1
                        // -
                        // trip2
                        if (tempSt != null) {
                            result = tempSt;
                            result = setinfoArray(result, straight[1]);
                            result = setinfoArray(result, splitArray(trip[1], case2[1]));
                            return result;
                        }
                        tempSt = getStraight(splitArray(straight[1], case2[1]));
                        if (tempSt != null) {
                            result = tempSt;
                            result = setinfoArray(result, straight[0]);
                            result = setinfoArray(result, splitArray(trip[1], case2[0]));
                            return result;
                        }
                        return straight;
                    }

                    if (lent1 == 4 && lent2 == 3) {
                        tempSt = getStraight(splitArray(straight[0], case1[0]));
                        if (tempSt != null) {
                            result = tempSt;
                            result = setinfoArray(result, straight[1]);
                            result = setinfoArray(result, splitArray(trip[0], case2[1]));
                            return result;
                        }
                        tempSt = getStraight(splitArray(straight[1], case2[1]));
                        if (tempSt != null) {
                            result = tempSt;
                            result = setinfoArray(result, straight[0]);
                            result = setinfoArray(result, splitArray(trip[0], case2[0]));
                            return result;
                        }
                        return straight;
                    }

                }
                if (trip1 != -1 && trip2 == -1) {// trip 1 chua, trip 2 khong
                    if (trip[0].Length == 4) {
                        if (checkExistCardInPhom(cardWin, case1[0]) == -1
                                && getNumCardwin(trip[0], cardWin) < 2) {// khong
                            // phai
                            // cardwin
                            int[] temTrip = splitArray(trip[0], case1[0]);
                            result = comparePhom(temTrip, straight[1], case2[0], cardWin, 0);
                            result = setinfoArray(result, straight[0]);
                            return result;
                        }

                    } else {
                        return straight;
                    }
                    return trip;
                }
                if (trip1 == -1 && trip2 != -1) {// trip 2 chua, trip 1 khong
                    if (trip[1].Length == 4) {
                        if (checkExistCardInPhom(cardWin, case2[0]) == -1 &&
                                getNumCardwin(trip[1], cardWin) < 2) {// khong
                            // phai
                            // cardwin
                            int[] temTrip = splitArray(trip[1], case1[1]);
                            result = comparePhom(temTrip, straight[1], case1[1], cardWin, 0);
                            result = setinfoArray(result, straight[0]);
                            return result;
                        }
                    } else {
                        return straight;
                    }
                    return trip;
                }
                if (str1 == -1 && str2 == -1) {// trip ko chua cardWin nao
                    return trip;
                }
                // if(trip1!=-1&&trip2!=-1&&str1!=-1&&str2!=-1){// ca hai cung
                // chua card win
                // }
                if (trip1 != -1 && trip2 != -1 && str1 != -1 && str2 == -1) {// tri
                    int[] temp = subTwoArray(straight[0], case1);
                    if (isStraight(temp)) {
                        return setinfoArray(trip, temp);
                    }
                    temp = subTwoArray(straight[1], case2);
                    if (isStraight(temp)) {
                        return setinfoArray(trip, temp);
                    }
                    // chua
                    // cardwin
                    // ,
                    // s1
                    // chua
                    // cardwin
                    // 2
                    // ko
                    return trip;
                }
                if (trip1 != -1 && trip2 != -1 && str1 == -1 && str2 != -1) {// tri
                    int[] temp = subTwoArray(straight[0], case1);
                    if (isStraight(temp)) {
                        return setinfoArray(trip, temp);
                    }
                    temp = subTwoArray(straight[1], case2);
                    if (isStraight(temp)) {
                        return setinfoArray(trip, temp);
                    }
                    // chua
                    // cardwin
                    // ,
                    // s1
                    // chua
                    // cardwin
                    // 2
                    // ko
                    return trip;
                }
                if (trip1 == -1 && trip2 != -1 && str1 != -1 && str2 != -1) {// tri
                    // chua
                    // cardwin
                    // ,
                    // s1
                    // chua
                    // cardwin
                    // 2
                    // ko
                    return straight;
                }
                if (trip1 != -1 && trip2 == -1 && str1 != -1 && str2 != -1) {// tri
                    // chua
                    // cardwin
                    // ,
                    // s1
                    // chua
                    // cardwin
                    // 2
                    // ko
                    return straight;
                }
                if (trip1 != -1 && trip2 != -1 && str1 != -1 && str2 != -1) {// tri
                    int[] strai0 = subTwoArray(straight[0], case1);
                    int[] strai1 = subTwoArray(straight[1], case2);
                    if (isStraight(strai0)) {
                        result = setinfoArray(trip, strai0);
                        return result;
                    } else if (isStraight(strai1)) {
                        result = setinfoArray(trip, strai1);
                        return result;
                    }

                    if (trip[0].Length == 4 || trip[1].Length == 4) {
                    } else {
                        //lay 2 traight 1 trip
                        int[] strai00 = splitArray(straight[0], case1[0]);
                        int[] strai01 = splitArray(straight[1], case2[0]);
                        if (isStraight(strai00) && isStraight(strai01)) {
                            result = setinfoArray(result, strai00);
                            result = setinfoArray(result, strai01);
                            result = setinfoArray(result, trip[0]);
                            return result;
                        }
                        strai00 = splitArray(straight[0], case1[1]);
                        strai01 = splitArray(straight[1], case2[1]);
                        if (isStraight(strai00) && isStraight(strai01)) {
                            result = setinfoArray(result, strai00);
                            result = setinfoArray(result, strai01);
                            result = setinfoArray(result, trip[1]);
                            return result;
                        }
                    }
                }
                result = scoreTrip > scoreStraight ? straight : trip;
            } else if (case1[0] != -1 && case1[1] != -1 && case2[0] == -1 && case2[1] == -1) {// s1
                // trung
                // 2t,
                // s2
                // ko
                // trung
                result = trip;
                result = setinfoArray(result, straight[1]);
            } else if (case1[0] == -1 && case1[1] == -1 && case2[0] != -1 && case2[1] != -1) {// s1
                // ko
                // trung,
                // s2
                // trung
                // 2t
                result = trip;
                result = setinfoArray(result, straight[0]);
            } else if ((case1[0] != -1 && case1[1] == -1 && case2[0] == -1 && case2[1] != -1)) {// s1
                // trung
                // t1
                // ko
                // trung
                // t2
                // and
                // s2
                // trung
                // t2
                // ko
                // trung
                // t1
                int[][] temp11 = comparePhom(trip[0], straight[0], case1[0], cardWin, 0);
                int[][] temp21 = comparePhom(trip[1], straight[1], case2[1], cardWin, 0);
                if (temp11 != null && temp21 != null) {
                    result = temp11;
                    if (temp21 != null) {
                        for (int i = 0; i < temp21.Length; i++) {
                            result = setinfoArray(result, temp21[i]);
                        }
                    }
                }
                // result=setinfoArray(result,
                // getScore(trip[0])>getScore(straight[0])?straight[0]:trip[0]);
                // result=setinfoArray(result,
                // getScore(trip[1])>getScore(straight[1])?straight[1]:trip[1]);

            } else if ((case1[0] == -1 && case1[1] != -1 && case2[0] != -1 && case2[1] == -1)) {// s1
                // trung
                // t2
                // ko
                // trung
                // t1
                // and
                // s2
                // trung
                // t1
                // ko
                // trung
                // t2
                int[][] temp11 = comparePhom(trip[1], straight[0], case1[1], cardWin, 0);
                int[][] temp21 = comparePhom(trip[0], straight[1], case2[0], cardWin, 0);
                if (temp11 != null && temp21 != null) {
                    result = temp11;
                    if (temp21 != null) {
                        for (int i = 0; i < temp21.Length; i++) {
                            result = setinfoArray(result, temp21[i]);
                        }
                    }
                }
                // result=setinfoArray(result,
                // getScore(trip[1])>getScore(straight[0])?straight[0]:trip[1]);
                // result=setinfoArray(result,
                // getScore(trip[0])>getScore(straight[1])?straight[1]:trip[0]);
            } else if ((case1[0] != -1 && case1[1] == -1 && case2[0] != -1 && case2[1] == -1)) {// t1
                // trung
                // 2
                // s
                // and
                // t2
                // ko
                result = straight;
                result = setinfoArray(result, trip[1]);
            } else if ((case1[0] == -1 && case1[1] != -1 && case2[0] == -1 && case2[1] != -1)) {// t1
                // ko
                // trung
                // and
                // t2
                // trung
                // 2
                // s
                result = straight;
                result = setinfoArray(result, trip[0]);
            }
        }
        return result;
    }
    /**
     * return score finish
     * 
     * @param src
     * @return
     */
    public static int getScoreFinal(int[] src) {
        if (src == null) {
            return 0;
        }
        int sc = 0;
        for (int i = 0; i < src.Length; i++) {
            sc += getCardInfo(src[i])[1] + 1;
        }
        return sc;
    }

    /**
     * kt sáº£nh cÃ³ trÃ¹ng trip hay khÃ´ng
     * 
     * @param trip
     * @param st
     * @return
     */
    private static int[] checkDupStraightTrip(int[][] trip, int[] st) {
        int[] index = new int[trip.Length];
        for (int i = 0; i < trip.Length; i++) {
            int dup = -1;
            dup = duplicateCard(trip[i], st);
            index[i] = dup;
        }
        return index;
    }

    /**
     * kt trip cÃ³ trÃ¹ng sáº£nh hay ko
     * 
     * @param straight
     * @param trip
     * @return
     */
    private static int[] checkDupTripStraight(int[][] straight, int[] trip) {
        int[] index = new int[straight.Length];
        for (int i = 0; i < straight.Length; i++) {
            int dup = -1;
            dup = duplicateCard(straight[i], trip);
            index[i] = dup;
        }
        return index;
    }

    /**
     * return phom triple or quad
     * 
     * @param card
     * @return
     */
    public static int[][] getTriple(int[] card) {
        scoreTrip = 0;
        int[] temp = sort(card);
        int j = 0;
        int[][] result = null;
        int len = temp.Length;
        int[] tpc = null;
        int preVCard = -1;
        for (int i = 0; i < len; i++) {
            int[] info = getCardInfo(temp[i]);
            if (tpc == null) {
                j++;
                preVCard = info[1];
                tpc = insertAndSort(tpc, temp[i]);
            } else if (info[1] == preVCard) {
                tpc = insertAndSort(tpc, temp[i]);
                j++;
                if (i >= len - 1) {
                    if (j > 2) {
                        result = setinfoArray(result, tpc);
                    } else {
                        // residualTrip=setArray(tpc,residualTrip);
                        scoreTrip += getScoreFinal(tpc);
                    }
                }
            } else {
                preVCard = info[1];
                if (j > 2) {
                    result = setinfoArray(result, tpc);
                } else {
                    // residualTrip=setArray(tpc,residualTrip);
                    scoreTrip += getScoreFinal(tpc);
                }
                tpc = null;
                tpc = insertArray(tpc, temp[i]);
                j = 1;
                if (i >= len - 1) {
                    scoreTrip += getScoreFinal(tpc);
                }
            }
        }
        return result;
    }

    /**
     * return phom straight
     * 
     * @param card
     * @return
     */
    public static int[][] getStraight(int[] card) {
        scoreStraight = 0;
        int[] temp = sortType(card);
        // int pos=0;
        int j = 0;
        int[][] result = null;
        int len = temp == null ? 0 : temp.Length;
        int[] tpc = null;
        int preVCard = -1;
        for (int i = 0; i < len; i++) {
            int[] info = getCardInfo(temp[i]);
            if (tpc == null) {
                j++;
                preVCard = info[0];// save co or ro or chuon or bi
                tpc = insertAndSort(tpc, temp[i]);
            } else if (info[0] == preVCard
                    && (temp[i] - tpc[tpc.Length - 1]) == 1) {
                tpc = insertAndSort(tpc, temp[i]);
                j++;
                if (i >= len - 1) {
                    if (tpc.Length > 2) {
                        result = setinfoArray(result, tpc);
                    } else {
                        scoreStraight += getScoreFinal(tpc);
                    }
                    j = 0;
                    tpc = null;
                }
            } else {
                preVCard = info[0];
                if (tpc.Length > 2) {
                    result = setinfoArray(result, tpc);
                } else {
                    scoreStraight += getScoreFinal(tpc);
                }
                tpc = null;
                tpc = insertArray(tpc, temp[i]);
                j = 1;
                if (i >= len - 1) {
                    scoreStraight += getScoreFinal(tpc);
                }
            }
        }
        return result;
    }

    public static bool isTrip(int[] card) {
        if (card == null) {
            return false;
        }
        if (card.Length < 3) {
            return false;
        }
        int[] info = getCardInfo(card[0]);
        for (int i = 1; i < card.Length; i++) {
            if (info[1] != getCardInfo(card[i])[1]) {
                return false;
            }
        }
        return true;
    }

    public static bool isStraight(int[] card) {
        if (card == null) {
            return false;
        }
        if (card.Length < 3) {
            return false;
        }
        for (int i = 0; i < card.Length - 1; i++) {
            int[] info1 = getCardInfo(card[i]);
            int[] info2 = getCardInfo(card[i + 1]);
            if ((info1[1] > info2[1]) || (abs(info2[1] - info1[1]) > 1)
                    || info1[1] == info2[1]) {
                return false;
            }
        }
        return true;
    }

    /**
     * kt lÃ¡ bÃ i insert vÃ o cÃ³ há»£p lá»‡ hay ko
     * 
     * @param cardHand
     * @param cardWin
     * @param card
     * @return
     */
    public static bool getPhomWithPreCard(int[] cards,
            int[] cardWin, int preCard) {
        int[][] result = null;
        int[][] trip = getTriple(cards);
        int[][] straight = getStraight(cards);

        int[] newcard = null;
        if (cardWin != null) {
            subTwoArray(insertArray(cards, preCard), cardWin);
        }
        int[][] newtrip = getTriple(newcard);
        int[][] newstraight = getStraight(newcard);

        int sizeCardwin = cardWin == null ? 0 : cardWin.Length;
        int sizeTrip = trip == null ? 0 : trip.Length;
        int sizeStraight = straight == null ? 0 : straight.Length;
        switch (sizeCardwin) {
            case 0:
                for (int i = 0; i < sizeTrip; i++) {
                    if (checkExistCardInPhom(trip[i], preCard) != -1) {
                        return true;
                    }
                }
                for (int i = 0; i < sizeStraight; i++) {
                    if (checkExistCardInPhom(straight[i], preCard) != -1) {
                        return true;
                    }
                }
                break;
            case 1:
                int[] tripConatanCardwin1 = null;
                int[] strConatanCardwin1 = null;
                int[] tripConatanpre = null;
                int[] strConatanpre = null;
                for (int i = 0, n = newtrip == null ? 0 : newtrip.Length; i < n; i++) {
                    if (checkExistCardInPhom(newtrip[i], preCard) != -1) {
                        tripConatanpre = newtrip[i];
                    }
                }
                for (int i = 0, n = newstraight == null ? 0 : newstraight.Length; i < n; i++) {
                    if (checkExistCardInPhom(newstraight[i], preCard) != -1) {
                        strConatanpre = newstraight[i];
                    }
                }
                if (tripConatanpre == null && strConatanpre == null) {
                    return false;
                }

                for (int i = 0, n = trip == null ? 0 : trip.Length; i < n; i++) {
                    if (checkExistCardInPhom(trip[i], cardWin) != -1) {
                        tripConatanCardwin1 = trip[i];
                    }
                }
                for (int i = 0, n = straight == null ? 0 : straight.Length; i < n; i++) {
                    if (checkExistCardInPhom(straight[i], cardWin) != -1) {
                        strConatanCardwin1 = straight[i];
                    }
                }
                if (tripConatanCardwin1 != null && strConatanCardwin1 == null) {
                    if (tripConatanpre != null && strConatanpre == null) {
                        if (getCardInfo(tripConatanCardwin1[0])[1] != getCardInfo(tripConatanpre[0])[1]) {
                            return true;
                        }
                        return false;
                    } else if (strConatanpre != null && tripConatanpre == null) {
                        int[] dup = checkDupTripStraight(
                                new int[][] { tripConatanCardwin1 }, strConatanpre);
                        if (dup == null) {
                            return true;
                        }
                        if (isTrip(subTwoArray(tripConatanCardwin1, dup))) {
                            return true;
                        }
                        return false;
                    } else {
                        int[] duptrip = checkDupTripStraight(
                                new int[][] { tripConatanCardwin1 }, strConatanpre);
                        if (duptrip == null) {
                            return true;
                        }
                        if (isTrip(subTwoArray(tripConatanCardwin1, duptrip))) {
                            return true;
                        }
                        if (getCardInfo(tripConatanCardwin1[0])[1] != getCardInfo(tripConatanpre[0])[1]) {
                            return true;
                        }
                        return false;
                    }

                } else if (tripConatanCardwin1 == null
                        && strConatanCardwin1 != null) {
                    if (tripConatanpre != null && strConatanpre == null) {
                        int[] dup = checkDupStraightTrip(
                                new int[][] { strConatanCardwin1 }, tripConatanpre);
                        if (dup == null) {
                            return true;
                        }
                        if (isStraight(subTwoArray(strConatanCardwin1, dup))) {
                            return true;
                        }
                        return false;
                    } else if (strConatanpre != null && tripConatanpre == null) {
                        int[] dup = checkDupTripStraight(
                                new int[][] { strConatanCardwin1 }, tripConatanpre);
                        if (dup == null) {
                            return true;
                        }
                        if (isStraight(subTwoArray(strConatanCardwin1, dup))) {
                            return true;
                        }
                        return false;
                    } else {
                        int[] duptrip = checkDupTripStraight(
                                new int[][] { tripConatanCardwin1 }, strConatanpre);
                        if (duptrip == null) {
                            return true;
                        }
                        if (isTrip(subTwoArray(tripConatanCardwin1, duptrip))) {
                            return true;
                        }
                        if (getCardInfo(tripConatanCardwin1[0])[1] != getCardInfo(tripConatanpre[0])[1]) {
                            return true;
                        }
                        return false;
                    }
                } else if (tripConatanCardwin1 != null
                        && strConatanCardwin1 != null) {
                }
                break;
            case 2:
                break;
        }
        return false;
    }

    public static int[][] getPhom3(int[] card, int[] cardWin) {
        int[][] result = null;
        int[][] trip = getTriple(card);
        int[][] straight = getStraight(card);
        if (trip == null && straight == null) {
            // System.out.println("NULLL");
            return null;
        }
        if (trip == null && straight != null) {
            // System.out.println("chi lay sanh");
            return straight;
        }
        if (trip != null && straight == null) {
            // System.out.println("chi lay trip");
            return trip;
        }
        int lenTrip = trip == null ? 0 : trip.Length;
        int lenStraight = straight == null ? 0 : straight.Length;
        if (lenTrip >= 1 && lenStraight == 1) {
            int[] info = getCardInfo(straight[0][0]);
            int[] phom = trip[0];
            int value = -1;
            for (int i = 0; i < phom.Length; i++) {
                int[] infor2 = getCardInfo(phom[i]);
                if (infor2[0] == info[0]) {
                    value = infor2[1];
                    break;
                }
            }
            if (value != -1 && trip[0].Length < 4
                    && checkExistCardInPhom(trip[0], cardWin) != -1) {
                int[] card2 = subTwoArray(card, trip[0]);
                int[][] straight2 = getStraight(card2);
                // tach phom ra
                if (straight2 != null && straight2.Length == 2) {
                    lenTrip = trip.Length;
                    lenStraight = straight2.Length;
                    straight = straight2;
                }
                if (straight2 != null && lenTrip == 2 && straight2.Length == 1) {
                    lenTrip = trip.Length;
                    lenStraight = straight2.Length;
                    straight = straight2;
                }
                if (straight2 == null) {
                    // lenStraight = 0;
                }
            }
        }
        if (lenTrip > 2 && lenStraight > 2) {
            int totalTrip = 0;
            int totalStraight = 0;
            for (int i = 0; i < 3; i++) {
                totalTrip += trip[i].Length;
                totalStraight += straight[i].Length;
            }
            return (totalStraight > totalTrip ? straight : trip);
        }
        if (lenTrip > 2 && lenStraight < 2) {
            return trip;
        }
        if (lenTrip < 2 && lenStraight > 2) {
            return straight;
        }
        if (lenStraight > 2 || (lenTrip == 0 && lenStraight > 0)) {
            return straight;
        }
        if ((lenTrip > 2) || (lenStraight == 0 && lenTrip > 0)) {
            return trip;
        }
        if (lenStraight > lenTrip) {// phom sanh nhieu hon trips
            // System.out.println("sanh nhieu hon " + lenStraight + " " +
            // lenTrip);
            result = selectPhomStraight(straight, trip[0], cardWin);
            // uu tiên l?y ph?m có ch?a card win tru?c
        } else if (lenStraight < lenTrip) {// phom trip nhieu hon sanh
            // uu tiên l?y ph?m có cardWin tru?c.
            // System.out.println("trip nhieu hon " + lenStraight + " " +
            // lenTrip);
            try {
                result = selectPhomTrip(trip, straight[0], cardWin);
            } catch (Exception e) {
            }
        } else {
            result = selectEqua(trip, straight, cardWin, false);
        }
        scoreTrip = 0;
        scoreStraight = 0;
        return result;
    }

    public static String getth(int id) {
        return (getCardInfo(id)[1] + 1) + "-" + st[getCardInfo(id)[0]];
    }

    public static int[][] getCardFromPlayer(int[] cardHand, int[] cardWin,
            int card) {
        int[] temp = insertArray(cardHand, card);
        int[][] phom = getPhom3(temp, insertArray(cardWin, card));
        if (phom == null) {
            return null;
        }
        if (!validPhom(phom, insertArray(cardWin, card))) {
            return null;
        }
        return phom;
    }

    public static bool checkCardWin(int[] src, int[] cardWin) {
        int[] ar1 = { 0, 1, 2 };
        int len = src.Length;
        int[] ar2 = { len - 1, len - 2, len - 3 };
        if (cardWin == null) {
            return false;
        }
        if (cardWin.Length == 2) {
            int dup1 = checkExistCardInPhom(src, cardWin[0]);
            int dup2 = checkExistCardInPhom(src, cardWin[1]);
            if (dup1 == -1 && dup2 == -1) {
                return false;
            }
            int d1 = checkExistCardInPhom(ar1, dup1);
            int d2 = checkExistCardInPhom(ar1, dup2);
            int d3 = checkExistCardInPhom(ar2, dup1);
            int d4 = checkExistCardInPhom(ar2, dup2);
            if ((d1 != -1 && d2 != -1) || (d3 != -1 && d4 != -1)) {
                return false;
            }
        }
        if (cardWin.Length == 1) {
            int dup = checkExistCardInPhom(src, cardWin[0]);
            if (dup == -1) {
                return false;
            }
        }
        return true;
    }

    /**
     * kt card Ä‘Æ°á»£c Ä‘Ã¡nh hay ko
     * 
     * @param card
     * @param cardWin
     * @param c
     * @return
     */
    public static bool checkCardHit(int[] card, int[] cardWin, int c) {
        if (card != null && cardWin != null) {
            if (checkExistCardInPhom(cardWin, c) != -1) {
                return false;
            }
            int[][] phom = getPhom3(card, cardWin);

            if (phom == null) {
                return true;
            }
            //
            // for(int i=0;i<phom.Length;i++){
            //
            // }
            int len = phom.Length;
            int[] tempArr = null;
            int visPhom = -1;
            for (int i = 0; i < phom.Length; i++) {
                if (checkExistCardInPhom(phom[i], c) != -1) {
                    visPhom = i;
                    break;
                }
            }
            if (visPhom != -1) {
                tempArr = splitArray(phom[visPhom], c);
                if (isPhom(tempArr)) {// pháº£i phá»?m sau khi Ä‘Ã¡nh ra lÃ¡
                    // bÃ i
                    // if(checkExistCardInPhom(tempArr, cardWin)!=-1){
                    //
                    // return true;
                    // }
                    // else return false;

                    if (checkTrip(tempArr)) {// trip
                        return true;
                    } else {
                        if (!checkStraight(tempArr)) {

                            int[][] tempStraig = getStraight(tempArr);
                            if (tempStraig == null) {
                                return false;
                            } else {
                                int nCardWin = 0;
                                if (tempStraig.Length == 1) {
                                    if (tempStraig[0].Length < cardWin.Length * 3) {
                                        return false;
                                    }
                                }
                                if (cardWin != null) {
                                    for (int i = 0; i < tempStraig.Length; i++) {
                                        int ncw = 0;
                                        for (int j = 0; j < cardWin.Length; j++) {
                                            if (checkExistCardInPhom(
                                                    tempStraig[i], cardWin[j]) != -1) {
                                                ncw++;
                                                nCardWin++;
                                            }
                                        }
                                        if (ncw > 1
                                                && tempStraig[i].Length >= 6) {
                                            int c1 = checkExistCardInPhom(
                                                    tempStraig[i], cardWin[0]);
                                            int c2 = checkExistCardInPhom(
                                                    tempStraig[i], cardWin[1]);
                                            int l = tempStraig[i].Length;
                                            if (c1 <= 2 && c2 > 2) {
                                                return true;
                                            }
                                            if (c2 <= 2 && c1 > 2) {
                                                return true;
                                            }

                                            if (c1 == 3 && (c2 > 3 || c2 < 3)) {
                                                return true;
                                            }
                                            if (c2 == 3 && (c1 > 3 || c1 < 3)) {
                                                return true;
                                            }

                                            if (c1 == 4
                                                    && ((c2 > 4 && l > 6) || c2 < 4)) {
                                                return true;
                                            }
                                            if (c2 == 4
                                                    && ((c1 > 4 && l > 6) || c1 < 4)) {
                                                return true;
                                            }

                                            if (c1 > 4 && c2 < 4 && l > 7) {
                                                return true;
                                            }
                                            if (c2 > 4 && c1 < 4 && l > 7) {
                                                return true;
                                            }
                                            return false;
                                        }
                                    }
                                    if (nCardWin < cardWin.Length) {
                                        return false;
                                    } else {
                                        return true;
                                    }
                                }
                            }

                        }
                        if (phom[visPhom].Length >= 6 && tempArr.Length < 6) {
                            if (cardWin != null) {
                                int ncard = 0;
                                for (int i = 0; i < cardWin.Length; i++) {
                                    if (checkExistCardInPhom(tempArr,
                                            cardWin[i]) != -1) {
                                        ncard++;
                                    }
                                }
                                if (ncard > 1) {
                                    return false;
                                } else {
                                    return true;
                                }
                            } else {
                                return true;
                            }
                            // return false;
                        }

                        if (checkExistCardInPhom(phom[visPhom], cardWin) != -1) {
                            if (checkCardWin(tempArr, cardWin)) {
                                return true;
                            } else {
                                return false;
                            }
                            // if(checkExistCardInPhom(getStraight(tempArr)[0],
                            // cardWin)!=-1){
                            // return true;
                            // }else {
                            // return false;
                            // }
                        } else {
                            return true;
                        }
                    }
                    // return true;
                } else {
                    tempArr = splitArray(card, c);
                    int[][] p = getPhom3(tempArr, cardWin);
                    if (p == null) {
                        return false;
                    }
                    int lenWin = cardWin.Length;
                    int lenP = p.Length;
                    if (lenWin == 3 && lenP < 3) {
                        return false;
                    }
                    int check1, check2;
                    if (lenWin < 3) {
                        if (lenWin == 2) {
                            if (lenP == 1) {
                                check1 = checkExistCardInPhom(p[0], cardWin[0]);
                                check2 = checkExistCardInPhom(p[0], cardWin[1]);
                                if ((check1 == -1 && check2 == -1)
                                        || (check1 == -1 && check2 != -1)
                                        || (check1 != -1 && check2 == -1)
                                        || (check1 != -1 && check2 != -1 && p[0].Length < 6)) {
                                    return false;
                                }
                            } else if (lenP == 2) {
                                check1 = checkExistCardInPhom(p[0], cardWin[0]);
                                check2 = checkExistCardInPhom(p[0], cardWin[1]);
                                if ((check1 == -1 && check2 == -1)
                                        || (check1 == -1 && check2 != -1)
                                        || (check1 != -1 && check2 == -1)
                                        || (check1 != -1 && check2 != -1 && p[0].Length < 6)) {
                                    return false;
                                }
                                check1 = checkExistCardInPhom(p[1], cardWin[0]);
                                check2 = checkExistCardInPhom(p[1], cardWin[1]);
                                if ((check1 == -1 && check2 == -1)
                                        || (check1 == -1 && check2 != -1)
                                        || (check1 != -1 && check2 == -1)
                                        || (check1 != -1 && check2 != -1 && p[1].Length < 6)) {
                                    return false;
                                }
                                check1 = checkExistCardInPhom(p[0], cardWin[0]);
                                check2 = checkExistCardInPhom(p[1], cardWin[1]);
                                if ((check1 == -1 && check2 == -1)
                                        || (check1 == -1 && check2 != -1)
                                        || (check1 != -1 && check2 == -1)) {
                                    return false;
                                }
                                check1 = checkExistCardInPhom(p[1], cardWin[0]);
                                check2 = checkExistCardInPhom(p[0], cardWin[1]);
                                if ((check1 == -1 && check2 == -1)
                                        || (check1 == -1 && check2 != -1)
                                        || (check1 != -1 && check2 == -1)) {
                                    return false;
                                }
                            }
                        } else if (lenWin == 1) {
                            if (lenP == 1) {
                                check1 = checkExistCardInPhom(p[0], cardWin[0]);
                                if ((check1 == -1)) {
                                    return false;
                                }
                            } else if (lenP == 2) {
                                check1 = checkExistCardInPhom(p[0], cardWin[0]);
                                check2 = checkExistCardInPhom(p[1], cardWin[0]);
                                if ((check1 == -1 && check2 == -1)) {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

        }
        return true;
    }

    public static int[][] selectPhomStraight(int[][] straight, int[] trip, int[] cardWin) {
        int[][] result = null;
        int[] case1 = checkDupTripStraight(straight, trip);
        int haveCardWin = -1;
        int card1 = -1, card2 = -1, strai1 = -1, strai2 = -1;
        int[] temp1, temp2;
        if (cardWin != null) {
            haveCardWin = checkExistCardInPhom(trip, cardWin);
            card1 = checkExistCardInPhom(cardWin, case1[0]);// case1 trung co
            // phai card win hay
            // ko
            card2 = checkExistCardInPhom(cardWin, case1[1]);// case2 trung co
            // phai cardwin hay
            // ko
            strai1 = checkExistCardInPhom(straight[0], cardWin);// card trung
            // voi card win
            // cua str1
            strai2 = checkExistCardInPhom(straight[1], cardWin);// card trung
            // voi card win
            // cua str2
        }
        if (case1[0] == -1 && case1[1] == -1) {// sanh va trip ko trung nhau
            result = straight;
            result = setinfoArray(result, trip);
            // result=setinfoArray(result, straight[0]);
            // result=setinfoArray(result, straight[1]);
            return result;
        } else if (case1[0] != -1 && case1[1] != -1) {
            if (trip.Length == 3) {
                if (haveCardWin == -1) {// trip ko chua cardWin          
                    temp1 = splitArray(straight[0], case1[0]);
                    temp2 = splitArray(straight[1], case1[1]);
                    if (strai1 != -1 && strai2 != -1) {// hai sanh cung chua
                        // cardWin
                        // result=straight;
                        if (isPhom(temp1)) {
                            result = comparePhom(trip, straight[1], case1[1], cardWin, 1);
                            result = setinfoArray(result, temp1);
                            return result;
                        }
                        if (isPhom(temp2)) {
                            result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                            result = setinfoArray(result, temp2);
                            return result;
                        }
                        return straight;
                    }
                    if (strai1 != -1 && strai2 == -1) {// 1 chua cardWin 2 ko
                        if (isPhom(temp1)) {
                            result = comparePhom(trip, straight[1], case1[1], cardWin, 1);
                            result = setinfoArray(result, temp1);
                            return result;
                        }
                    }
                    if (strai1 == -1 && strai2 != -1) {// 1 chua cardWin 2 ko
                        if (isPhom(temp2)) {
                            result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                            result = setinfoArray(result, temp2);
                            return result;
                        }
                    }
                    if (strai1 == -1 && strai2 == -1) {// ko chua card win
                        if (isPhom(temp1) && isPhom(temp2)) {
                            result = setinfoArray(result, temp1);
                            result = setinfoArray(result, temp2);
                            result = setinfoArray(result, trip);
                            return result;
                        }
                        if (isPhom(temp1)) {
                            result = comparePhom(trip, straight[1], case1[1], cardWin, 1);
                            result = setinfoArray(result, temp1);
                            return result;
                        }
                        if (isPhom(temp2)) {
                            result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                            result = setinfoArray(result, temp2);
                            return result;
                        }
                    }
                    return straight;
                } else {
                    // trip co chua card win
                    temp1 = splitArray(straight[0], case1[0]);
                    temp2 = splitArray(straight[1], case1[1]);
                    int[][] tempSt1 = getStraight(temp1);
                    int[][] tempSt2 = getStraight(temp2);
                    if (strai1 == -1 && strai2 == -1) {// 2 sanh ko co sanh nao
                        // chua cardwin
                        result = tempSt1;
                        if (tempSt2 != null) {
                            for (int i = 0; i < tempSt2.Length; i++) {
                                result = setinfoArray(result, tempSt2[i]);
                            }
                        }
                        result = setinfoArray(result, trip);
                        return result;
                    }
                    if (strai1 != -1 && strai2 != -1) {
                        if (card1 != -1 && card2 != -1) {
                            //sanh 1,sanh 2 trung trip la cardwin(trip chua 2 cardwin)
                            return straight;

                        } else if (card1 != -1 && card2 == -1) {
                            if (tempSt1 != null && tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                return straight;
                            }
                        } else if (card1 == -1 && card2 != -1) {
                            if (tempSt1 != null && tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                return straight;
                            }

                        } else if (card1 == -1 && card2 == -1) {
                            if (tempSt1 != null && tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                return straight;
                            }
                        }
                    }
                    if (strai1 != -1 && strai2 == -1) {// sanh 1 co cardWin sanh
                        // 2 ko
                        if (card1 != -1) {// card trung la card win
                            if (tempSt1 != null && tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else if (tempSt1 != null && tempSt2 == null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                return result;
                            } else {
                                return straight;
                            }

                        } else {// card trung ko phai card win
                            if (tempSt1 != null && tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                            } else if (tempSt1 != null && tempSt2 == null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                            } else {
                                result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                            }
                            return result;
                        }
                    }

                    if (strai1 == -1 && strai2 != -1) {// sanh 2 co cardWin sanh
                        // 1 ko
                        if (card2 != -1) {// card trung la card win
                            if (tempSt2 != null && tempSt1 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else if (tempSt2 != null && tempSt1 == null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                return straight;
                            }
                        } else {// card trung ko phai card win
                            if (tempSt2 != null && tempSt1 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                            } else if (tempSt2 != null && tempSt1 == null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt2[0]);
                            } else {
                                result = comparePhom(trip, straight[1], case1[1], cardWin, 1);
                            }
                            return result;
                        }
                    }

                }

                // result=setinfoArray(result, trip[0]);
            } else {
                // kt sanh ngan truoc       
                temp1 = splitArray(straight[0], case1[0]);
                temp2 = splitArray(straight[1], case1[1]);
                int[][] tempSt1 = getStraight(temp1);
                int[][] tempSt2 = getStraight(temp2);
                if (strai1 == -1 && strai2 == -1) {// 2 sanh ko co sanh nao
                    // chua cardwin
                    if (tempSt1 != null && tempSt2 == null) {
                        result = setinfoArray(result, tempSt1[0]);
                        result = setinfoArray(result, straight[1]);
                        result = setinfoArray(result, splitArray(trip, case1[1]));
                        return result;
                    }
                    if (tempSt1 == null && tempSt2 != null) {
                        result = setinfoArray(result, tempSt2[0]);
                        result = setinfoArray(result, straight[0]);
                        result = setinfoArray(result, splitArray(trip, case1[0]));
                        return result;
                    }
                    if (tempSt1 == null && tempSt2 == null) {
                        result = setinfoArray(result, straight[1]);
                        result = setinfoArray(result, splitArray(trip, case1[1]));
                        return result;
                    }
                    if (tempSt1 != null && tempSt2 != null) {
                        result = setinfoArray(result, tempSt2[0]);
                        result = setinfoArray(result, tempSt1[0]);
                        result = setinfoArray(result, trip);
                        return result;
                    }
                }
                if (strai1 != -1 && strai2 != -1) {
                    if (card1 != -1 || card2 != -1)// neu 1 trong 2 sanh
                    // trung la card win
                    {
                        if (tempSt1 != null && tempSt2 == null) {
                            for (int i = 0; i < tempSt1.Length; i++) {
                                result = setinfoArray(result, tempSt1[i]);
                            }
                            result = setinfoArray(result, splitArray(trip, case1[1]));
                            result = setinfoArray(result, straight[1]);
                            return result;
                        } else if (tempSt1 == null && tempSt2 != null) {
                            for (int i = 0; i < tempSt2.Length; i++) {
                                result = setinfoArray(result, tempSt2[i]);
                            }
                            result = setinfoArray(result, splitArray(trip, case1[0]));
                            result = setinfoArray(result, straight[0]);
                            return result;
                        }
                        return straight;
                    }
                    if (card1 == -1 && card2 == -1) {// card trung ko phai
                        // card win
                        result = tempSt2;
                        result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                        return result;
                    }
                }
                if (strai1 != -1 && strai2 == -1) {// sanh 1 co cardWin sanh
                    // 2 ko
                    if (card1 != -1) {// card trung la card win
                        if (tempSt1 != null) {
                            if (tempSt2 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                result = setinfoArray(result, splitArray(trip, case1[1]));
                                result = setinfoArray(result, straight[1]);
                                result = setinfoArray(result, tempSt1[0]);
                                return result;
                            }

                        } else {
                            if (tempSt2 != null) {
                                result = setinfoArray(result, splitArray(trip, case1[0]));
                                result = setinfoArray(result, straight[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                result = comparePhom(splitArray(trip, case1[0]), straight[1], case1[1], cardWin, 1);
                                result = setinfoArray(result, straight[0]);
                                return result;
                            }
                        }
                    } else {
                        if (tempSt2 != null) {
                            result = setinfoArray(result, splitArray(trip, case1[0]));
                            result = setinfoArray(result, tempSt2[0]);
                        } else {
                            result = comparePhom(splitArray(trip, case1[0]), straight[1], case1[1], cardWin, 1);
                        }
                        result = setinfoArray(result, straight[0]);
                        return result;
                    }
                }

                if (strai1 == -1 && strai2 != -1) {// sanh 2 co cardWin sanh
                    // 1 ko
                    if (card2 != -1) {// card trung la card win
                        if (tempSt2 != null) {
                            if (tempSt1 != null) {
                                result = setinfoArray(result, trip);
                                result = setinfoArray(result, tempSt1[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            } else {
                                result = setinfoArray(result, splitArray(trip, case1[0]));
                                result = setinfoArray(result, straight[0]);
                                result = setinfoArray(result, tempSt2[0]);
                                return result;
                            }

                        } else {
                            if (tempSt1 != null) {
                                result = setinfoArray(result, splitArray(trip, case1[1]));
                                result = setinfoArray(result, straight[1]);
                                result = setinfoArray(result, tempSt1[0]);
                                return result;
                            } else {
                                result = comparePhom(splitArray(trip, case1[1]), straight[0], case1[0], cardWin, 1);
                                result = setinfoArray(result, straight[1]);
                                return result;
                            }

                        }
                    } else {
                        if (tempSt1 != null) {
                            result = setinfoArray(result, splitArray(trip, case1[1]));
                            result = setinfoArray(result, tempSt1[0]);
                        } else {
                            result = comparePhom(splitArray(trip, case1[1]), straight[0], case1[0], cardWin, 1);
                        }
                        result = setinfoArray(result, straight[1]);
                        return result;
                    }
                }
            }
        } else if (case1[0] != -1) {// 1 sanh trung 1 sanh ko
            int[] temp = splitArray(straight[0], case1[0]);
            if (haveCardWin != -1) {
                if (trip.Length == 3) {
                    if (isPhom(temp)) {
                        result = setinfoArray(result, getStraight(temp)[0]);
                    }
                    result = setinfoArray(result, trip);
                    result = setinfoArray(result, straight[1]);
                } else if (trip.Length == 4) {

                    result = setinfoArray(result, splitArray(trip, case1[0]));
                    result = setinfoArray(result, straight[0]);
                    result = setinfoArray(result, straight[1]);
                }
            } else {
                if (straight[0].Length == 5) {
                    int[][] nStraight = null;
                    nStraight = getStraight(temp);
                    if (nStraight != null) {
                        result = setinfoArray(result, trip);
                        result = setinfoArray(result, straight[1]);
                        result = setinfoArray(result, nStraight[0]);
                    }
                } else {
                    result = comparePhom(trip, straight[0], case1[0], cardWin, 1);
                    result = setinfoArray(result, straight[1]);
                }

            }
        } else if (case1[1] != -1) {// 1 sanh trung 1 sanh ko
            int[] temp = splitArray(straight[1], case1[1]);
            if (haveCardWin != -1) {
                if (trip.Length == 3) {
                    if (isPhom(temp)) {
                        result = setinfoArray(result, getStraight(temp)[0]);
                    }
                    result = setinfoArray(result, trip);
                    result = setinfoArray(result, straight[0]);
                } else if (trip.Length == 4) {
                    result = setinfoArray(result, splitArray(trip, case1[1]));
                    result = setinfoArray(result, straight[0]);
                    result = setinfoArray(result, straight[1]);
                }
            } else {
                if (straight[1].Length == 5) {
                    int[][] nStraight = null;
                    nStraight = getStraight(temp);
                    if (nStraight != null) {
                        result = setinfoArray(result, trip);
                        result = setinfoArray(result, straight[0]);
                        result = setinfoArray(result, nStraight[0]);
                    }
                } else {
                    result = comparePhom(trip, straight[1], case1[1], cardWin, 1);
                    result = setinfoArray(result, straight[0]);
                }
            }
        }
        return result;
    }

    /**
     * chá»?n phá»?m cÃ³ chá»©a cardWin Phom: Length luÃ´n =2
     * 
     * @return
     */
    public static int[][] selectPhomTrip(int[][] trip, int[] straight, int[] cardWin) {

        int[][] result = null;
        if (trip == null) {
            return (result = setinfoArray(result, straight));
        }
        if (straight == null) {
            return trip;
        }
        int[] dup = checkDupStraightTrip(trip, straight);
        //3T 1T
        int card1 = -1, card2 = -1;
        int haveCardWin = -1;
        int trip1 = -1;
        int trip2 = -1;
        int[] temp1, temp2;
        if (cardWin != null) {
            card1 = checkExistCardInPhom(cardWin, dup[0]);
            card2 = checkExistCardInPhom(cardWin, dup[1]);
            haveCardWin = checkExistCardInPhom(straight, cardWin);// card win
            // cua
            // straight
            trip1 = checkExistCardInPhom(trip[0], cardWin);// carin win cua trips
            // 1
            trip2 = checkExistCardInPhom(trip[1], cardWin);// card win cua trip
            // 2
        }
        int kTrip = getCardInfo(trip[1][0])[1];
        int kStraight = getCardInfo(straight[straight.Length - 1])[1];
        int lenTrip1 = trip[0].Length, lenTrip2 = trip[1].Length;
        if (haveCardWin == -1) {// khong ton tai cardWin trong phom sanh
            // case1 = checkDupStraightTrip(trip, straight[0]);
            if (dup[0] != -1 && dup[1] != -1) {// sanh nam giua trip
                if (trip1 == -1 && trip2 == -1) {// ko co trip nao chua card win
                    int[] nstrai = subTwoArray(straight, dup);
                    if (trip[0].Length == 4 && trip[1].Length == 4) {
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        result = setinfoArray(result, straight);
                        return result;
                    }
                    if (trip[0].Length == 3 && trip[1].Length == 4) {
                        result = comparePhom(trip[0], straight, dup[0], cardWin, 0);
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        return result;
                    }
                    if (trip[0].Length == 4 && trip[1].Length == 3) {
                        result = comparePhom(trip[1], straight, dup[1], cardWin, 0);
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        return result;
                    }
                    if (isPhom(nstrai)) {
                        result = setinfoArray(trip, nstrai);
                        return result;
                    }
                    return trip;
                }
                if (trip1 != -1 && trip2 != -1) {// ca hai cung chua cardWin
                    if (lenTrip1 == 3 && lenTrip2 == 3) {

                        return trip;
                    }
                    if (lenTrip1 == 4 && lenTrip2 == 4) {
                        result = setinfoArray(result, straight);
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        return result;
                    }
                    if (lenTrip1 == 3 && lenTrip2 == 4) {
                        result = comparePhom(trip[1], splitArray(straight, dup[0]), dup[1], cardWin, 1);
                        result = setinfoArray(result, trip[0]);
                        return result;
                    }
                    if (lenTrip1 == 4 && lenTrip2 == 3) {
                        result = comparePhom(trip[0], splitArray(straight, dup[1]), dup[0], cardWin, 1);
                        result = setinfoArray(result, trip[1]);
                        return result;
                    }

                    int[] temp = splitArray(straight, dup[0]);
                    temp = splitArray(temp, dup[1]);
                    result = getStraight(temp);
                    result = setinfoArray(result, trip[0]);
                    result = setinfoArray(result, trip[1]);
                    return result;
                }
                if (trip1 != -1 && trip2 == -1) {
                    if ((lenTrip1 == 3 && lenTrip2 == 4)) {
                        result = comparePhom(trip[1], splitArray(straight, dup[0]), dup[1], cardWin, 1);
                        result = setinfoArray(result, trip[0]);
                        return result;
                    }
                    if (lenTrip1 == 4 && lenTrip2 == 4) {
                        result = setinfoArray(result, straight);
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        return result;
                    }
                    if ((lenTrip1 == 3 && lenTrip2 == 3)) {
                        return trip;
                    }
                    if ((lenTrip1 == 4 && lenTrip2 == 3)) {
                        result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        //                        if (result.Length > 1) {
                        //                            result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        //                        } else {
                        //                            result = setinfoArray(result, trip[0]);
                        //                        }
                        return result;
                    }
                }
                if (trip1 == -1 && trip2 != -1) {// trip 2 chua card win
                    if ((lenTrip2 == 3 && lenTrip1 == 4)) {
                        result = comparePhom(trip[0], splitArray(straight, dup[1]), dup[0], cardWin, 1);
                        result = setinfoArray(result, trip[1]);
                        return result;
                    }
                    if (lenTrip2 == 4 && lenTrip1 == 4) {
                        result = setinfoArray(result, straight);
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        return result;
                    }
                    if ((lenTrip2 == 3 && lenTrip1 == 3)) {
                        result = comparePhom(trip[0], splitArray(straight, dup[1]), dup[0], cardWin, 0);
                        if (!isTrip(result[0])) {
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        } else {
                            result = setinfoArray(result, trip[1]);
                        }
                        return result;
                    }
                    if ((lenTrip2 == 4 && lenTrip1 == 3)) {
                        result = comparePhom(trip[0], straight, dup[0], cardWin, 0);
                        if (result.Length > 1) {
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        } else {
                            if (!checkStraight(result[0])) {
                                result = setinfoArray(result, trip[1]);
                            } else {
                                result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            }
                        }
                        return result;
                    }
                }

                if (kTrip >= kStraight) {
                    result = comparePhom(trip[0], splitArray(straight, dup[1]), dup[0], cardWin, 1);
                    result = setinfoArray(result, trip[1]);
                } else {
                    result = comparePhom(trip[1], splitArray(straight, dup[0]), dup[1], cardWin, 1);
                    result = setinfoArray(result, trip[0]);
                }
            } else if (dup[0] != -1) {// trung trip 1 ko trung trip 2
                if (trip1 != -1) {// phom trip1 co chứa card wins
                    if (trip[0].Length == 3) {
                        result = getStraight(splitArray(straight, dup[0]));
                        result = setinfoArray(result, trip[0]);
                        result = setinfoArray(result, trip[1]);
                        //                        result = setinfoArray(result, straight);
                        //                        result = setinfoArray(result, trip[1]);
                    } else {
                        result = setinfoArray(result, straight);
                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                        result = setinfoArray(result, trip[1]);
                    }
                    return result;
                }
                result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                result = setinfoArray(result, trip[1]);
                return result;
            } else if (dup[1] != -1) {// trung trip 2 ko trung trip 1
                if (trip2 != -1) {
                    if (trip[1].Length == 3) {
                        result = getStraight(splitArray(straight, dup[1]));
                        result = setinfoArray(result, trip[0]);
                        result = setinfoArray(result, trip[1]);
                    } else {
                        result = setinfoArray(result, straight);
                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                        result = setinfoArray(result, trip[0]);
                    }
                    return result;
                }
                result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                result = setinfoArray(result, trip[0]);
            } else if (dup[0] == -1 && dup[1] == -1) {
                result = trip;
                result = setinfoArray(result, straight);
            }
        } else {// ton tai card win trong phom sanh
            if (dup[0] == -1 && dup[1] == -1) {
                result = trip;
                result = setinfoArray(result, straight);
            } else if (dup[0] != -1 && dup[1] != -1) {// 2 trip trung straight   
                if (trip1 == -1 && trip2 == -1) {// 2 trip ko chua cardwinp
                    int[] tempSp1 = splitArray(straight, dup[0]);
                    int[] tempSp2 = splitArray(straight, dup[1]); //true
                    int[] tempSp3 = splitArray(splitArray(straight, dup[0]), dup[1]);
                    bool isStr1 = isStraight(tempSp1);
                    bool isStr2 = isStraight(tempSp2);
                    if (isStraight(tempSp3)) {
                        result = trip;
                        result = setinfoArray(result, tempSp3);
                        return result;
                    }
                    if (isStr1) {
                        result = comparePhom(trip[1], tempSp1, dup[1], cardWin, 1);
                        result = setinfoArray(result, trip[0]);
                        return result;
                    }
                    if (isStr2) {
                        result = comparePhom(trip[0], tempSp2, dup[0], cardWin, 1);
                        result = setinfoArray(result, trip[1]);
                        return result;
                    }
                    result = setinfoArray(result, straight);
                    return result;
                }
                if (trip1 != -1 && trip2 != -1) {// 2 trip chua cardWin va

                    if (card1 == -1 && card2 == -1) {//
                        if (lenTrip1 == 4 && lenTrip2 == 3) {
                            temp1 = splitArray(trip[0], dup[0]);
                            result = setinfoArray(result, temp1);
                            temp2 = trip[1];
                            result = setinfoArray(result, temp2);
                            int[] splitStraight = splitArray(straight, dup[1]);
                            if (isStraight(splitStraight)) {
                                result = setinfoArray(result, splitStraight);
                            }
                        }
                        if (lenTrip1 == 3 && lenTrip2 == 4) {
                            temp2 = splitArray(trip[1], dup[1]);
                            result = setinfoArray(result, temp2);
                            temp1 = trip[0];
                            result = setinfoArray(result, temp1);
                            int[] splitStraight = splitArray(straight, dup[0]);
                            if (isStraight(splitStraight)) {
                                result = setinfoArray(result, splitStraight);
                            }
                        }
                        if (lenTrip1 == 4 && lenTrip2 == 4) {
                            temp2 = splitArray(trip[1], dup[1]);
                            result = setinfoArray(result, temp2);
                            temp1 = splitArray(trip[1], dup[0]);
                            result = setinfoArray(result, temp1);
                            result = setinfoArray(result, straight);
                        }
                        return result;
                    }
                    if (card1 != -1 && card2 == -1) {
                        if ((lenTrip1 == 3 && lenTrip2 == 3) || lenTrip2 == 3) {
                            result = trip;
                            return result;
                        }
                        if (lenTrip1 == 3 && lenTrip2 == 4) {
                            if (straight.Length == 4) {
                                int[] splitStraight = splitArray(straight, dup[0]);
                                if (isStraight(splitStraight)) {// neu sau khi
                                    // no la
                                    // straight
                                    result = setinfoArray(result, splitStraight);
                                    result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                    result = setinfoArray(result, trip[0]);
                                } else {
                                    result = trip;
                                }
                            } else {
                                result = trip;
                            }
                            return result;
                        }
                        result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                        bool check = false;
                        for (int i = 0; i < result.Length; i++) {
                            if (checkExistCardInPhom(result[i], dup[0]) != -1) {
                                check = true;
                                break;
                            }
                        }
                        if (check) {
                            temp1 = splitArray(trip[0], dup[0]);
                        } else {
                            temp1 = trip[0];
                        }
                        if (isPhom(temp1)) {
                            result = setinfoArray(result, temp1);
                        }
                        return result;
                    }
                    if (card1 == -1 && card2 != -1) {
                        if ((lenTrip1 == 3 && lenTrip2 == 3) || lenTrip1 == 3) {
                            result = trip;
                            return result;
                        }
                        if (lenTrip1 == 4 && lenTrip2 == 3) {
                            if (straight.Length == 4) {
                                int[] splitStraight = splitArray(straight, dup[1]);
                                if (isStraight(splitStraight)) {// neu sau khi
                                    // no la
                                    // straight
                                    result = setinfoArray(result, splitStraight);
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                    result = setinfoArray(result, trip[1]);
                                } else {
                                    result = trip;
                                }
                            } else {
                                result = trip;
                            }
                            return result;
                        }
                        result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                        bool check = false;
                        for (int i = 0; i < result.Length; i++) {
                            if (checkExistCardInPhom(result[i], dup[1]) != -1) {
                                check = true;
                                break;
                            }
                        }
                        if (check) {
                            temp1 = splitArray(trip[1], dup[1]);
                        } else {
                            temp1 = trip[1];
                        }
                        if (isPhom(temp1)) {
                            result = setinfoArray(result, temp1);
                        }
                        return result;
                    }
                    if (card1 != -1 && card2 != -1 && straight.Length >= 4) {
                        int[] splitStraight = null;
                        if (lenTrip1 == 4 && (lenTrip2 == 3 || lenTrip2 == 4)) {
                            splitStraight = splitArray(straight, dup[1]);
                            int[] win = splitArray(cardWin, dup[1]);
                            if (isStraight(splitStraight)) {
                                result = comparePhom(trip[0], splitStraight, dup[0], win, 1);
                                result = setinfoArray(result, trip[1]);
                                return result;
                            }
                        }
                        if ((lenTrip1 == 3 || lenTrip1 == 4) && lenTrip2 == 4) {
                            splitStraight = splitArray(straight, dup[0]);
                            int[] win = splitArray(cardWin, dup[0]);
                            if (isStraight(splitStraight)) {
                                result = comparePhom(trip[1], splitStraight, dup[1], win, 1);
                                result = setinfoArray(result, trip[0]);
                                return result;
                            }
                        }
                    }
                    return trip;
                }
                if (trip1 != -1 && trip2 == -1) {// trip 1 chua cardWin trip2 ko
                    if (card1 != 1) {// la trung la la trong card win
                        if (lenTrip1 == 3 && lenTrip2 == 3) {
                            int[] strCut = subTwoArray(straight, dup);
                            if (isStraight(strCut)) {
                                result = trip;
                                result = setinfoArray(result, strCut);
                                return result;
                            } else {
                                return trip;
                            }
                        }
                        if (lenTrip1 == 4 && lenTrip2 == 4) {
                            result = setinfoArray(result, straight);
                            result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            return result;
                        }
                        if (lenTrip1 == 3 && lenTrip2 == 4) {
                            result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                            if (result.Length > 1) {
                                result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            } else {
                                if (checkTrip(result[0])) {// lay trip
                                    result = setinfoArray(result, trip[1]);
                                } else {
                                    result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                }
                            }
                            // result=setinfoArray(result, splitArray(trip[1],
                            // dup[1]));
                            return result;
                        }
                        if (lenTrip1 == 4 && lenTrip2 == 3) {
                            result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                            if (result.Length > 1) {
                                result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            } else {
                                if (checkTrip(result[0])) {// lay trip
                                    result = setinfoArray(result, trip[0]);
                                } else {
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                }
                            }
                            // result=setinfoArray(result, splitArray(trip[0],
                            // dup[0]));
                            return result;
                        }
                    }
                    if (card1 == -1) {// card trùng ko phải cardWin
                        if (lenTrip1 == 3) {

                            return trip;
                        }
                        if (lenTrip1 == 4) {
                            if (lenTrip2 == 3) {
                                result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                                if (result.Length > 1) {
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                } else {
                                    if (checkTrip(result[0])) {// lay trip
                                        result = setinfoArray(result, trip[0]);
                                    } else {
                                        result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                    }
                                }
                                // result=setinfoArray(result,
                                // splitArray(trip[0], dup[0]));
                                return result;
                            }
                            if (lenTrip2 == 4) {
                                result = setinfoArray(result, straight);
                                result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                return result;
                            }
                        }
                    }
                }
                if (trip1 == -1 && trip2 != -1) {// trip 2 chua cardWin trip 1
                    // ko
                    if (card2 != -1) {// la trung la la trong card win
                        if (lenTrip2 == 3 && lenTrip1 == 3) {
                            int[] strCut = subTwoArray(straight, dup);
                            if (isStraight(strCut)) {
                                result = trip;
                                result = setinfoArray(result, strCut);
                                return result;
                            } else {
                                return trip;
                            }
                        }
                        if (lenTrip2 == 4 && lenTrip1 == 4) {
                            result = setinfoArray(result, straight);
                            result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            return result;
                        }
                        if (lenTrip2 == 3 && lenTrip1 == 4) {
                            result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                            if (result.Length > 1) {
                                // result=setinfoArray(result,
                                // splitArray(trip[0], dup[0]));
                                if ((checkExistCardInPhom(result[0], dup[0]) != -1) || (checkExistCardInPhom(result[1], dup[0]) != -1)) {
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                } else {
                                    result = setinfoArray(result, trip[0]);
                                }
                            } else {
                                if (checkTrip(result[0])) {// lay trip
                                    result = setinfoArray(result, trip[0]);
                                } else {
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                }
                            }
                            return result;
                        }
                        if (lenTrip2 == 4 && lenTrip1 == 3) {
                            result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                            if (result.Length > 1) {
                                if ((checkExistCardInPhom(result[0], dup[1]) != -1) || (checkExistCardInPhom(result[1], dup[1]) != -1)) {
                                    result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                } else {
                                    result = setinfoArray(result, trip[1]);
                                }
                            } else {
                                if (checkTrip(result[0])) {// lay trip
                                    result = setinfoArray(result, trip[1]);
                                } else {
                                    result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                }
                            }
                            // result=setinfoArray(result, splitArray(trip[1],
                            // dup[1]));
                            return result;
                        }
                    }
                    if (card2 == -1) {// card trùng ko phải cardWin
                        if (lenTrip2 == 3 && lenTrip1 == 3) {
                            result = comparePhom(trip[1], straight, dup[1], cardWin, 1);

                            // result=setinfoArray(result,
                            // splitArray(trip[1], dup[1]));
                            return result;
                        }
                        if (lenTrip2 == 3 && lenTrip1 == 4) {
                            result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                            if (result.Length > 1) {
                                result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            } else {
                                if (checkTrip(result[0])) {// lay trip
                                    result = setinfoArray(result, trip[0]);
                                } else {
                                    result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                }
                            }
                            // result=setinfoArray(result,
                            // splitArray(trip[1], dup[1]));
                            return result;
                        }
                        if (lenTrip2 == 4) {
                            if (lenTrip1 == 3) {
                                result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                                if (result.Length > 1) {
                                    result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                } else {
                                    if (checkTrip(result[0])) {// lay trip
                                        result = setinfoArray(result, trip[1]);
                                    } else {
                                        result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                    }
                                }
                                // result=setinfoArray(result,
                                // splitArray(trip[1], dup[1]));
                                return result;
                            }
                            if (lenTrip1 == 4) {
                                result = setinfoArray(result, straight);
                                result = setinfoArray(result, splitArray(trip[0], dup[0]));
                                result = setinfoArray(result, splitArray(trip[1], dup[1]));
                                return result;
                            }
                        }
                    }
                }
            }
            if (dup[0] != -1 && dup[1] == -1) {
                if (trip1 == -1 && (trip2 == -1 || trip2 != -1)) {
                    int[][] temSt = getStraight(splitArray(straight, dup[0]));
                    if (card1 != -1) {
                        if (temSt != null) {
                            result = setinfoArray(result, trip[0]);
                            result = setinfoArray(result, temSt[0]);
                            result = setinfoArray(result, trip[1]);
                            return result;
                        } else {
                            result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                            result = setinfoArray(result, trip[1]);
                            return result;
                        }
                    } else {
                        if (temSt != null) {
                            result = setinfoArray(result, trip[0]);
                            result = setinfoArray(result, temSt[0]);
                            result = setinfoArray(result, trip[1]);
                            return result;
                        } else {
                            result = setinfoArray(result, trip[1]);
                            result = setinfoArray(result, straight);
                            if (trip[0].Length == 4) {
                                result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            }
                            return result;
                        }
                    }
                }
                if (trip1 != -1 && (trip2 != -1 || trip2 == -1)) {
                    int[][] temSt = getStraight(splitArray(straight, dup[0]));
                    if (card1 != -1) {
                        if (trip[0].Length == 4) {
                            result = setinfoArray(result, trip[1]);
                            result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            result = setinfoArray(result, straight);
                            return result;
                        } else {
                            if (temSt != null) {
                                result = setinfoArray(result, trip[1]);
                                result = setinfoArray(result, temSt[0]);
                                result = setinfoArray(result, trip[0]);
                                return result;
                            } else {
                                result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                                result = setinfoArray(result, trip[1]);
                                return result;
                            }
                        }
                    } else {
                        if (trip[0].Length == 4) {
                            result = setinfoArray(result, trip[1]);
                            result = setinfoArray(result, splitArray(trip[0], dup[0]));
                            result = setinfoArray(result, straight);
                            return result;
                        } else {
                            if (temSt != null) {
                                result = setinfoArray(result, trip[1]);
                                result = setinfoArray(result, temSt[0]);
                                result = setinfoArray(result, trip[0]);
                                return result;
                            } else {
                                result = comparePhom(trip[0], straight, dup[0], cardWin, 1);
                                result = setinfoArray(result, trip[1]);
                                return result;
                            }
                        }
                    }
                }
            } else if (dup[0] == -1 && dup[1] != -1) {
                if (trip2 == -1 && (trip1 == -1 || trip1 != -1)) {
                    int[][] temSt = getStraight(splitArray(straight, dup[1]));
                    if (card2 != -1) {
                        if (temSt != null) {
                            result = setinfoArray(result, trip[1]);
                            result = setinfoArray(result, temSt[0]);
                            result = setinfoArray(result, trip[0]);
                            return result;
                        } else {
                            result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                            result = setinfoArray(result, trip[0]);
                            return result;
                        }
                    } else {
                        if (temSt != null) {
                            result = setinfoArray(result, trip[1]);
                            result = setinfoArray(result, temSt[0]);
                            result = setinfoArray(result, trip[0]);
                            return result;
                        } else {
                            result = setinfoArray(result, trip[0]);
                            result = setinfoArray(result, straight);
                            if (trip[1].Length == 4) {
                                result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            }
                            return result;
                        }
                    }
                }
                if (trip2 != -1 && (trip1 != -1 || trip1 == -1)) {
                    int[][] temSt = getStraight(splitArray(straight, dup[1]));
                    if (card1 != -1) {
                        if (trip[1].Length == 4) {
                            result = setinfoArray(result, trip[0]);
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            result = setinfoArray(result, straight);
                            return result;
                        } else {
                            if (temSt != null) {
                                result = setinfoArray(result, trip[0]);
                                result = setinfoArray(result, temSt[0]);
                                result = setinfoArray(result, trip[1]);
                                return result;
                            } else {
                                result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                                result = setinfoArray(result, trip[0]);
                                return result;
                            }
                        }
                    } else {
                        if (trip[1].Length == 4) {
                            result = setinfoArray(result, trip[0]);
                            result = setinfoArray(result, splitArray(trip[1], dup[1]));
                            result = setinfoArray(result, straight);
                            return result;
                        } else {
                            if (temSt != null) {
                                result = setinfoArray(result, trip[0]);
                                result = setinfoArray(result, temSt[0]);
                                result = setinfoArray(result, trip[1]);
                                return result;
                            } else {
                                result = comparePhom(trip[1], straight, dup[1], cardWin, 1);
                                result = setinfoArray(result, trip[0]);
                                return result;
                            }
                        }
                    }
                }
            }
        }
        return result;
    }

    /**
     * kt xem card Äƒn Ä‘Æ°á»£c cÃ³ trÃ¹ng phá»?m vá»›i nhá»¯ng card Ä‘Ã£ Äƒn
     * trÆ°á»›c Ä‘Ã³ hay chÆ°a
     * 
     * @param src
     *            : phá»?m
     * @param arrCheck
     *            : cardWin
     * @param cardCheck
     *            : card cáº§n kt
     * @return true náº¿u Ä‘Ãºng, false náº¿u sai
     */
    private static bool checkDupCardWin_Card(int[] src, int[] arrCheck,
            int cardCheck) {
        if (arrCheck == null) {
            return false;
        }
        int stPhom = -1;
        int checkCard = -1;
        stPhom = checkExistCardInPhom(src, arrCheck);
        checkCard = checkExistCardInPhom(src, cardCheck);
        // if(checkCard==-1)
        // return true;
        if (checkStraight(src)) {// phom straight
            if (src.Length < 6) {
                if ((stPhom != -1 && checkCard != -1)) {
                    return true;
                }
            } else {
                bool sub = checkSub(src, arrCheck, cardCheck);
                if ((src.Length == 6 || src.Length >= 9) && sub) {
                    return true;
                }
                if ((src.Length > 6 && src.Length < 9) && sub) {
                    return true;
                }
                // if(arrCheck!=null&&arrCheck.Length==2&&src.Length<9){
                // System.out.println("true cho nay ne");
                // return true;
                // }

            }
        } else if (checkTrip(src)) {// phom trip
            if ((stPhom != -1 && checkCard != -1)) {// phom Ä‘Ã£ Äƒn vÃ o 1 card
                // or
                // ko co card dc an trong
                // phom
                return true;
            }
        }
        return false;
    }

    /**
     * kt trong tá»«ng phá»?m cÃ³ 2 card Äƒn hay ko
     * 
     * @param src
     * @param arrCheck
     * @param cardCheck
     * @return
     */
    private static bool checkSub(int[] src, int[] arrCheck, int cardCheck) {
        if (checkStraight(src)) {
            int kt = checkExistCardInPhom(src, cardCheck);
            int k1 = checkExistCardInPhom(src, arrCheck);
            if (src.Length == 6 || src.Length == 9) {
                int[][] subArray = new int[2][];
                int[][] subArray1 = new int[2][];
                int check1 = -1, check2 = -1, check3 = -1, check4 = -1;
                int cWin1 = -1, cWin2 = -1;
                for (int i = 0; i < 2; i++) {
                    subArray[i][0] = src[0 + i * 3];
                    subArray[i][1] = src[1 + i * 3];
                    subArray[i][2] = src[2 + i * 3];

                    subArray1[i][0] = src[src.Length - 1 - i * 3];
                    subArray1[i][1] = src[src.Length - 2 - i * 3];
                    subArray1[i][2] = src[src.Length - 3 - i * 3];
                    if (arrCheck.Length == 2) {
                        cWin1 = checkExistCardInPhom(subArray[i], arrCheck[0]);
                        cWin2 = checkExistCardInPhom(subArray[i], arrCheck[1]);
                    }
                    check1 = checkExistCardInPhom(subArray[i], cardCheck);
                    check3 = checkExistCardInPhom(subArray[i], arrCheck);

                    if ((check1 != -1 && check3 != -1)
                            || (cWin1 != -1 && cWin2 != -1)) {
                        return true;
                    }
                    cWin1 = -1;
                    cWin2 = -1;
                    if (arrCheck.Length == 2) {
                        cWin1 = checkExistCardInPhom(subArray1[i], arrCheck[0]);
                        cWin2 = checkExistCardInPhom(subArray1[i], arrCheck[1]);
                    }
                    check2 = checkExistCardInPhom(subArray1[i], cardCheck);
                    check4 = checkExistCardInPhom(subArray1[i], arrCheck);
                    if ((check2 != -1 && check4 != -1)
                            || (cWin1 != -1 && cWin2 != -1)) {
                        return true;
                    }
                }
            } else {
                // if (arrCheck != null) {
                if (arrCheck.Length == 2) {
                    if (src.Length > 9) {
                        // 0 1 2 3 4 5 6 7 8
                        // {3,4,5,6,7,8,9,10,11,12};{6,10}; 7
                        k1 = checkExistCardInPhom(src, arrCheck[0]);
                        int k2 = checkExistCardInPhom(src, arrCheck[1]);
                        if (k1 <= 2) {
                            if (k2 <= 5 && kt > 5) {
                                return false;
                            }
                            if (k2 == 6 && (kt > 6 || (kt < 6 && kt > 2))) {
                                return false;
                            }
                            if (k2 > 6 && kt <= 6 && kt > 2) {
                                return false;
                            }
                            return true;
                        } else if (k2 <= 2) {
                            if (k1 <= 5 && kt > 5) {
                                return false;
                            }
                            if (k1 == 6 && (kt > 6 || (kt < 6 && kt > 2))) {
                                return false;
                            }
                            if (k1 > 6 && kt <= 6 && kt > 2) {
                                return false;
                            }
                            return true;
                        } else if (k1 == 3) {
                            if (k2 <= 2 && kt > 5) {
                                return false;
                            }
                            if (k2 == 6 && (kt <= 2 || kt > 6)) {
                                return false;
                            }
                            if (k2 > 6 && (kt <= 2 || (kt > 3 && kt <= 6))) {
                                return false;
                            }
                            return true;
                        } else if (k2 == 3) {
                            if (k1 <= 2 && kt > 5) {
                                return false;
                            }
                            if (k1 == 6 && (kt <= 2 || kt > 6)) {
                                return false;
                            }
                            if (k1 > 6 && (kt <= 2 || (kt > 3 && kt <= 6))) {
                                return false;
                            }
                            return true;
                        } else if (k1 > 2 && k1 <= 5) {
                            if (k2 > 5 && kt <= 2) {
                                return false;
                            }
                            if (k2 <= 2 && kt > 5) {
                                return false;
                            }
                            if (k2 == 6 && (kt <= 2 || kt > 6)) {
                                return false;
                            }
                            // +" "+kt);
                            return true;
                        } else if (k2 > 2 && k2 <= 5) {
                            if (k1 > 5 && kt <= 2) {
                                return false;
                            }
                            if (k1 <= 2 && kt > 5) {
                                return false;
                            }
                            if (k1 == 6 && (kt <= 2 || kt > 6)) {
                                return false;
                            }
                            if (k1 > 6 && (kt <= 6 && kt > 5)) {
                                return false;
                            }
                            return true;
                        } else if (k1 == 6) {
                            if (k2 < 4 && kt > 6) {
                                return false;
                            }
                            if (k2 > 6 && kt < 4) {
                                return false;
                            }
                            return true;
                        } else if (k2 == 6) {
                            if (k1 < 4 && kt > 6) {
                                return false;
                            }
                            if (k1 > 6 && kt < 4) {
                                return false;
                            }
                            return true;
                        } else if (k1 > 6) {
                            if (k2 <= 2 && kt > 2 && kt <= 6) {
                                return false;
                            }
                            if (k2 == 3 && (kt <= 2 || (kt > 3 && kt <= 6))) {
                                return false;
                            }
                            if (k2 > 3 && kt <= 3) {
                                return false;
                            }
                            return true;
                        } else if (k2 > 6) {
                            if (k1 <= 2 && kt > 2 && kt <= 6) {
                                return false;
                            }
                            if (k1 == 3 && (kt <= 2 || (kt > 3 && kt <= 6))) {
                                return false;
                            }
                            if (k1 > 3 && kt <= 3) {
                                return false;
                            }
                            return true;
                        }
                    } else if (src.Length < 9) {
                        return true;
                    }
                    // }
                } else {
                    if (src.Length >= 6) {
                        if (src.Length == 6) {
                            if (kt != -1
                                    && k1 != -1
                                    && ((kt > 2 && k1 <= 2) || (kt <= 2 && k1 > 2))) {
                                return false;
                            }
                        } else if (src.Length > 6 && src.Length < 9) {
                            if (arrCheck.Length == 1) {
                                if (kt != -1
                                        && k1 != -1
                                        && (((kt > 2 && k1 <= 2) || (kt <= 2 && k1 > 2))
                                                || ((kt > 3 && k1 <= 3) || (kt <= 3 && k1 > 3)) || ((kt > 4 && k1 <= 4) || (kt <= 4 && k1 > 4)))) {
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        } else if (src.Length >= 9) {
                            if (arrCheck.Length == 1) {
                                if (kt != -1
                                        && k1 != -1
                                        && (((kt > 7 && k1 <= 7) || (kt <= 7 && k1 > 7))
                                                | ((kt > 6 && k1 <= 6) || (kt <= 6 && k1 > 6))
                                                || ((kt > 5 && k1 <= 5) || (kt <= 5 && k1 > 5))
                                                || ((kt > 2 && k1 <= 2) || (kt <= 2 && k1 > 2))
                                                || ((kt > 3 && k1 <= 3) || (kt <= 3 && k1 > 3)) || ((kt > 4 && k1 <= 4) || (kt <= 4 && k1 > 4)))) {
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private static bool checkStraight(int[] card) {
        int Length = card.Length;
        for (int i = 0; i < Length - 1; i++) {
            int value = abs(card[i] - card[i + 1]);
            if (value > 1 || value == 0) {
                return false;
            }
        }
        return true;
    }

    private static bool checkTrip(int[] card) {
        int Length = card.Length;
        if (Length < 3) {
            return false;
        }
        int[] info, info1;
        for (int i = 0; i < Length - 1; i++) {
            info = getCardInfo(card[i]);
            info1 = getCardInfo(card[i + 1]);
            if (info[1] != info1[1]) {
                return false;
            }
        }
        return true;
    }

    /**
     * merge 2 array
     * 
     * @param src
     * @param des
     * @return
     */
    public static int[] mergeArray(int[] src, int[] des) {
        if (des == null) {
            return src;
        }
        if (src == null) {
            return des;
        }
        if (des != null && src != null) {
            // int temp[] = new int[des.Length + src.Length];
            // System.arraycopy(src, 0, temp, 0, src.Length);
            // System.arraycopy(des, 0, temp, src.Length, des.Length);
            List<string> temp = new List<string>();
            for (int i = 0; i < src.Length; i++) {
                if (!temp.Contains(src[i] + "")) {
                    temp.Add(src[i] + "");
                }
            }
            for (int i = 0; i < des.Length; i++) {
                if (!temp.Contains(des[i] + "")) {
                    temp.Add(des[i] + "");
                }
            }
            return vector2Array(temp);
        }
        return null;
    }

    public static int[] mergeArray2(int[] src, int[] des) {
        if (des == null) {
            return src;
        }
        if (src == null) {
            return des;
        }
        if (des != null && src != null) {
            // int temp[] = new int[des.Length + src.Length];
            // System.arraycopy(src, 0, temp, 0, src.Length);
            // System.arraycopy(des, 0, temp, src.Length, des.Length);
            List<string> temp = new List<string>();
            for (int i = 0; i < src.Length; i++) {
                if (!temp.Contains(src[i] + "") && src[i] != -1) {
                    temp.Add(src[i] + "");
                }
            }
            for (int i = 0; i < des.Length; i++) {
                if (!temp.Contains(des[i] + "") && des[i] != -1) {
                    temp.Add(des[i] + "");
                }
            }
            return vector2Array(temp);
        }
        return null;
    }

    /**
     * add array src to des
     * 
     * @param des
     * @param src
     * @return
     */
    private static int[][] setinfoArray(int[][] des, int[] src) {
        if (des == null) {
            des = new int[1][];
            des[0] = src;
        } else {
            int[][] temp = new int[des.Length + 1][];
            for (int i = 0; i < des.Length; i++) {
                temp[i] = trimArray(des[i]);
            }
            temp[des.Length] = trimArray(src);
            return temp;
        }
        return des;
    }

    public static int[] vector2Array(List<String> card) {
        int[] result = new int[card.Count];
        for (int i = 0; i < card.Count; i++) {
            result[i] = int.Parse(card[i]);
        }
        return result;
    }

    /**
     * insert one element to array
     * 
     * @param card
     * @param c
     * @return
     */
    public static int[] insertArray(int[] card, int c) {
        List<string> v = new List<string>();
        // int len = 1;
        int[] temp = null;
        // if (card != null) {
        // len = card.Length + 1;
        // temp = new int[len];
        // for (int i = 0; i < len - 1; i++) {
        // temp[i] = card[i];
        // }
        // temp[len - 1] = c;
        // } else {
        // temp = new int[1];
        // temp[0] = c;
        // }
        if (card != null) {
            for (int i = 0; i < card.Length; i++) {
                v.Add(card[i] + "");
            }
            v.Add(c + "");
            temp = vector2Array(v);
        } else {
            temp = new int[] { c };
        }
        return temp != null ? trimArray(temp) : temp;
    }

    public static String[] insertArray(String[] card, String c) {
        int len = 1;
        String[] temp = null;
        if (card != null) {
            len = card.Length + 1;
            temp = new String[len];
            for (int i = 0; i < len - 1; i++) {
                temp[i] = card[i];
            }
            temp[len - 1] = c;
        } else {
            temp = new String[1];
            temp[0] = c;
        }
        return temp;
    }

    /**
     * insert element theo straight
     * 
     * @param card
     * @param c
     * @return
     */
    public static int[] insertAndSort(int[] card, int c) {
        int len = 1;
        int[] temp = null;
        if (card != null) {
            len = card.Length + 1;
            temp = new int[len];
            List<string> v = new List<string>();
            bool addOk = false;
            for (int i = 0; i < card.Length; i++) {
                if (!v.Contains(card[i] + "")) {
                    if (card[i] > c && !addOk) {
                        v.Add(c + "");
                        addOk = true;
                    }
                    v.Add(card[i] + "");
                }
            }
            if (!addOk) {
                v.Add(c + "");
            }
            // if(!v.contains(c+""))
            // v.addElement(c+"");
            // for (int i = 0; i < len - 1; i++) {
            // if (card[i] > c) {
            // temp[i] = c;
            // for (int j = i + 1; j < len; j++) {
            // temp[j] = card[j - 1];
            // }
            // return temp;
            // } else {
            // temp[i] = card[i];
            // }
            // }
            temp = vector2Array(v);
            // temp=PageMansger.sort(temp);
        } else {
            temp = new int[1];
            temp[0] = c;
        }
        return temp != null ? trimArray(temp) : temp;
    }

    public static int[] insertCard(int[] card, int c) {
        int len = 1;
        int[] temp = null;
        int[] cInfo = getCardInfo(c);
        if (card != null) {
            len = card.Length + 1;
            temp = new int[len];
            for (int i = 0; i < len - 1; i++) {
                int[] info = getCardInfo(card[i]);
                if (info[1] > cInfo[1]) {
                    temp[i] = c;
                    for (int j = i + 1; j < len; j++) {
                        temp[j] = card[j - 1];
                    }
                    return temp;
                } else {
                    temp[i] = card[i];
                }
            }
            temp[len - 1] = c;
        } else {
            temp = new int[1];
            temp[0] = c;
        }
        return temp;
    }

    /**
     * 
     * @param card
     * @param cards
     * @return
     */
    public static int checkExistCardInPhom(int[] cards, int card) {
        if (cards != null) {
            for (int i = 0; i < cards.Length; i++) {
                if (card == cards[i]) {
                    return i;
                }
            }
        }
        return -1;
    }

    public static int checkExistCardInPhom(String[] cards, String card) {
        if (cards != null) {
            for (int i = 0; i < cards.Length; i++) {
                if (card.Equals(cards[i])) {
                    return i;
                }
            }
        }
        return -1;
    }

    /**
     * kt 2 mang co trung phan tu nao hay ko
     * 
     * @param cardSrc
     * @param cardsTest
     * @return
     */
    public static int checkExistCardInPhom(int[] cardSrc, int[] cardsTest) {
        if (cardsTest != null) {
            for (int i = 0; i < cardSrc.Length; i++) {
                for (int j = 0; j < cardsTest.Length; j++) {
                    if (cardsTest[j] == cardSrc[i]) {
                        return cardsTest[j];
                    }
                }

            }
        }
        return -1;
    }

    /**
     * check array is phom
     * 
     * @param card
     * @return
     */
    public static bool isPhom(int[] card) {
        if (getTriple(card) != null || getStraight(card) != null) {
            return true;
        }
        return false;
    }

    /**
     * split an element out array
     * 
     * @param card
     * @param t
     * @return
     */
    public static int[] splitArray(int[] card, int t) {
        int[] tem = null;
        if (card != null) {
            // tem=new int[card.Length-1];
            int j = 0;
            for (int i = 0; i < card.Length; i++) {
                if (card[i] != t) {
                    tem = insertAndSort(tem, card[i]);
                }
            }
        }
        return tem;
    }

    public static int[] compareArray(int[] trip, int[] straight,
            bool dupTrip, bool dupStraight, int scoreTrip,
            int scoreStraight) {
        if (dupTrip && !dupStraight) {
            return trip;
        }
        if (dupStraight && !dupTrip) {
            return straight;
        }

        if (dupTrip && dupStraight) {
            if (scoreTrip > scoreStraight) {
                return trip;
            } else {
                return straight;
            }
        }
        return null;
    }

    /**
     * so sÃ¡nh 2 phá»?m cÃ³ trÃ¹ng nhau , náº¿u cÃ³ tinh giáº£n cho trÆ°á»?ng
     * há»£p tá»‘i Æ°u
     * 
     * @param card1
     * @param card2
     */
    // public static int[][] comparePhom(int trip[],int straight[],int dup,int
    // cardWin[]){
    public static int[][] comparePhom(int[] trip, int[] straight, int dup,
            int[] cardWin, int tripOrStraight) {
        int[][] temp = null;
        int len1 = trip.Length;
        int len2 = straight.Length;
        if (len1 < 3 && len2 < 3 || (!isPhom(trip) && !isPhom(straight))) {
            return null;
        }
        if (len1 < 3 || !isPhom(trip)) {
            return setinfoArray(temp, straight);
        }
        if (len2 < 3 || !isPhom(straight)) {
            return setinfoArray(temp, trip);
        }
        int scTrip = getScore(trip), scStraight = getScore(straight);
        bool dupTrip = checkCardWin(trip, cardWin);
        bool dupStraight = checkCardWin(straight, cardWin);

        if (len1 == len2) {// hai phom bang nhau
            if (len1 == 3) {
                if (cardWin != null) {
                    if (dupTrip && dupStraight) {
                        if (scTrip > scStraight) {
                            temp = setinfoArray(temp, trip);
                            return temp;
                        } else {
                            temp = setinfoArray(temp, straight);
                            return temp;
                        }
                    }
                    if (checkCardWin(trip, cardWin)) {
                        temp = setinfoArray(temp, trip);
                        return temp;
                    } else if (checkCardWin(straight, cardWin)) {
                        temp = setinfoArray(temp, straight);
                        return temp;
                    }
                }
                if (tripOrStraight == 0) {
                    // if (scTrip >= scStraight) {
                    temp = setinfoArray(temp, trip);
                    // } else {
                    // temp = setinfoArray(temp, straight);
                    // }
                } else {
                    // if (scTrip > scStraight) {
                    // temp = setinfoArray(temp, trip);
                    // } else {
                    temp = setinfoArray(temp, straight);
                    // }
                }
            } else {// >3
                // tempCard=splitArray(trip, dup);
                temp = setinfoArray(temp, straight);
                temp = setinfoArray(temp, splitArray(trip, dup));
            }
        } else if (len1 < len2) {// phom sanh dai hon phom trip
            if (len1 == 3) {
                int[] t = splitArray(straight, dup);
                if (!checkStraight(t)) {
                    int[][] st = getStraight(t);
                    if (st == null) {
                        if ((!dupTrip && !dupStraight) || (dupTrip && dupStraight)) {
                            temp = setinfoArray(temp, getScore(trip) > getScore(straight) ? trip : straight);
                        } else {
                            temp = setinfoArray(temp, dupTrip ? trip : straight);
                        }
                    } else {
                        temp = setinfoArray(temp, st[0]);
                        temp = setinfoArray(temp, trip);
                        return temp;
                    }
                } else {
                    temp = setinfoArray(temp, trip);
                    temp = setinfoArray(temp, t);
                }
            } else {
                temp = setinfoArray(temp, straight);
                temp = setinfoArray(temp, splitArray(trip, dup));
            }
        } else if (len1 > len2) {// phom trip dai hon phom sanh
            temp = setinfoArray(temp, straight);
            temp = setinfoArray(temp, splitArray(trip, dup));
        }
        return temp;
    }

    /**
     * gá»­i card
     * 
     */
    public static int[] attachCard(int[][] phom, int[] layCard) {
        int[] result = null;
        int[] temp = layCard;
        if (phom != null) {
            int lenPhom = phom.Length;
            for (int i = 0; i < lenPhom; i++) {
                for (int j = 0; j < temp.Length; j++) {
                    int[] temPhom = insertArray(phom[i], temp[j]);
                    if (isTrip(temPhom) || isStraight(temPhom)) {
                        result = insertArray(result, temp[j]);
                        temp = splitArray(temp, temp[j]);
                    }

                }
            }
        }
        return result;
    }

    /**
     * 
     * @param arr
     * @param Length
     * @return
     */
    private static bool checkLoop(int[][] arr, int Length) {
        for (int j = 1; j < Length; j++) {
            if (arr[0][1] != arr[j][1]) {
                return false;
            }
        }
        return true;
    }

    /**
     * sort máº£ng
     * 
     * @param card
     * @return
     */
    public static int[] sortType(int[] card) {
        int len = card.Length;
        int[] temp = null;
        int start = 0;
        int j = 0;
        while (start < 4) {
            int k = -1;
            for (int i = 0; i < len; i++) {
                int[] info = getCardInfo(card[i]);// 0-type 1-cardID
                if (info[0] == start) {
                    if (k == -1) {
                        k = j;
                    }
                    temp = insertAndSort(temp, card[i]);
                }
            }
            start++;
        }
        // Page.card=temp;
        return temp;
    }

    /**
     * get total score a phom
     * 
     * @param card
     * @return
     */
    private static int getScore(int[] card) {
        int score = 0;
        for (int i = 0; i < card.Length; i++) {
            score += getCardInfo(card[i])[1] + 1;
        }
        return score;
    }

    /**
     * check duplicate beweent phom1 and phom2
     * 
     * @param card1
     * @param card2
     * @return
     */
    public static int duplicateCard(int[] card1, int[] card2) {
        int len1 = card1.Length;
        int len2 = card2.Length;
        if (len1 >= len2) {
            for (int i = 0; i < len1; i++) {
                for (int j = 0; j < len2; j++) {
                    if (card1[i] == card2[j]) {
                        return card1[i];
                    }
                }
            }
        }
        if (len2 >= len1) {
            for (int i = 0; i < len2; i++) {
                for (int j = 0; j < len1; j++) {
                    if (card2[i] == card1[j]) {
                        return card2[i];
                    }
                }
            }
        }
        return -1;
    }

    public static int[] trimArray(int[] array) {
        // int result[] = null;
        List<string> temp = new List<string>();
        for (int i = 0; i < array.Length; i++) {
            if (!temp.Contains(array[i] + "")) {
                temp.Add(array[i] + "");
            }
            // if (temp.size()==0) {
            // temp.addElement(array[i]+"");
            // } else {
            // bool dup=false;
            // for(int j=0;j<temp.size();j++){
            // int value=Integer.parseInt((String)temp.elementAt(j));
            // if(value==array[i]){
            // dup=true;
            // break;
            // }
            // }
            // // result = insertArray(result, array[i]);
            // if(!dup)
            // temp.addElement(array[i]+"");
            // }
        }
        return vector2Array(temp);
    }

    public static int[] sortPhom(int[] card, int[][] phom) {
        int[] result = null;

        if (phom != null) {
            for (int i = 0; i < phom.Length; i++) {
                result = mergeArray(result, phom[i]);
            }
            List<string> v = new List<string>();
            for (int i = 0; i < result.Length; i++) {
                v.Add(result[i] + "");
            }
            for (int i = 0; i < card.Length; i++) {
                if (!v.Contains(card[i] + "")) {
                    v.Add(card[i] + "");
                }
                // if (checkExistCardInPhom(result, card[i]) == -1)
                // result = insertArray(result, card[i]);

            }
            result = vector2Array(v);
        } else {
            result = card;
        }
        return trimArray(result);
    }

    public static int[] getCardInfo(int i) {
        int[] turn = { i / 13, i % 13 };// 0 - type 1 - cardID
        return turn;
    }

    public static int abs(int x) {
        return x > 0 ? x : -x;
    }

    public static int[] sort(int[] arr) {// mang cac so thu tu quan bai tu 0-51
        int[] turn = arr;
        int Length = turn.Length;
        for (int i = 0; i < Length - 1; i++) {
            int min = i;
            for (int j = i + 1; j < Length; j++) {
                if (getCardInfo(turn[j])[1] < getCardInfo(turn[min])[1]) {
                    // swap
                    min = j;
                }
            }
            int temp = turn[i];
            turn[i] = turn[min];
            turn[min] = temp;
        }
        return turn;
    }

    public static List<String> Array2Vector(int[] array) {
        List<String> temp = new List<String>();
        if (array != null) {
            for (int i = 0; i < array.Length; i++) {
                temp.Add(array[i] + "");
            }
        }

        return temp;
    }

    public static List<String> Array2Vector(int[][] array) {
        List<String> temp = new List<String>();
        for (int i = 0; i < array.Length; i++) {
            for (int j = 0; j < array[i].Length; j++) {
                temp.Add(array[i][j] + "");
            }
        }
        return temp;
    }

    public static int countCardInPhom(int[][] phom) {
        int count = 0;
        if (phom != null) {
            for (int i = 0; i < phom.Length; i++) {
                for (int j = 0; j < phom.Length; j++) {
                    count++;
                }
            }
        }
        return count;
    }

    public static List<String> removePhomInCards(int[] card, int[][] phom) {
        List<String> temp = Array2Vector(card);
        for (int i = 0; i < phom.Length; i++) {
            for (int j = 0; j < phom[i].Length; j++) {
                temp.Remove(phom[i][j] + "");
            }
        }
        return temp;

    }

    public static int[][] getCardHandMinh(int[] phom1, int[] phom2,
            int[] phom3, int[] card, int[] cardWin) {
        int c = duplicateCard(phom1, phom2);
        // if (cardWin == null) {
        // if (c == -1) {
        // if (phom3 == null) {
        // System.out.println("---555555555550000000000");
        // int phom[][] = new int[1][];
        // phom[0] = phom1;
        // return getCardHandMinhSimple(card, phom);
        // } else {
        // System.out.println("---555555555555111111111111111");
        // int phom[][] = new int[2][];
        // phom[0] = phom1;
        // phom[1] = phom3;
        // return getCardHandMinhSimple(card, phom);
        // }
        //
        // } else {
        // if (phom3 == null) {
        // System.out.println("----55555555522222222222222");
        // int phom[][] = new int[1][];
        // phom[0] = phom2;
        // int phomminh[][] = new int[1][];
        // phomminh[0] = phom1;
        // return concactArrayMinh(
        // getCardHandMinhSimple(card, phomminh),
        // getCardHandMinhSimple(card, phom), false);
        // } else {
        // System.out.println("-----5555555555333333333333");
        // int phom[][] = new int[2][];
        // phom[0] = phom2;
        // phom[1] = phom3;
        // int phomminh[][] = new int[2][];
        // phomminh[0] = phom1;
        // phomminh[1] = phom3;
        // return concactArrayMinh(
        // getCardHandMinhSimple(card, phomminh),
        // getCardHandMinhSimple(card, phom), false);
        // }
        //
        // }
        // } else {
        if (c == -1) {
            if (phom3 == null) {
                int[][] phom = new int[1][];
                phom[0] = phom1;
                return getCardHandMinhSimple(card, phom);
            } else {
                int[][] phom = new int[2][];
                phom[0] = phom1;
                phom[1] = phom3;
                return getCardHandMinhSimple(card, phom);
            }
        } else if (checkContains(phom1, cardWin) != -1) {
            if (checkContains(phom2, checkContains(phom1, cardWin))) {
                if (phom3 == null) {
                    int[][] phom = new int[1][];
                    phom[0] = phom2;
                    int[][] phomminh = new int[1][];
                    phomminh[0] = phom1;
                    return concactArrayMinh(
                            getCardHandMinhSimple(card, phomminh),
                            getCardHandMinhSimple(card, phom));
                } else if (checkContains(phom3, phom2) == -1) {
                    int[][] phom = new int[2][];
                    phom[0] = phom2;
                    phom[1] = phom3;
                    int[][] phomminh = new int[2][];
                    phomminh[0] = phom1;
                    phomminh[1] = phom3;
                    return concactArrayMinh(
                            getCardHandMinhSimple(card, phomminh),
                            getCardHandMinhSimple(card, phom));
                } else if (checkContains(phom3, cardWin) == -1) {
                    int[][] phom = new int[1][];
                    phom[0] = phom2;
                    int[][] phomminh = new int[2][];
                    phomminh[0] = phom1;
                    phomminh[1] = phom3;
                    return concactArrayMinh(
                            getCardHandMinhSimple(card, phomminh),
                            getCardHandMinhSimple(card, phom));
                } else {
                    int[][] phomminh = new int[2][];
                    phomminh[0] = phom1;
                    phomminh[1] = phom3;
                    return getCardHandMinhSimple(card, phomminh);
                }
            } else {
                if (phom3 == null) {
                    int[][] phom = new int[1][];
                    phom[0] = phom1;
                    return getCardHandMinhSimple(card, phom);
                } else {
                    int[][] phom = new int[2][];
                    phom[0] = phom1;
                    phom[1] = phom3;
                    return getCardHandMinhSimple(card, phom);
                }
            }

        } else {
            if (phom3 == null) {
                int[][] phom = new int[1][];
                phom[0] = phom2;
                int[][] phomminh = new int[1][];
                phomminh[0] = phom1;
                return concactArrayMinh(getCardHandMinhSimple(card, phomminh),
                        getCardHandMinhSimple(card, phom));
            } else if (checkContains(phom3, phom2) == -1) {
                int[][] phom = new int[2][];
                phom[0] = phom2;
                phom[1] = phom3;
                int[][] phomminh = new int[2][];
                phomminh[0] = phom1;
                phomminh[1] = phom3;
                return concactArrayMinh(getCardHandMinhSimple(card, phomminh),
                        getCardHandMinhSimple(card, phom));
            } else if (checkContains(phom3, cardWin) == -1) {
                int[][] phom = new int[1][];
                phom[0] = phom2;
                int[][] phomminh = new int[2][];
                phomminh[0] = phom1;
                phomminh[1] = phom3;
                return concactArrayMinh(getCardHandMinhSimple(card, phomminh),
                        getCardHandMinhSimple(card, phom));
            } else {
                int[][] phomminh = new int[2][];
                phomminh[0] = phom1;
                phomminh[1] = phom3;
                return getCardHandMinhSimple(card, phomminh);
            }
        }
        // }
    }

    public static bool checkContains(int[] card, int cardCheck) {
        List<String> c = Array2Vector(card);
        if (c.Contains(cardCheck + ""))
            return true;
        return false;
    }

    public static int checkContains(int[] card, int[] cardCheck) {
        List<String> c = Array2Vector(card);
        for (int i = 0; i < cardCheck.Length; i++) {
            if (c.Contains(cardCheck[i] + ""))
                return cardCheck[i];
        }
        return -1;
    }

    public static int[][] getCardHandMinhSimple(int[] card, int[][] phom) {
        int[][] minh = null;
        List<String> temp = removePhomInCards(card, phom);
        if (temp.Count > 2) {
            minh = new int[3][];
            List<String> temp21 = null, temp20 = null, temp22 = null;
            temp20 = Array2Vector(phom);
            temp21 = Array2Vector(phom);
            temp22 = Array2Vector(phom);
            int[] minh21 = sortCa(vector2Array(temp), 0);
            for (int i = 0; i < minh21.Length; i++) {
                temp21.Add(minh21[i] + "");
            }
            minh[1] = vector2Array(temp21);

            int[] minh22 = sortCa(vector2Array(temp), 1);

            for (int i = 0; i < minh22.Length; i++) {
                temp22.Add(minh22[i] + "");
            }

            minh[2] = vector2Array(temp22);
            int[] minh20 = vector2Array(temp);
            for (int i = 0; i < minh20.Length; i++) {
                temp20.Add(minh20[i] + "");

            }
            minh[0] = vector2Array(temp20);

        } else {
            minh = new int[1][];
            List<String> temp20 = null;
            temp20 = Array2Vector(phom);
            int[] minh20 = vector2Array(temp);
            for (int i = 0; i < minh20.Length; i++) {
                temp20.Add(minh20[i] + "");

            }
            minh[0] = vector2Array(temp20);
        }
        return minh;
    }

    public static int[][] concactArrayMinh(int[][] array1, int[][] array2) {
        if (array1 == null)
            return array2;
        else if (array2 == null)
            return array1;
        int[][] minh = null;
        minh = new int[array1.Length + array2.Length][];

        int i;
        for (i = 0; i < array1.Length; i++) {
            minh[i] = array1[i];
            // minh[i] = new int[array1[i].Length];
            // for (int j = 0; j < array1[i].Length; j++) {
            // minh[i][j] = array1[i][j];
            // }
        }

        for (int i1 = 0; i1 < array2.Length; i1++) {
            minh[i + i1] = array2[i1];
            // minh[i + i1] = new int[array2[i1].Length];
            // for (int j = 0; j < array2[i1].Length; j++) {
            // minh[i + i1][j] = array2[i1][j];
            // }
        }

        return minh;
    }

    public static bool compare2Straight(int[] s1, int[] s2) {
        if (s1.Length == s2.Length) {
            int i1 = 0, i2 = 0;
            for (int i = 0; i < s2.Length; i++) {
                i1 += s1[i];
                i2 += s2[i];
            }
            if (i1 == i2)
                return true;
        }

        return false;
    }

    public static bool compare2Trip(int[] t1, int[] t2) {

        if (t1.Length == t2.Length) {
            t1 = sortType(t1);
            t2 = sortType(t2);
            for (int i = 0; i < t2.Length; i++) {
                if (t1[i] != t2[i])
                    return false;
            }
            return true;

        }
        return false;
    }

    public static int[][] sortPhom1(int[] card, int[][] phom, int[] cardWin) {
        int[][] minh = null;
        int[][] t = getTriple(card);
        int[][] s = getStraight(card);
        if (phom != null) {
            if (phom.Length == 1) {
                if (isTrip(phom[0])) {
                    if (s != null) {
                        minh = getCardHandMinh(phom[0], s[0], null, card,
                                cardWin);
                        return optimized(minh);
                    } else {
                        minh = getCardHandMinhSimple(card, phom);
                        return optimized(minh);
                    }
                } else {
                    if (t != null) {
                        minh = getCardHandMinh(phom[0], t[0], null, card,
                                cardWin);
                        return optimized(minh);
                    } else {
                        minh = getCardHandMinhSimple(card, phom);
                        return optimized(minh);
                    }
                }
            } else {
                if (isTrip(phom[0])) {
                    if (isTrip(phom[1])) {
                        if (s != null) {
                            if (s.Length == 1) {
                                minh = getCardHandMinh(phom[0], s[0], phom[1],
                                        card, cardWin);
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], s[0], phom[0],
                                                card, cardWin));
                                return optimized(minh);
                            } else {
                                minh = getCardHandMinh(phom[0], s[0], phom[1],
                                        card, cardWin);
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], s[0], phom[0],
                                                card, cardWin));
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[0], s[1], phom[1],
                                                card, cardWin));
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], s[1], phom[0],
                                                card, cardWin));
                                return optimized(minh);
                            }
                        } else {
                            minh = getCardHandMinhSimple(card, phom);
                            return optimized(minh);
                        }
                    } else {
                        if (s != null) {
                            if (s.Length == 1) {
                                if (!compare2Straight(phom[1], s[0])) {
                                    minh = getCardHandMinh(phom[0], s[0],
                                            phom[1], card, cardWin);
                                    return optimized(minh);
                                } else {
                                    minh = getCardHandMinhSimple(card, phom);
                                    return optimized(minh);
                                }
                            } else {
                                if (!compare2Straight(phom[1], s[0])) {
                                    minh = getCardHandMinh(phom[0], s[0],
                                            phom[1], card, cardWin);

                                } else {
                                    minh = getCardHandMinhSimple(card, phom);
                                }
                                if (!compare2Straight(phom[1], s[1])) {
                                    minh = concactArrayMinh(
                                            minh,
                                            getCardHandMinh(phom[0], s[1],
                                                    phom[1], card, cardWin));
                                } else {
                                    minh = concactArrayMinh(minh,
                                            getCardHandMinhSimple(card, phom));
                                }
                                return optimized(minh);

                            }

                        } else {
                            minh = getCardHandMinhSimple(card, phom);
                            return optimized(minh);
                        }
                    }
                } else {
                    if (isStraight(phom[1])) {
                        if (t != null) {
                            if (t.Length == 1) {
                                minh = getCardHandMinh(phom[0], t[0], phom[1],
                                        card, cardWin);
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], t[0], phom[0],
                                                card, cardWin));
                                return optimized(minh);
                            } else {
                                minh = getCardHandMinh(phom[0], t[0], phom[1],
                                        card, cardWin);
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], t[0], phom[0],
                                                card, cardWin));
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[0], t[1], phom[1],
                                                card, cardWin));
                                minh = concactArrayMinh(
                                        minh,
                                        getCardHandMinh(phom[1], t[1], phom[0],
                                                card, cardWin));
                                return optimized(minh);
                            }
                        } else {
                            minh = getCardHandMinhSimple(card, phom);
                            return optimized(minh);
                        }
                    } else {
                        if (t != null) {
                            if (t.Length == 1) {
                                if (!compare2Trip(phom[1], t[0])) {
                                    minh = getCardHandMinh(phom[0], t[0],
                                            phom[1], card, cardWin);
                                    return optimized(minh);
                                } else {
                                    minh = getCardHandMinhSimple(card, phom);
                                    return optimized(minh);
                                }
                            } else {
                                if (!compare2Trip(phom[1], t[0])) {
                                    minh = getCardHandMinh(phom[0], t[0],
                                            phom[1], card, cardWin);

                                } else {
                                    minh = getCardHandMinhSimple(card, phom);

                                }
                                if (!compare2Trip(phom[1], t[1])) {
                                    minh = concactArrayMinh(
                                            minh,
                                            getCardHandMinh(phom[0], t[1],
                                                    phom[1], card, cardWin));
                                } else {
                                    minh = concactArrayMinh(minh,
                                            getCardHandMinhSimple(card, phom));
                                }
                                return optimized(minh);

                            }

                        } else {
                            minh = getCardHandMinhSimple(card, phom);
                            return optimized(minh);
                        }
                    }
                }

            }

        } else {
            minh = new int[3][];
            minh[0] = trimArray(card);
            minh[1] = sortCa(card, 0);
            minh[2] = sortCa(card, 1);
        }

        return optimized(minh);

    }

    public static int[] removeCard(int[] card, int idcard) {
        List<String> minh = new List<String>();
        minh = Array2Vector(card);
        for (int i = 0; i < card.Length; i++) {
            if (minh.Contains(idcard + "")) {
                minh.Remove(idcard + "");
            }
        }
        return vector2Array(minh);

    }

    public static int[][] optimized(int[][] card) {
        if (card == null)
            return null;
        List<String> a = new List<String>();
        List<int> b = new List<int>();
        String s = null;
        for (int i = 0; i < card.Length; i++) {
            s = card2String(card[i]);
            if (!a.Contains(s)) {
                a.Add(s);
            } else
                b.Add(i);
        }
        int[][] c = new int[card.Length - b.Count][];
        int count = 0;
        for (int i = 0; i < card.Length; i++) {
            if (b.Contains(i)) {
                continue;
            } else {
                c[count] = card[i];
                count++;
            }
        }
        return c;
    }

    public static String card2String(int[] card) {
        if (card == null) {
            return null;
        }
        String s = "";
        for (int i = 0; i < card.Length; i++) {
            s += card[i];
        }
        return s;

    }

    public static int[][] getPhomHa(int[] card, int[] cardWin) {
        int[] phom1 = null;
        int[] phom2 = null;
        if (isTrip(getArraybyIndex(card, 0, 3))) {
            if (isStraight(getArraybyIndex(card, 3, 3))) {
                if (isStraight(getArraybyIndex(card, 3, 4))) {
                    if (isTrip(getArraybyIndex(card, 0, 4))) {
                        if (cardWin != null) {
                            if (cardWin.Length == 2) {
                                if (cardWin[0] % 13 == cardWin[1] % 13) {
                                    phom1 = getArraybyIndex(card, 0, 3);
                                    if (isStraight(getArraybyIndex(card, 3, 5))) {
                                        phom2 = getArraybyIndex(card, 3, 5);
                                    } else {
                                        phom2 = getArraybyIndex(card, 3, 4);
                                    }
                                } else {
                                    phom1 = getArraybyIndex(card, 0, 4);
                                    if (isStraight(getArraybyIndex(card, 4, 4))) {
                                        phom2 = getArraybyIndex(card, 4, 4);
                                    } else {
                                        phom2 = getArraybyIndex(card, 4, 3);
                                    }
                                }
                            } else {
                                phom1 = getArraybyIndex(card, 0, 4);
                                if (isStraight(getArraybyIndex(card, 4, 4))) {
                                    phom2 = getArraybyIndex(card, 4, 4);
                                } else {
                                    phom2 = getArraybyIndex(card, 4, 3);
                                }
                            }
                        } else {
                            phom1 = getArraybyIndex(card, 0, 4);
                            if (isStraight(getArraybyIndex(card, 4, 4))) {
                                phom2 = getArraybyIndex(card, 4, 4);
                            } else {
                                phom2 = getArraybyIndex(card, 4, 3);
                            }
                        }

                    } else {
                        if (isStraight(getArraybyIndex(card, 3, 5))) {
                            phom1 = getArraybyIndex(card, 0, 3);
                            phom2 = getArraybyIndex(card, 3, 5);
                        } else {
                            phom1 = getArraybyIndex(card, 0, 3);
                            phom2 = getArraybyIndex(card, 3, 4);
                        }
                    }

                } else {
                    phom1 = getArraybyIndex(card, 0, 3);
                    phom2 = getArraybyIndex(card, 3, 3);
                }
            } else if (isTrip(getArraybyIndex(card, 0, 4))) {
                phom1 = getArraybyIndex(card, 0, 4);
                if (isStraight(getArraybyIndex(card, 4, 3))) {
                    if (isStraight(getArraybyIndex(card, 4, 4))) {
                        if (isStraight(getArraybyIndex(card, 4, 5))) {
                            phom2 = getArraybyIndex(card, 4, 5);
                        } else {
                            phom2 = getArraybyIndex(card, 4, 4);
                        }
                    } else {
                        phom2 = getArraybyIndex(card, 4, 3);
                    }
                } else {

                }
            } else {
                phom1 = getArraybyIndex(card, 0, 3);
                if (isTrip(getArraybyIndex(card, 3, 3))) {
                    if (isTrip(getArraybyIndex(card, 3, 4))) {
                        phom2 = getArraybyIndex(card, 3, 4);
                    } else {
                        phom2 = getArraybyIndex(card, 3, 3);
                    }
                } else {
                    if (isStraight(getArraybyIndex(card, 3, 3))) {
                        if (isStraight(getArraybyIndex(card, 3, 4))) {
                            if (isStraight(getArraybyIndex(card, 3, 5))) {
                                phom2 = getArraybyIndex(card, 3, 5);
                            } else {
                                phom2 = getArraybyIndex(card, 3, 4);
                            }
                        } else {
                            phom2 = getArraybyIndex(card, 3, 3);
                        }
                    } else {

                    }

                }
            }

        } else {
            if (isStraight(getArraybyIndex(card, 0, 3))) {
                if (isTrip(getArraybyIndex(card, 3, 3))) {
                    if (isTrip(getArraybyIndex(card, 3, 4))) {
                        if (isStraight(getArraybyIndex(card, 0, 4))) {
                            if (cardWin != null) {
                                if (cardWin.Length == 2) {
                                    if (cardWin[0] % 13 == cardWin[1] % 13) {
                                        phom1 = getArraybyIndex(card, 0, 4);
                                        phom2 = getArraybyIndex(card, 4, 3);
                                    } else {
                                        phom1 = getArraybyIndex(card, 0, 3);
                                        phom2 = getArraybyIndex(card, 3, 4);
                                    }
                                } else {
                                    phom1 = getArraybyIndex(card, 0, 3);
                                    phom2 = getArraybyIndex(card, 3, 4);
                                }
                            } else {
                                phom1 = getArraybyIndex(card, 0, 3);
                                phom2 = getArraybyIndex(card, 3, 4);
                            }
                        }
                    } else {
                        phom1 = getArraybyIndex(card, 0, 3);
                        phom2 = getArraybyIndex(card, 3, 3);
                    }
                } else {
                    if (isStraight(getArraybyIndex(card, 0, 4))) {
                        if (isTrip(getArraybyIndex(card, 4, 3))) {
                            if (isTrip(getArraybyIndex(card, 4, 4))) {
                                if (isStraight(getArraybyIndex(card, 0, 5))) {
                                    if (cardWin != null) {
                                        if (cardWin.Length == 2) {
                                            if (cardWin[0] % 13 == cardWin[1] % 13) {
                                                phom1 = getArraybyIndex(card,
                                                        0, 5);
                                                phom2 = getArraybyIndex(card,
                                                        5, 3);
                                            } else {
                                                phom1 = getArraybyIndex(card,
                                                        0, 4);
                                                phom2 = getArraybyIndex(card,
                                                        4, 4);
                                            }
                                        } else {
                                            phom1 = getArraybyIndex(card, 0, 4);
                                            phom2 = getArraybyIndex(card, 4, 4);
                                        }
                                    } else {
                                        phom1 = getArraybyIndex(card, 0, 4);
                                        phom2 = getArraybyIndex(card, 4, 4);
                                    }
                                }

                            } else {
                                phom1 = getArraybyIndex(card, 0, 4);
                                phom2 = getArraybyIndex(card, 4, 3);
                            }
                        } else {
                            if (isStraight(getArraybyIndex(card, 0, 5))) {
                                if (isTrip(getArraybyIndex(card, 5, 3))) {
                                    phom1 = getArraybyIndex(card, 5, 3);
                                    phom2 = getArraybyIndex(card, 0, 5);

                                } else {
                                    if (isStraight(getArraybyIndex(card, 0, 6))) {
                                        if (isStraight(getArraybyIndex(card, 0,
                                                7))) {
                                            if (isStraight(getArraybyIndex(
                                                    card, 0, 8))) {
                                                phom1 = getArraybyIndex(card,
                                                        0, 8);
                                            } else {
                                                phom1 = getArraybyIndex(card,
                                                        0, 7);
                                            }
                                        } else {
                                            phom1 = getArraybyIndex(card, 0, 6);
                                        }
                                    } else {
                                        phom1 = getArraybyIndex(card, 0, 5);
                                        if (isStraight(getArraybyIndex(card, 5,
                                                3))) {
                                            phom2 = getArraybyIndex(card, 5, 3);
                                        }
                                    }
                                }
                            } else {
                                phom1 = getArraybyIndex(card, 0, 4);
                                if (isStraight(getArraybyIndex(card, 4, 3))) {
                                    if (isStraight(getArraybyIndex(card, 4, 4))) {
                                        phom2 = getArraybyIndex(card, 4, 4);
                                    } else {
                                        phom2 = getArraybyIndex(card, 4, 3);
                                    }
                                }
                            }
                        }
                    } else {
                        phom1 = getArraybyIndex(card, 0, 3);
                        if (isStraight(getArraybyIndex(card, 3, 3))) {
                            phom2 = getArraybyIndex(card, 3, 3);
                        }
                    }
                }
            }
        }
        if (phom1 == null) {
            return null;
        } else {
            if (phom2 == null) {
                int[][] phom = new int[1][];
                phom[0] = phom1;
                return phom;
            } else {
                int[][] phom = new int[2][];
                phom[0] = phom1;
                phom[1] = phom2;
                return phom;
            }
        }
    }

    public static int[] getArraybyIndex(int[] card, int begin, int Length) {
        if (card == null) {
            return null;
        } else if (card.Length < begin + Length) {
            return null;
        }
        int[] k = new int[Length];
        for (int i = 0; i < Length; i++) {
            k[i] = card[i + begin];
        }
        return k;
    }

    public static int[][] getPhom3Minh(int[] card, int[] cardWin) {
        int[][] result = null;
        int[][] trip = getTriple(card);
        int[][] straight = getStraight(card);
        if (trip == null && straight == null) {
            // System.out.println("NULLL");
            return null;
        }
        if (trip == null && straight != null) {
            // System.out.println("chi lay sanh");
            return straight;
        }
        if (trip != null && straight == null) {
            // System.out.println("chi lay trip");
            return trip;
        }
        int lenTrip = trip == null ? 0 : trip.Length;
        int lenStraight = straight == null ? 0 : straight.Length;
        // neu thang strainght chua thang nao trong trip thi loai bo no va lam
        // lai
        // neu no chi la phom
        if (lenTrip >= 1 && lenStraight == 1) {
            int[] info = getCardInfo(straight[0][0]);
            int[] phom = trip[0];
            int value = -1;
            for (int i = 0; i < phom.Length; i++) {
                int[] infor2 = getCardInfo(phom[i]);
                if (infor2[0] == info[0]) {
                    value = infor2[1];
                    break;
                }
            }
            if (value != -1 && trip[0].Length < 4
                    && checkExistCardInPhom(trip[0], cardWin) != -1) {
                int[] card2 = subTwoArray(card, trip[0]);
                int[][] straight2 = getStraight(card2);
                // tach phom ra
                if (straight2 != null && straight2.Length == 2) {
                    lenTrip = trip.Length;
                    lenStraight = straight2.Length;
                    straight = straight2;
                }
                if (straight2 != null && lenTrip == 2 && straight2.Length == 1) {
                    lenTrip = trip.Length;
                    lenStraight = straight2.Length;
                    straight = straight2;
                }
                if (straight2 == null) {
                    // lenStraight = 0;
                }
            }
        }

        if (lenTrip > 2 && lenStraight > 2) {
            int totalTrip = 0;
            int totalStraight = 0;
            for (int i = 0; i < 3; i++) {
                totalTrip += trip[i].Length;
                totalStraight += straight[i].Length;
            }
            return (totalStraight > totalTrip ? straight : trip);
        }
        if (lenTrip > 2 && lenStraight < 2) {
            return trip;
        }
        if (lenTrip < 2 && lenStraight > 2) {
            return straight;
        }
        if (lenStraight > 2 || (lenTrip == 0 && lenStraight > 0)) {
            return straight;
        }
        if ((lenTrip > 2) || (lenStraight == 0 && lenTrip > 0)) {
            return trip;
        }
        if (lenStraight > lenTrip) {// phom sanh nhieu hon trips
            // System.out.println("sanh nhieu hon " + lenStraight + " " +
            // lenTrip);
            result = selectPhomStraight(straight, trip[0], cardWin);
            // Æ°u tiÃªn láº¥y phá»?m cÃ³ chá»©a card win trÆ°á»›c
        } else if (lenStraight < lenTrip) {// phom trip nhieu hon sanh
            // Æ°u tiÃªn láº¥y phá»?m cÃ³ cardWin trÆ°á»›c.
            // System.out.println("trip nhieu hon " + lenStraight + " " +
            // lenTrip);
            try {
                result = selectPhomTrip(trip, straight[0], cardWin);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        } else {
            result = selectEqua(trip, straight, cardWin, false);
        }
        scoreTrip = 0;
        scoreStraight = 0;
        if (result != null) {
            int[][] a = new int[result.Length][];
            int count = 0;
            for (int i = result.Length - 1; i >= 0; i--) {
                a[count] = result[i];
                count++;
            }
            return a;
        }
        return result;
    }

    public static int[] sortCa(int[] card, int type) {
        if (card.Length < 3) {
            return card;
        }
        List<String> a = Array2Vector(card);
        List<String> c = new List<String>();
        if (type == 0) {
            for (int i = 0; i < card.Length; i++) {
                if (!isInCaNgang(i, card)) {
                    c.Add(card[i] + "");
                    a.Remove(card[i] + "");
                }
            }
        } else {
            for (int i = 0; i < card.Length; i++) {
                if (!isInCaDoc(i, card)) {
                    c.Add(card[i] + "");
                    a.Remove(card[i] + "");
                }
            }
            int[] b = sortType(vector2Array(a));
            a = Array2Vector(b);

        }

        for (int i = 0; i < c.Count; i++) {
            a.Add(c[i]);
        }
        int[] d = new int[a.Count];
        for (int i = 0; i < a.Count; i++) {
            d[i] = int.Parse(a[i]);
        }
        return d;
    }

    public static bool isInCaNgang(int id, int[] card) {
        for (int i = 0; i < card.Length; i++) {
            if (abs(i - id) <= 1 && i != id) {
                if (card[id] % 13 == card[i] % 13) {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool isInCaDoc(int id, int[] card) {
        for (int i = 0; i < card.Length; i++) {
            if (abs(i - id) <= 2 && i != id) {
                if (card[i] / 13 == card[id] / 13
                        && abs(card[i] % 13 - card[id] % 13) <= 2) {
                    return true;
                }
            }
        }
        return false;
    }

    private static int getNumCardwin(int[] src, int[] cardWin) {
        int num = 0;
        try {
            for (int i = 0, n = src.Length; i < n; i++) {
                for (int j = 0; j < cardWin.Length; j++) {
                    if (src[i] == cardWin[j]) {
                        num++;
                    }
                }
            }
        } catch (Exception e) {
        }
        return num;
    }

    public static bool validPhom(int[][] phom, int[] cardWin) {
        int l = 0;
        for (int i = 0; i < phom.Length; i++) {
            l += phom[i].Length;
            if (!isTrip(phom[i]) && !isStraight(phom[i])) {
                return false;
            }
        }
        if (cardWin != null) {
            int len = 0;
            int ncw = 0;
            for (int i = 0; i < phom.Length; i++) {
                len += phom[i].Length;
                for (int j = 0; j < cardWin.Length; j++) {
                    if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                        ncw++;
                    }
                }
            }
            if (ncw < cardWin.Length || ncw == 0) {
                return false;
            }
            if (len < l) {
                return false;
            }
            List<string> pos = new List<string>();
            for (int i = 0; i < phom.Length; i++) {
                int ndup = 0;
                pos.Clear();
                if (checkTrip(phom[i])) {
                    for (int j = 0; j < cardWin.Length; j++) {
                        if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                            ndup++;
                        }
                    }
                    if (ndup > 1) {
                        return false;
                    }
                } else {
                    if (phom[i].Length < 6) {
                        for (int j = 0; j < cardWin.Length; j++) {
                            if (checkExistCardInPhom(phom[i], cardWin[j]) != -1) {
                                ndup++;
                            }
                        }
                        if (ndup > 1) {
                            return false;
                        }
                    } else if (phom[i].Length == 6) {
                        for (int j = 0; j < phom[i].Length; j++) {
                            for (int k = 0; k < cardWin.Length; k++) {
                                if (phom[i][j] == cardWin[k]) {
                                    pos.Add(j + "");
                                    ndup++;
                                }
                            }
                        }
                        if (ndup == 3) {
                            return false;
                        }
                        if (ndup > 1) {
                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            if ((pos1 < 3 && pos2 < 3)
                                    || (pos1 >= 3 && pos2 >= 3)) {
                                return false;
                            }
                        }
                    } else if (phom[i].Length > 6) {
                        for (int j = 0; j < phom[i].Length; j++) {
                            for (int k = 0; k < cardWin.Length; k++) {
                                if (phom[i][j] == cardWin[k]) {
                                    pos.Add(j + "");
                                    ndup++;
                                }
                            }
                        }
                        if (ndup == 3) {
                            if (phom[i].Length < 9) {
                                return false;
                            }
                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            int pos3 = int.Parse(pos[2]);
                            if ((pos1 < 3 && pos2 < 3)
                                    || (pos1 >= 3 && pos1 < 6 && pos2 < 6 && pos2 >= 3)
                                    || (pos1 >= 6 && pos2 >= 6)) {
                                return false;
                            }
                            if ((pos1 < 3 && pos3 < 3)
                                    || (pos1 >= 3 && pos1 < 6 && pos3 < 6 && pos3 >= 3)
                                    || (pos1 >= 6 && pos3 >= 6)) {
                                return false;
                            }
                            if ((pos2 < 3 && pos3 < 3)
                                    || (pos3 >= 3 && pos3 < 6 && pos2 < 6 && pos2 >= 3)
                                    || (pos3 >= 6 && pos2 >= 6)) {
                                return false;
                            }
                        } else if (ndup > 1) {
                            int pos1 = int.Parse(pos[0]);
                            int pos2 = int.Parse(pos[1]);
                            if ((pos1 < 3 && pos2 < 3)) {
                                return false;
                            }
                            if (phom[i].Length == 7) {
                                if ((pos1 >= 4 && pos2 >= 4)) {
                                    return false;
                                }
                            } else if (phom[i].Length == 8) {
                                if ((pos1 >= 5 && pos2 >= 5)) {
                                    return false;
                                }
                            } else if (phom[i].Length == 9) {
                                if ((pos1 >= 6 && pos2 >= 6)) {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }
}
