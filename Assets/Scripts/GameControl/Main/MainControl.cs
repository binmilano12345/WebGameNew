using AppConfig;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beebyte.Obfuscator;
//using Facebook.Unity;

public class MainControl : MonoBehaviour {
	public static MainControl instance;
    [SerializeField]
    ArrayCard cardHand;
	[SerializeField]
	Text txt_hotline;

    void Start() {
        GameControl.instance.UnloadScene(SceneName.SCENE_ROOM);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadGameScene();
        GameControl.instance.UnloadSubScene();
		SetHotline ();
//		StartCoroutine (InitFacebookSDK ());

//		Card.setCardType(1);
//		int[] arrr1 = new int[] {0,1,2,3,4};
//		int[] arrr2 = new int[] {5,6,7,8,9};
//		int[] arrr3 = new int[] {10,11,12};

//		TYPE_CARD typeC_1 = TypeCardMauBinh.GetTypeCardMauBinh (arrr1);
//		TYPE_CARD typeC_2 = TypeCardMauBinh.GetTypeCardMauBinh (arrr2);
		//		TYPE_CARD typeC_3 = TypeCardMauBinh.GetTypeCardMauBinh (arrr3);
		//		Debug.LogError (typeC_1 + "\n" + typeC_2+ "\n" + typeC_3);
//		string str = "";
//		for (int i = 0; i < arrr1.Length; i++) {
//			str += TypeCardMauBinh.GetValue (arrr1 [i]);
//		}
//		Debug.LogError (typeC_1 + "\n" + str);
	}

	public void SetHotline(){
		txt_hotline.text = "Hotline: " + GameConfig.HOT_LINE;
	}
	[SkipRename]
    public void OnClick_LoginFacebook() {
//		FB.LogInWithReadPermissions (perms, AuthCallback);
    }

[SkipRename]
    public void OnClick_Login() {
        LoadAssetBundle.LoadScene(SceneName.SUB_LOGIN, SceneName.SUB_LOGIN);
    }
	[SkipRename]
    public void OnClick_Reg() {
        LoadAssetBundle.LoadScene(SceneName.SUB_REGISTER, SceneName.SUB_REGISTER);
    }
	[SkipRename]
    public void OnClick_Setting() {
        LoadAssetBundle.LoadScene(SceneName.SUB_SETTING, SceneName.SUB_SETTING);
    }
	[SkipRename]
    public void OnClick_Hotline() {
		GameControl.instance.CallPhone (GameConfig.HOT_LINE);
    }
	/*
	#region Facebook control

	IEnumerator InitFacebookSDK ()
	{
		//		yield return new WaitUntil (() => !string.IsNullOrEmpty (GameConfig.FacebookId));
		//		if (string.IsNullOrEmpty (GameConfig.FacebookId)) {
		//			StartCoroutine (InitFacebookSDK ());
		//		}
		PopupAndLoadingScript.instance.ShowLoading ();
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			//			if (string.IsNullOrEmpty (GameConfig.FacebookId)) {
			FB.Init (InitCallback, OnHideUnity);
			//			} else {
			//				FB.Init (GameConfig.FacebookId, null, true, true, true, false, true, null, "en_US", OnHideUnity, InitCallback);
			//			}
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
		yield return new WaitForEndOfFrame ();
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}

		PopupAndLoadingScript.instance.HideLoading ();
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	List<string> perms = new List<string> () { "public_profile", "email", "user_friends" };

	private void AuthCallback (ILoginResult result)
	{
		if (FB.IsLoggedIn && !result.Cancelled && string.IsNullOrEmpty (result.Error)) {
			// AccessToken class will have session details
			string aToken = AccessToken.CurrentAccessToken.TokenString;
			ClientConfig.UserInfo.UNAME = "";
			ClientConfig.UserInfo.PASSWORD = "";
			ClientConfig.UserInfo.IS_FIRST_TIME_LOGIN_FACEBOOK = 1;

			SendData.doLogin("", "", (sbyte) 1, ClientConfig.HardWare.IMEI, "", (sbyte) 1, "", aToken, "", false);

			// Print current access token's granted permissions
			//foreach (string perm in aToken.Permissions) {
			//    Debug.Log(perm);
			//}
		} else {
			Debug.Log ("User cancelled login");
		}

//		Debug.LogError (result.ToString ());
	}

	#endregion
*/
}
