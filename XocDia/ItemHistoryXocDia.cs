using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHistoryXocDia : MonoBehaviour {
    [SerializeField]
    Image[] img_dices;
	[SerializeField]
	GameObject obj_arrow;

    public int result { get; set; }
	public void SetInfo(int result) {
        this.result = result;
        
		for (int i = 0; i < img_dices.Length; i++) {
			LoadAssetBundle.LoadSprite(img_dices[i], BundleName.UI, i < result ? UIName.UI_XD_RED : UIName.UI_XD_WHITE);
        }
    }

	public void SetShowArrow(bool isShow){
		obj_arrow.transform.parent.gameObject.SetActive (isShow);
	}

	public void SetArrowUp(bool isDown){
		Vector3 vt = obj_arrow.transform.localScale;
		vt.y = isDown ? 1 : -1;
		obj_arrow.transform.localScale = vt;
	}
}
