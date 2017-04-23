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
        int index = 0;
        switch (result) {
            case 1://chan
                index = 2; 
                break;
            case 2:
                index = 3; //le
                break;
            case 3:
                index = 0; //4 vang
                break;
            case 4://3 trang 1 vang
                index = 3;
                break;
            case 5:
                index = 1; //1 trang 3 vang
                break;
            case 6:
                index = 4; // 4 trang
                break;
        }
        for (int i = 0; i < index; i++) {
            Image image = img_dices[i];
			LoadAssetBundle.LoadSprite(image, BundleName.UI, UIName.UI_XD_WHITE);
        }
        for (int i = index; i < img_dices.Length; i++) {
            Image image = img_dices[i];
            LoadAssetBundle.LoadSprite(image, BundleName.UI, UIName.UI_XD_RED);
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
