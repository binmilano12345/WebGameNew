using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppConfig;
using System.Text.RegularExpressions;

public class DialogChangePass : MonoBehaviour {
	[SerializeField]
	InputField ip_old_pass, ip_new_pass, ip_again_pass;

	public void OnClickChangePass(){
		string old_pass = ip_old_pass.text.Trim ();
		string new_pass = ip_new_pass.text.Trim ();
		string again_pass = ip_again_pass.text.Trim ();

		if (string.IsNullOrEmpty(old_pass)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("setting_oldpassnull"));
			return;
		}else if (!old_pass.Equals(ClientConfig.UserInfo.PASSWORD)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("setting_oldpasserr"));
			return;
		} else if (string.IsNullOrEmpty(new_pass)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("setting_newpassnull"));
			return;
		} else if (!new_pass.Equals(again_pass)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_matkhau_khonggiongnhau"));
			return;
		} else if (!Regex.IsMatch(new_pass, @"^([a-zA-Z0-9])+$")) {
			PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_matkhauyeu"));
			return;
		}
		string sms = GameConfig.SMS_CHANGE_PASS_SYNTAX
		             + " " + ClientConfig.UserInfo.USER_ID
		             + " " + old_pass
		             + " " + new_pass;
		PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("gui_sms_doi_mk"), delegate {
			GameControl.instance.SendSMS (GameConfig.SMS_CHANGE_PASS_NUMBER, sms);
		});
		//doi mat khau
	}
}
