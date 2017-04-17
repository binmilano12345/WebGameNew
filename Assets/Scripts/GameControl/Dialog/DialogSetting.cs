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
        tg_sound.isOn = SettingConfig.IsSound ;
        tg_auto_ready.isOn = SettingConfig.IsAutoReady;
        tg_invite.isOn = SettingConfig.IsInvite;

        tg_sound.onValueChanged.AddListener(OnChangeAmThanh);
        tg_auto_ready.onValueChanged.AddListener(OnChangeRung);
        tg_invite.onValueChanged.AddListener(OnChangeNhanLoiMoi);
    }

    public void OnChangeAmThanh(bool value) {
        SettingConfig.IsSound = value;
    }
    public void OnChangeNhanLoiMoi(bool value) {
        SettingConfig.IsInvite = value;
    }
    public void OnChangeRung(bool value) {
        SettingConfig.IsAutoReady = value;
    }
}
