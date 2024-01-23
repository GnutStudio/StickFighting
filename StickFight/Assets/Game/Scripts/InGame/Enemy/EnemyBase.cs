using UnityEngine;
using DG.Tweening;

public class EnemyBase : CharacterBase {
    [SerializeField] private Transform display;
    [SerializeField] protected EnemyAnim enemyAnim;
    [SerializeField] protected BeamRayCast eye_Befor, eye_After, distan_attack;
    [SerializeField] protected EnemyStatus curStatus;
    [SerializeField] protected EnemyHeartBar enemyBar;
    [SerializeField] protected EnemyAttack enemyAttack;
    [SerializeField] protected Collider2D col2D;
    [SerializeField] protected Rigidbody2D rg2D;
    [Header("ForceDie")]
    [SerializeField] protected Vector2 forceDie = new Vector2(5,5);
    [Header("Effect")]
    [SerializeField] protected ParticleSystem particleBlood;
    [SerializeField] protected ParticleSystem particleDiePref;
    [SerializeField] protected AudioClip soundDie;
    public Rigidbody2D Rg2D => rg2D;
    public Transform Display => display;
    public EnemyStatus CurStatus => curStatus;
    protected EnemyStatus afterUpdateStatus;
    public DirHorizontal dirFace {
        get {
            if(display.localEulerAngles.y == 0) {
                return DirHorizontal.RIGHT;
            } else {
                return DirHorizontal.LEFT;
            }
        }
    }
    public float PercentHeart {
        get {
            return curHeart / (float)originHeart;
        }
    }

    protected Tween tween;

    protected override void Awake() {
        LevelMap perentMap = transform.GetComponentInParent<LevelMap>();
        if(perentMap != null) {
            LevelInfo level = RuleDameAndHeart.GetHeartDameNormalEnemy(perentMap.Level);
            originHeart = level.Heart;
            originDame = level.Damage;
        }
        curHeart = originHeart;
        curDame = originDame;
    }

    protected virtual void Start() {
        SetStatus(EnemyStatus.IDLE);
        enemyBar.Init();
    }

    protected virtual void Update() {
        if(curStatus == EnemyStatus.IDLE || curStatus == EnemyStatus.MOVE || curStatus == EnemyStatus.NONE || curStatus == EnemyStatus.DETECH) {
            afterUpdateStatus = EnemyStatus.MOVE;
            SetupStatus();
            SetStatus(afterUpdateStatus);
        }
        //if(Input.GetKeyDown(KeyCode.D)) {
        //    Die(InGameManager.Instance.Player.gameObject);
        //}
    }

    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        Vector3 randomPos = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f),0);
        SpawnerTextDame.Instance.Spawner(transform.position + randomPos, dame.ToString());
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            curStatus = EnemyStatus.GET_DAME;
            rg2D.velocity = Vector2.zero;
            enemyAnim.SetAnimGetDame(() => {
                SetStatus(EnemyStatus.IDLE);
            });
            enemyBar.UpdateHeart(PercentHeart);
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }
    }
    protected virtual void SetupStatus() {
        foreach(var col in distan_attack.ArrayCollider2D) {
            if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                afterUpdateStatus = EnemyStatus.ATTACK;
                if(!enemyAttack.CanAttack) {
                    afterUpdateStatus = EnemyStatus.IDLE;
                }
                return;
            }
        }

        foreach(var col in eye_Befor.ArrayCollider2D) {
            if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                afterUpdateStatus = EnemyStatus.DETECH;
                break;
            }
        }

        if(afterUpdateStatus != EnemyStatus.DETECH) {
            foreach(var col in eye_After.ArrayCollider2D) {
                if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                    afterUpdateStatus = EnemyStatus.DETECH;
                    Flip();
                    break;
                }
            }
        }
    }

    public void Flip() {
        //rg2D.velocity = Vector3.zero;
        if(dirFace == DirHorizontal.RIGHT) {
            display.localEulerAngles = new Vector3(0, 180, 0);
        } else if(dirFace == DirHorizontal.LEFT) {
            display.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void Flip(DirHorizontal dir) {
        if(dir == DirHorizontal.RIGHT) {
            display.localEulerAngles = new Vector3(0, 0, 0);
        } else if(dir == DirHorizontal.LEFT) {
            display.localEulerAngles = new Vector3(0, -180, 0);
        }
    }

    protected virtual bool SetStatus(EnemyStatus status) {
        if(curStatus == status) {
            return true;
        }
        curStatus = status;
        if(curStatus == EnemyStatus.ATTACK) {
            enemyAttack.Attack(() => {
                SetStatus(EnemyStatus.IDLE);
            });
        } else if(curStatus == EnemyStatus.DETECH || curStatus == EnemyStatus.MOVE) {
            enemyAnim.SetAnimWalk();
        } else {
            enemyAnim.AnimIdle();
            return false;
        }
        return true;
    }

    protected virtual void Die(GameObject objMakeDame = null) {
        curStatus = EnemyStatus.DIE;
        enemyAnim.SetAnimDie();
        tween = DOVirtual.DelayedCall(2f, () => {
            transform.gameObject.SetActive(false);
        });
        col2D.isTrigger = true;
        if(objMakeDame != null && objMakeDame.transform.position.x > transform.position.x) {
            rg2D.AddForce(new Vector2(-forceDie.x, forceDie.y), ForceMode2D.Impulse);
        } else {
            rg2D.AddForce(forceDie, ForceMode2D.Impulse);
        }
        int randomCoin = Random.Range(1,3);
        SpawnerCoin.Instance.Spawner(transform.position, randomCoin);
        // SpawnerCoin.Instance.Spawner(InGameManager.Instance.LevelMap .transform.position, randomCoin);
        InGameManager.Instance.AddEnemyDie(this);
        SoundManager.Instance.PlaySound(soundDie);
    }

    protected virtual void OnDisable() {
        tween.CheckKillTween();
    }
}

public enum EnemyStatus {
    NONE = 0,
    IDLE = 1,
    MOVE = 2,
    DETECH = 3,
    ATTACK = 4,
    GET_DAME = 5,
    DIE = 6,
    WIN = 7,
}

