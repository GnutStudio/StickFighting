using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : EnemyAttack
{
    [SerializeField] private SkeletonArrow skeletonArrow;
    [SerializeField] private Transform point;
    [SerializeField] private float angleStart;
    private float gravityScaleArrow;
    protected override void Awake() {
        base.Awake();
        gravityScaleArrow = -Physics2D.gravity.y;
    }

    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        SkeletonArrow newArrow = skeletonArrow.Spawn(InGameManager.Instance.LevelMap.transform);
        newArrow.transform.position = point.position;
        newArrow.MoveTarget(enemyBase.curDame,GetForceArrow());
    }

    private Vector2 GetForceArrow() {
        float angleTarget = enemyBase.dirFace == DirHorizontal.RIGHT?angleStart:180-angleStart;  
        Vector2 force = new Vector2(0,0);
        Vector2 positionTarget = InGameManager.Instance.Player.transform.position;
        Vector2 distance = positionTarget - (Vector2)point.transform.position;
        //distance = new Vector2(-1,-1);
        float tan = Mathf.Tan(angleTarget*Mathf.PI/180f);
        float timeSquare = (distance.x*tan-distance.y)*2/gravityScaleArrow;
        if(timeSquare > 0) {
            float time = Mathf.Sqrt(timeSquare);
            force.x = distance.x / time;
            force.y = tan * force.x;
        }
        return force;
    }


}
