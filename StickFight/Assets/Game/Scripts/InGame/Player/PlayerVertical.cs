using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerVertical : MonoBehaviour {
    [Header("Jump")]
    [SerializeField] private Vector2 jumpForceGround = new Vector2(0,9f);
    [SerializeField] private Vector2 jumpForceWall = new Vector2(9f,9f);
    [SerializeField] private float timeForOneJump = 0.2f;
    [Header("Support")]
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Player player;
    [Header("ListAnim")]
    [SerializeField] private List<EnumPlayerStatus> lstStatusIdle;
    private TurnJump turnJump;
    public TurnJump JumpInfo => turnJump;
    private bool doJump;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb2D => playerMovement.Rb2D;

    private void Awake() {
        turnJump = new TurnJump();
    }
    public void Init(PlayerMovement playerMovement) {
        this.playerMovement = playerMovement;
    }

    private void Update() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.DIE || player.CurStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        Inputs();
    }

    private void Inputs() {
        if(Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump")) {
            doJump = true;
        }
    }
    private void FixedUpdate() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.DIE || player.CurStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        //Hanlder Input
        if(doJump) {
            if(playerMovement.PlayerTourch == PlayerTourch.GROUND) {
                turnJump.Set(EnumJumpType.JUMP_I, jumpForceGround, timeForOneJump);
                player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMPBEFOR, () => {
                    player.SetAnimCheckStatus(EnumPlayerStatus.JUMP, lstStatusIdle);
                });
            } else if(playerMovement.PlayerTourch == PlayerTourch.WALL) {
                Vector2 bounce = jumpForceWall;
                if(playerMovement.PlayerHor.PlayerFace == DirHorizontal.RIGHT) {
                    bounce.x = -bounce.x;
                }
                turnJump.Set(EnumJumpType.JUMP_I, bounce, timeForOneJump);
                player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMPBEFOR, () => {
                    player.SetAnimCheckStatus(EnumPlayerStatus.JUMP, lstStatusIdle);
                });
            } else if(playerMovement.PlayerTourch == PlayerTourch.AIR) {
                if(turnJump.TypeJump == EnumJumpType.JUMP_II) {

                } else if(turnJump.TypeJump == EnumJumpType.JUMP_I) {
                    turnJump.Set(EnumJumpType.JUMP_II, jumpForceGround, timeForOneJump);
                    player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMPBEFOR, () => {
                        player.SetAnimCheckStatus(EnumPlayerStatus.JUMP, lstStatusIdle);
                    });
                } else {
                    turnJump.Set(EnumJumpType.JUMP_I, jumpForceGround, timeForOneJump);
                    player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMPBEFOR, () => {
                        player.SetAnimCheckStatus(EnumPlayerStatus.JUMP, lstStatusIdle);
                    });
                }
            }
            doJump = false;
        }

        //Hanlder movement
        if(turnJump.TimeJump > 0) {
            rb2D.gravityScale = 1f;
            rb2D.velocity = new Vector2(rb2D.velocity.x + turnJump.Force.x, turnJump.Force.y);
        } else {
            rb2D.gravityScale = 3f;
            if(playerMovement.PlayerTourch == PlayerTourch.GROUND) {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                //rb2D.gravityScale = 1f;
            }
        }

        //Hanlder Anim
        if(rb2D.velocity.y > 0) {
            player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMP);
        } else {
            if(playerMovement.PlayerTourch == PlayerTourch.AIR) {
                player.SetPlayerStatusCheckRank(EnumPlayerStatus.JUMPFALL);
            } else if(playerMovement.PlayerTourch == PlayerTourch.WALL) {
                player.SetPlayerStatusCheckRank(EnumPlayerStatus.CLIMB);
            } else if(playerMovement.PlayerTourch == PlayerTourch.GROUND) {
                player.SetIdleCheckStatus(lstStatusIdle);
            }
        }

        //hanlder Time
        if(turnJump.TimeJump > 0) {
            turnJump.TimeJump -= Time.fixedDeltaTime;
        }
    }

    public void SetUpNoVertical() {
        turnJump.Defaul();
        player.SetIdleCheckStatus(lstStatusIdle);
    }

    public class TurnJump {
        public EnumJumpType TypeJump;
        public Vector2 Force;
        public float TimeJump;

        public TurnJump() {
            this.TypeJump = EnumJumpType.NONE;
            this.Force = Vector2.zero;
            this.TimeJump = 0f;
        }

        public TurnJump(EnumJumpType type, Vector2 force, float timeJump) {
            this.TypeJump = type;
            this.Force = force;
            this.TimeJump = timeJump;
        }

        public void Set(EnumJumpType type, Vector2 force, float timeJump) {
            this.TypeJump = type;
            this.Force = force;
            this.TimeJump = timeJump;
        }

        public void Defaul() {
            //this.TypeJump = EnumJumpType.NONE;
            this.Force = Vector2.zero;
            this.TimeJump = 0f;
        }
    }
}

public enum EnumJumpType {
    NONE,
    JUMP_I,
    JUMP_II
}




