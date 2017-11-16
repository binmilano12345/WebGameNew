using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class LoadSceneAtTheBegining : MonoBehaviour {
	const string MENU_RUN = "SBoy/Run Load %LEFT";
	const string MENU_RED = "SBoy/Run Load Red %UP";

	[MenuItem(MENU_RUN)]
    public static void RunLoad() {
        //SceneManager.LoadScene(0);
        if (EditorApplication.isPlaying) {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Res/Scenes/load.unity", OpenSceneMode.Single);
        EditorApplication.isPlaying = true;
    }


	[MenuItem(MENU_RED)]
	static void RunLoadInfo_Red() {
		string nameApp = "ATM";
		if (EditorUtility.DisplayDialog("Set Config", "Set config " + nameApp + "?", "OK", "Cancel")) {
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {
				PlayerSettings.Android.keystoreName = "Assets/Res/keystore/user_atm.keystore";
				PlayerSettings.Android.keystorePass = "123456s";
				PlayerSettings.Android.keyaliasName = "game";
				PlayerSettings.Android.keyaliasPass = "123456s";
			}
			PlayerSettings.productName = nameApp;
			PlayerSettings.applicationIdentifier = "com.sboy.atmcard";
			//string pathFile = EditorUtility.SaveFolderPanel("Chọn thư mục", "", "");
			//BuildPipeline.BuildPlayer(new string[] { "Assets/Res/Scenes/load.unity" }, pathFile, BuildTarget.Android, BuildOptions.None); 
		}
	}

}
#endif
