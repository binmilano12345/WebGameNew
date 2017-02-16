using UnityEngine;
using System.Collections;
//using DG.Tweening;
using UnityEngine.SceneManagement;
//using AppConfig;

public class LoadAssetView : MonoBehaviour {
    public static LoadAssetView Instance;

    public RectTransform Logo;
    public RectTransform ProgressBar;

    [HideInInspector]
    public bool HideComplete = false;

    [HideInInspector]
    public bool CanUnLoad = false;

    // Use this for initialization
    void Awake() {
        Instance = this;
    }

    public void HideScene() {
        //if (Logo != null) Logo.transform.DOLocalMoveY(264f, 0.2f);
        //if (Logo != null) Logo.gameObject.SetActive(false);
        //if (ProgressBar != null) ProgressBar.DOAnchorPosY (-630, 0.2f, false).OnComplete(()=>{
        //HideComplete = true;
        //});
    }

    void Update() {
        if (HideComplete && CanUnLoad) {
            //SceneManager.UnloadScene (SceneName.LOADASSET_SCENE_NAME);
        }
    }
}
