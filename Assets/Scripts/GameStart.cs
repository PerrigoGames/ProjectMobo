using UnityEngine;

public class GameStart : MonoBehaviour {
	
	public GameObject MainMenu;
	
	// Use this for initialization
	void Start () {
		LeanTween.alphaCanvas(MainMenu.GetComponent<CanvasGroup>(), 0, 0f);
		Invoke("ShowMenu", 3);
	}

	void ShowMenu() {
		LeanTween.alphaCanvas(MainMenu.GetComponent<CanvasGroup>(), 1, 0.5f);
	}
}
