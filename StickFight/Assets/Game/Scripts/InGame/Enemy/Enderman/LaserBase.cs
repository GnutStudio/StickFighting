using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBase : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem particleStart,particleEnd;
    [Header("Customer")]
    [SerializeField] private float distance = 10f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private int dame;
    public bool OnLaser => line.gameObject.activeInHierarchy;

    public void TurnOn(Vector3 direction,int dame)
    {
        line.gameObject.SetActive(true);
        this.direction = direction;
        this.dame = dame;
        particleStart.gameObject.SetActive(true);
        particleEnd.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        line.gameObject.SetActive(false);
        particleStart.gameObject.SetActive(false);
        particleEnd.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (OnLaser)
        {
            Vector2 lastPoint = transform.position + (Vector3)direction * distance;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance,layerMask);
            if (hit)
            {
                lastPoint = hit.point;
                Player player = hit.collider.transform.parent.GetComponent<Player>();
                if (player!=null) {
                    player.GetDameStun(dame);
                }
            }
            Draw2DRay(transform.position,lastPoint);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        particleEnd.transform.position = endPos;
    }
}
