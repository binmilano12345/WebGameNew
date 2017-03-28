using AppConfig;
using DataBase;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardMauBinh : MonoBehaviour
{
	[SerializeField]
	ArrayCard[] arrayCard;
//3-5-5
	//[SerializeField]
	//Text txt_sum_score;
	public MauBinhPlayer player;

	float[] PosYMe = new float[3] { 270, 120, -30 };
	const float MaxWidthMe = 100;

	float[] PosYOther = new float[3] { 10, 5, 0 };
	const float MaxWidthOther = 10;

	public void Init ()
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			int count = 3;
			if (i > 0) {
				count = 5;
			}
			arrayCard [i].CardCount = count;
			arrayCard [i].MaxWidth = count * MaxWidthMe;
			arrayCard [i].Init ();
		}
		SetCardMauBinh (true);
	}

	public void SetCard (int[] arr, bool isTao, UnityAction callback = null)
	{
		StopAllCoroutines ();

		int[] chi1 = new int[5];
		int[] chi2 = new int[5];
		int[] chi3 = new int[3];
		for (int i = 0; i < arr.Length; i++) {
			if (i < 5) {
				chi1 [i] = arr [i];
			} else if (i < 10) {
				chi2 [i - 5] = arr [i];
			} else {
				chi3 [i - 10] = arr [i];
			}
		}

		if (isTao) {
			for (int i = 0; i < arrayCard.Length; i++) {
				Vector3 vt = arrayCard [i].transform.localPosition;
				vt.y = PosYMe [i];
				arrayCard [i].transform.localPosition = vt;
			}
		} else {
			for (int i = 0; i < arrayCard.Length; i++) {
				Vector3 vt = arrayCard [i].transform.localPosition;
				vt.y = 0;
				arrayCard [i].transform.localPosition = vt;
			}
		}
	
		arrayCard [0].ChiaBaiTienLen (chi1, isTao, () => {
			arrayCard [1].ChiaBaiTienLen (chi2, isTao, () => {
				arrayCard [2].ChiaBaiTienLen (chi3, isTao, () => {
					numc = 0;
					if (callback != null)
						callback.Invoke ();
				});
			});
		});
	}

	void SetCard (int[] arr)
	{
		int[] chi1 = new int[5];
		int[] chi2 = new int[5];
		int[] chi3 = new int[3];
		for (int i = 0; i < arr.Length; i++) {
			if (i < 5) {
				chi1 [i] = arr [i];
			} else if (i < 10) {
				chi2 [i - 5] = arr [i];
			} else {
				chi3 [i - 10] = arr [i];
			}
		}

		arrayCard [0].SetCardWithArrID (chi1, false);
		arrayCard [1].SetCardWithArrID (chi2, false);
		arrayCard [2].SetCardWithArrID (chi3, false);
	}

	public void SetSoBai (bool isTao)
	{//xep xong bai
		if (isTao) {
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = 3;
				if (i > 0) {
					count = 5;
				}
				int[] arrcccccc = TypeCardMauBinh.SortDescendingArrCard (GetArrCardIDWithChi (i));
				arrayCard [i].MaxWidth = count * (MaxWidthMe - 30);
				arrayCard [i].SetCardWithArrID (arrcccccc);
				arrayCard [i].SortCardActive ();
				if (i != arrayCard.Length - 1) {
					Vector3 vt = arrayCard [i].transform.localPosition;
					vt.y = PosYMe [i];
					arrayCard [i].transform.localPosition = vt;

					vt.y -= (120 - i * 60);
					arrayCard [i].transform.DOLocalMoveY (vt.y, 0.2f);
				}
			}
		} else {
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = 3;
				if (i > 0) {
					count = 5;
				}
				arrayCard [i].MaxWidth = count * MaxWidthOther;
				arrayCard [i].SortCardActive ();

				arrayCard [i].transform.DOLocalMoveY (PosYOther [i], 0.2f);
			}
		}
	}

	public void SetXepLai (bool isTao)
	{//xep lai bai
		if (isTao) {
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = 3;
				if (i > 0) {
					count = 5;
				}
				arrayCard [i].MaxWidth = count * MaxWidthMe;
				arrayCard [i].SortCardActive ();

				arrayCard [i].transform.DOLocalMoveY (PosYMe [i], 0.2f);
			}
		} else {
			for (int i = 0; i < arrayCard.Length; i++) {
				arrayCard [i].SetLaiHetCardVeToaDo0 ();
				arrayCard [i].transform.DOLocalMoveY (0, 0.2f);
			}
		}
	}

	public void SetCardKhiKetThuc (int[] chi, bool isTao)
	{
		//bool isTao = dataplayer.DisplaName.Equals(ClientConfig.UserInfo.UNAME);
		//int[] chi1 = new int[3];
		//int[] chi2 = new int[5];
		//int[] chi3 = new int[5];
		//for (int i = 0; i < dataplayer.cards.Length; i++) {
		//    if (i < 3) {
		//        chi1[i] = dataplayer.cards[i];
		//    } else if (i < 8) {
		//        chi2[i - 3] = dataplayer.cards[i];
		//    } else {
		//        chi3[i - 8] = dataplayer.cards[i];
		//    }
		//}

		//SetCardKetThuc(false, chi1, chi2, chi3, isTao);

		#region Thang trang, Lung
		//int sum = 0;
		//if (dataplayer.MauBinh != 0 || dataplayer.IsBinhLung) {
		//    switch (dataplayer.MauBinh) {
		//        case 10:
		//            sum = 16;
		//            break;
		//        case 11:
		//            sum = 18;
		//            break;
		//        case 12:
		//            sum = 20;
		//            break;
		//        case 13:
		//            sum = 30;
		//            break;
		//        case 14:
		//            sum = 36;
		//            break;
		//        case 15:
		//            sum = 108;
		//            break;
		//    }
		//if (MauBinhViewScript.instance != null && isTao) {
		//    MauBinhViewScript.instance.SetScoreMauBinh(0, 0, 0, true);
		//}

		//txt_sum_score.text = "Tổng: " + "<color=orange>+" + sum + "</color>";
		//player.SetTypeCard(dataplayer.IsBinhLung ? "Binh Lủng" : GameConfig.STR_THANG_TRANG[dataplayer.MauBinh - 10], arrayCard[0].transform.localPosition + new Vector3(0, 60, 0), isTao);
		//}
		#endregion
		//StartCoroutine(BatDauDien(dataplayer, chi1, chi2, chi3, isTao));
	}

	public void SetTouchCardHand (bool isTouched = false)
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			arrayCard [i].SetTouchCardHand (isTouched);
		}
	}

	void SetCardMauBinh (bool isMB = false)
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			arrayCard [i].SetIsCardDragDrop (isMB);
		}
	}

	public void ResetCard ()
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			arrayCard [i].ResetCard ();
		}
	}

	public int[] GetArrCardID ()
	{
		List<int> list = new List<int> ();
		for (int i = arrayCard.Length - 1; i >= 0; i--) {
			for (int j = 0; j < arrayCard [i].listCardHand.Count; j++) {
				list.Add (arrayCard [i].listCardHand [j].ID);
			}
		}
		return list.ToArray ();
	}

	public int[] GetArrCardIDWithChi (int chi)
	{
		List<int> list = new List<int> ();
		for (int j = 0; j < arrayCard [chi].listCardHand.Count; j++) {
			list.Add (arrayCard [chi].listCardHand [j].ID);
		}
		return list.ToArray ();
	}

	public void SetPositionArryCard (Align_Anchor align)
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			arrayCard [i].align_Anchor = align;
			arrayCard [i].SetPositonCardHand ();
		}
	}

	public void SetActiveCard (bool isActive = false)
	{
		for (int i = 0; i < arrayCard.Length; i++) {
			arrayCard [i].SetActiveCardHand (isActive);
		}
	}

	public IEnumerator BatDauDien (int[] chi1, int[] chi2, int[] chi3, bool isTao)
	{
		int sum = 0;
		yield return new WaitForSeconds (1);

		//if (dataplayer.MauBinh == 0 && !dataplayer.IsBinhLung) {
		//    sum += dataplayer.HeSoChi[0] + dataplayer.BonusChi[0];
		//    ShowChi(chi1, 0, isTao, dataplayer.HeSoChi[0], sum, dataplayer.BonusChi[0]);
		//}
		//yield return new WaitForSeconds(2.5f);
		//if (dataplayer.MauBinh == 0 && !dataplayer.IsBinhLung) {
		//    sum += dataplayer.HeSoChi[1] + dataplayer.BonusChi[1];
		//    ShowChi(chi2, 1, isTao, dataplayer.HeSoChi[1], sum, dataplayer.BonusChi[1]);
		//}
		//yield return new WaitForSeconds(2.5f);
		//if (dataplayer.MauBinh == 0 && !dataplayer.IsBinhLung) {
		//    sum += dataplayer.HeSoChi[2] + dataplayer.BonusChi[2];
		//    ShowChi(chi3, 2, isTao, dataplayer.HeSoChi[2], sum, dataplayer.BonusChi[2]);
		//}
		//yield return new WaitForSeconds(2.5f);
		//for (int i = 0; i < dataplayer.playerWinMB.Count; i++) {
		//    sum += dataplayer.playerWinMB[i].Score;
		//}

		//if (sum < 0) {
		//    txt_sum_score.DOText("<color=white>Tổng</color> " + "<color=white>" + sum + "</color>", 0.2f);
		//} else {
		//    txt_sum_score.DOText("<color=white>Tổng</color> " + "<color=orange>+" + sum + "</color>", 0.2f);
		//}

		//SetCardKetThuc(true, chi1, chi2, chi3, isTao);

		//player.HienTienVaHieuUngKhiKetThucGame(dataplayer.tienBiTru);
		//player.SetTien(dataplayer.moneyChip);
		//player.SetTypeCard("", Vector3.zero, isTao);

		//yield return new WaitForSeconds(2);
		//if (MauBinhViewScript.instance != null && isTao) {
		//    MauBinhViewScript.instance.SetScoreMauBinh(0, 0, 0, true);
		//}
		//txt_sum_score.gameObject.SetActive(false);
	}

	void SetCardKetThuc (bool isOne, int[] chi1, int[] chi2, int[] chi3, bool isTao)
	{
		Vector3 vt;
		if (isTao) {
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = 3;
				if (i > 0) {
					count = 5;
				}
				arrayCard [i].MaxWidth = count * MaxWidthMe;

				arrayCard [i].transform.localScale = isOne ? Vector3.zero : Vector3.one;

				vt = arrayCard [i].transform.localPosition;
				vt.y = PosYMe [i];
				arrayCard [i].transform.localPosition = vt;
			}
			//if (isOne)//sua
			//    MauBinhViewScript.instance.SetActiveCardHand();
		} else {
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = 3;
				if (i > 0) {
					count = 5;
				}
				arrayCard [i].MaxWidth = count * (MaxWidthOther + 30);
				arrayCard [i].transform.localScale = isOne ? Vector3.zero : Vector3.one;

				vt = arrayCard [i].transform.localPosition;
				vt.y = PosYOther [i] + ((i != arrayCard.Length - 1) ? (60 - i * 30) : 0);
				arrayCard [i].transform.localPosition = vt;
			}
		}

		arrayCard [0].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi1));
		arrayCard [1].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi2));
		arrayCard [2].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi3));

		if (isOne)
			EffectScale (arrayCard [2].transform, 1.1f, () => {
				EffectScale (arrayCard [1].transform, 1.1f, () => {
					EffectScale (arrayCard [0].transform, 1.1f, () => {
					});
				});
			});
	}

	void ShowChi (int[] arrChi, int chi, bool isTao, int hsc, int sum, int bonus)
	{
		int type = (int)TypeCardMauBinh.GetTypeCardMauBinh (arrChi);
		arrChi = TypeCardMauBinh.SortDescendingArrCard (arrChi);
		for (int i = 0; i < arrayCard.Length; i++) {
			if (i != chi) {
				arrayCard [i].transform.DOScale (1, 0.1f);// = Vector3.one;
				arrayCard [i].transform.SetSiblingIndex (i);
			}
		}
		arrayCard [chi].transform.SetSiblingIndex (4);
		arrayCard [chi].transform.DOScale (1.2f, 0.2f);// = Vector3.one;
		Vector3 vt = arrayCard [chi].transform.localPosition;

		if (isTao) {
			vt.y += 40;
		} else {
			vt.y += 80;
		}

		//player.SetTypeCard(GameConfig.STR_TYPE_CARD[type], vt, isTao);//sua
		arrayCard [chi].SetActiveCardWithArrID (arrChi);

		SetTextScore (hsc, chi, sum, bonus, isTao);
	}

	public void DoiChi ()
	{
		ArrayCard temp = arrayCard [1];
		float vt1Y = arrayCard [1].transform.localPosition.y;
		float vt2Y = arrayCard [2].transform.localPosition.y;

		arrayCard [2].transform.DOLocalMoveY (vt1Y, 0.2f);
		arrayCard [1].transform.DOLocalMoveY (vt2Y, 0.2f);

		arrayCard [1] = arrayCard [2];
		arrayCard [1].transform.SetAsFirstSibling ();
		arrayCard [2] = temp;

		arrayCard [0].transform.SetAsFirstSibling ();
	}

	#region Effect

	void EffectScale (Transform tran, float endValue = 1.1f, UnityAction callback = null)
	{
		tran.transform.DOScale (endValue, 0.2f).OnComplete (delegate {
			tran.transform.DOScale (1f, 0.1f);
			if (callback != null)
				callback.Invoke ();
		});
	}

	#endregion

	void SetTextScore (int score, int chi, int sum, int bonus, bool isTao)
	{
		//if (MauBinhViewScript.instance != null && isTao)
		//    MauBinhViewScript.instance.SetScoreMauBinh(score, bonus, chi);

		//if (sum < 0) {
		//    txt_sum_score.DOText("<color=white>Tổng</color> " + "<color=white>" + sum + "</color>", 0.2f);
		//} else {
		//    txt_sum_score.DOText("<color=white>Tổng</color> " + "<color=orange>+" + sum + "</color>", 0.2f);
		//}
		//SetPositonTextScore(chi);
		//txt_sum_score.gameObject.SetActive(true);
		//EffectScale(txt_sum_score.transform);
	}
	//public float x1 = 150, x2 = 330;
	//public float xsum = 160;
	void SetPositonTextScore (int chi)
	{
		Vector3 vtpo = transform.localPosition;
		vtpo.y += 100;
		//txt_sum_score.transform.localPosition = vtpo;
	}

	int numc = 0;
	Dictionary<int, Vector3> oldPosition = new Dictionary<int, Vector3> ();

	public void XepBai ()
	{
		oldPosition.Clear ();
		int j = 0;
		for (int i = 0; i < arrayCard.Length; i++) {
			for (j = 0; j < arrayCard [i].listCardHand.Count; j++) {
				Card c = arrayCard [i].GetCardbyIndex (j);
				Vector3 position = c.transform.position;
				if (!oldPosition.ContainsKey (c.ID))
					oldPosition.Add (c.ID, position);
			}
		}

		numc++;
		if (numc > 7)
			numc = 1;
		int[] arrSorted = TypeCardMauBinh.SortCardMauBinh (GetArrCardID (), ref numc);
		Vector3 newPos, oldPos;
		for (j = 0; j < arrayCard [0].listCardHand.Count; j++) {
			Card c = arrayCard [0].GetCardbyIndex (j);
			newPos = c.transform.position;
			oldPos = oldPosition [arrSorted [j]];
			c.SetCardWithId (arrSorted [j]);
			c.transform.position = oldPos;
			c.transform.DOMove (newPos, 0.2f);
		}
		for (j = 0; j < arrayCard [1].listCardHand.Count; j++) {
			Card c = arrayCard [1].GetCardbyIndex (j);
			newPos = c.transform.position;
			oldPos = oldPosition [arrSorted [3 + j]];
			c.SetCardWithId (arrSorted [3 + j]);
			c.transform.position = oldPos;
			c.transform.DOMove (newPos, 0.2f);
		}
		for (j = 0; j < arrayCard [2].listCardHand.Count; j++) {
			Card c = arrayCard [2].GetCardbyIndex (j);
			newPos = c.transform.position;
			oldPos = oldPosition [arrSorted [8 + j]];
			c.SetCardWithId (arrSorted [8 + j]);
			c.transform.position = oldPos;
			c.transform.DOMove (newPos, 0.2f);
		}
		//SetCard();
	}
}
