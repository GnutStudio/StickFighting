// using UnityEngine;
//
// public class WitchMove : MonoBehaviour
// {
//     [SerializeField] protected EnemyBase enemyBase;
//     [SerializeField] protected Rigidbody2D rb2D;
//
//
//     [SerializeField] private Transform leftTop, leftBot, mid, rightTop, rightBot;
//     [SerializeField] protected BeamRayCast beamLeft;
//     [SerializeField] protected BeamRayCast beamRight;
//
//
//     protected virtual void FixedUpdate()
//     {
//         if(!CheckFront(vec) || !CheckBack(ColPos)) 
//         {
//             rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
//             if(enemyBase.CurStatus == EnemyStatus.IDLE) 
//             {
//                 enemyBase.Flip();
//             }
//             return;
//         }
//         
//         
//         // var b = transform.position == pos.transform.position;
//     }
//     
//     protected bool CheckFront(Vector3 ColPos) 
//     {
//         Collider2D collider2D = null;
//         foreach(var col in beamLeft.ArrayCollider2D) 
//         {
//             if(col != null) 
//             {
//                 collider2D = col;
//                 var pos = col.transform.position;
//                 ColPos = pos;
//                 break;
//             }
//         }
//         return collider2D != null;
//     }
//     
//     protected bool CheckBack(Vector3 ColPos) 
//     {
//         Collider2D collider2D = null;
//         foreach(var col in beamRight.ArrayCollider2D) 
//         {
//             if(col != null) 
//             {
//                 collider2D = col;
//                 var Pos = col.transform.position;
//                 ColPos = Pos;
//                 break;
//             }
//         }
//         return collider2D != null;
//     }
// }
