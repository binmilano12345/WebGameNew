using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class LoadSceneAtTheBegining : MonoBehaviour {

    [MenuItem("SBoy/Run Load %LEFT")]
    public static void RunLoad() {
        //SceneManager.LoadScene(0);
        if (EditorApplication.isPlaying) {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/load.unity", OpenSceneMode.Single);
        EditorApplication.isPlaying = true;
    }
}
#endif
