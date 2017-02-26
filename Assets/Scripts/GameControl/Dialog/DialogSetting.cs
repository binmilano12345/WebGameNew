using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSetting : MonoBehaviour {

    [SerializeField]
    Toggle tg_sound, tg_auto_ready, tg_invite;

    // Use this for initialization
    void Start () {
        tg_sound.isOn = SettingConfig.IsSound == 1 ? true : false;
        tg_auto_ready.isOn = SettingConfig.IsAutoReady == 1 ? true : false;
        tg_invite.isOn = SettingConfig.IsInvite == 1 ? true : false;

        tg_sound.onValueChanged.AddListener(OnChangeAmThanh);
        tg_auto_ready.onValueChanged.AddListener(OnChangeRung);
        tg_invite.onValueChanged.AddListener(OnChangeNhanLoiMoi);
    }

    public void OnChangeAmThanh(bool value) {
        SettingConfig.IsSound = value ? 1 : 0;
    }
    public void OnChangeNhanLoiMoi(bool value) {
        SettingConfig.IsInvite = value ? 1 : 0;
    }
    public void OnChangeRung(bool value) {
        SettingConfig.IsAutoReady = value ? 1 : 0;
    }
}
