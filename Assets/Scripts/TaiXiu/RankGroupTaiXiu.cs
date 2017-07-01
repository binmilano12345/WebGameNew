using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankGroupTaiXiu : MonoBehaviour {
	[SerializeField]
	Transform tf_parent_day, tf_parent_week;
	[SerializeField]
	Toggle tg_day, tg_week;
	GameObject objRankUI;
	//public void Init(List<DataItemRankTaiXiu> list){
	//	if (objRankUI == null) {
	//		LoadAssetBundle.LoadPrefab (AppConfig.BundleName.PREFAPS, AppConfig.PrefabsName.PRE_ITEM_RANK_TX, (objPre) => {
	//			objRankUI = objPre;
	//			for (int i = 0; i < list.Count; i++) {
	//				GameObject obj = Instantiate (objRankUI);
	//				if (list [i].TypeID == 0) {
	//					obj.transform.SetParent (tf_parent_day);
	//				} else {
	//					obj.transform.SetParent (tf_parent_week);
	//				}

	//				obj.transform.localScale = Vector3.one;
	//				obj.GetComponent<ItemRankTXUI>().SetUI (list [i]);
	//			}
	//			PopupAndLoadingScript.instance.HideLoading ();
	//		});
	//	} else {
	//		for (int i = 0; i < list.Count; i++) {
	//			GameObject obj = Instantiate (objRankUI);
	//			if (list [i].TypeID == 0) {
	//				obj.transform.SetParent (tf_parent_day);
	//			} else {
	//				obj.transform.SetParent (tf_parent_week);
	//			}
	//			obj.transform.localScale = Vector3.one;
	//			obj.GetComponent<ItemRankTXUI>().SetUI (list [i]);
	//		}

	//		PopupAndLoadingScript.instance.HideLoading ();
	//	}

	//	gameObject.SetActive (true);
	//}
	//[Beebyte.Obfuscator.SkipRename]
	public void OnChangeTabDay(){
		if (tg_day.isOn && tf_parent_day.childCount <= 0) {
			TaiXiuViewScript.instance.RankRequest (0);
		}
	}
	//[Beebyte.Obfuscator.SkipRename]
	public void OnChangeTabWeek(){
		if (tg_week.isOn && tf_parent_week.childCount <= 0) {
			TaiXiuViewScript.instance.RankRequest (1);
		}
	}

}
