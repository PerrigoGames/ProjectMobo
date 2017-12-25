using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTickManager : MonoBehaviour {
    
    private const int TICKS_PER_SECOND = 2;
    private const float TICK_DURATION = 1f / TICKS_PER_SECOND;
    private readonly List<GameTickReceiver> _receivers;
    private float _startTime;
    private float _lastUpdateTime;
    private long _numberOfTicks;

    public static GameTickManager FindManagerInGame() {
        var managerEntity = GameObject.Find("GameTickManager");
        return managerEntity != null ? 
            managerEntity.GetComponent<GameTickManager>() : null;
    }

    public GameTickManager() {
        _receivers = new List<GameTickReceiver>();
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        _lastUpdateTime = _startTime = Time.time;
    }

    // Subscribes a GameTickReceiver to receive tick events
    public void Subscribe(GameTickReceiver receiver) {
        _receivers.Add(receiver);
    }
    
    void Update() {
        var currTime = Time.time;
        var deltaTime = currTime - _lastUpdateTime;
        if (deltaTime > TICK_DURATION) {
            var ticks = (int)Math.Floor(deltaTime / TICK_DURATION);
            var tickTime = ticks * TICK_DURATION;
            Tick(tickTime);
            _lastUpdateTime += tickTime;
            _numberOfTicks += ticks;
            
//            Debug.Log("Tick:  last=" + _lastUpdateTime + 
//                      ", curr=" + currTime +
//                      "\ndelta=" + deltaTime +
//                      ", ticks=" + ticks +
//                      ", tickTime=" + tickTime +
//                      ", totalTicks=" + _numberOfTicks);
        }
    }

    private void Tick(float amount) {
        foreach (var r in _receivers) {
            r.Tick(amount);
        }
    }
}