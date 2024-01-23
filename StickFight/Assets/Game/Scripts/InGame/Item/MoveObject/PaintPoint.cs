using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPoint : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private float radios = 0.5f;
    private void OnDrawGizmosSelected() {
        Gizmos.color = index.GetColorByInt();
        Gizmos.DrawWireSphere(transform.position, radios);
    }
}
