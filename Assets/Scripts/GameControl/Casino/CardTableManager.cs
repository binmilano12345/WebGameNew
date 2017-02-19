using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class CardTableManager : MonoBehaviour {
    List<Card> listCardTrenBan = new List<Card>();

    public void SinhCardGiuaCMNBan(int[] cards, Transform tranformCuaCaiThangDanh) {
        //LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CARD, (objPre) => {
        if (GameControl.instance.objCard != null) {
            GameObject objPreCard = GameControl.instance.objCard;
            Card[] hanLuonMangCard = new Card[cards.Length];
            for (int i = 0; i < cards.Length; i++) {
                GameObject card = Instantiate(objPreCard);
                card.transform.SetParent(transform);
                card.transform.localPosition = Vector3.zero;
                card.transform.localScale = Vector3.zero;
                card.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
                card.transform.SetAsLastSibling();
                Card c = card.GetComponent<Card>();
                c.setSmall(true);
                c.SetTouched(false);
                c.SetCardWithId(cards[i]);
                hanLuonMangCard[i] = c;
                listCardTrenBan.Add(c);
            }
            //SapXepCardChinhCMNGiua(hanLuonMangCard);

            BayTuCaiThangDangDanhChoDenGiuaBan(hanLuonMangCard, tranformCuaCaiThangDanh);
            //    Destroy(objPre);
            //});
        }
    }

    public void MinhDanh(int[] cards, ArrayCard cardHand, UnityAction callBack) {
        if (GameControl.instance.objCard != null) {
            //LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CARD, (objPre) => {
            GameObject objPreCard = GameControl.instance.objCard;
            Card[] hanLuonMangCard = new Card[cards.Length];
            for (int i = 0; i < cards.Length; i++) {
                GameObject card = Instantiate(objPreCard);
                card.transform.SetParent(transform);
                card.transform.localPosition = Vector3.zero;
                card.transform.localScale = Vector3.zero;
                card.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-10, 10)));
                card.transform.SetAsLastSibling();
                Card c = card.GetComponent<Card>();
                c.setSmall(false);
                c.SetTouched(false);
                c.SetCardWithId(cards[i]);
                hanLuonMangCard[i] = c;
                listCardTrenBan.Add(c);
            }
            StartCoroutine(BayTuCardHandRaGiuaBan(hanLuonMangCard, cardHand, callBack));
            //    Destroy(objPre);
            //});
        }
    }

    public void SinhCardKhiKetNoiLai(int[] cards) {
        //Debug.LogError("                       SinhCardKhiKetNoiLai");
        if (GameControl.instance.objCard != null) {
            //GameObject objPreCard = PopupAndLoadingScript.instance.objCard;
            //Debug.LogError("co null ko:  " + (PopupAndLoadingScript.instance.objCard == null));
            Card[] hanLuonMangCard = new Card[cards.Length];
            for (int i = 0; i < cards.Length; i++) {
                GameObject card = Instantiate(GameControl.instance.objCard);
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
                hanLuonMangCard[i] = c;
                listCardTrenBan.Add(c);

                //Debug.LogError("Id card:      " + c.ID);
            }
            //Debug.LogError("Sinh dc tung nay bai: " + hanLuonMangCard.Length);
            SapXepCardChinhCMNGiua(hanLuonMangCard);
        }
    }

    void SapXepCardChinhCMNGiua(Card[] cards) {
        float disCard = 50.0f;
        float posY = UnityEngine.Random.Range(-30.0f, 30.0f);
        if (cards.Length % 2 == 0) {
            for (int i = 0; i < cards.Length; i++) {
                cards[i].transform.localPosition = new Vector3(
                    -((int)cards.Length / 2 - 0.5f)
                            * disCard + i * disCard, posY, 0);
                //cards[i].transform.SetSiblingIndex(i);
            }
        } else {
            for (int i = 0; i < cards.Length; i++) {
                cards[i].transform.localPosition = new Vector3(
                    -((int)cards.Length / 2) * disCard
                            + i * disCard, posY, 0);
                //cards[i].transform.SetSiblingIndex(i);
            }
        }
    }

    void BayTuCaiThangDangDanhChoDenGiuaBan(Card[] cards, Transform viTriCuaThangDanh) {
        Vector3 vt = transform.InverseTransformPoint(viTriCuaThangDanh.position);
        for (int i = 0; i < cards.Length; i++) {
            StartCoroutine(cards[i].MoveFrom(vt, 0.2f, 0));
        }
        //yield return new WaitForSeconds(0.2f);
        SapXepCardChinhCMNGiua(cards);
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
                    //cardHand.listIdCardHand.Remove(cccc.ID);
                });
            }
        }
        SapXepCardChinhCMNGiua(cards);
        yield return new WaitForSeconds(0.2f);
        if (callBack != null) {
            callBack.Invoke();
        }
    }

    public void UpHetCMNBaiXuong() {
        try {
            for (int i = 0; i < listCardTrenBan.Count; i++) {
                listCardTrenBan[i].SetCardWithId(53);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void XoaHetCMNBaiTrenBan() {
        for (int i = 0; i < listCardTrenBan.Count; i++) {
            Destroy(listCardTrenBan[i].gameObject);
        }
        listCardTrenBan.Clear();
    }
}
