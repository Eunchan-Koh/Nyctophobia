using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class itemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal , Magnet, Shield, Madness}

    [Header("Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemInitialDesc;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;
    
    
    [Header("Level Data")]
    public float baseDamage;
    public int baseCount;
    public float baseEffectSize;
    public float[] damages;
    public int[] counts;
    public float[] effectSizes;

    [Header("Weapon")]
    public GameObject projectile;
    public float posOffset;


}
