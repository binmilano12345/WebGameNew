using System.Collections;
using System.Collections.Generic;
using AppConfig;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LiengPlayer : BasePlayer {
	public ArrayCard CardHand;
	public ChipControl chipControl;
	List<GameObject> chipPool = new List<GameObject>();
	//[SerializeField]
	GameObject objChip;
	[SerializeField]
	Text txt_diem;

	public override void SetInfo(DataBase.PlayerData playerData) {
		base.SetInfo(playerData);
		MoneyFollow = playerData.FolowMoney;
	}

	public long MoneyFollow { get; set; }
	private long _moneyChip;
	public long MoneyChip {
		get { return _moneyChip; }
		set {
			_moneyChip = value;
			Debug.LogError("Tien ng choi:   " + _moneyChip);
			chipControl.MoneyChip = _moneyChip;
		}
	}

	public void MoveChip(long money, Vector3 posTo) {
		if (objChip == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, "ChipInGame", (objPre) => {
				objChip = objPre;
				objChip.SetActive(false);
				for (int i = 0; i < 3; i++) {
					GameObject obj = Instantiate(objChip);
					obj.transform.SetParent(chipControl.transform);
					obj.transform.localScale = Vector3.one;
					obj.transform.position = chipControl.transform.position;
					obj.SetActive(true);
					obj.transform.DOMove(posTo, 0.4f).SetDelay(i * 0.2f).OnComplete(() => {
						//if (i >= 2) {
						//	MoneyChip += money;
						//}

						obj.SetActive(false);
					});
					chipPool.Add(obj);
				}
			});
		} else {
			for (int i = 0; i < chipPool.Count; i++) {
				GameObject obj = chipPool[i];
				obj.transform.position = chipControl.transform.position;
				obj.SetActive(true);
				obj.transform.DOMove(posTo, 0.4f).SetDelay(0.2f).OnComplete(() => {
					//if (i >= chipPool.Count - 1) {
					//	MoneyChip += money;
					//}
					obj.SetActive(false);
				});
			}
		}
	}

	public void SetDiemLieng(bool isActive, int[] array_int) {
		txt_diem.gameObject.SetActive(isActive);
		if (isActive)
			txt_diem.text = GameControl.TinhDiemLieng(array_int);
	}

	public void SetDiemBaCay(bool isActive, int[] array_int) {
		txt_diem.gameObject.SetActive(isActive);
		if (isActive)
			txt_diem.text = GameControl.TinhDiemBaCay(array_int);
	}

	public void SetDiemBaCay(string str_score) {
		txt_diem.gameObject.SetActive(true);
		txt_diem.text = str_score;
	}
}

