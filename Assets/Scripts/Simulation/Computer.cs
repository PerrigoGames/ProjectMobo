using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour, GameTickReceiver {

	protected List<Task> tasks;
	private Image progressImage;

	public Computer() {
		tasks = new List<Task>();
	}

	protected virtual void Start() {
		var manager = GameTickManager.FindManagerInGame();
		if (manager != null) {
			manager.Subscribe(this);
		}
		var progressEntity = transform.Find("Canvas").Find("Progress");
		if (progressEntity != null) {
			progressImage = progressEntity.GetComponent<Image>();
			progressImage.type = Image.Type.Filled;
		}
	}

	public virtual void Tick(float time) {
		UpdateProgressBar();
	}

	protected void WorkOnTask(float time) {
		if (tasks.Count == 0) return;
		var task = tasks[0];
		while (task != null && time > 0) {
			task.Advance(time);
			time = task.ExcessProgress;
			if (task.Completed) {
				tasks.RemoveAt(0);
				if (task.CanRestart) {
					task.Restart();
					tasks.Add(task);
				}
				task = tasks[0];
			}
		}
	}

	protected void UpdateProgressBar() {
		if (progressImage != null) {
//			Debug.Log("Updating progress=" + (tasks.Count > 0 ? 
//				tasks[0].PercentProgress : 0));
			progressImage.fillAmount = tasks.Count > 0 ? 
				tasks[0].PercentProgress : 0;
		}
	}
}
