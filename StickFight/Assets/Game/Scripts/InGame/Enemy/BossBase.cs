using STU;
using UnityEngine;
using DG.Tweening;

public class BossBase : EnemyBase {
    private const float TIME_DELAY_BOSS_ACTIVE = 2f;
    private bool bossActive = false;
    protected virtual void OnEnable() {
        EventDispatcher.AddListener<EventKey.BossArea>(HalderPlayerInArea);
    }

    protected override void OnDisable() {
        base.OnDisable();
        EventDispatcher.RemoveListener<EventKey.BossArea>(HalderPlayerInArea);
    }

    private void HalderPlayerInArea(EventKey.BossArea evt) {
        ProcameraController.Instance.SetTarget(transform);
        tween.CheckKillTween();
        tween = DOVirtual.DelayedCall(TIME_DELAY_BOSS_ACTIVE, () => {
            ProcameraController.Instance.SetTarget(InGameManager.Instance.Player.transform);
            bossActive = true;
        });

    }
    protected override void Awake() {
        LevelMap perentMap = transform.GetComponentInParent<LevelMap>();
        if(perentMap != null) {
            LevelInfo level = RuleDameAndHeart.GetHeartDameBoss(perentMap.Level);
            originHeart = level.Heart;
            originDame = level.Damage;
        }
        curHeart = originHeart;
        curDame = originDame;
    }
    protected override void Start() {
        SetStatus(EnemyStatus.IDLE);
    }

    protected override void Update() {
        if(bossActive == false) {
            return;
        }
        base.Update();
    }

    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            //curStatus = EnemyStatus.GET_DAME;
            rg2D.velocity = Vector2.zero;
            enemyAnim.SetAnimGetDame(() => {
                enemyAnim.AnimIdle();
            });
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }
    }

    protected override void Die(GameObject objMakeDame = null) {
        curStatus = EnemyStatus.DIE;
        enemyAttack.enabled = false;
        ProcameraController.Instance.SetTarget(transform);
        enemyAnim.SetAnimDie(() => {
            gameObject.SetActive(false);
            if(particleDiePref != null) {
                var par = particleDiePref.Spawn(InGameManager.Instance.LevelMap.transform);
                par.transform.position = transform.position;
            }
            ProcameraController.Instance.SheckCamera();
            SpawnerCoin.Instance.SpawnerII(transform.position + Vector3.up * 2, 15, () => {
                ProcameraController.Instance.SetTarget(InGameManager.Instance.Player.transform);
                InGameManager.Instance.AddEnemyDie(this);
            });
        });
    }

    protected override void SetupStatus() {
        base.SetupStatus();
    }
}
