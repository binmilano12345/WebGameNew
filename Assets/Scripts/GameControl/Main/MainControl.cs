using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour {
    [SerializeField]
    ArrayCard cardHand;

    void Start() {
        GameControl.instance.UnloadSubScene();
        GameControl.instance.UnloadScene(SceneName.SCENE_ROOM);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadGameScene();
        GameControl.instance.UnloadSubScene();

        //int[] cardH = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        //string msg = "";
        //string[] chat = new string[] { "Bích", "Tép", "Rô", "Cơ" };
        //for (int i = 0; i < cardH.Length; i++) {
        //    msg += " " + AutoChooseCard.GetValue(cardH[i]) + " " + chat[AutoChooseCard.GetType(cardH[i])];
        //}
        //Debug.LogError(msg);
        //cardHand.InitDemo(cardH);

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

    }
}
