using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class CardTableManager : MonoBehaviour {
    List<Card> listCardTrenBan = new List<Card>();
    [SerializeField]
    Image effect_fire;
    const int NUM_CARD = 20;

    public void SinhCardGiuaCMNBan(int[] cards, Transform tranformCuaCaiThangDanh) {
        Card[] hanLuonMangCard = new Card[cards.Length];
        for (int i = 0; i < cards.Length; i++) {
            GameObject card = GetCard();
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.zero;
            card.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
            card.transform.SetAsLastSibling();
            Card c = card.GetComponent<Card>();
            c.setSmall(true);
            c.SetTouched(false);
            c.SetCardWithId(cards[i]);
            c.SetVisible(true);
            hanLuonMangCard[i] = c;
        }
        BayTuCaiThangDangDanhChoDenGiuaBan(hanLuonMangCard, tranformCuaCaiThangDanh);
    }

    public void MinhDanh(int[] cards, ArrayCard cardHand, UnityAction callBack) {
        Card[] hanLuonMangCard = new Card[cards.Length];
        for (int i = 0; i < cards.Length; i++) {
            GameObject card = GetCard();
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.zero;
            card.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
            card.transform.SetAsLastSibling();
            Card c = card.GetComponent<Card>();
            c.setSmall(false);
            c.SetTouched(false);
            c.SetCardWithId(cards[i]);
            c.SetVisible(true);
            hanLuonMangCard[i] = c;
            listCardTrenBan.Add(c);
        }
        StartCoroutine(BayTuCardHandRaGiuaBan(hanLuonMangCard, cardHand, callBack));
    }

    public void SinhCardKhiKetNoiLai(int[] cards) {
        Card[] hanLuonMangCard = new Card[cards.Length];
        for (int i = 0; i < cards.Length; i++) {
            GameObject card = GetCard();
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;
            card.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
            card.transform.SetAsLastSibling();
            Card c = card.GetComponent<Card>();
            c.SetVisible(true);
            c.setSmall(true);
            c.SetTouched(false);
            c.SetCardWithId(cards[i]);
            c.SetVisible(true);
            hanLuonMangCard[i] = c;
            listCardTrenBan.Add(c);

        }
        SapXepCardChinhCMNGiua(hanLuonMangCard);
    }

    void SapXepCardChinhCMNGiua(Card[] cards) {
        float disCard = 50.0f;
        float posY = UnityEngine.Random.Range(-30.0f, 30.0f);
        effect_fire.transform.localPosition = new Vector3(0, posY, 0);
        effect_fire.transform.SetAsLastSibling();
        if (cards.Length % 2 == 0) {
            for (int i = 0; i < cards.Length; i++) {
                cards[i].transform.localPosition = new Vector3(
                    -((int)cards.Length / 2 - 0.5f)
                            * disCard + i * disCard, posY, 0);
                cards[i].transform.SetAsLastSibling();
            }
        } else {
            for (int i = 0; i < cards.Length; i++) {
                cards[i].transform.localPosition = new Vector3(
                    -((int)cards.Length / 2) * disCard
                            + i * disCard, posY, 0);
                cards[i].transform.SetAsLastSibling();
            }
        }
    }

    void BayTuCaiThangDangDanhChoDenGiuaBan(Card[] cards, Transform viTriCuaThangDanh) {
        Vector3 vt = transform.InverseTransformPoint(viTriCuaThangDanh.position);
        for (int i = 0; i < cards.Length; i++) {
            StartCoroutine(cards[i].MoveFrom(vt, 0.2f, 0));
        }
        SapXepCardChinhCMNGiua(cards);
        StartCoroutine(ShowHideEffect(0.3f));
    }

    IEnumerator BayTuCardHandRaGiuaBan(Card[] cards, ArrayCard cardHand, UnityAction callBack) {
        for (int i = 0; i < cards.Length; i++) {
            Card cccc = cardHand.GetCardbyIDCard(cards[i].ID);
            if (cccc != null) {
                Vector3 vtfrom = transform.InverseTransformPoint(cccc.transform.position);
                StartCoroutine(cards[i].MoveFromCardHand(vtfrom, 0.2f, 0));
                cccc.transform.DOScale(0, 0.1f).OnComplete(() => {
                    cccc.SetVisible(false);
                    cccc.transform.localScale = Vector3.one;
                });
            }
        }
        SapXepCardChinhCMNGiua(cards);
        yield return new WaitForSeconds(0.2f);
        if (callBack != null) {
            callBack.Invoke();
        }
        StartCoroutine(ShowHideEffect(0));
    }

    IEnumerator ShowHideEffect(float time) {
        yield return new WaitForSeconds(time);
        effect_fire.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        effect_fire.gameObject.SetActive(false);
    }

    public void UpHetCMNBaiXuong() {
        try {
            for (int i = 0; i < listCardTrenBan.Count; i++) {
                listCardTrenBan[i].SetCardWithId(52);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void XoaHetCMNBaiTrenBan() {
        //for (int i = 0; i < listCardTrenBan.Count; i++) {
        //    Destroy(listCardTrenBan[i].gameObject);
        //}
        //listCardTrenBan.Clear();
        for (int i = 0; i < listCardTrenBan.Count; i++) {
            listCardTrenBan[i].gameObject.SetActive(false);
            listCardTrenBan[i].transform.SetParent(null);
        }
    }

    GameObject GetCard() {
        if (listCardTrenBan.Count > NUM_CARD) {
            for (int i = 0; i < listCardTrenBan.Count; i++) {
                if (!listCardTrenBan[i].gameObject.activeSelf) {
                    return listCardTrenBan[i].gameObject;
                }
            }
            for (int i = 0; i < listCardTrenBan.Count; i++) {
                if (listCardTrenBan[i].ID == 52) {
                    return listCardTrenBan[i].gameObject;
                }
            }
        }
        GameObject obj = Instantiate(GameControl.instance.objCard);
        listCardTrenBan.Add(obj.GetComponent<Card>());
        return obj;
    }
}
