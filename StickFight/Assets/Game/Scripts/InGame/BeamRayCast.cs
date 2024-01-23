using UnityEngine;

public class BeamRayCast : MonoBehaviour {
    [SerializeField] private int amountRaycast;
    [SerializeField] private float space;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layer;
    [Header("Color")]
    [SerializeField] private int color; //1.green, 2.red, 3.yellow, 4.blue

    Collider2D[] arrayCollider2D;
    public Collider2D[] ArrayCollider2D {
        get {
            SetRayCast();
            return arrayCollider2D;
        }
    }
    private void Awake() {
        arrayCollider2D = new Collider2D[amountRaycast];
    }

    public void SetRayCast() {
        float centerY = (amountRaycast-1)/2f;
        for(int i = 0; i < amountRaycast; i++) {
            Vector2 origin = (Vector2)transform.position + (centerY-i)*space*(Vector2)transform.up;
            RaycastHit2D result = Physics2D.Raycast(origin, transform.right, distance, layer);
            arrayCollider2D[i] = result.collider;
        }
    }
    
    void OnDrawGizmosSelected() {
        // Draw a semitransparent blue cube at the transforms position
        float centerY = (amountRaycast-1)/2f;
        for(int i = 0; i < amountRaycast; i++) {
            Vector2 from = (Vector2)transform.position + (centerY-i)*space*(Vector2)transform.up;
            Vector2 to = from + (Vector2)transform.right*distance;
            Gizmos.color = color.GetColorByInt();
            Gizmos.DrawLine(from, to);
        }
    }
}
