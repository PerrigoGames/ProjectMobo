using UnityEngine;

public class EasyComputer : Computer, Task.TaskListener {

    public float Speed = 1f;
    public float TaskLength = 10f;
    public Transform GemPrefab;

    protected override void Start() {
        base.Start();
        var task = new DurdleTask(TaskLength);
        task.Listeners.Add(this);
        tasks.Add(task);
    }
    
    public override void Tick(float time) {
        WorkOnTask(time * Speed);
        UpdateProgressBar();
    }

    public void OnReset(Task task) {
        Debug.Log("Durdle reset");
    }

    public void OnRestart(Task task) {
        Debug.Log("Durdle restart");
    }

    public void OnComplete(Task task) {
        Debug.Log("Durdle complete");
        if (GemPrefab != null) {
            var gem = Instantiate(GemPrefab, transform);
        }
    }
}