using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAnimation : MonoBehaviour {

	public float AnimTime = 0.5f;
	public float RotationSpeed;
	public float MovementAmount = 0.25f;
	public float Delay;
	public LeanTweenType tweenType = LeanTweenType.linear;
	private float _angle;

	void Start() {
		LeanTween.moveLocalY(gameObject, transform.localPosition.y + MovementAmount, AnimTime)
			.setEase(tweenType)
			.setDelay(Delay);
		LeanTween.alpha(gameObject, 0, AnimTime)
			.setEase(tweenType)
			.setDelay(Delay);
		Destroy(gameObject, AnimTime + Delay);
	}
	
	void Update () {
		_angle += RotationSpeed * Time.deltaTime;
		if (_angle > 360) {
			_angle -= 360;
		}
		var target = Quaternion.Euler(-90, _angle, 0);
		transform.rotation = target;
	}
}
