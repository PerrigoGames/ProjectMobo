using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour {

	public float HoverDistance = 0.25f;
	public float HoverCycleTime = 2f;
	
	void Start () {
		var startPos = transform.localPosition;
		startPos.y -= HoverDistance / 2;
		transform.localPosition = startPos;
		LeanTween.moveLocalY(gameObject, startPos.y + HoverDistance, HoverCycleTime / 2)
			.setEase(LeanTweenType.easeInOutSine)
			.setLoopPingPong();
	}
}
