using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCS : MonoBehaviour {
    [SerializeField] private float percent = 0.25f;
    [SerializeField] private GameObject itemBoost;
    [SerializeField] private GameObject icon;
    [SerializeField] private bool noAds;

    private void Start() {
        icon.SetActive(!noAds);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            if(noAds) {
                player.Healing(percent);
                itemBoost.SetActive(false);
                this.gameObject.SetActive(false);
            } else {
                player.Healing(percent);
                itemBoost.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}
