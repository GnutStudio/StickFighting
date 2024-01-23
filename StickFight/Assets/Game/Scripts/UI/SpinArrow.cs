using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinArrow : MonoBehaviour
{
    [SerializeField] private AudioClip audioSound;

    private ItemStackView viewCol;
    public ItemStackView ViewCol => viewCol;

    private void OnTriggerEnter2D(Collider2D collision) {
        ItemStackView view = collision.GetComponent<ItemStackView>();
        if(view != null) {
            viewCol = view;
            SoundManager.Instance.PlaySound(audioSound);
            //audioSource.PlayOneShot(audioSound);
        }
    }
}
