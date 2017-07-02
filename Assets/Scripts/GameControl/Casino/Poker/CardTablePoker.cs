using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardTablePoker : MonoBehaviour {
	[SerializeField]
	ArrayCard ArrayCardTable;

	public void Init() {
		ArrayCardTable.CardCount = 5;
		ArrayCardTable.MaxWidth = 500;
		ArrayCardTable.isTouched = false;
		ArrayCardTable.Init();
	}
	List<int> list_card = new List<int>();
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
			}

		}
		ArrayCardTable.SortCardActive();
	}

	public void XoaHetBaiTrenBai() {
		ArrayCardTable.SetActiveCardHand();
	}

	public void SinhCardKhiKetNoiLai(int[] cards) {
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

		return arr.AddAndGetCardOnArray();
	}
}
