using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetechPlayer : MonoBehaviour
{
    private Action callback;
    public void Init(Action callback) {
        this.callback = callback; 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if(player) {
            callback?.Invoke();
        }
    }
}
