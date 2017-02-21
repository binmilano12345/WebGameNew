using AppConfig;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RegisterControl : MonoBehaviour {
    public static RegisterControl instance;
    [SerializeField]
    InputField ip_tk, ip_mk, ip_lai_mk;

    void Awake() {
        instance = this;
    }
    #region Click    
    public void OnClick_Register() {
        string tk = ip_tk.text.Trim();
        string mk = ip_mk.text.Trim();
        string lai_mk = ip_lai_mk.text.Trim();
        if (string.IsNullOrEmpty(tk) || (tk.Length < 5 && tk.Length > 20) || !Regex.IsMatch(tk, @"^([a-zA-Z0-9])+$")) {
            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_tenkhonghople"));
            return;
        } else if (string.IsNullOrEmpty(mk) || (mk.Length < 5 && mk.Length > 20) || !Regex.IsMatch(mk, @"^([a-zA-Z0-9])+$")) {
            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_matkhauyeu"));
            return;
        } else if (string.IsNullOrEmpty(lai_mk) || (lai_mk.Length < 5 && lai_mk.Length > 20) || !Regex.IsMatch(lai_mk, @"^([a-zA-Z0-9])+$")) {
            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_matkhauyeu"));
            return;
        } else if (!mk.Equals(lai_mk)) {
            PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("register_matkhau_khonggiongnhau"));
            return;
        }
        string imei = ClientConfig.HardWare.IMEI;//"352888065147086";
        SendData.onRegister(tk, mk, imei, false);
    }
    #endregion

    public void OnHide() {
        GetComponent<UIPopUp>().HideDialog();
    }
}
