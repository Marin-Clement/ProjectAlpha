using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    public int id;
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public int itemPrice;
    public int itemRarity;
    public List<Object> itemStats;

    [Header("Item Type")]
    public bool isWeapon;
    public bool isArmor;
    public bool isConsumable;
    public bool isSpell;
    public bool isPassive;
}
