using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryGroupTaiXiu : MonoBehaviour {
	[SerializeField]
	Transform tf_parent;

	GameObject objRankUI;
	//public void Init(List<DataItemHistoryTaiXiu> list){
	//	if (objRankUI == null) {
	//		LoadAssetBundle.LoadPrefab (AppConfig.BundleName.PREFAPS, AppConfig.PrefabsName.PRE_ITEM_HISTORY_TX, (objPre) => {
	//			objRankUI = objPre;
	//			objRankUI.SetActive(false);
	//			for (int i = 0; i < list.Count; i++) {
	//				GameObject obj = Instantiate (objRankUI);
	//				obj.transform.SetParent (tf_parent);
	//				obj.transform.localScale = Vector3.one;
	//				obj.SetActive(true);
	//				obj.GetComponent<ItemHistoryTXUI>().SetUI (list [i]);
	//			}
	//			PopupAndLoadingScript.instance.HideLoading ();
	//		});
	//	} else {
	//		int indexCount = tf_parent.childCount;
	//		int i = 0;
	//		for (i = 0; i < indexCount; i++) {
	//			tf_parent.GetChild(i).GetComponent<ItemHistoryTXUI>().SetUI(list [i]);
	//		}
	//		for (i = indexCount; i < list.Count; i++) {
	//			GameObject obj = Instantiate (objRankUI);
	//			obj.transform.SetParent (tf_parent);
	//			obj.transform.localScale = Vector3.one;
	//			obj.SetActive(true);
	//			obj.GetComponent<ItemHistoryTXUI>().SetUI (list [i]);
	//		}
	//		PopupAndLoadingScript.instance.HideLoading ();
	//	}
	//	OnShow();
	//}
	public void OnShow() { 
		gameObject.SetActive (true);
	}
}
