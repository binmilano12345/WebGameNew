using AppConfig;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainControl : MonoBehaviour {
	public static MainControl instance;
    [SerializeField]
    ArrayCard cardHand;
	[SerializeField]
	Text txt_hotline;
	void Awake(){
	
	}

    void Start() {
        GameControl.instance.UnloadScene(SceneName.SCENE_ROOM);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadGameScene();
        GameControl.instance.UnloadSubScene();
        PopupAndLoadingScript.instance.OnHideAll();
		SetHotline ();
	}

	public void SetHotline(){
		txt_hotline.text = "Hotline: " + GameConfig.HOT_LINE;
	}

    public void OnClick_LoginFacebook() {
    }
    public void OnClick_Login() {
        LoadAssetBundle.LoadScene(SceneName.SUB_LOGIN, SceneName.SUB_LOGIN);
    }
    public void OnClick_Reg() {
        LoadAssetBundle.LoadScene(SceneName.SUB_REGISTER, SceneName.SUB_REGISTER);
    }
    public void OnClick_Setting() {
        LoadAssetBundle.LoadScene(SceneName.SUB_SETTING, SceneName.SUB_SETTING);
    }
    public void OnClick_Hotline() {
		GameControl.instance.CallPhone (GameConfig.HOT_LINE);
    }
}
