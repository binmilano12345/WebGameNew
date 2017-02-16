using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchOutHide : MonoBehaviour {
    RectTransform rect1;
    // Use this for initialization
    void Start () {
        rect1 = GetComponent<Image>().rectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && rect1 != null) {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect1, Input.mousePosition)) {
                gameObject.SetActive(false);
            }
        }
    }
}
