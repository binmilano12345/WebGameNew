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
	//3-5-5==321
	//[SerializeField]
	//Text txt_sum_score;
	public MauBinhPlayer player;

	float[] PosYMe = new float[3] { 252, 116, -20 };
	const float MaxWidthMe = 100;

	float[] PosYOther = new float[3] { 5, 0, -5 };
	const float MaxWidthOther = 10;

	public void Init (bool isMe)
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
		SetCardMauBinh (isMe);
		SetTouchCardHand (isMe);
	}

	public void SetCard (int[] arr, bool isTao, UnityAction callback = null)
	{
//		StopAllCoroutines ();
		isOne = false;
		int[] chi3 = new int[3];
		int[] chi2 = new int[5];
		int[] chi1 = new int[5];
		for (int i = 0; i < arr.Length; i++) {
			if (i < 3) {
				chi3 [i] = arr [i];
			} else if (i < 8) {
				chi2 [i - 3] = arr [i];
			} else {
				chi1 [i - 8] = arr [i];
			}
		}
		int count = 3;
		if (isTao) {
			for (int i = 0; i < arrayCard.Length; i++) {
				Vector3 vt = arrayCard [i].transform.localPosition;
				vt.y = PosYMe [i];
				arrayCard [i].transform.localPosition = vt;

				count = i == 0 ? 3 : 5;
				arrayCard [i].CardCount = count;
				arrayCard [i].MaxWidth = count * MaxWidthMe;
			}
		} else {
			for (int i = 0; i < arrayCard.Length; i++) {
				Vector3 vt = arrayCard [i].transform.localPosition;
				vt.y = 0;
				arrayCard [i].transform.localPosition = vt;
			}
		}

		arrayCard [0].ChiaBaiTienLen (chi3, isTao, () => {
			arrayCard [1].ChiaBaiTienLen (chi2, isTao, () => {
				arrayCard [2].ChiaBaiTienLen (chi1, isTao, () => {
					numc = 0;
					if (callback != null)
						callback.Invoke ();
				});
			});
		});
	}

	#region Set bai khi dang choi

	public	IEnumerator SetDangChoi (bool isXepXong)
	{
		yield return new WaitForSeconds (1);

		for (int i = 0; i < arrayCard.Length; i++) {
			Vector3 vt = arrayCard [i].transform.localPosition;
			vt.y = 0;
			arrayCard [i].transform.localPosition = vt;
			arrayCard [i].SetCardDangChoiVeToaDo0 ();
		}

		yield return new WaitForSeconds (0.5f);
		if (isXepXong)
			SetSoBai (false);
	}

	#endregion

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
		if (!isActive) {
			player.SetDisableAction ();
			player.SetDisableEffectRank ();
		}
	}

	#region Set card khi ket thuc thang trang

	public void SetCardKetThuc (int[] chi1, int[] chi2, int[] chi3, bool isTao)
	{
		StartCoroutine (WinMauBinh (chi1, chi2, chi3, isTao));
	}

	IEnumerator WinMauBinh (int[] chi1, int[] chi2, int[] chi3, bool isTao)
	{
		yield return new WaitForEndOfFrame ();
		Vector3 vt;
		for (int i = 0; i < arrayCard.Length; i++) {
			int count = (i == 0 ? 3 : 5);
			vt = arrayCard [i].transform.localPosition;
			arrayCard [i].transform.localScale = Vector3.zero;
			if (isTao) {
				arrayCard [i].MaxWidth = count * MaxWidthMe;
				vt.y = PosYMe [i];
			} else {
				arrayCard [i].MaxWidth = count * (MaxWidthOther + 20);
				vt.y = 80 - i * 40;
			}

			arrayCard [i].SortCardActive (true);
			arrayCard [i].transform.localPosition = vt;
			arrayCard [i].transform.SetAsLastSibling ();
		}
		yield return new WaitForSeconds (1);
		arrayCard [2].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi1));
		arrayCard [1].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi2));
		arrayCard [0].SetActiveCardWithArrID (TypeCardMauBinh.SortDescendingArrCard (chi3));

		EffectScale (arrayCard [0].transform, 1.1f, () => {
			EffectScale (arrayCard [1].transform, 1.1f, () => {
				EffectScale (arrayCard [2].transform, 1.1f, () => {
				});
			});
		});
		yield return new WaitForSeconds (2);
		SetActiveCard ();
	}

	#endregion

	#region So cac chi

	public void ShowChi (int[] arrChi, int chi, int typeC, bool isTao/*, int hsc, int sum, int bonus*/)
	{
		StartCoroutine (SoChi (arrChi, chi, typeC, isTao));
	}

	bool isOne = false;

	IEnumerator SoChi (int[] arrChi, int chi, int typeC, bool isTao)
	{
		yield return new WaitForEndOfFrame ();
		#region Them
		Vector3 vt;
		if (!isOne) {//chi == 2 ||
			isOne = true;
			yield return new WaitForSeconds (1);
			for (int i = 0; i < arrayCard.Length; i++) {
				int count = (i == 0 ? 3 : 5);
				vt = arrayCard [i].transform.localPosition;
				arrayCard [i].transform.localScale = Vector3.zero;
				if (isTao) {
					arrayCard [i].MaxWidth = count * MaxWidthMe;
					vt.y = PosYMe [i];
				} else {
					arrayCard [i].MaxWidth = count * (MaxWidthOther + 20);
					vt.y = 80 - i * 40;
				}

				arrayCard [i].SortCardActive (true);
				arrayCard [i].transform.localPosition = vt;
				arrayCard [i].transform.DOScale (1, 0.2f);
			}

			yield return new WaitForSeconds (1);
		}
		#endregion

		int typeCC = (int)TypeCardMauBinh.GetTypeCardMauBinh (arrChi);
		arrChi = TypeCardMauBinh.SortDescendingArrCard (arrChi);
		for (int i = 0; i < arrayCard.Length; i++) {
			if (i != chi) {
				arrayCard [i].transform.DOScale (1, 0.1f);// = Vector3.one;
				arrayCard [i].transform.SetSiblingIndex (i);
			}
		}
		arrayCard [chi].transform.SetSiblingIndex (4);
		arrayCard [chi].transform.DOScale (1.2f, 0.2f);// = Vector3.one;
		vt = arrayCard [chi].transform.localPosition;

		if (isTao) {
			vt.y += 40;
		} else {
			vt.y += 80;
		}

		player.SetTypeCard (GameConfig.STR_TYPE_CARD [typeCC], vt, isTao);//sua
		arrayCard [chi].SetActiveCardWithArrID (arrChi);

		if (chi == 0) {
			yield return new WaitForSeconds (2);
			arrayCard [chi].transform.DOScale (1f, 0.2f);
			arrayCard [chi].transform.SetAsFirstSibling ();
//			EffectScale (arrayCard [2].transform, 1.1f, () => {
//				arrayCard [1].transform.SetAsFirstSibling ();
//				EffectScale (arrayCard [1].transform, 1.1f, () => {
//					arrayCard [0].transform.SetAsFirstSibling ();
//					EffectScale (arrayCard [0].transform, 1.1f, () => {
//					});
//				});
//			});
//			yield return new WaitForSeconds (2);
//			SetActiveCard ();
		}
	}

	#endregion

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
