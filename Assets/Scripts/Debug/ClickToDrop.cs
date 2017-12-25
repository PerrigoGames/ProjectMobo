using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDrop : MonoBehaviour {

	void OnMouseDown() {
		Debug.Log("Clicked");
		if (GetComponent<Rigidbody>() != null) {
			GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
