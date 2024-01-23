using Puppy.Engine.UnityInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject {
    [SerializeField] private ItemID itemID;
    [SerializeField,SpriteField] private Sprite icon;
    [SerializeField] private string description;
    public ItemID ItemID => itemID;
    public Sprite Icon => icon;
    public string Description => description;
}
