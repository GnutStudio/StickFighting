using UnityEngine;
using DG.Tweening;

public class WeaponCS : MonoBehaviour {
    [SerializeField] private WeaponID weaponID;
    [SerializeField] private SpriteRenderer img;
    [SerializeField] private GameObject iconAds;
    [SerializeField] private Collider2D col2D;
    [SerializeField] private float timeDelay = 2f;
    [Header("Customer")]
    [SerializeField] private bool noAds;
    [SerializeField] private bool noDisable;
    private Tween tween;
    private void Start() {
        WeaponData data = weaponID.GetDataWeaponByID();
        img.sprite = data.Icon;
        iconAds.SetActive(!noAds);
        col2D.enabled = true;
        if (noDisable) {
            noAds = true;
            iconAds.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if (player != null) {
            if (noAds) {
                player.SetWeapon(weaponID.GetDataWeaponByID());
                if (noDisable) {
                    noAds = false;
                    iconAds.SetActive(true);
                    col2D.enabled = false;
                    img.gameObject.SetActive(false);
                    tween.CheckKillTween();
                    tween = DOVirtual.DelayedCall(timeDelay, () => {
                        img.gameObject.SetActive(true);
                        col2D.enabled = true;
                    });
                } else {
                    gameObject.SetActive(false);
                }
                return;
            }

            player.SetWeapon(weaponID.GetDataWeaponByID());
            if (noDisable) {
                col2D.enabled = false;
                img.gameObject.SetActive(false);
                tween.CheckKillTween();
                tween = DOVirtual.DelayedCall(timeDelay, () => {
                    img.gameObject.SetActive(true);
                    col2D.enabled = true;
                });
            } else {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }

    private void OnValidate() {

    }
}
