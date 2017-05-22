using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class XocDiaPlayer : BasePlayer
{
	const float TIME = 0.1f;
	[SerializeField]
	Transform tf_chip;
	//Dat cuoc.
	public List<GameObject> CurrentChipCua_0 = new List<GameObject> ();
	public List<GameObject> CurrentChipCua_1 = new List<GameObject> ();
	public List<GameObject> CurrentChipCua_2 = new List<GameObject> ();
	public List<GameObject> CurrentChipCua_3 = new List<GameObject> ();
	public List<GameObject> CurrentChipCua_4 = new List<GameObject> ();
	public List<GameObject> CurrentChipCua_5 = new List<GameObject> ();

	//Cache chip dat cuoc.
	private List<GameObject> CacheChipDatCua_0 = new List<GameObject> ();
	private List<GameObject> CacheChipDatCua_1 = new List<GameObject> ();
	private List<GameObject> CacheChipDatCua_2 = new List<GameObject> ();
	private List<GameObject> CacheChipDatCua_3 = new List<GameObject> ();
	private List<GameObject> CacheChipDatCua_4 = new List<GameObject> ();
	private List<GameObject> CacheChipDatCua_5 = new List<GameObject> ();

	public void	ActionChipDatCuoc (int cua, Vector3 pos, GameObject objPre)
	{
		GameObject obj = GetObjChipHide (objPre);
		switch (cua) {
		case 0:
			CurrentChipCua_0.Add (obj);
			CacheChipDatCua_0.Add (obj);
			break;
		case 1:
			CurrentChipCua_1.Add (obj);
			CacheChipDatCua_1.Add (obj);
			break;
		case 2:
			CurrentChipCua_2.Add (obj);
			CacheChipDatCua_2.Add (obj);
			break;
		case 3:
			CurrentChipCua_3.Add (obj);
			CacheChipDatCua_3.Add (obj);
			break;
		case 4:
			CurrentChipCua_4.Add (obj);
			CacheChipDatCua_4.Add (obj);
			break;
		case 5:
			CurrentChipCua_5.Add (obj);
			CacheChipDatCua_5.Add (obj);
			break;
		}

		obj.transform.DOMove (pos, TIME);
		XocDiaControl.instance.AddChipToList (cua, obj);
	}

	public void	ActionChipDatX2 (int cua, Vector3 pos)
	{
		int size = CurrentChipCua_0.Count;
		for (int i = 0; i < size; i++) {
			GameObject obj = GetObjChipHide (CurrentChipCua_0 [i]);
			switch (cua) {
			case 0:
				CurrentChipCua_0.Add (obj);
				CacheChipDatCua_0.Add (obj);
				break;
			case 1:
				CurrentChipCua_1.Add (obj);
				CacheChipDatCua_1.Add (obj);
				break;
			case 2:
				CurrentChipCua_2.Add (obj);
				CacheChipDatCua_2.Add (obj);
				break;
			case 3:
				CurrentChipCua_3.Add (obj);
				CacheChipDatCua_3.Add (obj);
				break;
			case 4:
				CurrentChipCua_4.Add (obj);
				CacheChipDatCua_4.Add (obj);
				break;
			case 5:
				CurrentChipCua_5.Add (obj);
				CacheChipDatCua_5.Add (obj);
				break;
			}

			obj.transform.DOMove (pos, TIME);

			XocDiaControl.instance.AddChipToList (cua, obj);
		}
	}

	public void ActionTraTienCuoc (long cua0, long cua1, long cua2, long cua3, long cua4, long cua5)
	{
		if (cua0 > 0) {
			for (int i = 0; i < CurrentChipCua_0.Count; i++) {
				GameObject obj = CurrentChipCua_0 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_0.Remove(obj);
					});
					XocDiaControl.instance.RemoveChipFromList (0, obj);
				}
			}
		}

		if (cua1 > 0) {
			for (int i = 0; i < CurrentChipCua_1.Count; i++) {
				GameObject obj = CurrentChipCua_1 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_1.Remove(obj);

					});
					XocDiaControl.instance.RemoveChipFromList (1, obj);
				}
			}
		}

		if (cua2 > 0) {
			for (int i = 0; i < CurrentChipCua_2.Count; i++) {
				GameObject obj = CurrentChipCua_2 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_2.Remove(obj);
					});
					XocDiaControl.instance.RemoveChipFromList (2, obj);
				}
			}
		}

		if (cua3 > 0) {
			for (int i = 0; i < CurrentChipCua_3.Count; i++) {
				GameObject obj = CurrentChipCua_3 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_3.Remove(obj);
					});
					XocDiaControl.instance.RemoveChipFromList (3, obj);
				}
			}
		}

		if (cua4 > 0) {
			for (int i = 0; i < CurrentChipCua_4.Count; i++) {
				GameObject obj = CurrentChipCua_4 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_4.Remove(obj);
					});
					XocDiaControl.instance.RemoveChipFromList (4, obj);
				}
			}
		}

		if (cua5 > 0) {
			for (int i = 0; i < CurrentChipCua_5.Count; i++) {
				GameObject obj = CurrentChipCua_5 [i];
				if (obj.activeSelf) {
					obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
						obj.SetActive (false);
						CurrentChipCua_5.Remove(obj);
					});
					XocDiaControl.instance.RemoveChipFromList (5, obj);
				}
			}
		}
	}

	public void ActionChipToPlayerWin(int cua1, int cua2){
		Debug.LogError ("cua 1: " + cua1 + "  cua 2: " + cua2);
		List<GameObject> list = new List<GameObject>();
		if (cua1 == 0) {
			list.AddRange(CurrentChipCua_0);
		} else {
			list.AddRange(CurrentChipCua_1);
		}

		switch (cua2) {
		case 2:
			list.AddRange(CurrentChipCua_2);
			break;
		case 3:
			list.AddRange(CurrentChipCua_3);
			break;
		case 4:
			list.AddRange(CurrentChipCua_4);
			break;
		case 5:
			list.AddRange(CurrentChipCua_5);
			break;
		}

		for (int i = 0; i < list.Count; i++) {
			GameObject obj = list [i];
			if (obj.activeSelf) {
				obj.transform.DOMove (transform.position, TIME).OnComplete (() => {
					obj.SetActive (false);
				});
			}
		}
	}
	GameObject GetObjChipHide (GameObject objPre)
	{
		GameObject obj;
		for (int i = 0; i < tf_chip.childCount; i++) {
			obj = null;
			obj = tf_chip.GetChild (i).gameObject;
			if (!obj.activeSelf) {
				obj.SetActive (true);
				return obj;
			}
		}

		obj = null;
		obj = Instantiate (objPre);
		obj.transform.SetParent (tf_chip);
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;

		obj.SetActive (true);

		return obj;
	}
}
