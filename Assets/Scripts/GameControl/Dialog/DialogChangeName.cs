using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppConfig;
using System.Text.RegularExpressions;

public class DialogChangeName : MonoBehaviour {
	[SerializeField]
	Text txt_current_name;
	[SerializeField]
	InputField ip_new_name;

	void OnEnable(){
		txt_current_name.text = string.Format (ClientConfig.Language.GetText ("change_name_tenhientai"), ClientConfig.UserInfo.DISPLAY_NAME);
	}

	public void OnClickChangeName(){
		string nameIp = ip_new_name.text.Trim ();
		if (string.IsNullOrEmpty(nameIp) || (nameIp.Length < 5 && nameIp.Length > 20) || !Regex.IsMatch(nameIp, @"^([a-zA-Z0-9])+$")) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_tenkhonghople"));
			return;
		}
		SendData.onChangeName (nameIp);
	}
}
