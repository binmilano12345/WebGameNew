using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppConfig;
using Us.Mobile.Utilites;
using System;

public class DialogInfoPlayer : MonoBehaviour {
	[SerializeField]
	Text txt_name, txt_id, txt_money, txt_sdt, txt_sovandau, txt_gold_max, txt_tyle_thang, txt_cap_nhat;
	[SerializeField]
	RawImage raw_avata;

	void OnEnable(){
		StartCoroutine (Init ());
	}
	IEnumerator Init(){
		yield return new WaitForEndOfFrame ();
		txt_name.text = ClientConfig.UserInfo.UNAME;
		txt_money.text = MoneyHelper.FormatMoneyNormal (ClientConfig.UserInfo.CASH_FREE) + GameConfig.MONEY_UNIT_VIP;
		txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
		txt_sdt.text = "SĐT:" + ClientConfig.UserInfo.PHONE;
		txt_gold_max.text = string.Format (ClientConfig.Language.GetText ("info_player_sogoldmax"), ClientConfig.UserInfo.SO_TIEN_MAX);
		txt_cap_nhat.text = string.Format (ClientConfig.Language.GetText ("info_player_capnhat"), ClientConfig.UserInfo.LOGIN_END);
		LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, ClientConfig.UserInfo.AVATAR_ID + "");
		SetThangThua ();
	}

	void SetThangThua(){
		string thang = ClientConfig.UserInfo.SO_LAN_THANG;
		string thua = ClientConfig.UserInfo.SO_LAN_THUA;
//		Debug.LogError (thang + "  ===  " + thua);
		string[] lThang = thang.Split (',');
		string[] lThua = thua.Split (',');
		int nThang = 0, nThua = 0;
		try {
			for (int i = 0; i < lThang.Length; i++) {
				nThang += int.Parse(lThang[i].Split('-')[1].Trim());
				nThua += int.Parse(lThua[i].Split('-')[1].Trim());
			}

			txt_sovandau.text = string.Format (ClientConfig.Language.GetText ("info_player_sovandau"), (nThang + nThua));
			if (nThang + nThua == 0) {
				txt_tyle_thang.text = string.Format (ClientConfig.Language.GetText ("info_player_tylethang"), 0);
			} else {
				float tylethang = (float) nThang * 100 / ((float) (nThang + nThua));
				txt_tyle_thang.text = string.Format (ClientConfig.Language.GetText ("info_player_tylethang"), tylethang);
			}
		} catch (Exception e) {
			Debug.LogException (e);
		}
	}

	public void OnClickChangeName(){
		LoadAssetBundle.LoadScene (SceneName.SUB_CHANGE_NAME, SceneName.SUB_CHANGE_NAME);
		OnHide();
	}	
	public void OnClickChangePass(){
		LoadAssetBundle.LoadScene (SceneName.SUB_CHANGE_PASS, SceneName.SUB_CHANGE_PASS);
		OnHide();
	}	
	public void OnClickChangeAvatar(){
		LoadAssetBundle.LoadScene (SceneName.SUB_CHANGE_AVATAR, SceneName.SUB_CHANGE_AVATAR);
		OnHide();
	}
	 void OnHide(){
		GetComponent<UIPopUp> ().HideDialog ();
	}
}
