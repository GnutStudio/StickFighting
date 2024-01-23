using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    public static float TIME_DELAY_KEY = 0.2f;
    [SerializeField] private Rigidbody2D rb2D;
    public Rigidbody2D Rb2D => rb2D;
    [Header("InfoCollider")]
    [SerializeField] private PlayerTourch playerTourch;
    [SerializeField] private OverlapBoxInfo boxGroundInfo;
    [SerializeField] private OverlapBoxInfo boxWallInfo;
    public PlayerTourch PlayerTourch => playerTourch;
    [Header("PartMove")]
    [SerializeField] private PlayerHorizontal playerHorizontal;
    [SerializeField] private PlayerVertical playerVertical;
    public PlayerHorizontal PlayerHor => playerHorizontal;

    private void Awake() {
        playerHorizontal.Init(this);
        playerVertical.Init(this);
    }
    
    private void Update() {
        if(Physics2D.OverlapBox(boxGroundInfo.transform.position, boxGroundInfo.size, boxGroundInfo.angle, boxGroundInfo.layerMask)) {
            playerTourch = PlayerTourch.GROUND;
        } else if(Physics2D.OverlapBox(boxWallInfo.transform.position, boxWallInfo.size, boxWallInfo.angle, boxWallInfo.layerMask)) {
            playerTourch = PlayerTourch.WALL;
        } else {
            playerTourch = PlayerTourch.AIR;
        }
    }

    #region Support
    [ContextMenu("SetUp")]
    public void SetUp() {

    }
    #endregion
}

public enum PlayerTourch {
    AIR = 0,
    WALL = 1,
    GROUND = 2,
}
