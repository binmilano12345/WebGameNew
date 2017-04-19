using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AutoChooseCardTaLa {
	#region Get Value
	public static int GetValue(int id) {
		int vl = id % 13;
		return vl;
	}
	#endregion
	#region Get Type
	public static int GetType(int id) {
		return id/13;
	}
	#endregion

	/// <summary>
	/// Lay phom tren tay tra ve nhieu mang
	/// </summary>
	public static List<int[]> GetPhomTrenTayMultiArray(int[] cardH) {
//		int[] cardH = (int[])cards.Clone ();
		List<int[]> lResult = new List<int[]>();
		List<int> lTemp = new List<int>();

		#region Lay bai phom cung gia tri
		var result = cardH.GroupBy(x => GetValue(x));
		foreach (var item in result) {
			lTemp.Clear();
			foreach (var item2 in item) {
				lTemp.Add(item2);
			}
			if (lTemp.Count > 2) {
				lResult.Add(lTemp.ToArray());
			}
		}
		#endregion
		#region Lay bai phom cung chat
		var result2 = cardH.GroupBy(a => GetType(a))
			.Select(g => g.OrderBy(x => GetValue(x))
				.GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)));

		foreach (var item in result2) {
			foreach (var item2 in item) {
				List<int> l = item2.ToList();
				if (l.Count > 2) {
					for (int i = 0; i < lResult.Count; i++) {
						for (int j = 0; j < l.Count; j++) {
							if(lResult[i].Contains(l[j])){
								l.RemoveAt(i);
							}
						}
					}
					lResult.Add(l.ToArray());
				}
			}
		}
		#endregion
		return lResult;
	}
	static List<int[]> GetPhomTrenTayMultiArray(int[] cardH, out List<int> CardOther) {
		List<int[]> lResult = new List<int[]>();
		List<int> lTemp = new List<int>();
		List<int> lOther = new List<int>();

		#region Lay bai phom cung gia tri
		var result = cardH.GroupBy(x => GetValue(x));
		foreach (var item in result) {
			lTemp.Clear();
			foreach (var item2 in item) {
				lTemp.Add(item2);
			}
			if (lTemp.Count > 2) {
				lResult.Add(lTemp.ToArray());
			} else {
				lOther.AddRange(lTemp);
			}
		}
		#endregion
		#region Lay bai phom cung chat
		var result2 = cardH.GroupBy(a => GetType(a))
			.Select(g => g.OrderBy(x => GetValue(x))
				.GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)));

		foreach (var item in result2) {
			foreach (var item2 in item) {
				List<int> l = item2.ToList();
				if (l.Count > 2) {
					for (int i = 0; i < lResult.Count; i++) {
						for (int j = 0; j < l.Count; j++) {
							if(lResult[i].Contains(l[j])){
								l.RemoveAt(i);
							}
						}
					}
					lResult.Add(l.ToArray());
				} else {
					lOther.AddRange(l);
				}
			}
		}
		#endregion
		CardOther = lOther;
		return lResult;
	}
	/// <summary>
	/// Lay phom tren tay tra ve 1 mang
	/// </summary>
	public static int[] GetPhomTrenTayOneArray(int[] cardH) {
		List<int> lResult = new List<int>();
		List<int> lTemp = new List<int>();

		#region Lay bai phom cung gia tri
		var result = cardH.GroupBy(x => GetValue(x));
		foreach (var item in result) {
			lTemp.Clear();
			lTemp.AddRange(item.ToList());
			if (lTemp.Count > 2) {
				for (int i = 0; i < lTemp.Count; i++) {
					if (!lResult.Contains(lTemp[i])) {
						lResult.Add(lTemp[i]);
					}
				}
			}
		}
		#endregion
		#region Lay bai phom cung chat
		var result2 = cardH.GroupBy(a => GetType(a))
			.Select(g => g.OrderBy(x => GetValue(x))
				.GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)));

		foreach (var item in result2) {
			foreach (var item2 in item) {
				lTemp.Clear();
				lTemp.AddRange(item2.ToList());
				if (lTemp.Count > 2) {
					for (int i = 0; i < lTemp.Count; i++) {
						if (!lResult.Contains(lTemp[i])) {
							lResult.Add(lTemp[i]);
						}
					}
				}
			}
		}
		#endregion

		return lResult.ToArray();
	}

	/// <summary>
	/// Lay phom an duoc tren tay tra ve 1 mang
	/// </summary>
	static int[] GetPhomAnDuoc(int[] cardH, int card) {
		int valueC = GetValue(card);
		int typeC = GetType(card);
		List<int> lResult = new List<int>();
		List<int> lSameType = new List<int>();
		List<int> lTemp = new List<int>();
		#region Lay bai cung gia tri
		for (int i = 0; i < cardH.Length; i++) {
			if (lTemp.Count >= 3) break;
			if (GetValue(cardH[i]) == valueC) {
				lTemp.Add(cardH[i]);
			}
			if (GetType(cardH[i]) == typeC) {
				lSameType.Add(cardH[i]);
			}
		}

		if (lTemp.Count >= 2) {
			lResult.AddRange(lTemp);
		}
		#endregion
		#region Lay bai sanh cung chat
		lSameType.Add(card);

		var result = lSameType.Distinct()
			.OrderBy(x => GetValue(x))
			.GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)).Distinct();

		foreach (var item in result) {
			lTemp.Clear();
			lTemp.AddRange(item.ToList());
			if (lTemp.Contains(card)) {
				lTemp.Remove(card);
				if (lTemp.Count > 1)
					lResult.AddRange(lTemp);
				break;
			}
		}
		#endregion
		if (lResult.Count > 1)
			return lResult.ToArray();

		return null;

	}
	/// <summary>
	/// Lay phom an duoc tren tay tra ve 1 mang co mang bai da an
	/// </summary>
	public static int[] GetPhomAnDuoc(int[] cardH, int card, int[] cardAn) {
		if (cardAn.Length <= 0) {
			return GetPhomAnDuoc(cardH, card);
		}
		List<int> cardOther;
		//Loai bo nhung phom da an trc do
		List<int[]> listPhomTT = GetPhomTrenTayMultiArray(cardH, out cardOther);
		for (int i = 0; i < listPhomTT.Count; i++) {
			List<int> l = listPhomTT[i].ToList();
			for (int j = 0; j < cardAn.Length; j++) {
				if (l.Contains(cardAn[j])) {
					if (l.Count < 5) {
						listPhomTT.RemoveAt(i);
						i--;
						break;
					} else {
						int indexCA = GetIndexCardInArray(cardAn, cardAn[i]);
						if (indexCA >= cardAn.Length - 2) {
							listPhomTT.RemoveAt(i);
							i--;
							break;
						}
					}
				}
			}
		}
		for (int i = 0; i < listPhomTT.Count; i++) {
			cardOther.AddRange(listPhomTT[i]);
		}
		int[] cardCotheAn = GetPhomAnDuoc(cardOther.ToArray(), card);
		return cardCotheAn;
	}

	public static int[] GetPhomDuocAn(int[] cardH, int[] cardAn) {
		List<int> cardOther;
		List<int> PhomAn = new List<int>();
		//Loai bo nhung phom da an trc do
		List<int[]> listPhomTT = GetPhomTrenTayMultiArray(cardH, out cardOther);
		for (int i = 0; i < listPhomTT.Count; i++) {
			List<int> l = listPhomTT[i].ToList();
			for (int j = 0; j < cardAn.Length; j++) {
				if (l.Contains(cardAn[j])) {
					if (l.Count < 5) {
						PhomAn.AddRange(listPhomTT[i]);
						listPhomTT.RemoveAt(i);
						i--;
						cardAn[j] = 53;
						break;
					} else {
						int indexCA = GetIndexCardInArray(cardAn, cardAn[i]);
						if (indexCA >= cardAn.Length - 2) {
							PhomAn.AddRange(listPhomTT[i]);
							listPhomTT.RemoveAt(i);
							i--;
							cardAn[j] = 53;
							break;
						}
					}
				}
			}
		}
		for (int i = 0; i < listPhomTT.Count; i++) {
			for (int j = 0; j < PhomAn.Count; j++) {
				List<int> l = listPhomTT[i].ToList();
				if (l.Contains(PhomAn[j])) {
					listPhomTT.RemoveAt(i);
					i--;
					break;
				}
			}
		}
		for (int i = 0; i < listPhomTT.Count; i++) {
			for (int j = 0; j < listPhomTT[i].Length; j++) {
				if (!PhomAn.Contains(listPhomTT[i][j])) {
					PhomAn.Add(listPhomTT[i][j]);
				}
			}
		}
		return PhomAn.ToArray();
	}
	/// <summary>
	/// Sap xep bai tren tay, uu tien phom
	/// </summary>
	/// 
	#region Sap Xep Bai Theo Chat
	static int[] SapXepBaiTheoChat(int[] cardHand) {
		List<int> lResult = new List<int>();
		for (int i = 0; i < 4; i++) {
			List<int> lCungChat = new List<int>();
			for (int j = 0; j < cardHand.Length; j++) {
				int s = GetType(cardHand[j]);
				if (i + 1 == s) {
					if (!lCungChat.Contains(cardHand[j])) {
						lCungChat.Add(cardHand[j]);
					}
				}
			}
			int[] arrCardCungChat = TypeCardMauBinh.SortArrCard(lCungChat.ToArray());
			lCungChat = new List<int>(arrCardCungChat);
			if (lCungChat.Count > 0)
				lResult.AddRange(lCungChat);
		}
		if (lResult.Count > 0)
			return lResult.ToArray();
		return null;
	}
	#endregion
	public static int[] SortCardTaLa(int[] cardH, List<int> CardEatted, ref int isTangDan) {
		List<int> result = new List<int>();
		List<int> CardAn = new List<int>();
		CardAn.AddRange(CardEatted);
		if (CardAn != null && CardAn.Count > 0) {
			result.AddRange(GetPhomDuocAn(cardH, CardAn.ToArray()));
		} else
			result.AddRange(GetPhomTrenTayOneArray(cardH));

		int[] temp = cardH.Except(result).ToArray();//lay cai ko chung giua phom va card tt
		List<int> result2 = new List<int>();
		switch (isTangDan) {
		case 1:
			result2.AddRange(TypeCardMauBinh.SortArrCard(temp));
			isTangDan = 2;
			break;
		case 2:
			var res = temp.GroupBy(x => GetType(x)).Select(g => g.OrderBy(y => GetValue(y)));
			foreach (var item in res) {
				result2.AddRange(item.ToList());
			}
			isTangDan = 3;
			break;
		case 3:
			result2.AddRange(SapXepBaiTheoChat(temp));
			isTangDan = 1;
			break;
		}

		result.AddRange(result2);
		return result.ToArray();
	}
	static int GetIndexCardInArray(int[] arr, int idC) {
		for (int i = 0; i < arr.Length; i++) {
			if (idC == arr[i]) {
				return i;
			}
		}
		return -1;
	}

	public static bool CheckPhom(int[] arr) {
		List<int[]> lResult = new List<int[]>();
		List<int> lTemp = new List<int>();
		#region Lay bai phom cung gia tri
		var result = arr.GroupBy(x => GetValue(x));

		foreach (var item in result) {
			if (item.ToList().Count == arr.Length) {
				return true;
			}
		}

		#endregion
		#region Lay bai phom cung chat
		var result2 = arr.GroupBy(a => GetType(a))
			.Select(g => g.OrderBy(x => GetValue(x))
				.GroupAdjacentBy((x, y) => GetValue(x) + 1 == GetValue(y)));

		foreach (var item in result2) {
			foreach (var item2 in item) {
				if (item2.ToList().Count == arr.Length) {
					return true;
				}
			}
		}
		#endregion

		return false;
	}
}
