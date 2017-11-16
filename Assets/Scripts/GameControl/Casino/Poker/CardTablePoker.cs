using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardTablePoker : MonoBehaviour {
	[SerializeField]
	ArrayCard ArrayCardTable;

	public void Init() {
		ArrayCardTable.align_Anchor = Align_Anchor.CENTER;
		ArrayCardTable.CardCount = 6;
		ArrayCardTable.MaxWidth = 600;
		ArrayCardTable.isTouched = false;
		ArrayCardTable.Init();
	}
	public List<int> list_card = new List<int>();
	public void AddCard(int[] arr_card) {
		int[] temp = arr_card.Except(list_card).ToArray();
		if (arr_card.Length <= 0) return;
		list_card.Clear();
		list_card.AddRange(arr_card);
		for (int i = 0; i < temp.Length; i++) {
			Card carddanh = GetCardOnArrayCard(ArrayCardTable);
			if (carddanh != null) {
				carddanh.SetVisible(true);
				carddanh.SetCardWithId(temp[i]);
				carddanh.SetDarkCard(false);
				carddanh.IsChoose = false;
			}
		}

		Debug.LogError("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
		ArrayCardTable.SortCardActive();
	}

	public void XoaHetBaiTrenBai() {
		list_card.Clear();
		ArrayCardTable.SetActiveCardHand();
	}

	public void SinhCardKhiKetNoiLai(int[] cards) {
		list_card.Clear();
		list_card.AddRange(cards);
		ArrayCardTable.SetBaiKhiKetNoiLai(cards, true);
	}

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

		Debug.LogError("=========================Tao moi roi!");
		return arr.AddAndGetCardOnArray();
	}

	public void showCardFinish(int[] arrC, int index) {
		//string str = "";
		//setAllDark();
		//for (int i = index; i < arrC.Length; i++) {
		//	Card c = ArrayCardTable.GetCardbyIDCard(arrC[i]);
		//	Debug.LogError("idididdiddddd    " + arrC[i]);
		//	if (c != null) {
		//		c.SetDarkCard(false);
		//		c.IsChoose = true;
		//		Debug.LogError(i + "  :  " + arrC[i]);
		//	}
		//}

		//Debug.LogError("-=-=: " + str);
		for (int i = 0; i < ArrayCardTable.listCardHand.Count; i++) {
			Card c = ArrayCardTable.listCardHand[i];
			c.SetDarkCard(true);
			c.IsChoose = false;
			for (int j = index; j < arrC.Length; j++) {
				if (c.ID == arrC[j]) {
					c.SetDarkCard(false);
					c.IsChoose = true;
					Debug.LogError(j + "  Id C:   " + arrC[j]);
					break;
				}
			}
		}
	}

	public void setAllDark() {
		ArrayCardTable.SetAllDark(true);
	}
}
