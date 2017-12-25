using System.Collections.Generic;
using UnityEngine;

public class GameTickComp : MonoBehaviour, GameTickReceiver {

	private List<GameTickReceiver> receivers;

	public GameTickComp() {
		receivers = new List<GameTickReceiver>();
	}

	private void Start () {
		var manager = GameTickManager.FindManagerInGame();
		if (manager != null) {
			manager.Subscribe(this);
		}
	}

	public virtual void Tick(float time) { }
}