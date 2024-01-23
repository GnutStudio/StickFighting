using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerHorizontal : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float speedGround = 9f;
    [SerializeField] private float speedAir = 6f;
    [SerializeField] private float speedAttack = 3f;
    [Header("Dash")]
    [SerializeField] private float timeDash = 0.2f;
    [SerializeField] private float speedDash = 36f;
    [SerializeField] private SkeletonGhost skeletonGhost;
    [Header("Display")]
    [SerializeField] private Transform display;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Player player;
    [Header("ListAnim")]
    [SerializeField] private List<EnumPlayerStatus> lstStatusIdle;
    public DirHorizontal PlayerFace => display.eulerAngles.y == 0 ? DirHorizontal.RIGHT : DirHorizontal.LEFT;
    public TurnMove MoveTurn;
    private float xDirectionalInput;
    private DirHorizontal direction;
    private PlayerMovement playerMovement;

    public bool _isMoving;
    // private Shadow _shadow;
    //private boo
    public float XDirectionalInput => xDirectionalInput;
    public void Init(PlayerMovement playerMovement) {
        this.playerMovement = playerMovement;
        MoveTurn = new TurnMove(DirHorizontal.RIGHT, 0f, TypeMove.NONE);
        direction = DirHorizontal.NONE;
        skeletonGhost.ghostingEnabled = false;
    }
    #region Update
    private void Update() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.DIE || player.CurStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        Inputs();
    }

    private void Inputs() {
        xDirectionalInput = CrossPlatformInputManager.GetAxis("Horizontal");
#if UNITY_EDITOR
        float xDirecInput = Input.GetAxis("Horizontal");
        if(Mathf.Abs(xDirecInput) > Mathf.Abs(xDirectionalInput)) {
            xDirectionalInput = xDirecInput;
        }
#endif
        if(Input.GetKeyDown(KeyCode.LeftArrow) || CrossPlatformInputManager.GetButtonDown("MoveLeft")) {
            direction = DirHorizontal.LEFT;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || CrossPlatformInputManager.GetButtonDown("MoveRight")) {
            direction = DirHorizontal.RIGHT;
        }
    }
    #endregion
    private void FixedUpdate() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.DIE || player.CurStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        Move();
    }

    private void Move() {
        //Hanlder Input
        if(direction != DirHorizontal.NONE) {
            if(MoveTurn.TypeMove != TypeMove.DASH) {
                if(MoveTurn.TypeMove == TypeMove.NORMAL) {
                    if(MoveTurn.Time > 0 && MoveTurn.Direc == direction) {
                        skeletonGhost.ghostingEnabled = true;
                        MoveTurn.Set(direction, timeDash, TypeMove.DASH);
                        Flip(direction);
                    } else {
                        MoveTurn.Set(direction, PlayerMovement.TIME_DELAY_KEY, TypeMove.NORMAL);
                        Flip(direction);
                    }
                } else if(MoveTurn.TypeMove == TypeMove.NONE) {
                    MoveTurn.Set(direction, PlayerMovement.TIME_DELAY_KEY, TypeMove.NORMAL);
                    Flip(direction);
                }
            }
            _isMoving = true;
            direction = DirHorizontal.NONE;
        }
         
        //Move
        if(MoveTurn.TypeMove == TypeMove.DASH) {
            rb2D.velocity = new Vector2(speedDash * (int)MoveTurn.Direc, 0);
            _isMoving = true;
            //rb2D.AddForce(new Vector2(speedDash,0f),ForceMode2D.Impulse);
        } else {
            if(playerMovement.PlayerTourch == PlayerTourch.AIR) {
                rb2D.velocity = new Vector2(xDirectionalInput * speedAir, rb2D.velocity.y);
            } else {
                if(playerAttack.IsAttacking) {
                    rb2D.velocity = new Vector2(xDirectionalInput * speedAttack, rb2D.velocity.y);
                    return;
                }

                if(playerMovement.PlayerTourch == PlayerTourch.WALL && xDirectionalInput * (int)PlayerFace > 0) {
                    return;
                }
                rb2D.velocity = new Vector2(xDirectionalInput * speedGround, rb2D.velocity.y);
            }
        }

        //Anim
        if(MoveTurn.TypeMove == TypeMove.DASH) {
            player.SetPlayerStatusCheckRank(EnumPlayerStatus.DASH);
        } else {
            if(Mathf.Abs(xDirectionalInput) > 0 && playerMovement.PlayerTourch == PlayerTourch.GROUND) {
                Flip(xDirectionalInput > 0 ? DirHorizontal.RIGHT : DirHorizontal.LEFT);
                var result = player.SetPlayerStatusCheckRank(EnumPlayerStatus.RUN);
            } else {
                player.SetIdleCheckStatus(lstStatusIdle);
            }
        }

        //CountDown Time
        if(MoveTurn.TypeMove != TypeMove.NONE && MoveTurn.Time >= 0) {
            MoveTurn.Time -= Time.fixedDeltaTime;
            if(MoveTurn.Time <= 0) {
                if(MoveTurn.TypeMove == TypeMove.DASH) {
                    player.SetIdleCheckStatus(lstStatusIdle);
                    skeletonGhost.ghostingEnabled = false;
                }
                MoveTurn.TypeMove = TypeMove.NORMAL;
            }
        }

        _isMoving = false;
    }

    private void Flip(DirHorizontal dir) {
        if(dir == DirHorizontal.RIGHT) {
            display.eulerAngles = new Vector3(0, 0, 0);
        } else if(dir == DirHorizontal.LEFT) {
            display.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void SetUpNoMove() {
        MoveTurn.Defaul();
        player.SetIdleCheckStatus(lstStatusIdle);
        skeletonGhost.ghostingEnabled = false;
    }

    public class TurnMove {
        public DirHorizontal Direc = default;
        public float Time = 0;
        public TypeMove TypeMove = default;

        public TurnMove(DirHorizontal direction, float timeDelay, TypeMove typeMove) {
            this.Direc = direction;
            this.Time = timeDelay;
            this.TypeMove = typeMove;
        }

        public void Set(DirHorizontal direction, float timeDelay, TypeMove typeMove) {
            this.Direc = direction;
            this.Time = timeDelay;
            this.TypeMove = typeMove;
        }

        public void Defaul() {
            this.Direc = DirHorizontal.NONE;
            this.Time = 0f;
            this.TypeMove = TypeMove.NONE;
        }
    }

    #region SetUp
    [ContextMenu("Setup")]
    public void Setup() {
        rb2D = GetComponent<Rigidbody2D>();
        display = transform;
        playerAttack = GetComponent<PlayerAttack>();
    }
    #endregion
}



public enum DirHorizontal {
    LEFT = -1,
    NONE = 0,
    RIGHT = 1,
}

public enum TypeMove {
    NONE,
    NORMAL,
    DASH
}

