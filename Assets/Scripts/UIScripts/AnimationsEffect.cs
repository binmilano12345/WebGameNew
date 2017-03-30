using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsEffect : MonoBehaviour
{
	[SerializeField]
	Sprite[] spriteAnims;
	[SerializeField]
	Image img;
	[SerializeField]
	float speed = 1;
	[SerializeField]
	bool isLoop = false;

	void Start ()
	{
		InvokeRepeating ("RunAnim", 0.5f, 0.5f);
	}
	int index = 0;

	void RunAnim ()
	{
		img.sprite = spriteAnims [index];
		index++;
		if (index >= spriteAnims.Length) {
			index = 0;
		}
	}
}
