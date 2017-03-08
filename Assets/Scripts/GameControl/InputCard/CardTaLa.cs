using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CardTaLa : MonoBehaviour {
    public ArrayCard ArrayCardHand;
    public ArrayCard[] ArrayCardPhom;
    public ArrayCard ArrayCardFire;

    const float CONST_DUR = 0.4f;

    int indexPhomHa = 0;
    int indexPhomAn = 0;
    public void Init(bool isTao) {
        ArrayCardHand.CardCount = 10;
        if (isTao) {
            ArrayCardHand.MaxWidth = 10 * 60;
            ArrayCardHand.isSmall = false;
            ArrayCardHand.isTouched = true;
        } else {
            ArrayCardHand.MaxWidth = 10 * 40;
            ArrayCardHand.isSmall = true;
            ArrayCardHand.isTouched = false;
        }

        ArrayCardHand.Init();
        ArrayCardHand.SetPositionCardInArray();
        for (int i = 0; i < ArrayCardPhom.Length; i++) {
            ArrayCardPhom[i].CardCount = 10;
            ArrayCardPhom[i].MaxWidth = 10 * 40;
            ArrayCardPhom[i].isSmall = true;
            ArrayCardPhom[i].isTouched = false;

            ArrayCardPhom[i].Init();
            ArrayCardPhom[i].SetPositionCardInArray();
        }

        ArrayCardFire.CardCount = 4;
        ArrayCardFire.MaxWidth = 4 * 40;
        ArrayCardFire.isSmall = true;
        ArrayCardFire.isTouched = false;

        ArrayCardFire.Init();
        ArrayCardFire.SetPositionCardInArray();
    }

    public void SetChiaBai(int[] arrCard, bool isTao, UnityAction callback = null) {
        ArrayCardHand.ResetCard(true);
        ArrayCardPhom[0].ResetCard(true);
        ArrayCardPhom[1].ResetCard(true);
        ArrayCardPhom[2].ResetCard(true);
        ArrayCardFire.ResetCard(true);

        ArrayCardHand.SetActiveCardHand();
        ArrayCardPhom[0].SetActiveCardHand();
        ArrayCardPhom[1].SetActiveCardHand();
        ArrayCardPhom[2].SetActiveCardHand();
        ArrayCardFire.SetActiveCardHand();

        ArrayCardHand.SetCardWithId53();
        ArrayCardPhom[0].SetCardWithId53();
        ArrayCardPhom[1].SetCardWithId53();
        ArrayCardPhom[2].SetCardWithId53();
        ArrayCardFire.SetCardWithId53();

        ArrayCardHand.ChiaBaiTienLen(arrCard, isTao, callback);
        indexPhomHa = 0;
        indexPhomAn = 0;
    }

    public void OnFireCard(int idCard, bool isTao) {
        if (isTao) {
            Card cardchuanbidanh = ArrayCardHand.GetCardbyIDCard(idCard);
            if (cardchuanbidanh == null) return;

            Card carddanh = GetCardOnArrayCard(ArrayCardFire);
            carddanh.SetCardWithId(idCard);
            carddanh.transform.localPosition = ArrayCardFire.GetPositonCardActive();
            Vector3 vtFrom = ArrayCardFire.transform.InverseTransformPoint(cardchuanbidanh.transform.position);
            StartCoroutine(carddanh.MoveFrom(vtFrom, CONST_DUR, 0, () => {
                ArrayCardFire.SortCardActive();
            }));
            cardchuanbidanh.SetVisible(false);

            ArrayCardHand.SortCardActive();
        } else {
            Card cardchuanbidanh = GetCardOnArrayCard(ArrayCardHand);
            Card carddanh = GetCardOnArrayCard(ArrayCardFire);
            carddanh.SetCardWithId(idCard);

            Vector3 vtFrom = ArrayCardFire.transform.InverseTransformPoint(cardchuanbidanh.transform.position);
            StartCoroutine(carddanh.MoveFrom(vtFrom, CONST_DUR, 0, () => {
                ArrayCardFire.SortCardActive();
            }));
            cardchuanbidanh.SetVisible(false);
        }
    }
    public void SetEatCard(int idCardEat, bool isTao, Card cardAnCuaThangkhac) {
        if (isTao) {
            Card cAn = GetCardOnArrayCard(ArrayCardHand);
            cAn.ResetCard(true);
            cAn.SetCardWithId(idCardEat);

            Vector3 vtFrom = ArrayCardHand.transform.InverseTransformPoint(cardAnCuaThangkhac.transform.position);
            cardAnCuaThangkhac.SetVisible(false);
            StartCoroutine(cAn.MoveFrom(vtFrom, CONST_DUR, 0, () => {
                //cAn.isCardAnnnnn = true;
                //cAn.SetActiveBorder(true);
                ArrayCardHand.SortCardActive();
                isSortOderBy = 1;
            }));
        } else {
            ArrayCard arr = ArrayCardPhom[indexPhomAn];
            Card cAn = GetCardOnArrayCard(arr);//sua
            cAn.transform.localPosition = arr.GetPositonCardActive();
            cAn.SetCardWithId(idCardEat);

            Vector3 vtFrom = arr.transform.InverseTransformPoint(cardAnCuaThangkhac.transform.position);
            cardAnCuaThangkhac.SetVisible(false);
            StartCoroutine(cAn.MoveFrom(vtFrom, CONST_DUR, 0, () => {
                arr.SortCardActive();
            }));
        }
        indexPhomAn++;
    }
    public int GetScoreEatCard() {
        if (NumCardFire() >= 3) {
            return 4;
        }

        if (indexPhomAn == 0) return 1;
        if (indexPhomAn == 1) return 2;

        return 4;
    }

    public void BocBai(int idCard, bool isTao) {
        //if (isTao) {
        Card c = GetCardOnArrayCard(ArrayCardHand);
        if (isTao)
            c.SetCardWithId(idCard);
        else
            c.SetCardWithId(53);
        Vector3 vt = ArrayCardHand.transform.InverseTransformPoint(ArrayCardHand.vtPosCenter);
        StartCoroutine(c.MoveFrom(vt, CONST_DUR, 0, () => {
            if (isTao) {
                c.ResetCard(true);
                //c.transform.SetAsLastSibling();
                ArrayCardHand.ResetCard();
                ArrayCardHand.SortCardActive();
                isSortOderBy = 1;
            }
        }));
        //}
    }
    public void ChuyenBai(int idCard, PhomPlayer playerTu) {
        Card cTu = playerTu.cardTaLaManager.ArrayCardFire.GetCardbyIDCard(idCard);
        Card cDen = GetCardOnArrayCard(ArrayCardFire);
        cDen.SetCardWithId(cTu.ID);

        Vector3 vtFrom = ArrayCardFire.transform.InverseTransformPoint(cTu.transform.position);
        StartCoroutine(cDen.MoveFrom(vtFrom, CONST_DUR, 0));
        cTu.SetVisible(false);
    }
    public void GuiBai(int[] idCards, PhomPlayer playerTu, bool isTao) {
        StartCoroutine(WaitGuiBai(idCards, playerTu, isTao));
    }
    IEnumerator WaitGuiBai(int[] idCards, PhomPlayer playerTu, bool isTao) {
        List<int> Phom1 = new List<int>();
        for (int i = 0; i < ArrayCardPhom[0].listCardHand.Count; i++) {
            Card c = ArrayCardPhom[0].listCardHand[i];
            if (c.isBatHayChua) {
                Phom1.Add(c.ID);
            }
        }

        for (int i = 0; i < idCards.Length; i++) {
            Phom1.Add(idCards[i]);
            Card cTu;
            if (isTao) {
                cTu = playerTu.cardTaLaManager.ArrayCardHand.GetCardbyIDCard(idCards[i]);
            } else {
                cTu = playerTu.cardTaLaManager.GetCardOnArrayCard(playerTu.cardTaLaManager.ArrayCardHand);
            }
            cTu.SetVisible(false);
            int indexP = 0;
            if (!AutoChooseCardTaLa.CheckPhom(Phom1.ToArray())) {
                Phom1.Remove(idCards[i]);
                indexP = 1;
            }
            ArrayCard arr = ArrayCardPhom[indexP];
            Card cDen = GetCardOnArrayCard(arr);
            cDen.transform.localPosition = arr.GetPositonCardActive();
            cDen.setSmall(!isTao);
            cDen.SetCardWithId(idCards[i]);
            Vector3 vtFrom = arr.transform.InverseTransformPoint(isTao ? cTu.transform.position : playerTu.cardTaLaManager.ArrayCardHand.transform.position);
            StartCoroutine(cDen.MoveFrom(vtFrom, CONST_DUR, 0, () => {
                arr.SortCardActive();
                cDen.setSmall(true);
            }));
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void SetCardKhiHetGame(int[] arrCards) {
        ArrayCardHand.ResetCard(true);
        int[] temp = arrCards.Except(CardPhom).ToArray();
        ArrayCardHand.SetCardKhiKetThucGame(temp);
    }
    List<int> CardPhom = new List<int>();
    public void HaBai(int[] idCards, bool isTao, List<int> CardAn) {
        CardPhom.Clear();
        CardPhom.AddRange(idCards);
        if (isTao) {
            ArrayCard arr = ArrayCardPhom[indexPhomHa];
            for (int i = 0; i < arr.listCardHand.Count; i++) {
                Card cc = arr.listCardHand[i];
                cc.ResetCard(true);
                if (i < idCards.Length) {
                    Card cHa = ArrayCardHand.GetCardbyIDCard(idCards[i]);
                    cc.transform.localPosition = arr.GetPositonCardActive();
                    cc.SetCardWithId(cHa.ID);
                    Vector3 vt = arr.transform.InverseTransformPoint(cHa.transform.position);
                    cHa.SetVisible(false);
                    if (i < idCards.Length - 1) {
                        StartCoroutine(cc.MoveFrom(vt, CONST_DUR, 0));
                    } else {
                        StartCoroutine(cc.MoveFrom(vt, CONST_DUR, 0, () => {
                            arr.SortCardActive();
                            ArrayCardHand.SortCardActive();
                        }));
                    }
                    cc.SetVisible(true);
                    if (CardAn.Contains(cc.ID)) {
                        //cc.SetActiveBorder(true);
                    }
                } else {
                    cc.SetVisible(false);
                }
            }
        } else {
            ArrayCard arr = ArrayCardPhom[indexPhomHa];
            for (int i = 0; i < arr.listCardHand.Count; i++) {
                Card cc = arr.listCardHand[i];
                cc.ResetCard(true);
                if (i < idCards.Length) {
                    //cc.SetVisible(true);
                    cc.transform.localPosition = arr.GetPositonCardActive();
                    cc.SetCardWithId(idCards[i]);
                    Vector3 vt = arr.transform.InverseTransformPoint(ArrayCardHand.transform.position);
                    if (i == idCards.Length - 1) {
                        StartCoroutine(cc.MoveFrom(vt, CONST_DUR, 0, () => {
                            arr.SortCardActive();
                        }));
                    } else {
                        StartCoroutine(cc.MoveFrom(vt, CONST_DUR, 0));
                    }
                    //if (CardAn.Contains(cc.ID)) {
                    //    cc.SetActiveBorder(true);
                    //}
                } else {
                    cc.SetVisible(false);
                }
            }
        }

        indexPhomHa++;
    }
    #region Sap Xep
    Dictionary<int, Vector3> oldPosition = new Dictionary<int, Vector3>();
    int isSortOderBy = 1;
    public void SortCard(List<int> CardAn) {
        oldPosition.Clear();
        List<int> arr_id = new List<int>();
        for (int i = 0; i < ArrayCardHand.listCardHand.Count; i++) {
            Card c = ArrayCardHand.listCardHand[i];
            if (c.isBatHayChua)
                arr_id.Add(c.ID);

            Vector3 position = c.transform.localPosition;
            if (!oldPosition.ContainsKey(c.ID))
                oldPosition.Add(c.ID, position);
        }
        int[] arrSorted = AutoChooseCardTaLa.SortCardTaLa(arr_id.ToArray(), CardAn, ref isSortOderBy);
        Vector3 newPos, oldPos;
        for (int j = 0; j < ArrayCardHand.listCardHand.Count; j++) {
            Card c = ArrayCardHand.listCardHand[j];
            if (j < arrSorted.Length) {
                c.SetVisible(true);
                c.ResetCard(true);
                newPos = c.transform.localPosition;
                oldPos = oldPosition[arrSorted[j]];

                c.SetCardWithId(arrSorted[j]);
                if (CardAn.Contains(c.ID)) {
                    //c.isCardAnnnnn = true;
                    //c.SetActiveBorder(true);
                } else {
                    //c.isCardAnnnnn = false;
                    //c.SetActiveBorder(false);
                }
                c.transform.localPosition = oldPos;
                c.transform.DOLocalMoveX(newPos.x, 0.2f);
                c.transform.SetSiblingIndex(j);
            } else {
                c.SetVisible(false);
            }
        }

        StartCoroutine(ChoXepLai());
    }
    IEnumerator ChoXepLai() {
        yield return new WaitForSeconds(0.22f);
        ArrayCardHand.SortCardActive();
    }
    #endregion
    /// <summary>
    /// Lay 1 quan bai bi an hoac id = 53 tu 1 array card
    /// </summary>
    public Card GetCardOnArrayCard(ArrayCard arr) {
        for (int i = 0; i < arr.listCardHand.Count; i++) {
            Card c = arr.listCardHand[i];
            if (!c.isBatHayChua) {
                return c;
            }
        }
        for (int i = 0; i < arr.listCardHand.Count; i++) {
            Card c = arr.listCardHand[i];
            if (c.ID == 53) {
                return c;
            }
        }

        return arr.AddAndGetCardOnArray();
    }
    public int[] GetCardIdCardHand() {
        List<int> list = new List<int>();
        for (int i = 0; i < ArrayCardHand.listCardHand.Count; i++) {
            Card c = ArrayCardHand.listCardHand[i];
            if (c.isBatHayChua) {
                list.Add(c.ID);
            }
        }
        return list.ToArray();
    }
    public void SetDragDropCard() {
        ArrayCardHand.SetIsCardDragDrop(true);
    }
    public void SapXepLaiBaiTrenTay() {
        ArrayCardHand.SortCardActive();
    }
    public void SetActiveCardHand() {
        ArrayCardHand.SetActiveCardHand();
        for (int i = 0; i < ArrayCardPhom.Length; i++) {
            ArrayCardPhom[i].SetActiveCardHand();
        }
        ArrayCardFire.SetActiveCardHand();
    }
    #region SET POSITION
    public void SetPositionArryCard(Align_Anchor alignHand, Align_Anchor alignPhom, Align_Anchor alignFire, int sit) {
        ArrayCardHand.align_Anchor = alignHand;
        if (sit == 0)
            ArrayCardHand.SetPositonCardHandTaLa();
        else {
            ArrayCardHand.SetPositonCardHand();
        }
        for (int i = 0; i < ArrayCardPhom.Length; i++) {
            ArrayCardPhom[i].align_Anchor = alignPhom;
            ArrayCardPhom[i].SetPositonCardHand();
        }
        ArrayCardFire.align_Anchor = alignFire;
        ArrayCardFire.SetPositonCardHand();

        switch (sit) {
            case 0:
                SetDefaultPositionMe();
                break;
            case 1:
            case 3:
                SetDefaultPosition_1_3(sit == 1);
                break;
            case 2:
                SetDefaultPosition_2();
                break;
        }
    }

    void SetDefaultPositionMe() {
        //Vector3 vtPos = ArrayCardHand.transform.localPosition;
        //vtPos.x += 160;
        //ArrayCardHand.transform.localPosition = vtPos;

        Vector3 vtPos = ArrayCardPhom[0].transform.localPosition;
        vtPos.x = -230;
        vtPos.y = -40;
        ArrayCardPhom[0].transform.localPosition = vtPos;
        vtPos.y = 0;
        ArrayCardPhom[1].transform.localPosition = vtPos;
        vtPos.y = 40;
        ArrayCardPhom[2].transform.localPosition = vtPos;

        vtPos = ArrayCardFire.transform.localPosition;
        vtPos.y = 150;
        ArrayCardFire.transform.localPosition = vtPos;

    }
    void SetDefaultPosition_1_3(bool isOne) {
        Vector3 vtPos = ArrayCardPhom[0].transform.localPosition;
        vtPos.x = isOne ? 70 : -60;
        vtPos.y = 110;
        ArrayCardPhom[0].transform.localPosition = vtPos;
        vtPos.y = 150;
        ArrayCardPhom[1].transform.localPosition = vtPos;
        vtPos.y = 190;
        ArrayCardPhom[2].transform.localPosition = vtPos;

        vtPos = ArrayCardFire.transform.localPosition;
        vtPos.x = isOne ? -180 : 180;
        vtPos.y = 0;
        ArrayCardFire.transform.localPosition = vtPos;
    }
    void SetDefaultPosition_2() {
        Vector3 vtPos = ArrayCardPhom[0].transform.localPosition;
        vtPos.x = -110;
        vtPos.y = -40;
        ArrayCardPhom[0].transform.localPosition = vtPos;
        vtPos.y = 0;
        ArrayCardPhom[1].transform.localPosition = vtPos;
        vtPos.y = 40;
        ArrayCardPhom[2].transform.localPosition = vtPos;

        vtPos = ArrayCardFire.transform.localPosition;
        vtPos.x = 0;
        vtPos.y = -150;
        ArrayCardFire.transform.localPosition = vtPos;
    }
    #endregion
    public int[] GetCardChooseInHand() {
        List<int> list = new List<int>();
        for (int i = 0; i < ArrayCardHand.listCardHand.Count; i++) {
            Card c = ArrayCardHand.listCardHand[i];
            if (c.isBatHayChua && c.IsChoose) {
                list.Add(c.ID);
            }
        }

        return list.ToArray();
    }
    public int NumCardFire() {
        int numC = 0;
        for (int i = 0; i < ArrayCardFire.CardCount; i++) {
            Card c = ArrayCardFire.GetCardbyIndex(i);
            if (c.ID != 53 && c.isBatHayChua) {
                numC++;
            }
        }
        return numC;
    }
    //demo
    public void SetCardAn(int cardAn, int indexPhom) {
        ArrayCardPhom[indexPhom].SetActiveCardWithArrID(new int[] { cardAn });
    }
}
