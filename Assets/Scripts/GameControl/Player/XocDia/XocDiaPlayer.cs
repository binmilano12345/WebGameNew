using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class XocDiaPlayer : BasePlayer
{
	const float RATE_TIME = 1000;
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

		float time = Vector3.Distance (obj.transform.position, pos) / RATE_TIME;
		obj.transform.DOMove (pos, time);
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

			float time = Vector3.Distance (obj.transform.position, pos) / RATE_TIME;
			obj.transform.DOMove (pos, time);
		}
	}

	public void ActionTraTienCuoc (long cua0, long cua1, long cua2, long cua3, long cua4, long cua5)
	{
		if (cua0 > 0) {
			for (int i = 0; i < CurrentChipCua_0.Count; i++) {
				if (CurrentChipCua_0 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_0 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_0 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_0 [i].SetActive (false);
					});
				}
			}
		}

		if (cua1 > 0) {
			for (int i = 0; i < CurrentChipCua_1.Count; i++) {
				if (CurrentChipCua_1 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_1 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_1 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_1 [i].SetActive (false);
					});
				}
			}
		}

		if (cua2 > 0) {
			for (int i = 0; i < CurrentChipCua_2.Count; i++) {
				if (CurrentChipCua_2 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_2 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_2 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_2 [i].SetActive (false);
					});
				}
			}
		}

		if (cua3 > 0) {
			for (int i = 0; i < CurrentChipCua_3.Count; i++) {
				if (CurrentChipCua_3 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_3 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_3 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_3 [i].SetActive (false);
					});
				}
			}
		}

		if (cua4 > 0) {
			for (int i = 0; i < CurrentChipCua_4.Count; i++) {
				if (CurrentChipCua_4 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_4 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_4 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_4 [i].SetActive (false);
					});
				}
			}
		}

		if (cua5 > 0) {
			for (int i = 0; i < CurrentChipCua_5.Count; i++) {
				if (CurrentChipCua_5 [i].activeSelf) {
					float time = Vector3.Distance (CurrentChipCua_5 [i].transform.position, transform.position) / RATE_TIME;
					CurrentChipCua_5 [i].transform.DOMove (transform.position, time).OnComplete (() => {
						CurrentChipCua_5 [i].SetActive (false);
					});
				}
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
