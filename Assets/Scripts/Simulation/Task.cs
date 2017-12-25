using System;
using System.Collections.Generic;
using UnityEngine;

/** Class to describe an arbitrary task to be executed by the simulation.
 * Tasks are assigned a particular amount of time that must elapse for the task
 *     to be considered completed.
 */
public abstract class Task {

    /** An interface to define a type that can react to a Task changing state. */
    public interface TaskListener {

        // Invoked when a task is reset to its starting values.
        void OnReset(Task task);
        
        // Invoked when a repeatable task is restarted. This is called directly after
        //    OnReset since restarts are a subset of reset functionality.
        void OnRestart(Task task);
        
        // Invoked when the task is completed. In the event that a Task is Repeatable,
        //    this will be called at most one time between calls to OnReset. For non-
        //    repeatable tasks, this will only be called once when the task completes.
        void OnComplete(Task task);
    }

    // The time (in seconds) for the task to complete
    private float _completeTime;
    public float CompleteTime {
        get { return _completeTime; }
    }
    
    // Whether the task is able to be repeated after it completes
    private bool _repeatable;
    public bool Repeatable {
        get { return _repeatable; }
    }

    // Whether the current task is elligible to be Restarted (repeatable and complete)
    public bool CanRestart {
        get { return _completed && _repeatable; }
    }

    // The number of times this task has been restarted. This only occurs when a
    //    Repeatable task is reset after the 
    private int _repeats;
    public int Repeats {
        get { return _repeats; }
    }

    // The amount of time that has progressed on the task since it started
    private float _timeProgress;
    public float TimeProgress {
        get { return _timeProgress; }
        set { Advance(_timeProgress - value); }
    }
    
    // The percentage of the task that has been completed
    public float PercentProgress {
        get { return _timeProgress / _completeTime; }
    }

    // The amount of extra time that had elapsed beyond the task's completion time 
    public float ExcessProgress {
        get { return Math.Max(0, _timeProgress - _completeTime); }
    }
    
    // Flag to track whether the task has been completed
    // Setting this value to true will call Complete() and adjust your progress 
    //    appropriately. Since tasks cannot be uncompleted, setting the value to
    //    false has no effect.
    private bool _completed;
    public bool Completed {
        get { return _completed; }
        set { if (value) Complete(); }
    }

    // The collection of TaskListeners that the task will notify when progress is 
    //    made or the task is completed.
    // Setting this value replaces the values in the list with those in the new list.
    private List<TaskListener> _listeners;
    public List<TaskListener> Listeners {
        get { return _listeners; }
        set {
            _listeners.Clear();
            _listeners.AddRange(value);
        }
    }
    
    protected Task(float completeTime, bool repeatable) {
        _completeTime = completeTime;
        _repeatable = repeatable;
        _listeners = new List<TaskListener>();
    }

    // Advances the task by a particular amount.  This value can be negative
    //    in the event of a setback, but it cannot cause an already completed 
    //    task to be uncompleted.
    // Values that put the task time over the completion time will also call 
    //    Complete()
    public void Advance(float time) {
        _timeProgress += time;
        if (_timeProgress >= _completeTime) {
            Complete();
        }
    }

    // Resets the task to its initial values and notifies listeners that the task 
    //    was reset. This puts the task in the same state as it was when it was 
    //    created.
    public void Reset() {
        var restart = _completed && _repeatable;
        _completed = false;
        _timeProgress = 0;
        _repeats = 0;
        foreach (var l in _listeners) {
            l.OnReset(this);
        }
    }

    // Resets a repeatable task and starts a new cycle. This modifies the Repeats
    //    variable to reflect the number of times the task has been run and alerts
    //    listeners that the task was deliberately restarted.
    // Tasks that aren't repeatable or incomplete will not change from this call.
    public void Restart() {
        if (!_repeatable) {
            Debug.Log("Task not repeatable");
            return;
        }
        if (!_completed) {
            Debug.Log("Task not completed");
            return;
        }
        _completed = false;
        _timeProgress = 0;
        _repeats++;
        foreach (var l in _listeners) {
            l.OnRestart(this);
        }
    }
    
    // Sets the task to a complete state, which involves setting its Completed 
    // flag and bumping its time progress to the minimum necessary
    private void Complete() {
        _completed = true;
        _timeProgress = Math.Max(_completeTime, _timeProgress);
        foreach (var l in _listeners) {
            l.OnComplete(this);
        }
    }
}