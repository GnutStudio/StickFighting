using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject {
    [SerializeField] private WeaponID id;
    [SerializeField] private Sprite icon;
    [SerializeField] private TypeWeapon type;
    [SerializeField] private int dame;
    [SerializeField] private string nameSkin;
    [Header("CustomBow")]
    [SerializeField] private WeaponArrow arrowPref;
    [SerializeField] private float speed;

    [Header("Customsword")] 
    [SerializeField] private SwordCus swordCus;
    
    public WeaponID ID => id;
    public Sprite Icon => icon;
    public TypeWeapon TypeWeapon => type;
    public int Dame => dame;
    public string NameSkin => nameSkin;
    public WeaponArrow ArrowPref => arrowPref;
    public float Speed => speed;
}

public enum WeaponID {
    NONE = 0,
    SWORD_NORMAL = 1,
    SWORD_GOOD = 2,
    SWORD_VIP = 3,
    SWORD_LEGAND = 4,

    BOW_NORMAL = 11,
    BOW_GOOD = 12,
    BOW_VIP = 13,
    BOW_LEGAND =14,
}

public enum TypeWeapon {
    NONE = 0,
    SORT = 1,
    LONG =2
}