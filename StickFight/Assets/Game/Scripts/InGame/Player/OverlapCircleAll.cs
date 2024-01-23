using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircleAll : MonoBehaviour
{
    public float lookRadius = 10f;
    public LayerMask layerMask;
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}