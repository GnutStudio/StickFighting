using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField, SpineSkin] private string nameSkin;
    [SerializeField, SpineSkin] private string nameWeapon;
    [SerializeField] private SkeletonAnimation Anim;
    private void Start() {
        BuildSkin();
    }

    [ContextMenu("BuildSkin")]
    public void BuildSkin() {
        Skin skin = new Skin("Skin");
        skin.AddSkin(Anim.Skeleton.Data.FindSkin(nameSkin));
        skin.AddSkin(Anim.Skeleton.Data.FindSkin(nameWeapon));
        Anim.Skeleton.SetSkin(skin);
        Anim.Skeleton.SetSlotsToSetupPose();
    }
}
