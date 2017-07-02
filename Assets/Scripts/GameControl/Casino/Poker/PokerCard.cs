using UnityEngine;
using System.Collections;

public class PokerCard {

    protected static int getValue (int i) {
        return Card.cardPaint[i] % 13 + 1;
    }

    protected static int getType (int i) {
        return Card.cardPaint[i] / 13;
    }

    protected static bool isContain (int[] a, int v) {
        if(a == null) {
            return false;
        }
        for(int i = 0; i < a.Length; i++) {
            if(a[i] == v) {
                return true;
            }
        }
        return false;
    }

    protected static int[] subArray (int[] a, int b) {
        if(isContain (a, b)) {
            int[] remain = new int[a.Length - 1];
            int k = 0;
            for(int i = 0; i < a.Length; i++) {
                if(b != a[i]) {
                    remain[k++] = a[i];
                }
            }
            return remain;
        } else {
            return a;
        }
    }

    public static int[] sortCaoThap (int[] arr) {
        // 0-51
        int[] turn = arr;
        int length = turn.Length;
        for(int i = 0; i < length - 1; i++) {
            int min = i;
            for(int j = i + 1; j < length; j++) {
                if(!(((getValue (turn[j]) < getValue (turn[min])) || getValue (turn[min]) == 1) && getValue (turn[j]) != 1)) {
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

    public static int[] sortValue (int[] arr) {// mang cac so thu tu quan bai tu
        // 0-51
        int[] turn = arr;
        int length = turn.Length;
        for(int i = 0; i < length - 1; i++) {
            int min = i;
            for(int j = i + 1; j < length; j++) {
                if(((getValue (turn[j]) < getValue (turn[min])) || getValue (turn[min]) == 1) && getValue (turn[j]) != 1) {
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

    public static int getTypeOfCardsPoker (int[] cards) {
        return getTypeOfCards (sortValue (cards));
    }

    public static int getTypeOfCards (int[] cards) {
        int[] y = new int[] { 0, 0, 0 };
        int count = 0;
        int a = 0;
        int typeofcard = 0;
        int card_temp = -1;
        for(int i = 0, n = cards.Length; i < n; i++) {
            if(card_temp == -1) {
                card_temp = cards[i];
                continue;
            } else if(getValue (card_temp) == getValue (cards[i])) {
                if(y[count] == 1) {
                    y[count] += 5;
                } else if(y[count] == 6) {
                    y[count] += 14;
                } else {
                    y[count]++;
                }
                a = 1;
            } else {
                card_temp = cards[i];
                if(a == 1) {
                    count++;
                    a = 0;
                }
            }
        }
        int valuenow = y[0] + y[1] + y[2];
        if(valuenow < 1) {
            typeofcard = checkThung (cards);
        } else if(0 < valuenow && valuenow < 2) {
            // 1 doi
            typeofcard = checkThung (cards);
            if(typeofcard != 0) {
            } else {
                typeofcard = 1;
            }
        } else if(1 < valuenow && valuenow < 4) {
            // 2 doi
            typeofcard = checkThung (cards);
            if(typeofcard != 0) {
            } else {
                typeofcard = 2;
            }
        } else if(valuenow == 6) {
            // sam co
            typeofcard = checkThung (cards);
            if(typeofcard != 0) {
            } else {
                typeofcard = 3;
            }
        } else if(6 < valuenow && valuenow < 13) {
            typeofcard = 6;
        } else {
            typeofcard = 7;
        }
        return typeofcard;
    }

    public static int checkThung (int[] cards) {
        int[] y = new int[4];
        int x = -1;
        int[] cardThung = null;
        for(int i = 0; i < cards.Length; i++) {

            switch(getType (cards[i])) {
                case 0:
                    y[0]++;
                    if(y[0] > 4) {
                        x = 0;
                    }
                    break;
                case 1:
                    y[1]++;
                    if(y[1] > 4) {
                        x = 1;
                    }
                    break;
                case 2:
                    y[2]++;
                    if(y[2] > 4) {
                        x = 2;
                    }
                    break;
                case 3:
                    y[3]++;
                    if(y[3] > 4) {
                        x = 3;
                    }
                    break;
                default:
                    break;
            }
        }
        if(x != -1) {
            for(int i = 0, n = cards.Length; i < n; i++) {
                if(getType (cards[i]) == x) {
                    cardThung = RTL.insertArray (cardThung, cards[i]);
                }
            }
            if(checkSanhPK (cardThung) && cardThung.Length > 4) {
                return 8;
            }
            return 5;
        } else {
            if(checkSanhPK (cards)) {
                return 4;
            }
        }
        return 0;
    }

    public static bool checkSanhPK (int[] cards) {
        int[] cardS = null;
        int cardTemp = -1;
        int cardsSize = cards.Length;
        for(int i = 0; i < cardsSize; i++) {
            if(cardTemp == -1) {
                cardTemp = cards[i];
                cardS = RTL.insertArray (cardS, cardTemp);
                continue;
            } else if(i >= 4 && getValue (cardTemp) == 5) {
                if(getValue (cards[i]) == 1 && getValue (cardS[0]) != 1) {
                    cardS = RTL.insertArray (cardS, cards[i]);
                } else if(getValue (cards[i]) == 6) {
                    cardS = RTL.insertArray (cardS, cards[i]);
                    cardTemp = cards[i];
                }
            } else if((getValue (cardTemp) == 13 ? 0 : getValue (cardTemp)) + 1 == getValue (cards[i])) {
                cardS = RTL.insertArray (cardS, cards[i]);
                cardTemp = cards[i];
            } else if(getValue (cardTemp) == getValue (cards[i])) {
                cardS = subArray (cardS, cardTemp);
                cardS = RTL.insertArray (cardS, cards[i]);
                cardTemp = cards[i];
            } else {
                if(cardS.Length <= 2) {
                    cardS = null;
                    cardS = RTL.insertArray (cardS, cards[i]);
                    cardTemp = cards[i];
                } else if(cardS.Length < 5) {
                    break;
                }
            }
        }
        if(cardS.Length >= 5) {
            return true;
        }
        return false;
    }

    public static int[] add2ArrayCard (int[] cards1, int[] cards2) {
        int[] card;
        int lenght = 0;
        if(cards1 == null && cards2 == null) {
            return new int[] { };
        } else if(cards1 == null) {
            lenght = cards2.Length;
            card = new int[lenght];
            for(int i = 0; i < cards2.Length; i++) {
                card[i] = cards2[i];
            }
        } else if(cards2 == null) {
            lenght = cards1.Length;
            card = new int[lenght];
            for(int i = 0; i < cards1.Length; i++) {
                card[i] = cards1[i];
            }
        } else {
            lenght = cards1.Length + cards2.Length;
            card = new int[lenght];
            for(int i = 0; i < cards1.Length; i++) {
                card[i] = cards1[i];
            }
            for(int i = 0; i < cards2.Length; i++) {
                card[i + cards1.Length] = cards2[i];
            }
        }
        return card;
    }

    public static bool isBiggerArray (int[] array1, int[] array2) {
        bool compar = false;

        // false 1 nhỏ hay bằng hơn 2;
        // true 1 lớn hơn 2

        if(getTypeOfCardsPoker (array1) > getTypeOfCardsPoker (array2)) {
            return true;
        }
        if(getTypeOfCardsPoker (array1) < getTypeOfCardsPoker (array2)) {
            return false;
        }

        // mau thau || thung
        if(getTypeOfCardsPoker (array1) == 0 || getTypeOfCardsPoker (array1) == 5) {

            int[] array1s = sortCaoThap (array1);
            int[] array2s = sortCaoThap (array2);
            return isBig (array1s, array2s);

        } else if(getTypeOfCardsPoker (array1) == 1 || getTypeOfCardsPoker (array1) == 3 || getTypeOfCardsPoker (array1) == 7) {
            int[] array1s = sortCaoThap (array1);
            int[] array2s = sortCaoThap (array2);
            int doi1 = 0, doi2 = 0;
            for(int i = 0; i < array1s.Length - 1; i++) {
                if(getValue (array1s[i]) == getValue (array1s[i + 1])) {
                    doi1 = getValue (array1s[i]);
                    break;
                }
            }

            for(int i = 0; i < array2s.Length - 1; i++) {
                if(getValue (array2s[i]) == getValue (array2s[i + 1])) {
                    doi2 = getValue (array2s[i]);
                    break;
                }
            }
            if((doi1 > doi2 && doi2 != 1) || (doi1 == 1 && doi2 != 1)) {
                return true;
            } else if((doi1 < doi2 && doi1 != 1) || (doi2 == 1 && doi1 != 1)) {
                return false;
            } else {
                return isBig (array1s, array2s);
            }
        } else if(getTypeOfCardsPoker (array1) == 6) {
            int[] array1s = sortCaoThap (array1);
            int[] array2s = sortCaoThap (array2);
            int ba1 = 0, ba2 = 0;
            for(int i = 0; i < array1s.Length - 2; i++) {
                if(getValue (array1s[i]) == getValue (array1s[i + 1]) && (getValue (array1s[i + 1]) == getValue (array1s[i + 2]))) {
                    ba1 = getValue (array1s[i]);
                    break;
                }
            }

            for(int i = 0; i < array2s.Length - 2; i++) {
                if(getValue (array2s[i]) == getValue (array2s[i + 1]) && (getValue (array2s[i + 1]) == getValue (array2s[i + 2]))) {
                    ba2 = getValue (array2s[i]);
                    break;
                }
            }
            if((ba1 > ba2 && ba2 != 1) || (ba1 == 1 && ba2 != 1)) {
                return true;
            } else if((ba1 < ba2 && ba1 != 1) || (ba2 == 1 && ba1 != 1)) {
                return false;
            } else {
                return isBig (array1s, array2s);
            }
        } else if(getTypeOfCardsPoker (array1) == 4 || (getTypeOfCardsPoker (array1) == 8)) {

            int[] array1s = sortCaoThap (array1);
            int[] array2s = sortCaoThap (array2);

            if(getValue (array1s[0]) == 1 && getValue (array1s[1]) == 2) {
                return false;
            } else if(getValue (array1s[0]) == 1 && getValue (array1s[1]) == 13) {
                if(getValue (array2s[0]) == 1 && getValue (array2s[1]) == 13) {
                    return false;
                } else {
                    return true;
                }
            } else if(getValue (array2s[0]) == 1 && getValue (array2s[1]) == 5) {// 15432
                if(getValue (array1s[0]) == 1 && getValue (array1s[1]) == 5) {
                    return false;
                } else {
                    return true;
                }

            } else if(getValue (array1s[0]) == 1 && getValue (array1s[1]) == 5) {// 15432
                return false;
            } else if(getValue (array2s[0]) == 1 && getValue (array2s[1]) == 13) {
                return false;
            } else {
                return isBig (array1s, array2s);
            }
        } else if(getTypeOfCardsPoker (array1) == 2) {
            int[] array1s = sortCaoThap (array1);
            int[] array2s = sortCaoThap (array2);

            int doi11 = -1, doi12 = -1;
            int doi21 = -1, doi22 = -1;
            for(int i = 0; i < array1s.Length - 1; i++) {
                if(getValue (array1s[i]) == getValue (array1s[i + 1])) {
                    if(doi11 == -1) {
                        doi11 = getValue (array1s[i]);
                    } else if(doi12 == -1) {
                        doi12 = getValue (array1s[i]);
                    }
                    break;
                }
            }

            for(int i = 0; i < array2s.Length - 1; i++) {
                if(getValue (array2s[i]) == getValue (array2s[i + 1])) {
                    if(doi21 == -1) {
                        doi21 = getValue (array2s[i]);
                    } else if(doi22 == -1) {
                        doi22 = getValue (array2s[i]);
                    }
                    break;
                }
            }

            if((doi11 > doi21 && doi21 != 1) || (doi11 == 1 && doi21 != 1)) {
                return true;
            } else if((doi11 < doi21 && doi11 != 1) || (doi21 == 1 && doi11 != 1)) {
                return false;
            } else {
                if((doi12 > doi22 && doi22 != 1) || (doi12 == 1 && doi22 != 1)) {
                    return true;
                } else if((doi12 < doi22 && doi12 != 1) || (doi22 == 1 && doi12 != 1)) {
                    return false;
                } else {
                    return isBig (array1s, array2s);
                }

            }

        }
        return compar;
    }

    public static bool isBig (int[] array1s, int[] array2s) {
        if(array1s.Length > array2s.Length) {
            for(int i = 0; i < array2s.Length; i++) {
                if((getValue (array1s[i]) > getValue (array2s[i]) && (getValue (array2s[i]) != 1))
                        || (getValue (array1s[i]) == 1 && getValue (array2s[i]) != 1)) {
                    return true;
                } else if((getValue (array1s[i]) < getValue (array2s[i]) && (getValue (array1s[i]) != 1))
                        || (getValue (array2s[i]) == 1 && getValue (array1s[i]) != 1)) {
                    return false;
                }
            }
            return false;
        } else {
            for(int i = 0; i < array1s.Length; i++) {
                if((getValue (array1s[i]) > getValue (array2s[i]) && (getValue (array2s[i]) != 1))
                        || (getValue (array1s[i]) == 1 && getValue (array2s[i]) != 1)) {
                    return true;
                } else if((getValue (array1s[i]) < getValue (array2s[i]) && (getValue (array1s[i]) != 1))
                        || (getValue (array2s[i]) == 1 && getValue (array1s[i]) != 1)) {
                    return false;
                }
            }
            return false;
        }
    }
}
