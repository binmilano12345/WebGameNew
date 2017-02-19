
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppConfig;
using System;
using System.Linq;
using UnityEngine.Events;
using DG.Tweening;

public enum Align_Anchor {
    NONE,
    LEFT,
    RIGHT,
    CENTER,
    TOP,
    BOT
};
public class ArrayCard : MonoBehaviour {
    public Align_Anchor align_Anchor = Align_Anchor.LEFT;
    public float MaxWidth;
    public int CardCount;
    public bool isSmall = false;
    public bool isTouched = true;
    [HideInInspector]
    public List<Card> listCardHand;
    public List<int> listIdCardHand;
    Vector3 vtPosCenter;
    float w_card, h_card;

    public void Init() {
        listCardHand = new List<Card>();
        listIdCardHand = new List<int>();

        //Debug.LogError(vtPosCenter);
        if (GameControl.instance.objCard != null) {
            GameObject objCard = GameControl.instance.objCard;
            for (int i = 0; i < CardCount; i++) {
                GameObject obj = Instantiate(objCard);
                obj.transform.SetParent(transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                Card card = obj.GetComponent<Card>();
                card.SetCardWithId(53);
                card.setSmall(isSmall);
                card.SetTouched(isTouched);
                if (i == 0) {
                    w_card = card.W_Card;
                    h_card = card.H_Card;
                }
                card.SetVisible(false);
                listCardHand.Add(card);
            }
            //if (gameObject.transform.parent.name.Equals (ClientConfig.UserInfo.UNAME))
            //	SetPositionCardInArray ();
            //else
            //	SetLaiHetCardVeToaDo0 ();

            //Debug.LogError("KHoi tao xong");
        }
    }

    public void InitKhiVaoBanDangDanh() {
        listCardHand = new List<Card>();
        listIdCardHand = new List<int>();

        //        Debug.LogError(vtPosCenter);
        if (GameControl.instance.objCard != null) {
            GameObject objCard = GameControl.instance.objCard;
            for (int i = 0; i < CardCount; i++) {
                GameObject obj = Instantiate(objCard);
                obj.transform.SetParent(transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                Card card = obj.GetComponent<Card>();
                card.SetCardWithId(53);
                card.setSmall(isSmall);
                card.SetTouched(isTouched);
                if (i == 0) {
                    w_card = card.W_Card;
                    h_card = card.H_Card;
                }
                card.SetVisible(true);
                listCardHand.Add(card);
            }
            //if (gameObject.transform.parent.name.Equals (ClientConfig.UserInfo.UNAME))
            //	SetPositionCardInArray ();
            //else
            //	SetLaiHetCardVeToaDo0 ();
        }

        listCardHand = new List<Card>();
        listIdCardHand = new List<int>();

        //        Debug.LogError(vtPosCenter);
    }

    public void SetPositionCardInArray() {
        float disCard;
        if (MaxWidth >= listCardHand.Count * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / listCardHand.Count;
        }
        switch (align_Anchor) {
            case Align_Anchor.LEFT:
                Anchor_Left(disCard);
                break;
            case Align_Anchor.RIGHT:
                Anchor_Right(disCard);
                break;
            case Align_Anchor.CENTER:
                Anchor_Center(disCard);
                break;
        }
    }

    public void SetLaiIdCardSauKhiDanh() {
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (card != null) {
                if (!card.isBatHayChua) {
                    for (int k = i; k < listCardHand.Count; k++) {
                        Card c = listCardHand[k];
                        if (c == listCardHand[listCardHand.Count - 1]) {
                            c.SetCardWithId(53);
                            c.SetVisible(false);
                        } else {
                            if (listCardHand[k + 1].ID != 53) {
                                c.SetCardWithId(listCardHand[k + 1].ID);
                                c.SetVisible(true);
                            } else {
                                for (int l = k + 1; l < listCardHand.Count; l++) {
                                    if (listCardHand[l].ID != 53) {
                                        c.SetCardWithId(listCardHand[l].ID);
                                        c.SetVisible(true);
                                        listCardHand[l].SetCardWithId(53);
                                        listCardHand[l].SetVisible(false);
                                        break;
                                    }

                                }
                                if (c.ID == 53)
                                    c.SetVisible(false);
                            }
                        }
                    }
                } else {
                    if (card.ID == 53) {
                        for (int k = i; k < listCardHand.Count; k++) {
                            Card c = listCardHand[k];
                            if (c == listCardHand[listCardHand.Count - 1]) {
                                c.SetCardWithId(53);
                                c.SetVisible(false);
                            } else {
                                if (listCardHand[k + 1].ID != 53) {
                                    c.SetCardWithId(listCardHand[k + 1].ID);
                                    c.SetVisible(true);
                                } else {
                                    for (int l = k + 1; l < listCardHand.Count; l++) {
                                        if (listCardHand[l].ID != 53) {
                                            c.SetCardWithId(listCardHand[l].ID);
                                            c.SetVisible(true);
                                            listCardHand[l].SetCardWithId(53);
                                            listCardHand[l].SetVisible(false);
                                            break;
                                        }
                                        c.SetVisible(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetPositonCardHand() {
        switch (align_Anchor) {
            case Align_Anchor.LEFT:
                transform.localPosition = new Vector3(120, 0, 0);
                break;
            case Align_Anchor.RIGHT:
                transform.localPosition = new Vector3(-120, 0, 0);
                break;
            case Align_Anchor.CENTER:
                //Camera ccc = GetCamera();

                //if (ccc != null) {
                Vector3 vtScreen =/* ccc.ScreenToWorldPoint*/(new Vector3(Screen.width / 2, 0, 0));
                Vector3 vt = transform.parent.transform.InverseTransformPoint(vtScreen);
                vt.y = 0;
                vt.z = 0;
                transform.localPosition = vt;
                //}
                break;
        }
    }

    public void SetPositonCardHandTaLa() {
        switch (align_Anchor) {
            case Align_Anchor.LEFT:
                transform.localPosition = new Vector3(120, 0, 0);
                break;
            case Align_Anchor.RIGHT:
                transform.localPosition = new Vector3(-120, 0, 0);
                break;
            case Align_Anchor.CENTER:
                //Camera ccc = GetCamera();

                //if (ccc != null) {
                Vector3 vtScreen = /*ccc.ScreenToWorldPoint*/(new Vector3(Screen.width / 2, 0, 0));
                Vector3 vt = transform.parent.transform.InverseTransformPoint(vtScreen);
                vt.y = -32.51f;
                vt.z = 0;
                vt.x += 100;
                transform.localPosition = vt;
                //}
                break;
        }
    }
    //Camera GetCamera() {
    //    Camera ccc = null;
    //    foreach (Camera c in Camera.allCameras) {
    //        if (c.name.Equals("Camera")) {
    //            ccc = c;
    //            break;
    //        }
    //    }
    //    return ccc;
    //}
    #region ANCHOR
    void Anchor_Left(float disCard) {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].transform.localPosition = new Vector3(i * disCard, 0, 0);
            listCardHand[i].transform.SetSiblingIndex(i);
        }
    }
    void Anchor_Right(float disCard) {
        for (int i = listCardHand.Count - 1; i >= 0; i--) {
            listCardHand[i].transform.localPosition = new Vector3(-(listCardHand.Count - 1 - i) * disCard, 0, 0);
            listCardHand[i].transform.SetSiblingIndex(i);
        }
    }
    void Anchor_Center(float disCard) {

        if (listCardHand.Count % 2 == 0) {
            for (int i = 0; i < listCardHand.Count; i++) {
                listCardHand[i].transform.localPosition = new Vector3(
                    -((int)listCardHand.Count / 2 - 0.5f)
                            * disCard + i * disCard, 0, 0);
                listCardHand[i].transform.SetSiblingIndex(i);
            }
        } else {
            for (int i = 0; i < listCardHand.Count; i++) {
                listCardHand[i].transform.localPosition = new Vector3(
                    -((int)listCardHand.Count / 2) * disCard
                            + i * disCard, 0, 0);
                listCardHand[i].transform.SetSiblingIndex(i);
            }
        }

    }

    void Anchoir_Left_Card_Enable() {
        int j = 0;
        float disCard;
        if (MaxWidth >= listCardHand.Count * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / listCardHand.Count;
        }
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (card.isBatHayChua) {
                //card.transform.localPosition = new Vector3(j * disCard, 0, 0);
                StartCoroutine(card.MoveTo(new Vector3(j * disCard, 0, 0), 0.1f, j * 0.05f));
                //card.transform.SetSiblingIndex(i);
                j++;
            }
        }
    }

    void Anchoir_Right_Card_Enable() {
        //for (int i = listCardHand.Count - 1; i >= 0; i--) {
        //    listCardHand[i].transform.localPosition = new Vector3(-(listCardHand.Count - 1 - i) * disCard, 0, 0);
        //}

        int j = listCardHand.Count - 1;
        float disCard;
        if (MaxWidth >= listCardHand.Count * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / listCardHand.Count;
        }
        //for (int i = 0; i < listCardHand.Count; i++) {
        for (int i = listCardHand.Count - 1; i >= 0; i--) {
            Card card = listCardHand[i];
            if (card.isBatHayChua) {
                //StartCoroutine(card.MoveTo(new Vector3(j * disCard, 0, 0), 0.1f, j * 0.05f));
                //j--;

                StartCoroutine(card.MoveTo(new Vector3(-(j - i) * disCard, 0, 0), 0.1f, i * 0.05f));
            }
        }
    }
    void Anchoir_Center_Card_Enable() {
        int j = 0;
        float disCard;

        if (MaxWidth >= listCardHand.Count * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / listCardHand.Count;
        }
        int countBat = 0;
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (card.isBatHayChua) {
                countBat++;
            }
        }
        if (countBat % 2 == 0) {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                if (card.isBatHayChua) {
                    Vector3 vt = new Vector3(-((int)countBat / 2 - 0.5f) * disCard + j * disCard, 0, 0);
                    StartCoroutine(card.MoveTo(vt, 0.05f, j * 0.01f));
                    j++;
                }
            }
        } else {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                if (card.isBatHayChua) {
                    Vector3 vt = new Vector3(-((int)countBat / 2) * disCard + j * disCard, 0, 0);
                    StartCoroutine(card.MoveTo(vt, 0.05f, j * 0.01f));
                    j++;
                }
            }
        }


    }
    #endregion

    public int GetSizeOfListIdCardHand() {
        return listIdCardHand.Count;
    }
    public int GetSizeOfListCardHand() {
        return listCardHand.Count;
    }
    public void AddIdToListIdCardHand(int id) {
        listIdCardHand.Add(id);
    }

    public void SetListIDCard(int[] cards) {
        listIdCardHand.Clear();
        for (int i = 0; i < cards.Length; i++) {
            AddIdToListIdCardHand(cards[i]);
        }
    }

    public void SetListCardHand(Card card) {
        if (!listCardHand.Contains(card))
            listCardHand.Add(card);
    }

    //    public delegate void CallBackChiaBai();
    //	UnityAction callBackChiaBai;
    public void ChiaBaiTienLen(int[] arrcard, bool isTao, UnityAction callBack = null) {
        //Camera ccc = GetCamera();
        //if (ccc != null) {
        Vector3 vtScreen = /*ccc.ScreenToWorldPoint*/(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        vtPosCenter = transform.InverseTransformPoint(vtScreen);
        vtPosCenter.z = 0; ;
        //		Debug.LogError ("from " + vtPosCenter);
        if (isTao)
            SetPositionCardInArray();
        else
            SetLaiHetCardVeToaDo0();
        SetListIDCard(arrcard);
        for (int i = 0; i < listCardHand.Count; i++) {
            //Card c = listCardHand[i];
            listCardHand[i].SetVisible(false);
            listCardHand[i].IsChoose = false;
            if (i < arrcard.Length) {
                if (isTao) {
                    listCardHand[i].SetCardWithId(arrcard[i]);
                } else {
                    listCardHand[i].SetCardWithId(53);
                    listCardHand[i].setSmall(true);
                    listCardHand[i].SetVisible(false);
                }
                StartCoroutine(listCardHand[i].MoveFrom(vtPosCenter, 0.5f, i * 0.1f));
            } else
                listCardHand[i].SetVisible(false);
        }
        if (callBack != null) {
            callBack();
            callBack = null;
        }
        StartCoroutine(ShowCardKhiChiaXong());
        //}
    }

    IEnumerator ShowCardKhiChiaXong() {
        yield return new WaitForSeconds(listCardHand.Count * 0.1f + 1);
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (card.ID != 53)
                listCardHand[i].SetVisible(true);
        }
    }

    public void SetBaiKhiKetNoiLai(int[] arrcard, bool isTao) {
        if (isTao)
            SetPositionCardInArray();
        else
            SetLaiHetCardVeToaDo0();
        SetListIDCard(arrcard);
        for (int i = 0; i < listCardHand.Count; i++) {
            //Card c = listCardHand[i];
            listCardHand[i].SetVisible(false);
            listCardHand[i].IsChoose = false;
            if (i < arrcard.Length) {
                listCardHand[i].SetVisible(true);
                if (isTao) {
                    listCardHand[i].SetCardWithId(arrcard[i]);
                } else {
                    listCardHand[i].SetCardWithId(53);
                    listCardHand[i].setSmall(true);
                    //listCardHand[i].transform.localPosition = Vector3.zero;
                }
            }
        }
        if (isTao)
            SapXepLaiBaiSauKhiDanh();
    }

    public Card GetCardbyIDCard(int id) {
        try {
            for (int i = 0; i < GetSizeOfListCardHand(); i++) {
                if (listCardHand[i].ID == id) {
                    return listCardHand[i];
                }
            }
        } catch (Exception e) {
        }

        return null;
    }

    public Card GetCardbyIndex(int index) {
        if (index > listCardHand.Count - 1) {
            index = listCardHand.Count - 1;
        }
        if (index < 0)
            index = 0;
        return listCardHand[index];
    }

    public void ClearCardHand() {
        listIdCardHand.Clear();
        for (int i = 0; i < listCardHand.Count; i++) {
            Destroy(listCardHand[i].gameObject);
        }
        listCardHand.Clear();
    }

    public void RemoveCardById(int id) {
        listIdCardHand.Remove(id);
        listCardHand.Remove(GetCardbyIDCard(id));
    }

    public void SetCardKhiKetThucGame(int[] arrCards, int sitNumer = -1) {
        float disCard;
        int dodai = listCardHand.Count;
        if (MaxWidth >= dodai * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / dodai;
        }
        if (align_Anchor == Align_Anchor.RIGHT) {
            int j = arrCards.Length - 1;
            for (int i = listCardHand.Count - 1; i >= 0; i--) {
                Card card = listCardHand[i];
                card.SetVisible(false);
                if (j >= 0) {
                    card.SetVisible(true);
                    card.SetDarkCard(false);
                    card.SetCardWithId(arrCards[j]);
                    card.setSmall(true);
                    card.transform.localPosition = Vector3.zero;
                    card.transform.DOLocalMoveX(-(dodai - 1 - i) * disCard, .8f);
                    j--;
                    card.transform.SetSiblingIndex(i);
                }
            }
        } else if (align_Anchor == Align_Anchor.LEFT) {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                card.SetVisible(false);
                if (i < arrCards.Length) {
                    card.SetVisible(true);
                    card.SetDarkCard(false);
                    card.SetCardWithId(arrCards[i]);
                    card.setSmall(true);
                    card.transform.localPosition = Vector3.zero;
                    card.transform.DOLocalMoveX(i * disCard, .8f);
                    card.transform.SetSiblingIndex(i);
                }
            }
        } else if (align_Anchor == Align_Anchor.CENTER) {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                card.SetVisible(false);
                if (i < arrCards.Length) {
                    card.SetVisible(true);
                    card.SetCardWithId(arrCards[i]);
                }
            }
            Anchoir_Center_Card_Enable();
        }
    }

    public void SetCardWithArrID(int[] arrCards, bool isSort = true) {
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (i < arrCards.Length) {
                card.SetCardWithId(arrCards[i]);
            }
        }
        if (isSort) {
            SetPositionCardInArray();
        }
    }
    public void SetChooseCard(int[] cards) {
        if (cards != null) {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card c = listCardHand[i];
                bool isNot = true;
                for (int j = 0; j < cards.Length; j++) {
                    if (c.ID == cards[j]) {
                        c.SetDarkCard(false);
                        c.SetTouched(true);
                        isNot = false;
                        break;
                    }
                }
                if (isNot) {
                    c.IsChoose = false;
                    c.SetDarkCard(true);
                    c.SetTouched(false);
                }
            }
        }
    }

    public void ResetCard() {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].SetDarkCard(false);
            listCardHand[i].SetTouched(true);
            listCardHand[i].IsChoose = false;
        }
    }

    public void SetCardWithId53() {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].SetCardWithId(52);
        }
    }

    public void SetLaiHetCardVeToaDo0() {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].transform.localPosition = Vector3.zero;
        }
    }

    public void SapXepLaiBaiSauKhiDanh() {
        //        Debug.LogError("Anchor " + align_Anchor);
        switch (align_Anchor) {
            case Align_Anchor.LEFT:
                Anchoir_Left_Card_Enable();
                break;
            case Align_Anchor.RIGHT:
                Anchoir_Right_Card_Enable();
                break;
            case Align_Anchor.CENTER:
                Anchoir_Center_Card_Enable();
                break;
        }
    }

    public void SetActiveCardHand(bool isActive = false) {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].SetVisible(isActive);
        }
    }

    public void SetVisible(bool isVisible) {
        gameObject.SetActive(isVisible);
    }
    public void SetTouchCardHand(bool isTouched = false) {
        this.isTouched = isTouched;
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].SetTouched(isTouched);
        }
    }
    bool isSetInputChooseCard = false;
    public void SetInputChooseCard() {
        //if (!isSetInputChooseCard) {
        //    isSetInputChooseCard = true;
        //    for (int i = 0; i < listCardHand.Count; i++) {
        //        Card c = listCardHand[i];
        //        c.setListenerClick(delegate {
        //            switch ((GameConfig.GameID)int.Parse(ClientConfig.UserInfo.CURRENT_GAME_ID)) {
        //                case GameConfig.GameID.TLMN:
        //                    AutoChooseCard.ClickCard(c, listCardHand.ToArray());
        //                    break;
        //                case GameConfig.GameID.SAM:
        //                    AutoChooseCardSam.ClickCard(c, listCardHand.ToArray());
        //                    break;
        //            }
        //        });
        //        c.isAuto = true;
        //    }
        //}
    }

    public void SetAutoChooseCard(bool isAuto) {
        for (int i = 0; i < listCardHand.Count; i++) {
            Card c = listCardHand[i];
            c.isAuto = isAuto;
        }
    }

    public void SetIsCardMauBinh(bool isMauBinh) {
        for (int i = 0; i < listCardHand.Count; i++) {
            listCardHand[i].SetIsCardMauBinh(isMauBinh);
        }
    }

    #region Ta La

    #region Set Position Card Danh Ra Va Card Ha Phom Tren Ban
    public void SetPostionCardDanhRaVaCardHaPhomTrenBan() {
        switch (align_Anchor) {
            case Align_Anchor.LEFT:
                SetLeft();
                break;
            case Align_Anchor.RIGHT:
                SetRight();
                break;
            case Align_Anchor.CENTER:
                SetCenter();
                break;
        }
    }

    void SetLeft() {
        float distance = 30;
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(i * distance, child.transform.localPosition.y, 0);
            child.SetSiblingIndex(i);
        }
    }

    void SetRight() {
        float distance = 30;
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(-(i * distance), child.localPosition.y, 0);
            child.SetAsFirstSibling();
        }
    }

    void SetCenter() {

    }

    #endregion

    #region Set Lai Card Danh Sau Khi Reconnect
    public void SetLaiCardDanhKhiKetNoiLai(int[] arrcard, Transform cardDanhRaTf) {
        for (int i = 0; i < cardDanhRaTf.childCount; i++) {
            Card c = cardDanhRaTf.GetChild(i).GetComponent<Card>();
            if (i < arrcard.Length) {
                c.SetCardWithId(arrcard[i]);
                c.SetVisible(true);
                c.SetTouched(false);
                c.setSmall(true);
            }
        }
    }
    #endregion

    #region Set Lai Card Ha Sau Khi Reconnect
    public void SetLaiCardHaKhiKetNoiLai(int[] arrcard, Transform cardHaTf) {
        for (int i = 0; i < cardHaTf.childCount; i++) {
            Card c = cardHaTf.GetChild(i).GetComponent<Card>();
            if (i < arrcard.Length) {
                c.SetCardWithId(arrcard[i]);
                c.SetVisible(true);
                c.SetTouched(false);
                c.setSmall(true);
            }
        }
    }
    #endregion

    public void ClearCardTaLaKhiKetThucGame() {
        for (int i = 0; i < transform.childCount; i++) {
            Card card = transform.GetChild(i).GetComponent<Card>();
            card.SetCardWithId(53);
            card.SetVisible(false);
        }
    }
    public int[] GetCardChoose() {
        List<int> cardDanh = new List<int>();
        for (int i = 0; i < listCardHand.Count; i++) {
            if (listCardHand[i].IsChoose && listCardHand[i].isBatHayChua) {
                cardDanh.Add(listCardHand[i].ID);
            }
        }
        return cardDanh.ToArray();
    }


    #region Sap Xep Lai Card Hand Sau Khi Ha
    public void SapXepLaiCardHandSauKhiHa() {
        int j = 0;
        float disCard;
        if (MaxWidth >= listCardHand.Count * w_card) {
            disCard = w_card;
        } else {
            disCard = MaxWidth / listCardHand.Count;
        }
        int countBat = 0;
        for (int i = 0; i < listCardHand.Count; i++) {
            Card card = listCardHand[i];
            if (card.isBatHayChua) {
                countBat++;
                //				Debug.LogError ("Co " + countBat + " bat");
            }
        }
        if (countBat % 2 == 0) {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                if (card.isBatHayChua) {
                    Vector3 vt = new Vector3(-((int)countBat / 2 - 0.5f) * disCard + j * disCard, 0, 0);
                    StartCoroutine(MoveTo(card, vt, 0.05f, j * 0.01f));
                    j++;
                }
            }
        } else {
            for (int i = 0; i < listCardHand.Count; i++) {
                Card card = listCardHand[i];
                if (card.isBatHayChua) {
                    Vector3 vt = new Vector3(-((int)countBat / 2) * disCard + j * disCard, 0, 0);
                    StartCoroutine(MoveTo(card, vt, 0.05f, j * 0.01f));
                    j++;
                }
            }
        }
    }

    IEnumerator MoveTo(Card card, Vector3 to, float dur, float wait) {
        yield return new WaitForSeconds(wait);
        card.transform.DOLocalMove(to, dur);
    }
    #endregion
    #endregion
}
