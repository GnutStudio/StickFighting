using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxInfo : MonoBehaviour {
    public Vector2 size;
    public float angle;
    public LayerMask layerMask;
    void OnDrawGizmosSelected() {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, size);
    }
}
